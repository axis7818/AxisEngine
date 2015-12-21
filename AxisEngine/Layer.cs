using AxisEngine.Physics;
using AxisEngine.Visuals;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AxisEngine
{
    public abstract class Layer : IUpdateable
    {
        private bool _enabled;
        private int _updateOrder;

        private List<WorldObject> WorldObjects;

        public Layer(CollisionManager collisionManager, DrawManager drawManager, TimeManager timeManager, params WorldObject[] worldObjects)
        {
            Initialize(collisionManager, drawManager, timeManager, worldObjects);
        }

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;
        public event EventHandler<WorldObjectEventArgs> WorldObjectAdded;
        public event EventHandler<WorldObjectEventArgs> WorldObjectRemoved;
        
        public CollisionManager CollisionManager { get; private set; }

        public DrawManager DrawManager { get; private set; }

        public TimeManager TimeManager { get; private set; }

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                if (EnabledChanged != null) EnabledChanged(this, new EventArgs());
            }
        }

        public int UpdateOrder
        {
            get { return _updateOrder; }
            set
            {
                _updateOrder = value;
                if (UpdateOrderChanged != null) UpdateOrderChanged(this, new EventArgs());
            }
        }

        protected abstract void UpdateThis(GameTime t);

        public void Add(WorldObject worldObject)
        {
            WorldObjects.Add(worldObject);
            OnWorldObjectAdded(worldObject);
        }

        public void AddRange(IEnumerable<WorldObject> worldObjects)
        {
            WorldObjects.AddRange(worldObjects);
            foreach (WorldObject worldObject in worldObjects)
                OnWorldObjectAdded(worldObject);
        }

        public void Remove(WorldObject worldObject)
        {
            WorldObjects.Remove(worldObject);
            OnWorldObjectRemoved(worldObject);
        }

        public void RemoveAll(Predicate<WorldObject> match)
        {
            foreach (WorldObject wo in WorldObjects.Where(x => match(x)))
                Remove(wo);
        }

        public void RemoveAt(int index)
        {
            if (index >= 0 && index < WorldObjects.Count)
            {
                WorldObject wo = WorldObjects[index];
                WorldObjects.RemoveAt(index);
                OnWorldObjectRemoved(wo);
            }
            else
                throw new IndexOutOfRangeException();
        }

        public void RemoveRange(int index, int count)
        {
            for (int i = 0; i < count; i++)
                RemoveAt(index);
        }

        public void Update(GameTime t)
        {
            if (Enabled)
            {
                // get the converted time
                GameTime scaledTime = TimeManager.ConvertTime(t);

                // update the objects in the layer
                UpdateThis(scaledTime);
                foreach (WorldObject x in WorldObjects)
                    x.Update(scaledTime);
            }
        }

        protected void Initialize(CollisionManager collisionManager, DrawManager drawManager, TimeManager timeManager, params WorldObject[] worldObjects)
        {
            WorldObjects = new List<WorldObject>();

            CollisionManager = collisionManager;
            DrawManager = drawManager;
            TimeManager = timeManager;

            AddRange(worldObjects);
            SortUpdateOrder();

            Enabled = true;
            UpdateOrder = 0;
        }

        private void OnWorldObjectAdded(WorldObject worldObject)
        {
            if (WorldObjectAdded != null) WorldObjectAdded(this, new WorldObjectEventArgs(worldObject));

            // set the layer
            worldObject.Layer = this;

            // hook up events
            worldObject.UpdateOrderChanged += worldObject_UpdateOrderChanged;

            // refresh the update order
            SortUpdateOrder();

            // add the worldObject to the Managers
            SubscribeToManagers(worldObject);
        }

        private void OnWorldObjectRemoved(WorldObject worldObject)
        {
            if (WorldObjectRemoved != null) WorldObjectRemoved(this, new WorldObjectEventArgs(worldObject));

            // remove the worldObject from the Managers
            UnsubscribeFromManagers(worldObject);
        }

        private void SortUpdateOrder()
        {
            WorldObjects.OrderBy(x => x.UpdateOrder);
        }

        private void SubscribeToManagers(WorldObject worldObject)
        {
            if (worldObject is ICollidable)
                CollisionManager.AddCollidable(worldObject as ICollidable);
            else if (worldObject is IDrawManageable)
                DrawManager.AddDrawable(worldObject as IDrawManageable);

            // subscribe components recursively
            foreach (WorldObject wo in worldObject.GetComponents())
            {
                SubscribeToManagers(wo);
            }
        }

        private void UnsubscribeFromManagers(WorldObject worldObject)
        {
            if (worldObject is ICollidable)
                CollisionManager.AddCollidable(worldObject as ICollidable);
            else if (worldObject is IDrawManageable)
                DrawManager.Remove(worldObject as IDrawManageable);

            // unsubscribe the components recursively
            foreach (WorldObject wo in worldObject.GetComponents())
            {
                UnsubscribeFromManagers(wo);
            }
        }

        private void worldObject_UpdateOrderChanged(object sender, EventArgs e)
        {
            SortUpdateOrder();
        }
    }
}