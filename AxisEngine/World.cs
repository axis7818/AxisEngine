using AxisEngine.Physics;
using AxisEngine.Visuals;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AxisEngine
{
    /// <summary>
    /// A scene within the game
    /// </summary>
    public class World : IUpdateable, IDrawable
    {
        /// <summary>
        /// The collision managers that belong to this world
        /// </summary>
        protected Dictionary<string, CollisionManager> CollisionManagers;

        /// <summary>
        /// The Content loaded that loads content into the world
        /// </summary>
        protected ContentManager Content;

        /// <summary>
        /// The draw managers that belong to this world
        /// </summary>
        protected Dictionary<string, DrawManager> DrawManagers;

        /// <summary>
        /// The time managers that belong to this world
        /// </summary>
        protected Dictionary<string, TimeManager> TimeManagers;

        /// <summary>
        /// The graphics device manager for this world
        /// </summary>
        protected GraphicsDeviceManager Graphics;

        /// <summary>
        /// The graphics device for this world
        /// </summary>
        protected GraphicsDevice GraphicsDevice;

        /// <summary>
        /// The layers that belong to this world
        /// </summary>
        protected List<Layer> Layers;

        /// <summary>
        /// the order in which this world is drawn
        /// </summary>
        private int _drawOrder;

        /// <summary>
        /// whether or not the world is enabled
        /// </summary>
        private bool _enabled;

        /// <summary>
        /// the update order of the world
        /// </summary>
        private int _updateOrder;

        /// <summary>
        /// whether or not the world is visible
        /// </summary>
        private bool _visible;

        /// <summary>
        /// initializes a new World
        /// </summary>
        /// <param name="graphics">the graphics manager</param>
        /// <param name="graphicsDevice">the graphics device</param>
        /// <param name="content">the content loader</param>
        /// <param name="layers">layers to add to the world</param>
        public World(GraphicsDeviceManager graphics, GraphicsDevice graphicsDevice, ContentManager content, params Layer[] layers)
        {
            Initialize(graphics, graphicsDevice, content, layers);
        }

        /// <summary>
        /// fired when the DrawOrder property is changed
        /// </summary>
        public event EventHandler<EventArgs> DrawOrderChanged;

        /// <summary>
        /// fired when the Enabled property is changed
        /// </summary>
        public event EventHandler<EventArgs> EnabledChanged;

        /// <summary>
        /// fired when a layer is added to the world
        /// </summary>
        public event EventHandler<LayerEventArgs> LayerAdded;

        /// <summary>
        /// fired when a layer is removed from the world
        /// </summary>
        public event EventHandler<LayerEventArgs> LayerRemoved;

        /// <summary>
        /// fired when the UpdateOrder property is changed
        /// </summary>
        public event EventHandler<EventArgs> UpdateOrderChanged;

        /// <summary>
        /// fired when the Visible property is changed
        /// </summary>
        public event EventHandler<EventArgs> VisibleChanged;

        /// <summary>
        /// the order in which this world is drawn
        /// </summary>
        public int DrawOrder
        {
            get { return _drawOrder; }
            set
            {
                _drawOrder = value;
                if (DrawOrderChanged != null) DrawOrderChanged(this, new EventArgs());
            }
        }

        /// <summary>
        /// whether or not the world is enabled
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
        /// the update order of the world
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
        /// whether or not the world is visible
        /// </summary>
        public bool Visible
        {
            get { return _visible; }
            set
            {
                _visible = value;
                if (VisibleChanged != null) VisibleChanged(this, new EventArgs());
            }
        }

        /// <summary>
        /// Adds a layer to the world
        /// </summary>
        /// <param name="layer">the layer to add</param>
        public void AddLayer(Layer layer)
        {
            if (!Layers.Contains(layer))
            {
                Layers.Add(layer);
                OnLayerAdded(layer);
            }
        }

        /// <summary>
        /// adds several layers to the world
        /// </summary>
        /// <param name="layers">the layers to add</param>
        public void AddLayers(IEnumerable<Layer> layers)
        {
            foreach (Layer layer in layers)
                AddLayer(layer);
        }

        /// <summary>
        /// draws the world
        /// </summary>
        /// <param name="t">the time since the last draw</param>
        public virtual void Draw(GameTime t)
        {
            if (Visible)
            {
                foreach (DrawManager drawer in DrawManagers.Values)
                    drawer.Draw(t);
            }
        }

        /// <summary>
        /// removes all layers that match a specified condition
        /// </summary>
        /// <param name="match">the condition to match</param>
        public void RemoveAllLayersWhere(Predicate<Layer> match)
        {
            foreach (Layer layer in Layers.Where(x => match(x)))
                RemoveLayer(layer);
        }

        /// <summary>
        /// removes a layer from the world
        /// </summary>
        /// <param name="layer">the layer to remove</param>
        public void RemoveLayer(Layer layer)
        {
            if (Layers.Contains(layer))
            {
                Layers.Remove(layer);
                OnLayerRemoved(layer);
            }
        }

        /// <summary>
        /// removes a layer at a given index
        /// </summary>
        /// <param name="index">the index to remove the layer from</param>
        public void RemoveLayerAt(int index)
        {
            if (index >= 0 && index < Layers.Count)
            {
                Layer layer = Layers[index];
                Layers.RemoveAt(index);
                OnLayerRemoved(layer);
            }
            else throw new IndexOutOfRangeException();
        }

        /// <summary>
        /// removes layers within a given index range
        /// </summary>
        /// <param name="index">the index to start at</param>
        /// <param name="count">the amount of consecutive layers to remove</param>
        public void RemoveLayersInRange(int index, int count)
        {
            for (int i = 0; i < count; i++)
                RemoveLayerAt(index + i);
        }

        /// <summary>
        /// sorts the layers by update order
        /// </summary>
        public void SortUpdateOrder()
        {
            Layers.OrderBy(x => x.UpdateOrder);
        }

        /// <summary>
        /// updates the world
        /// </summary>
        /// <param name="t">the time since the last update</param>
        public virtual void Update(GameTime t)
        {
            if (Enabled)
            {
                foreach (Layer layer in Layers)
                    layer.Update(t);

                foreach (CollisionManager collisionManager in CollisionManagers.Values)
                    collisionManager.Update(t);
            }
        }

        /// <summary>
        /// initializes the variables and parameters
        /// </summary>
        /// <param name="graphics">the graphics manager</param>
        /// <param name="graphicsDevice">the graphics device</param>
        /// <param name="content">the content loader</param>
        /// <param name="layers">layers to add to the world</param>
        private void Initialize(GraphicsDeviceManager graphics, GraphicsDevice graphicsDevice, ContentManager content, params Layer[] layers)
        {
            Layers = new List<Layer>();
            CollisionManagers = new Dictionary<string, CollisionManager>();
            DrawManagers = new Dictionary<string, DrawManager>();
            TimeManagers = new Dictionary<string, TimeManager>();

            Graphics = graphics;
            GraphicsDevice = graphicsDevice;
            Content = content;

            AddLayers(layers);
            SortUpdateOrder();

            Enabled = true;
            UpdateOrder = 0;

            Visible = true;
            DrawOrder = 0;
        }

        /// <summary>
        /// handles when one of the layers have an update order that has changed
        /// </summary>
        /// <param name="sender">the sender of the event</param>
        /// <param name="e">the arguments that go along with the event</param>
        private void layer_UpdateOrderChanged(object sender, EventArgs e)
        {
            SortUpdateOrder();
        }

        /// <summary>
        /// handles the adding of a layer to the world
        /// </summary>
        /// <param name="layer">the layer that was added</param>
        private void OnLayerAdded(Layer layer)
        {
            if (LayerAdded != null) LayerAdded(this, new LayerEventArgs() { Layer = layer });

            // hook up events
            layer.UpdateOrderChanged += layer_UpdateOrderChanged;

            // refresh the update order
            SortUpdateOrder();
        }

        /// <summary>
        /// handles the removing of a layer
        /// </summary>
        /// <param name="layer">the layer that was removed</param>
        private void OnLayerRemoved(Layer layer)
        {
            if (LayerRemoved != null) LayerRemoved(this, new LayerEventArgs() { Layer = layer });

            try
            {
                layer.UpdateOrderChanged -= layer_UpdateOrderChanged;
            }
            catch (Exception) { }
        }
    }
}