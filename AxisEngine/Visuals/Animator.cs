using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace AxisEngine.Visuals
{
    /// <summary>
    /// A world object that displays and animated sprite
    /// </summary>
    public class Animator : WorldObject, IDrawManageable
    {
        /// <summary>
        /// whether or not the animator centers the sprites
        /// </summary>
        public bool Center = false;

        /// <summary>
        /// the current animation that is playing
        /// </summary>
        private Animation _currentAnimation;

        /// <summary>
        /// sets the net orgin for all textures
        /// </summary>
        private Vector2 _orgin = new Vector2(0, 0);

        /// <summary>
        /// A list of animations to play
        /// </summary>
        private Dictionary<string, Animation> Animations;

        /// <summary>
        /// the set of textures to pull from
        /// </summary>
        private List<Texture2D> Textures;

        /// <summary>
        /// instantiates an Animator
        /// </summary>
        /// <param name="textures">The collection of textures that will be pulled from (by index)</param>
        public Animator(List<Texture2D> textures)
        {
            Textures = textures;
            Animations = new Dictionary<string, Animation>();
            DrawOrder = 0;
        }

        /// <summary>
        /// instantiates an Animator
        /// </summary>
        /// <param name="textures">The collection of textures that will be pulled from (by index)</param>
        /// <param name="drawOrder">The order in which to draw the animator</param>
        public Animator(List<Texture2D> textures, int drawOrder)
        {
            Textures = textures;
            Animations = new Dictionary<string, Animation>();
            DrawOrder = drawOrder;
        }

        /// <summary>
        /// The color to apply to the texture
        /// </summary>
        public Color Color
        {
            get
            {
                if (CurrentAnimation != null)
                {
                    return CurrentAnimation.Frame.Color;
                }
                else
                {
                    return Color.White;
                }
            }
        }

        /// <summary>
        /// The order in which to draw the animator
        /// </summary>
        public int DrawOrder { get; set; }

        /// <summary>
        /// The offset to render the texture at
        /// </summary>
        public Vector2 Offset
        {
            get
            {
                return CurrentAnimation.Frame.Offset;
            }
        }

        /// <summary>
        /// the orgin of the sprites to draw (set Center to true to center the sprites). Setting this value applies the offset to all textures
        /// </summary>
        public Vector2 Orgin
        {
            get
            {
                Vector2 orgin = _orgin;
                if (Center)
                {
                    orgin += new Vector2(Texture.Width * 0.5f, Texture.Height * 0.5f);
                }
                return orgin;
            }
            set
            {
                _orgin = value;
            }
        }

        /// <summary>
        /// gets the rotation of the frame to display. This combines the world object's rotation with the individual frame's rotation
        /// </summary>
        public new float Rotation
        {
            get
            {
                return base.Rotation + CurrentAnimation.Frame.Rotation;
            }
        }

        /// <summary>
        /// gets the scale of the frame to display. This combines the world object's scale with the individual frame's scale
        /// </summary>
        public new Vector2 Scale
        {
            get
            {
                return base.Scale * CurrentAnimation.Frame.Scale;
            }
        }

        /// <summary>
        /// gets the sprite effect to apply to the frame being displayed
        /// </summary>
        public SpriteEffects SpriteEffect
        {
            get
            {
                return CurrentAnimation.Frame.SpriteEffect;
            }
        }

        /// <summary>
        /// the texture to draw
        /// </summary>
        public Texture2D Texture
        {
            get
            {
                int index;
                if (CurrentAnimation != null)
                {
                    index = CurrentAnimation.Frame.Index;
                }
                else
                {
                    index = 0;
                }

                return Textures[index];
            }
        }

        /// <summary>
        /// the current animation that is playing
        /// </summary>
        public Animation CurrentAnimation
        {
            get
            {
                return _currentAnimation;
            }
            private set
            {
                bool reset = false;
                if (_currentAnimation != value)
                {
                    reset = true;
                }
                _currentAnimation = value;

                if (reset)
                    _currentAnimation.Reset();
            }
        }

        /// <summary>
        /// Adds an animation to the Animator
        /// </summary>
        /// <param name="name">the name of the animation</param>
        /// <param name="animation">the animation</param>
        public void AddAnimation(string name, Animation animation)
        {
            if (Animations.ContainsKey(name))
            {
                throw new ArgumentException("the animation: " + name + " already exists in this Animator.");
            }
            else if (!animation.HasValidFrames(Textures.Count))
            {
                throw new ArgumentException("the animation's frames are not valid for use with this Animator.");
            }

            Animations[name] = animation;

            if (CurrentAnimation == null)
            {
                CurrentAnimation = animation;
            }
        }

        /// <summary>
        /// Add a range of animations
        /// </summary>
        /// <param name="animations">dictionary that holds the animations to be added [key: name] [value: Animation]</param>
        public void AddAnimations(Dictionary<string, Animation> animations)
        {
            foreach (string name in animations.Keys)
            {
                AddAnimation(name, animations[name]);
            }
        }

        /// <summary>
        /// removes an animation
        /// </summary>
        /// <param name="name">the name of the animation to remove</param>
        public void RemoveAnimation(string name)
        {
            if (!Animations.ContainsKey(name))
            {
                throw new ArgumentException("The animation: " + name + " could not be found.");
            }

            Animations.Remove(name);
        }

        /// <summary>
        /// Sets the current animation that is playing
        /// </summary>
        /// <param name="name">The name of the animation to select</param>
        public void SetCurrentAnimation(string name)
        {
            if (!Animations.ContainsKey(name))
            {
                throw new ArgumentException("Could not find the animation: " + name);
            }

            CurrentAnimation = Animations[name];
            //CurrentAnimation.Reset();
        }

        /// <summary>
        /// overrides the update function from WorldObject
        /// </summary>
        /// <param name="t">The time elapsed since the last update</param>
        public override void UpdateThis(GameTime t)
        {
            if (CurrentAnimation != null)
                CurrentAnimation.Update(t);

            base.UpdateThis(t);
        }
    }
}