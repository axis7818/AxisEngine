using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace AxisEngine.Visuals
{
    public class Animator : WorldObject, IDrawManageable
    {
        private const string DEFAULT = "DEFAULT_ANIMATION";

        private Dictionary<string, Animation> _animations;
        private string _currentAnimation;
        private Vector2 _offset;

        public Animator(Animation defaultAnimation)
        {
            _offset = Vector2.Zero;

            _animations = new Dictionary<string, Animation>();
            _currentAnimation = DEFAULT;
            _animations[DEFAULT] = defaultAnimation;
                        
            DrawOrder = 0;
            Color = Color.White;
            Origin = Vector2.Zero;
            Rotation = 0;
            SpriteEffect = SpriteEffects.None;
            LayerDepth = 0;
            DestinationRectangle = null;
        }

        #region IDRAWMANAGEABLE
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
        #endregion IDRAWMANAGEABLE

        public Animation CurrentAnimation
        {
            get { return _animations[_currentAnimation]; }
        }

        protected override void UpdateThis(GameTime t)
        {
            CurrentAnimation.Update(t);
        }

        public void SetCurrentAnimation(string name)
        {
            if (!_animations.ContainsKey(name))
                throw new ArgumentException("cannot find the animation: " + name);

            if (!CurrentAnimation.FinishBeforeTransition)
            {
                // Switch immediately to the new animation
                _currentAnimation = name;
                CurrentAnimation.Reset();                
            }
            else
            {
                // Set the current animation to wait for the end, upon finishing, switch animations
                CurrentAnimation.WaitForEnd();
                EventHandler<AnimationEventArgs> animSwitch = (sender, args) =>
                {
                    _currentAnimation = name;
                    CurrentAnimation.Reset();
                };
                CurrentAnimation.AnimationFinished += animSwitch;
            }
        }
    }
}