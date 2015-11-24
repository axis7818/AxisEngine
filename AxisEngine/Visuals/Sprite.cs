using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AxisEngine.Visuals
{
    public class Sprite : WorldObject, IDrawManageable
    {
        private Vector2 _offset;

        public Sprite(Texture2D texture)
        {
            Texture = texture;
            DrawOrder = 0;
            Color = Color.White;
            Origin = Vector2.Zero;
            SpriteEffect = SpriteEffects.None;
            SourceRectangle = null;
            DestinationRectangle = null;

            _offset = Vector2.Zero;
        }

        public Rectangle? SourceRectangle { get; set; }

        public Rectangle? DestinationRectangle { get; set; }

        public float LayerDepth
        {
            get
            {
                return 0;
            }
        }

        public Vector2 DrawPosition
        {
            get
            {
                return Position + _offset;
            }
        }

        public int DrawOrder { get; set; }

        public Color Color { get; set; }

        public Vector2 Origin { get; set; }

        public SpriteEffects SpriteEffect { get; set; }

        public Texture2D Texture { get; set; }

        public void Offset(Vector2 offset)
        {
            _offset = offset;
        }

        public void Center()
        {
            _offset = new Vector2(-Texture.Width * 0.5f, -Texture.Height * 0.5f);
        }

        public void Trim(int margin)
        {
            SourceRectangle = new Rectangle(margin, margin, Texture.Width - 2 * margin, Texture.Height - 2 * margin);
        }
    }
}