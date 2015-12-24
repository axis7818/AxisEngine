using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace AxisEngine.Visuals
{
    public class Sprite : WorldObject, IDrawManageable
    {
        private Vector2 _offset;
        private bool _visible;
        private int _drawOrder;

        public Sprite(Texture2D texture)
        {
            Texture = texture;
            DrawOrder = 0;
            Color = Color.White;
            Origin = Vector2.Zero;
            SpriteEffect = SpriteEffects.None;
            SourceRectangle = null;
            DestinationRectangle = null;
            Visible = true;

            _offset = Vector2.Zero;
        }

        public event EventHandler<EventArgs> VisibleChanged;
        public event EventHandler<EventArgs> DrawOrderChanged;
        
        public new Vector2 Scale
        {
            get { return base.Scale; }
            set
            {
                base.Scale = value;
                _offset *= value;
            }
        }

        public Rectangle? SourceRectangle { get; set; }

        public Rectangle? DestinationRectangle { get; set; }

        public float LayerDepth
        {
            get { return 0; }
        }

        public Vector2 DrawPosition
        {
            get { return Position + _offset; }
        }

        public int DrawOrder
        {
            get { return _drawOrder; }
            set
            {
                _drawOrder = value;
                if (DrawOrderChanged != null)
                    DrawOrderChanged(this, EventArgs.Empty);
            }
        }

        public Color Color { get; set; }

        public Vector2 Origin { get; set; }

        public SpriteEffects SpriteEffect { get; set; }

        public Texture2D Texture { get; set; }

        public Rectangle DrawArea
        {
            get
            {
                Rectangle bounds = Texture.Bounds;
                return new Rectangle((int)Position.X, (int)Position.Y,
                    bounds.Width, bounds.Height);
            }
        }

        public bool Visible
        {
            get { return _visible; }
            set
            {
                _visible = value;
                if (VisibleChanged != null)
                    VisibleChanged(this, EventArgs.Empty);
            }
        }

        public void Offset(Vector2 amount)
        {
            _offset = amount;
        }

        public void Center()
        {
            _offset = new Vector2(-Texture.Width * Scale.X * 0.5f, -Texture.Height * Scale.Y * 0.5f);
        }

        public void Trim(int margin)
        {
            SourceRectangle = new Rectangle(margin, margin, Texture.Width - 2 * margin, Texture.Height - 2 * margin);
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            //TODO: test the camera offsets
            spriteBatch.Draw(Texture,
                             DrawPosition - camera.Position,
                             DestinationRectangle,
                             SourceRectangle,
                             Origin,
                             Rotation,
                             Scale,
                             Color,
                             SpriteEffect,
                             LayerDepth);
        }

        protected override void UpdateThis(GameTime t)
        {
            
        }
    }
}