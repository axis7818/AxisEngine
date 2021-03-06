﻿using Microsoft.Xna.Framework;
using System;
using System.Linq;
using System.Collections.Generic;

namespace AxisEngine
{
    public abstract class WorldObject : IUpdateable
    {
        public int _updateOrder;
        private bool _enabled;
        private Layer _layer;
        private WorldObject _owner;
        private Vector2 _position;
        private float _rotation;
        private Vector2 _scale;
        private List<WorldObject> _components;

        public WorldObject()
        {
            Initialize(null, Vector2.Zero);
        }

        public WorldObject(WorldObject owner)
        {
            Initialize(owner, Vector2.Zero);
        }

        public WorldObject(WorldObject owner = null, Vector2? position = null, params WorldObject[] components)
        {
            Initialize(owner, position ?? null, components);
        }

        public event EventHandler<WorldObjectEventArgs> ComponentAdded;
        public event EventHandler<WorldObjectEventArgs> ComponentRemoved;
        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<LayerEventArgs> LayerChanged;
        public event EventHandler<WorldObjectEventArgs> OwnerChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;
        
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                if (EnabledChanged != null) EnabledChanged(this, new EventArgs());
            }
        }

        public bool HasOwner
        {
            get { return Owner == null ? false : true; }
        }

        public Layer Layer
        {
            get { return _layer; }
            set
            {
                _layer = value;
                OnLayerChanged(_layer);
            }
        }

        public WorldObject Owner
        {
            get { return _owner; }
            set
            {
                _owner = value;
                if (_owner != null && !_owner.GetComponents().Contains(this))
                {
                    _owner.AddComponent(this);
                }
                OnOwnerChanged(_owner);
            }
        }

        public Vector2 Position
        {
            get
            {
                Vector2 pos = _position;
                if (HasOwner)
                {
                    pos += Owner.Position;
                }
                return pos;
            }
            set { BasePosition = value; }
        }

        public Vector2 BasePosition
        {
            get { return _position; }
            private set { _position = value; }
        }

        public float Rotation
        {
            get
            {
                float rot = _rotation;
                if (HasOwner)
                {
                    rot += Owner.Rotation;
                }
                return rot;
            }
            set { BaseRotation = value; }
        }

        public float BaseRotation
        {
            get { return _rotation; }
            private set { _rotation = value; }
        }

        public Vector2 Scale
        {
            get
            {
                Vector2 scale = _scale;
                if (HasOwner)
                    scale *= Owner.Scale;
                return scale;
            }
            set { _scale = value; }
        }

        public Vector2 BaseScale
        {
            get { return _scale; }
            private set { _scale = value; }
        }

        public int UpdateOrder
        {
            get { return _updateOrder; }
            set
            {
                _updateOrder = value;
                if (UpdateOrderChanged != null)
                    UpdateOrderChanged(this, new EventArgs());
            }
        }

        public WorldObject RootObject
        {
            get
            {
                WorldObject result = this;
                while (result.HasOwner)
                    result = result.Owner;
                return result;
            }
        }

        protected abstract void UpdateThis(GameTime t);

        public void AddComponent(WorldObject component)
        {
            _components.Add(component);
            if (component.Owner != this)
                component.Owner = this;
            OnComponentAdded(component);

            if (!CircularComponentCheck(RootObject))
                throw new InvalidComponentsException("An invalid component configuration was detected. Make sure you do not have duplicate components or a circular arrangement of components.");
        }

        public void AddComponents(IEnumerable<WorldObject> components)
        {
            foreach (WorldObject wo in components)
                AddComponent(wo);
        }

        public WorldObject[] GetComponents()
        {
            return _components.ToArray();
        }

        public void RemoveComponent(WorldObject component)
        {
            if (_components.Contains(component))
            {
                _components.Remove(component);
                OnComponentRemoved(component);
            }
        }

        public void RemoveComponents(IEnumerable<WorldObject> components)
        {
            foreach (WorldObject wo in components)
                RemoveComponent(wo);
        }

        public void Update(GameTime t)
        {
            if (Enabled)
            {
                UpdateThis(t);
                UpdateComponents(t);
            }
        }

        protected virtual void OnLayerChanged(Layer layer)
        {
            if (LayerChanged != null)
                LayerChanged(this, new LayerEventArgs(layer));

            if (layer != null)
                foreach (WorldObject wo in _components)
                    wo.Layer = layer;
        }

        private void Initialize(WorldObject owner = null, Vector2? position = null, params WorldObject[] components)
        {
            Owner = owner;
            _components = new List<WorldObject>(components);

            BasePosition = position ?? Vector2.Zero;
            BaseScale = new Vector2(1, 1);
            BaseRotation = 0;

            Enabled = true;
            UpdateOrder = 0;
        }

        private void OnComponentAdded(WorldObject component)
        {
            if (ComponentAdded != null)
                ComponentAdded(this, new WorldObjectEventArgs(component));
        }

        private void OnComponentRemoved(WorldObject component)
        {
            if (ComponentRemoved != null)
                ComponentRemoved(this, new WorldObjectEventArgs(component));
        }

        protected virtual void OnOwnerChanged(WorldObject owner)
        {
            if (OwnerChanged != null)
                OwnerChanged(this, new WorldObjectEventArgs(owner));

            Layer = owner == null ? null : owner.Layer;
        }

        private void UpdateComponents(GameTime t)
        {
            foreach (WorldObject wo in _components)
                if (wo.Enabled)
                    wo.Update(t);
        }

        public static bool CircularComponentCheck(WorldObject root)
        {
            return CircularComponentCheck(root, new HashSet<WorldObject>());
        }

        private static bool CircularComponentCheck(WorldObject root, HashSet<WorldObject> log)
        {
            // check if the root is in the log, if it is, that means that we have a loop
            if (log.Contains(root))
                return false;

            // otherwise add the root, and perform a recursive call on all of the components.
            log.Add(root);
            foreach(WorldObject wo in root.GetComponents())
            {
                // if any fail, then stop checking.
                if (!CircularComponentCheck(wo, log))
                    return false;
            }

            // if none failed, then it checks out
            return true;
        }
    }
}