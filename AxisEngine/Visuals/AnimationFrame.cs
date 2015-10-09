using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AxisEngine.Visuals
{
    /// <summary>
    /// describes the data for an animator to hold about how to render a frame of a specific animation
    /// </summary>
    public class AnimationFrame
    {
        /// <summary>
        /// the color tint to apply to this frame
        /// </summary>
        public Color Color;

        /// <summary>
        /// the index of the texture to render from the animator
        /// </summary>
        public int Index;

        /// <summary>
        /// The offset to render the texture at
        /// </summary>
        public Vector2 Offset;

        /// <summary>
        /// the rotation of the frame (in radians)
        /// </summary>
        public float Rotation;

        /// <summary>
        /// a scaling factor
        /// </summary>
        public Vector2 Scale;

        /// <summary>
        /// The effect to apply to the sprite
        /// </summary>
        public SpriteEffects SpriteEffect;

        /// <summary>
        /// initialize an AnimationFrame
        /// </summary>
        /// <param name="index">the index of the animation's texture</param>
        public AnimationFrame(int index)
        {
            Initialize(index, Color.White, new Vector2(1, 1), Vector2.Zero, 0, SpriteEffects.None);
        }

        /// <summary>
        /// Sets all of the properties
        /// </summary>
        /// <param name="index">the index of the animation's texture</param>
        /// <param name="color">the color tint to apply to the texture [default to white]</param>
        /// <param name="scale">a scaling factor to apply [default to Vector2(1, 1)]</param>
        /// <param name="rotation">the rotation of the texture (in radians)</param>
        /// <param name="spriteEffect">the effect to apply to the sprite</param>
        public AnimationFrame(int index = 0, Color? color = null, Vector2? scale = null, Vector2? offset = null, float rotation = 0, SpriteEffects spriteEffect = SpriteEffects.None)
        {
            Initialize(0, color ?? Color.White, scale ?? new Vector2(1, 1), offset ?? Vector2.Zero, rotation, spriteEffect);
        }

        /// <summary>
        /// Sets all of the properties
        /// </summary>
        /// <param name="index">the index of the animation's texture</param>
        /// <param name="color">the color tint to apply to the texture</param>
        /// <param name="scale">a scaling factor to apply</param>
        /// <param name="rotation">the rotation of the texture (in radians)</param>
        private void Initialize(int index, Color color, Vector2 scale, Vector2 offset, float rotation, SpriteEffects effect)
        {
            Index = index;
            Color = color;
            Scale = scale;
            Offset = offset;
            Rotation = rotation;
            SpriteEffect = effect;
        }
    }
}