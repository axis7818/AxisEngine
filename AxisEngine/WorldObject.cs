using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace AxisEngine
{
    /// <summary>
    /// The base object that can exist in the world. World objects can be stacked in a tree structure to form an entity in the game world.
    /// </summary>
    public abstract class WorldObject : IUpdateable
    {
        /// <summary>
        /// the update order of the object
        /// </summary>
        public int _updateOrder;

        /// <summary>
        /// whether or not the world object is enabled
        /// </summary>
        private bool _enabled;

        /// <summary>
        /// A reference to the layer that the object is owned by
        /// </summary>
        private Layer _layer;

        /// <summary>
        /// The world object that owns this one
        /// </summary>
        private WorldObject _owner;

        /// <summary>
        /// the position of this object
        /// </summary>
        private Vector2 _position;

        /// <summary>
        /// The rotation of the world object (in radians)
        /// </summary>
        private float _rotation;

        /// <summary>
        /// The scaling factor of the World
        /// </summary>
        private Vector2 _scale;

        /// <summary>
        /// the list of children objects
        /// </summary>
        private List<WorldObject> Components;

        /// <summary>
        /// instantiates a new world object
        /// </summary>
        public WorldObject()
        {
            Initialize(null, Vector2.Zero);
        }

        /// <summary>
        /// instantiates a new world object
        /// </summary>
        /// <param name="owner">thw owner of the object</param>
        public WorldObject(WorldObject owner)
        {
            Initialize(owner, Vector2.Zero);
        }

        /// <summary>
        /// instantiates a new world object
        /// </summary>
        /// <param name="owner">thw owner of the object</param>
        /// <param name="position">the relative position of the object</param>
        /// <param name="components">a list of objects that will be components of this one</param>
        public WorldObject(WorldObject owner = null, Vector2? position = null, params WorldObject[] components)
        {
            Initialize(owner, position ?? null, components);
        }

        /// <summary>
        /// fired when a component is added
        /// </summary>
        public event EventHandler<WorldObjectEventArgs> ComponentAdded;

        /// <summary>
        /// fired when a component is removed
        /// </summary>
        public event EventHandler<WorldObjectEventArgs> ComponentRemoved;

        /// <summary>
        /// fired when the Enabled property changes
        /// </summary>
        public event EventHandler<EventArgs> EnabledChanged;

        /// <summary>
        /// fired when the object's layer changes
        /// </summary>
        public event EventHandler<LayerEventArgs> LayerChanged;

        /// <summary>
        /// fired when the object's owner changes
        /// </summary>
        public event EventHandler<WorldObjectEventArgs> OwnerChanged;

        /// <summary>
        /// fired when the UpdateOrder property changes
        /// </summary>
        public event EventHandler<EventArgs> UpdateOrderChanged;

        /// <summary>
        /// whether or not the world object is enabled
        /// </summary>
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                if (EnabledChanged != null) EnabledChanged(this, new EventArgs());
            }
        }

        /// <summary>
        /// whether or not this object has an owner object
        /// </summary>
        public bool HasOwner
        {
            get
            {
                return Owner == null ? false : true;
            }
        }

        /// <summary>
        /// A reference to the layer that the object is owned by
        /// </summary>
        public Layer Layer
        {
            get { return _layer; }
            set
            {
                _layer = value;
                OnLayerChanged(_layer);
            }
        }

        /// <summary>
        /// The world object that owns this one
        /// </summary>
        public WorldObject Owner
        {
            get { return _owner; }
            set
            {
                _owner = value;
                if (_owner != null)
                {
                    _owner.AddComponent(this);
                }
                OnOwnerChanged(_owner);
            }
        }

        /// <summary>
        /// Gets the position of the object in the world
        /// </summary>
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
            set
            {
                BasePosition = value;
            }
        }

        /// <summary>
        /// The position of the object relative to its owner
        /// </summary>
        public Vector2 BasePosition
        {
            get
            {
                return _position;
            }
            private set
            {
                _position = value;
            }
        }

        /// <summary>
        /// Gets the rotation of the world object in the world (radians)
        /// </summary>
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
            set
            {
                BaseRotation = value;
            }
        }

        /// <summary>
        /// The rotation of the object relative to its owner
        /// </summary>
        public float BaseRotation
        {
            get
            {
                return _rotation;
            }
            private set
            {
                _rotation = value;
            }
        }

        /// <summary>
        /// Gets pr sets the scaling factor of the object in the world
        /// </summary>
        public Vector2 Scale
        {
            get
            {
                Vector2 scale = _scale;
                if (HasOwner)
                {
                    scale *= Owner.Scale;
                }
                return scale;
            }
            set
            {
                BaseScale = value;
            }
        }

        /// <summary>
        /// The Scale of the object relative to its owner
        /// </summary>
        public Vector2 BaseScale
        {
            get
            {
                return _scale;
            }
            private set
            {
                _scale = value;
            }
        }

        /// <summary>
        /// the update order of the object
        /// </summary>
        public int UpdateOrder
        {
            get { return _updateOrder; }
            set
            {
                _updateOrder = value;
                if (UpdateOrderChanged != null) UpdateOrderChanged(this, new EventArgs());
            }
        }

        /// <summary>
        /// gets the root object of this one
        /// </summary>
        public WorldObject RootObject
        {
            get
            {
                WorldObject result = this;
                while (result.HasOwner)
                {
                    result = result.Owner;
                }
                return result;
            }
        }

        /// <summary>
        /// adds a component to the World Object
        /// </summary>
        /// <param name="component">the component to be added</param>
        public void AddComponent(WorldObject component)
        {
            if (!Components.Contains(component))
            {
                Components.Add(component);
                if (component.Owner != this)
                {
                    component.Owner = this;
                }
                OnComponentAdded(component);
            }
        }

        /// <summary>
        /// adds components to the World Object
        /// </summary>
        /// <param name="components">components to add</param>
        public void AddComponents(IEnumerable<WorldObject> components)
        {
            foreach (WorldObject wo in components)
            {
                AddComponent(wo);
            }
        }

        /// <summary>
        /// gets the components that this object has
        /// </summary>
        /// <returns>a list of WorldObjects that are components of this object</returns>
        public WorldObject[] GetComponents()
        {
            return Components.ToArray();
        }

        /// <summary>
        /// remove a component
        /// </summary>
        /// <param name="component">the component to remove</param>
        public void RemoveComponent(WorldObject component)
        {
            if (Components.Contains(component))
            {
                Components.Remove(component);
                OnComponentRemoved(component);
            }
        }

        /// <summary>
        /// removes several world objects from the components
        /// </summary>
        /// <param name="components">the components to remove</param>
        public void RemoveComponents(IEnumerable<WorldObject> components)
        {
            foreach (WorldObject wo in components)
            {
                RemoveComponent(wo);
            }
        }

        /// <summary>
        /// updates the world object
        /// </summary>
        /// <param name="t">the time since the last update</param>
        public void Update(GameTime t)
        {
            if (Enabled)
            {
                UpdateThis(t);
            }
        }

        /// <summary>
        /// performs an update on this object specifically. override to update the object
        /// </summary>
        /// <param name="t">the time since the last update</param>
        public virtual void UpdateThis(GameTime t)
        {
            UpdateComponents(t);
        }

        /// <summary>
        /// handles the events on the layer changing
        /// </summary>
        /// <param name="layer">the new layer</param>
        protected virtual void OnLayerChanged(Layer layer)
        {
            if (LayerChanged != null)
                LayerChanged(this, new LayerEventArgs() { Layer = layer });

            if (layer != null)
            {
                foreach (WorldObject wo in Components)
                    wo.Layer = layer;
            }
        }

        /// <summary>
        /// sets the values of some properties and variables
        /// </summary>
        /// <param name="owner">thw owner of the object</param>
        /// <param name="position">the relative position of the object</param>
        /// <param name="components">a list of objects that will be components of this one</param>
        private void Initialize(WorldObject owner = null, Vector2? position = null, params WorldObject[] components)
        {
            Owner = owner;
            Components = new List<WorldObject>(components);

            BasePosition = position ?? Vector2.Zero;
            BaseScale = new Vector2(1, 1);
            BaseRotation = 0;

            Enabled = true;
            UpdateOrder = 0;
        }

        /// <summary>
        /// handles the events on adding a component
        /// </summary>
        /// <param name="component">the component that is added</param>
        private void OnComponentAdded(WorldObject component)
        {
            if (ComponentAdded != null)
                ComponentAdded(this, new WorldObjectEventArgs() { WorldObject = component });
        }

        /// <summary>
        /// handles the events on removing an object
        /// </summary>
        /// <param name="component">the component that is removed</param>
        private void OnComponentRemoved(WorldObject component)
        {
            if (ComponentRemoved != null)
                ComponentRemoved(this, new WorldObjectEventArgs() { WorldObject = component });
        }

        /// <summary>
        /// handles events on the owner changing
        /// </summary>
        /// <param name="owner">the new owner</param>
        protected void OnOwnerChanged(WorldObject owner)
        {
            if (OwnerChanged != null)
                OwnerChanged(this, new WorldObjectEventArgs() { WorldObject = owner });

            Layer = owner == null ? null : owner.Layer;
        }

        /// <summary>
        /// updates each component
        /// </summary>
        /// <param name="t">the time since the last update</param>
        private void UpdateComponents(GameTime t)
        {
            foreach (WorldObject wo in Components)
            {
                if (wo.Enabled)
                    wo.Update(t);
            }
        }
    }
}