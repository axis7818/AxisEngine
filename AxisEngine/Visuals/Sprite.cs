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
        /// Instantiates a sprite
        /// </summary>
        /// <param name="texture">The texture that is displayed</param>
        public Sprite(Texture2D texture)
        {
            Initialize(texture, Vector2.Zero, Color.White, SpriteEffects.None, Vector2.Zero, 0);
        }

        /// <summary>
        /// Instantiates a sprite
        /// </summary>
        /// <param name="texture">The texture that is displayed</param>
        /// <param name="orgin">The orgin of rotation</param>
        /// <param name="color">the color tint to apply</param>
        /// <param name="effect">The effect to apply to the sprite</param>
        /// <param name="offset">an offset to render the texture at</param>
        public Sprite(Texture2D texture, Vector2? orgin = null, Color? color = null, SpriteEffects spriteEffect = SpriteEffects.None, Vector2? offset = null, int drawOrder = 0)
        {
            Initialize(texture, orgin ?? Vector2.Zero, color ?? Color.White, spriteEffect, offset ?? Vector2.Zero, drawOrder);
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
        /// The offset to render the sprite at
        /// </summary>
        public Vector2 Offset { get; set; }

        /// <summary>
        /// The orgin of the sprite with a default of (0, 0)
        /// </summary>
        public Vector2 Orgin { get; set; }

        /// <summary>
        /// The effect to apply to the sprite
        /// </summary>
        public SpriteEffects SpriteEffect { get; set; }

        /// <summary>
        /// The texuture that is displayed for the sprite
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// centers the sprite based on the texture's dimensions
        /// </summary>
        public void Center()
        {
            Orgin = new Vector2(Texture.Width * 0.5f, Texture.Height * 0.5f);
        }

        /// <summary>
        /// sets the default values of the sprite
        /// </summary>
        /// <param name="texture">The texture that is displayed</param>
        /// <param name="orgin">The orgin of rotation</param>
        /// <param name="color">the color tint to apply</param>
        /// <param name="effect">The effect to apply to the sprite</param>
        /// <param name="offset">an offset to render the texture at</param>
        private void Initialize(Texture2D texture, Vector2 orgin, Color color, SpriteEffects effect, Vector2 offset, int drawOrder)
        {
            DrawOrder = drawOrder;
            Texture = texture;
            Color = color;
            Orgin = orgin;
            SpriteEffect = effect;
            Offset = offset;
        }
    }
}