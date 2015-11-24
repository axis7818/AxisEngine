using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace AxisEngine.Visuals
{
    public class Animator : WorldObject, IDrawManageable
    {
        private List<Animation> _animations;
        private int _currentAnimation;
        private Vector2 _offset;

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

        public Animation CurrentAnimation
        {
            get { return _animations[_currentAnimation]; }
        }

        public Rectangle? DestinationRectangle { get; set; }

        public Rectangle? SourceRectangle
        {
            get { return CurrentAnimation.SourceRectangle; }
        }

        public float LayerDepth { get; set; }

        public Texture2D Texture
        {
            get { return CurrentAnimation.Texture; }
        }

        public SpriteEffects SpriteEffect { get; set; }

        public Vector2 DrawPosition
        {
            get { return Position + _offset; }
        }

        public int DrawOrder { get; set; }

        public Color Color { get; set; }

        public Vector2 Origin { get; set; }
    }
}