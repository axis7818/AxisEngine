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

        private SpriteBatch SpriteBatch;

        private List<IDrawManageable> ThingsToDraw;

        public DrawManager(GraphicsDevice graphicsDevice)
        {
            // initialize some members
            ThingsToDraw = new List<IDrawManageable>();
            GraphicsDevice = graphicsDevice;
            SpriteBatch = new SpriteBatch(graphicsDevice);

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

        public Vector2 ScreenCenter
        {
            get
            {
                return new Vector2(GraphicsDevice.Viewport.Width * 0.5f, GraphicsDevice.Viewport.Height * 0.5f);
            }
        }

        public Vector2 ScreenSize
        {
            get
            {
                return new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
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

        public int Size
        {
            get
            {
                return ThingsToDraw.Count;
            }
        }

        public void AddDrawable(IDrawManageable toDraw)
        {
            ThingsToDraw.Add(toDraw);
            SortByUpdateOrder();
        }

        public bool Contains(IDrawManageable test)
        {
            return ThingsToDraw.Contains(test);
        }

        public virtual void Draw(GameTime t)
        {
            if (Visible)
            {
                SpriteBatch.Begin();

                foreach (IDrawManageable toDraw in ThingsToDraw)
                    SpriteBatch.Draw(toDraw.Texture,
                                     toDraw.DrawPosition,
                                     toDraw.DestinationRectangle,
                                     toDraw.SourceRectangle,
                                     toDraw.Origin,
                                     toDraw.Rotation,
                                     toDraw.Scale,
                                     toDraw.Color,
                                     toDraw.SpriteEffect,
                                     toDraw.LayerDepth);

                SpriteBatch.End();
            }
        }

        public void Remove(IDrawManageable toRemove)
        {
            ThingsToDraw.Remove(toRemove);
        }

        private void SortByUpdateOrder()
        {
            ThingsToDraw = ThingsToDraw.OrderBy(x => x.DrawOrder).ToList();
        }

        public IEnumerator GetEnumerator()
        {
            foreach (IDrawManageable dr in ThingsToDraw)
            {
                yield return dr;
            }
        }
    }
}