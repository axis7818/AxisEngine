using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AxisEngine.Visuals
{
    /// <summary>
    /// an animated sprite to add motion to the graphics 
    /// </summary>
    public class Animation
    {
        /// <summary>
        /// the spritesheet that holds all of the frames
        /// </summary>
        private Texture2D _atlas;

        /// <summary>
        /// creates a new animation object
        /// </summary>
        /// <param name="atlas">the spritesheet that holds all of the frames</param>
        public Animation(Texture2D atlas)
        {
            _atlas = atlas;
        }

        /// <summary>
        /// returns a texture that represents the Current Animation's frame
        /// </summary>
        public Texture2D Texture
        {
            get
            {
                return _atlas;
            }
        }

        /// <summary>
        /// gets the rectangle that captures the current frame of the animation
        /// </summary>
        public Rectangle SourceRectangle
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
