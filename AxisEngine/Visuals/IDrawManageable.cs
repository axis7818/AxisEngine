using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AxisEngine.Visuals
{
    /// <summary>
    /// an interface that allows an object to be drawn by a DrawManager
    /// </summary>
    public interface IDrawManageable
    {
        /// <summary>
        /// The order to draw
        /// </summary>
        int DrawOrder { get; }

        /// <summary>
        /// the color tint to apply to the texture
        /// </summary>
        Color Color { get; }

        /// <summary>
        /// An offset position to render the texture at
        /// </summary>
        Vector2 Offset { get; }

        /// <summary>
        /// whether or not the texture should be centered
        /// </summary>
        Vector2 Orgin { get; }

        /// <summary>
        /// the position to draw at
        /// </summary>
        Vector2 Position { get; }

        /// <summary>
        /// The rotation of the texture (in radians)
        /// </summary>
        float Rotation { get; }

        /// <summary>
        /// The Scaling factors
        /// </summary>
        Vector2 Scale { get; }

        /// <summary>
        /// The effect to apply to the sprite
        /// </summary>
        SpriteEffects SpriteEffect { get; }

        /// <summary>
        /// the texture to draw
        /// </summary>
        Texture2D Texture { get; }
    }
}