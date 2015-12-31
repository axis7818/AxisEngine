using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace AxisEngine.Visuals
{
    public class Animator : WorldObject, IDrawManageable
    {
        public const string DEFAULT = "DEFAULT_ANIMATION";

        private Dictionary<string, Animation> _animations;
        private string _currentAnimation;
        private Vector2 _offset;
        private int _drawOrder;

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
            Visible = true;
        }

        public event EventHandler<EventArgs> DrawOrderChanged;
        
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

        public bool Visible { get; set; }

        public Animation CurrentAnimation
        {
            get { return _animations[_currentAnimation]; }
        }

        public int Width
        {
            get { return CurrentAnimation.Width; }
        }

        public int Height
        {
            get { return CurrentAnimation.Height; }
        }

        protected override void UpdateThis(GameTime t)
        {
            CurrentAnimation.Update(t);
        }

        public void RemoveAnimation(string name)
        {
            _animations.Remove(name);
        }

        public void AddAnimation(string name, Animation anim)
        {
            if (_animations.ContainsKey(name))
                throw new ArgumentException("animation [" + name + "] is already in the animator");

            _animations[name] = anim;
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            spriteBatch.Draw(Texture,
                             camera.WorldPointToViewportPoint(DrawPosition),
                             DestinationRectangle,
                             SourceRectangle,
                             Origin,
                             Rotation,
                             Scale * camera.Zoom,
                             Color,
                             SpriteEffect,
                             LayerDepth);
        }

        public void SetCurrentAnimation(string name)
        {
            if (!_animations.ContainsKey(name))
                throw new ArgumentException("cannot find the animation: " + name);

            if(name != _currentAnimation)
            {
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

        public bool IsViewableTo(Camera camera)
        {
            //TODO: implement
            return true;
        }
    }
}