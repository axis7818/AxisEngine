using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AxisEngine.Visuals
{
    /// <summary>
    /// A world object that represents a single sprite
    /// </summary>
    public class Sprite : WorldObject, IDrawManageable
    {
        /// <summary>
        /// an offset for the sprite
        /// </summary>
        private Vector2 _offset;

        /// <summary>
        /// Instantiates a sprite
        /// </summary>
        /// <param name="texture">The texture that is displayed</param>
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

        /// <summary>
        /// the ractangle from the sprite to use
        /// </summary>
        public Rectangle? SourceRectangle { get; set; }

        /// <summary>
        /// The rectangle to draw to
        /// </summary>
        public Rectangle? DestinationRectangle { get; set; }

        /// <summary>
        /// ???
        /// </summary>
        public float LayerDepth
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// The position to draw the sprite at
        /// </summary>
        public Vector2 DrawPosition
        {
            get
            {
                return Position + _offset;
            }
        }

        /// <summary>
        /// The order to draw the sprite
        /// </summary>
        public int DrawOrder { get; set; }

        /// <summary>
        /// The color tint applied to the texture
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// The orgin of the sprite with a default of (0, 0)
        /// </summary>
        public Vector2 Origin { get; set; }

        /// <summary>
        /// The effect to apply to the sprite
        /// </summary>
        public SpriteEffects SpriteEffect { get; set; }

        /// <summary>
        /// The texuture that is displayed for the sprite
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// offsets the sprite drawing if using the upper-left corner isn't where the spirte should be
        /// </summary>
        /// <param name="offset"></param>
        public void Offset(Vector2 offset)
        {
            _offset = offset;
        }

        /// <summary>
        /// centers the sprite based on the texture's dimensions
        /// </summary>
        public void Center()
        {
            _offset = new Vector2(-Texture.Width * 0.5f, -Texture.Height * 0.5f);
        }

        /// <summary>
        /// remove a margin around the texture
        /// </summary>
        /// <param name="margin"></param>
        public void Trim(int margin)
        {
            SourceRectangle = new Rectangle(margin, margin, Texture.Width - 2 * margin, Texture.Height - 2 * margin);
        }
    }
}