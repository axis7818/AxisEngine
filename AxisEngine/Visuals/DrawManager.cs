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
        private List<TextSprite> _textToDraw;

        public DrawManager(GraphicsDevice graphicsDevice)
        {
            // initialize some members
            _thingsToDraw = new List<IDrawManageable>();
            _textToDraw = new List<TextSprite>();
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
            get { return _thingsToDraw.Count + _textToDraw.Count; }
        }

        public void AddDrawable(IDrawManageable toDraw)
        {
            _thingsToDraw.Add(toDraw);
            SortByUpdateOrder();
        }

        public void AddTextSprite(TextSprite textSprite)
        {
            _textToDraw.Add(textSprite);
        }

        public bool Contains(IDrawManageable test)
        {
            return _thingsToDraw.Contains(test);
        }

        public bool Contains(TextSprite textSprite)
        {
            return _textToDraw.Contains(textSprite);
        }

        public virtual void Draw(GameTime t)
        {
            if (Visible)
            {
                _spriteBatch.Begin();

                foreach (IDrawManageable toDraw in _thingsToDraw)
                    if(toDraw.Visible)
                        _spriteBatch.Draw(toDraw.Texture,
                                         toDraw.DrawPosition,
                                         toDraw.DestinationRectangle,
                                         toDraw.SourceRectangle,
                                         toDraw.Origin,
                                         toDraw.Rotation,
                                         toDraw.Scale,
                                         toDraw.Color,
                                         toDraw.SpriteEffect,
                                         toDraw.LayerDepth);

                foreach (TextSprite text in _textToDraw)
                    if(text.Visible)
                        _spriteBatch.DrawString(text.SpriteFont, 
                                                text.Text, 
                                                text.Position, 
                                                text.Color);

                _spriteBatch.End();
            }
        }

        public void Remove(IDrawManageable toRemove)
        {
            _thingsToDraw.Remove(toRemove);
        }

        public void Remove(TextSprite toRemove)
        {
            _textToDraw.Remove(toRemove);
        }

        private void SortByUpdateOrder()
        {
            _thingsToDraw = _thingsToDraw.OrderBy(x => x.DrawOrder).ToList();
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