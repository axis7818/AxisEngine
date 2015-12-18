using AxisEngine.Physics;
using AxisEngine.AxisDebug;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AxisEngine.Visuals
{
    public class DrawManager : IDrawable, IEnumerable
    {
        private int _drawOrder; 
        private bool _visible; 
        private SpriteBatch _spriteBatch;
        private List<IDrawManageable> _thingsToDraw;

        public DrawManager(GraphicsDevice graphicsDevice)
        {
            // initialize some members
            _thingsToDraw = new List<IDrawManageable>();
            GraphicsDevice = graphicsDevice;
            _spriteBatch = new SpriteBatch(graphicsDevice);

            // set some values
            Visible = true;
            DrawOrder = 0;
        }

        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;

        public int DrawOrder
        {
            get { return _drawOrder; }
            set
            {
                _drawOrder = value;
                if (DrawOrderChanged != null) DrawOrderChanged(this, new EventArgs());
            }
        }

        public GraphicsDevice GraphicsDevice { get; private set; }

        public Point ScreenCenter
        {
            get { return new Point(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2); }
        }

        public Point ScreenSize
        {
            get { return new Point(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height); }
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

        public int Size
        {
            get { return _thingsToDraw.Count; }
        }

        public void AddDrawable(IDrawManageable toDraw)
        {
            _thingsToDraw.Add(toDraw);
            toDraw.DrawOrderChanged += DrawOrderChanged;
            SortByUpdateOrder();
        }

        public bool Contains(IDrawManageable test)
        {
            return _thingsToDraw.Contains(test);
        }
        
        public virtual void Draw(GameTime t)
        {
            if (Visible)
            {
                _spriteBatch.Begin();

                foreach(IDrawManageable toDraw in _thingsToDraw)
                {
                    if (toDraw.Visible)
                    {
                        toDraw.Draw(_spriteBatch);
                    }
                }

                _spriteBatch.End();
            }
        }

        public void DrawWireFrames(CollisionManager collisionManager)
        {
            _spriteBatch.Begin();
            foreach(ICollidable coll in collisionManager)
            {
                if(coll.WireFrame != null)
                    _spriteBatch.Draw(coll.WireFrame, coll.Position, Color.White);
            }
            _spriteBatch.End();
        }

        public void Remove(IDrawManageable toRemove)
        {
            _thingsToDraw.Remove(toRemove);
            toRemove.DrawOrderChanged -= DrawOrderChangedHandler;
        }
        
        private void SortByUpdateOrder()
        {
            _thingsToDraw = _thingsToDraw.OrderBy(x => x.DrawOrder).ToList();
        }

        private void DrawOrderChangedHandler(object sender, EventArgs args)
        {
            SortByUpdateOrder();
        }

        public IEnumerator GetEnumerator()
        {
            foreach (IDrawManageable dr in _thingsToDraw)
            {
                yield return dr;
            }
        }
    }
}