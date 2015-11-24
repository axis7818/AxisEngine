using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AxisEngine.Visuals
{
    /// <summary>
    /// Manages the rendering of IDrawManageable objects
    /// </summary>
    public class DrawManager : IDrawable, IEnumerable
    {
        /// <summary>
        /// the order in which the draw manager is drawn
        /// </summary>
        private int _drawOrder;

        /// <summary>
        /// whether or not the draw manager is drawing to the screen
        /// </summary>
        private bool _visible;

        /// <summary>
        /// the sprite batch that will draw all of the objects
        /// </summary>
        private SpriteBatch SpriteBatch;

        /// <summary>
        /// The list of things that need to be drawn in the draw manager
        /// </summary>
        private List<IDrawManageable> ThingsToDraw;

        /// <summary>
        /// instantiates the DrawManager
        /// </summary>
        /// <param name="graphicsDevice">the Graphics device to draw to</param>
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

        /// <summary>
        /// fired when the DrawOrder property is changed
        /// </summary>
        public event EventHandler<EventArgs> DrawOrderChanged;

        /// <summary>
        /// fired when the visible property is changed
        /// </summary>
        public event EventHandler<EventArgs> VisibleChanged;

        /// <summary>
        /// the order in which the draw manager is drawn
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
        /// The graphics device that this is drawing to
        /// </summary>
        public GraphicsDevice GraphicsDevice { get; private set; }

        /// <summary>
        /// gets the center of the screen in pxl coords
        /// </summary>
        public Vector2 ScreenCenter
        {
            get
            {
                return new Vector2(GraphicsDevice.Viewport.Width * 0.5f, GraphicsDevice.Viewport.Height * 0.5f);
            }
        }

        /// <summary>
        /// gets the size of the screen in pxl coords
        /// </summary>
        public Vector2 ScreenSize
        {
            get
            {
                return new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            }
        }

        /// <summary>
        /// whether or not the draw manager is drawing to the screen
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
        /// gets the number of items being managed
        /// </summary>
        public int Size
        {
            get
            {
                return ThingsToDraw.Count;
            }
        }

        /// <summary>
        /// adds an IDrawManageable to the Manager
        /// </summary>
        /// <param name="toDraw">the IDrawManageable to add</param>
        public void AddDrawable(IDrawManageable toDraw)
        {
            ThingsToDraw.Add(toDraw);
            SortByUpdateOrder();
        }

        /// <summary>
        /// Checks to see if the manager contains the IDrawManageable
        /// </summary>
        /// <param name="test">the IDrawManageable to test</param>
        public bool Contains(IDrawManageable test)
        {
            return ThingsToDraw.Contains(test);
        }

        /// <summary>
        /// draws all of the IDrawManageables
        /// </summary>
        /// <param name="t">the time elapsed since the last draw</param>
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

        /// <summary>
        /// Removes an IDrawManageable from the manager
        /// </summary>
        /// <param name="toRemove">the IDrawManageable to remove</param>
        public void Remove(IDrawManageable toRemove)
        {
            ThingsToDraw.Remove(toRemove);
        }

        /// <summary>
        /// Sorts the ThingsToDraw list by DrawOrder
        /// </summary>
        private void SortByUpdateOrder()
        {
            ThingsToDraw = ThingsToDraw.OrderBy(x => x.DrawOrder).ToList();
        }

        /// <summary>
        /// enumerates the draw manager for access to the drawmanageables
        /// </summary>
        public IEnumerator GetEnumerator()
        {
            foreach (IDrawManageable dr in ThingsToDraw)
            {
                yield return dr;
            }
        }
    }
}