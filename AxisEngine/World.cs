using AxisEngine.Physics;
using AxisEngine.Visuals;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AxisEngine
{
    public abstract class World : IUpdateable, IDrawable
    {
        public Color BackgroundColor = Color.CornflowerBlue;

        protected Dictionary<string, CollisionManager> CollisionManagers;
        protected Dictionary<string, DrawManager> DrawManagers;
        protected Dictionary<string, TimeManager> TimeManagers;
        protected GraphicsDeviceManager Graphics;
        protected GraphicsDevice GraphicsDevice;
        protected List<Layer> Layers;

        protected Dictionary<string, Camera> Cameras;
        protected string currentCamera = null;

        private string _name;
        private int _drawOrder;
        private bool _enabled;
        private int _updateOrder;
        private bool _visible;
        private bool _end = false;
        private bool _quit = false;
        private string _nextWorld = null;
        
        public World(string name, GraphicsDeviceManager graphics, GraphicsDevice graphicsDevice)
        {
            Graphics = graphics;
            GraphicsDevice = graphicsDevice;
            UpdateOrder = 0;
            DrawOrder = 0;
            _name = name;
        }
        
        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<LayerEventArgs> LayerAdded;
        public event EventHandler<LayerEventArgs> LayerRemoved;
        public event EventHandler<EventArgs> UpdateOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;
        public event EventHandler<WorldChangingEventArgs> EndWorld;

        public Camera ActiveCamera
        {
            get { return Cameras[currentCamera]; }
        }

        public string Name
        {
            get { return _name; }
        }

        public int DrawOrder
        {
            get { return _drawOrder; }
            set
            {
                _drawOrder = value;
                if (DrawOrderChanged != null) DrawOrderChanged(this, new EventArgs());
            }
        }

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

        public bool Visible
        {
            get { return _visible; }
            set
            {
                _visible = value;
                if (VisibleChanged != null) VisibleChanged(this, new EventArgs());
            }
        }

        protected abstract void SetUpManagers(GraphicsDevice graphicsDevice);
        protected abstract void Load();
        protected abstract void Unload();
        
        public void AddCamera(Camera newCamera)
        {
            if (Cameras.ContainsKey(newCamera.Name))
                throw new ArgumentException("This camera has already been added to the world");

            Cameras[newCamera.Name] = newCamera;
        }

        public void SetCamera(string name)
        {
            if (!Cameras.ContainsKey(name))
                throw new ArgumentException("The camera, " + name + ", could not be found.");

            currentCamera = name;
        }

        public string[] GetCameras()
        {
            return Cameras.Keys.ToArray();
        }

        public void RemoveCamera(string name)
        {
            Cameras.Remove(name);

            // handle the case where the current camera has been removed
            if (currentCamera.Equals(name))
            {
                string[] cameras = GetCameras();
                if (cameras.Length <= 0)
                    throw new InvalidComponentsException("The world has no cameras!");
                currentCamera = cameras[0];
            }
        }

        public void Initialize()
        {
            // create the manager objects and layers object
            Layers = new List<Layer>();
            CollisionManagers = new Dictionary<string, CollisionManager>();
            DrawManagers = new Dictionary<string, DrawManager>();
            TimeManagers = new Dictionary<string, TimeManager>();

            // set up the cameras
            Cameras = new Dictionary<string, Camera>();
            Camera defaultCamera = new Camera(Camera.DEFAULT_NAME);
            AddCamera(defaultCamera);
            currentCamera = defaultCamera.Name;

            // turn updating and drawing on
            Enabled = true;
            Visible = true;

            // we dont want to end if we are initializing
            _end = false;
            _nextWorld = null;

            // do any custom loading
            SetUpManagers(GraphicsDevice);
            Load();
        }

        public void Dispose()
        { 
            // remove managers and layers
            Layers = null;
            CollisionManagers = null;
            DrawManagers = null;
            TimeManagers = null;

            // remove camera stuff
            Cameras = null;
            currentCamera = null;

            // turn off updating and drawing
            Enabled = false;
            Visible = false;

            // do any custom unloading
            Unload();
        }

        public void AddLayer(Layer layer)
        {
            if (!Layers.Contains(layer))
            {
                Layers.Add(layer);
                OnLayerAdded(layer);
            }
        }

        public void AddLayers(IEnumerable<Layer> layers)
        {
            foreach (Layer layer in layers)
                AddLayer(layer);
        }

        public void Draw(GameTime t)
        {
            if (Visible)
            {
                foreach (DrawManager drawer in DrawManagers.Values)
                    drawer.Draw(ActiveCamera);
            }
        }

        public void RemoveAllLayersWhere(Predicate<Layer> match)
        {
            foreach (Layer layer in Layers.Where(x => match(x)))
                RemoveLayer(layer);
        }

        public void RemoveLayer(Layer layer)
        {
            if (Layers.Contains(layer))
            {
                Layers.Remove(layer);
                OnLayerRemoved(layer);
            }
        }

        public void RemoveLayerAt(int index)
        {
            if (index >= 0 && index < Layers.Count)
            {
                Layer layer = Layers[index];
                Layers.RemoveAt(index);
                OnLayerRemoved(layer);
            }
            else
                throw new IndexOutOfRangeException();
        }

        public void RemoveLayersInRange(int index, int count)
        {
            for (int i = 0; i < count; i++)
                RemoveLayerAt(index + i);
        }

        public void SortUpdateOrder()
        {
            Layers.OrderBy(x => x.UpdateOrder);
        }

        public virtual void Update(GameTime t)
        {
            if (Enabled)
            {
                foreach (Layer layer in Layers)
                    layer.Update(t);

                foreach (CollisionManager collisionManager in CollisionManagers.Values)
                    collisionManager.Update(t);
            }

            if (_end && EndWorld != null)
            {
                EndWorld(this, new WorldChangingEventArgs(Name, _nextWorld));
            }
            if (_quit)
            {
                EndWorld(this, WorldChangingEventArgs.QuittingArgs);
            }
        }

        private void layer_UpdateOrderChanged(object sender, EventArgs e)
        {
            SortUpdateOrder();
        }

        private void OnLayerAdded(Layer layer)
        {
            if (LayerAdded != null) LayerAdded(this, new LayerEventArgs(layer));

            // hook up events
            layer.UpdateOrderChanged += layer_UpdateOrderChanged;

            // refresh the update order
            SortUpdateOrder();
        }

        private void OnLayerRemoved(Layer layer)
        {
            if (LayerRemoved != null) LayerRemoved(this, new LayerEventArgs(layer));

            try
            {
                layer.UpdateOrderChanged -= layer_UpdateOrderChanged;
            }
            catch (Exception) { }
        }

        protected void End(string nextWorld)
        {
            _end = true;
            _nextWorld = nextWorld;
        }

        protected void Quit()
        {
            _quit = true;
        }
    }
}