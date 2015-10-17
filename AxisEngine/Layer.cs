using AxisEngine.Physics;
using AxisEngine.Visuals;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AxisEngine
{
    /// <summary>
    /// holds a list of WorldObjects and manages their presence in the game. They are managed through a CollisionManager (physics) and a DrawManager (rendering)
    /// </summary>
    public class Layer : IUpdateable
    {
        /// <summary>
        /// whether or not the layer is enabled
        /// </summary>
        private bool _enabled;

        /// <summary>
        /// The order that this layer is updated relative to other layers being updated
        /// </summary>
        private int _updateOrder;

        /// <summary>
        /// The List of WorldObjects that belong to the layer. The update and display of these objects are managed by the Layer
        /// </summary>
        private List<WorldObject> WorldObjects;

        /// <summary>
        /// instantiates a layer
        /// </summary>
        /// <param name="collisionManager">the collision manager for the layer</param>
        /// <param name="drawManager">the draw manager for the layer</param>
        /// <param name="worldObjects">a list of world objects to add to the layer</param>
        public Layer(CollisionManager collisionManager, DrawManager drawManager, TimeManager timeManager, params WorldObject[] worldObjects)
        {
            Initialize(collisionManager, drawManager, timeManager, worldObjects);
        }

        /// <summary>
        /// fired when the Enabled property has changed
        /// </summary>
        public event EventHandler<EventArgs> EnabledChanged;

        /// <summary>
        /// fired when the updateOrder has changed
        /// </summary>
        public event EventHandler<EventArgs> UpdateOrderChanged;

        /// <summary>
        /// fired when a new object is added to the layer
        /// </summary>
        public event EventHandler<WorldObjectEventArgs> WorldObjectAdded;

        /// <summary>
        /// fired when an WorldObject is removed from the layer
        /// </summary>
        public event EventHandler<WorldObjectEventArgs> WorldObjectRemoved;

        /// <summary>
        /// The Object that handles all collision management for the layer
        /// </summary>
        public CollisionManager CollisionManager { get; private set; }

        /// <summary>
        /// The object that handles all rendering management for the layer
        /// </summary>
        public DrawManager DrawManager { get; private set; }

        /// <summary>
        /// The object that handles the passing of time for the layer
        /// </summary>
        public TimeManager TimeManager { get; private set; }

        /// <summary>
        /// whether or not the layer is enabled
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
        /// The order that this layer is updated relative to other layers being updated
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
        /// adds a world object to the layer
        /// </summary>
        /// <param name="worldObject">the added object</param>
        public void Add(WorldObject worldObject)
        {
            WorldObjects.Add(worldObject);
            OnWorldObjectAdded(worldObject);
        }

        /// <summary>
        /// adds some world objects to the layer
        /// </summary>
        /// <param name="worldObjects">the world obejcts that are added</param>
        public void AddRange(IEnumerable<WorldObject> worldObjects)
        {
            WorldObjects.AddRange(worldObjects);
            foreach (WorldObject worldObject in worldObjects)
                OnWorldObjectAdded(worldObject);
        }

        /// <summary>
        /// removes a world object from the layer
        /// </summary>
        /// <param name="worldObject">the world object that was removed</param>
        public void Remove(WorldObject worldObject)
        {
            WorldObjects.Remove(worldObject);
            OnWorldObjectRemoved(worldObject);
        }

        /// <summary>
        /// removes all world objects from the layer that match a given condition
        /// </summary>
        /// <param name="match">the condition to match</param>
        public void RemoveAll(Predicate<WorldObject> match)
        {
            foreach (WorldObject wo in WorldObjects.Where(x => match(x)))
                Remove(wo);
        }

        /// <summary>
        /// removes a world object at a given position
        /// </summary>
        /// <param name="index">the position</param>
        public void RemoveAt(int index)
        {
            if (index >= 0 && index < WorldObjects.Count)
            {
                WorldObject wo = WorldObjects[index];
                WorldObjects.RemoveAt(index);
                OnWorldObjectRemoved(wo);
            }
            else throw new IndexOutOfRangeException();
        }

        /// <summary>
        /// removes world objects in the specified range
        /// </summary>
        /// <param name="index">start index</param>
        /// <param name="count">amount of objects to remove</param>
        public void RemoveRange(int index, int count)
        {
            for (int i = 0; i < count; i++)
                RemoveAt(index + i);
        }

        /// <summary>
        /// Updates the layer
        /// </summary>
        /// <param name="t">the time that has passed in the last frame</param>
        public void Update(GameTime t)
        {
            if (Enabled)
            {
                GameTime scaledTime = TimeManager.ConvertTime(t);

                UpdateThis(scaledTime);

                foreach (WorldObject x in WorldObjects)
                    x.Update(scaledTime);
            }
        }

        /// <summary>
        /// an update method for the layer that is meant to be overriden
        /// </summary>
        /// <param name="t">the time passed since the last frame</param>
        public virtual void UpdateThis(GameTime t)
        {
            /* This is meant to be overriden */
        }

        /// <summary>
        /// sets the default values for the layer
        /// </summary>
        /// <param name="collisionManager">the collision manager for the layer</param>
        /// <param name="drawManager">the draw manager for the layer</param>
        /// <param name="worldObjects">a list of world objects to add to the layer</param>
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

        /// <summary>
        /// handles the firing of WorldObjectAdded
        /// </summary>
        /// <param name="worldObject">The WorldObject that was added</param>
        private void OnWorldObjectAdded(WorldObject worldObject)
        {
            if (WorldObjectAdded != null) WorldObjectAdded(this, new WorldObjectEventArgs() { WorldObject = worldObject });

            // set the layer
            worldObject.Layer = this;

            // hook up events
            worldObject.UpdateOrderChanged += worldObject_UpdateOrderChanged;

            // refresh the update order
            SortUpdateOrder();

            // add the worldObject to the Managers
            SubscribeToManagers(worldObject);
        }

        /// <summary>
        /// handles the firing of WorldObjectRemoved
        /// </summary>
        /// <param name="worldObject">The world object that was removed</param>
        private void OnWorldObjectRemoved(WorldObject worldObject)
        {
            if (WorldObjectRemoved != null) WorldObjectRemoved(this, new WorldObjectEventArgs() { WorldObject = worldObject });

            // remove the worldObject from the Managers
            UnsubscribeFromManagers(worldObject);
        }

        /// <summary>
        /// sorts the update order of the world objects in the layer
        /// </summary>
        private void SortUpdateOrder()
        {
            WorldObjects.OrderBy(x => x.UpdateOrder);
        }

        /// <summary>
        /// subscribes a world obejct to the managers based on its available types
        /// </summary>
        /// <param name="worldObject">the world object</param>
        private void SubscribeToManagers(WorldObject worldObject)
        {
            // subscribe this object
            if (worldObject is ICollisionManageable)
                CollisionManager.Add(worldObject as ICollisionManageable);
            else if (worldObject is IDrawManageable)
                DrawManager.AddDrawable(worldObject as IDrawManageable);

            // subscribe components recursively
            foreach (WorldObject wo in worldObject.GetComponents())
            {
                SubscribeToManagers(wo);
            }
        }

        /// <summary>
        /// subscribes a world obejct to the managers based on its available types
        /// </summary>
        /// <param name="worldObject">the world object</param>
        private void UnsubscribeFromManagers(WorldObject worldObject)
        {
            // unsubscribe this object
            if (worldObject is ICollisionManageable)
            {
                ICollisionManageable coll = worldObject as ICollisionManageable;
                if (CollisionManager.Contains(coll))
                    CollisionManager.Remove(coll);
            }
            else if (worldObject is IDrawManageable)
            {
                IDrawManageable draw = worldObject as IDrawManageable;
                if (DrawManager.Contains(draw))
                    DrawManager.Remove(draw);
            }

            // unsubscribe the components recursively
            foreach (WorldObject wo in worldObject.GetComponents())
            {
                UnsubscribeFromManagers(wo);
            }
        }

        /// <summary>
        /// called whenever one of the world objects have an update order that has changed
        /// </summary>
        /// <param name="sender">the sender of the event</param>
        /// <param name="e">the event args</param>
        private void worldObject_UpdateOrderChanged(object sender, EventArgs e)
        {
            SortUpdateOrder();
        }
    }
}