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
        /// the origin of rotation
        /// </summary>
        Vector2 Origin { get; }

        /// <summary>
        /// the position to draw at
        /// </summary>
        Vector2 DrawPosition { get; }

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

        /// <summary>
        /// ????
        /// </summary>
        float LayerDepth { get; }

        /// <summary>
        /// The source rectangle to use from the texture
        /// </summary>
        Rectangle? SourceRectangle { get; }

        /// <summary>
        /// The destination rectangle to be drawn to
        /// </summary>
        Rectangle? DestinationRectangle { get; }
    }
}