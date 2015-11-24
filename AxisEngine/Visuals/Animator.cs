using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AxisEngine.Visuals
{
    /// <summary>
    /// Provides a way to display an animation
    /// </summary>
    public class Animator : WorldObject, IDrawManageable
    {
        /// <summary>
        /// the animations that the animator has access to
        /// </summary>
        private List<Animation> _animations;

        /// <summary>
        /// The index of the current animation
        /// </summary>
        private int _currentAnimation;

        /// <summary>
        /// an offset for the animation if it isn't being displayed properly
        /// </summary>
        private Vector2 _offset;

        /// <summary>
        /// creates a new animator object
        /// </summary>
        public Animator()
        {
            _animations = new List<Animation>();
            _currentAnimation = 0;
            _offset = Vector2.Zero;

            DrawOrder = 0;
            Color = Color.White;
            Origin = Vector2.Zero;
            Rotation = 0;
            SpriteEffect = SpriteEffects.None;
            LayerDepth = 0;
            DestinationRectangle = null;
        }

        /// <summary>
        /// the currently selected animation
        /// </summary>
        public Animation CurrentAnimation
        {
            get
            {
                return _animations[_currentAnimation];
            }
        }

        /// <summary>
        /// the rectangle to draw the animation to
        /// </summary>
        public Rectangle? DestinationRectangle { get; set; }

        /// <summary>
        /// the 'frame' rectangle for the current frame of the current animation
        /// </summary>
        public Rectangle? SourceRectangle
        {
            get
            {
                return CurrentAnimation.SourceRectangle;
            }
        }

        /// <summary>
        /// ???
        /// </summary>
        public float LayerDepth { get; set; }

        /// <summary>
        /// the current animation's currenet frame
        /// </summary>
        public Texture2D Texture
        {
            get
            {
                return CurrentAnimation.Texture;
            }
        }

        /// <summary>
        /// the effect to draw the sprite with
        /// </summary>
        public SpriteEffects SpriteEffect { get; set; }

        /// <summary>
        /// the position on the screen to draw the animation
        /// </summary>
        public Vector2 DrawPosition
        {
            get
            {
                return Position + _offset;
            }
        }

        /// <summary>
        /// the order in the drawing layer
        /// </summary>
        public int DrawOrder { get; set; }

        /// <summary>
        /// the color tint of the sprite
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// the origin of rotation for the animator
        /// </summary>
        public Vector2 Origin { get; set; }
    }
}
