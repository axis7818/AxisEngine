using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AxisEngine.Visuals
{
    public class Animation
    {
        private Texture2D _atlas;

        public Animation(Texture2D atlas)
        {
            _atlas = atlas;
        }

        public Texture2D Texture
        {
            get { return _atlas; }
        }

        public Rectangle SourceRectangle
        {
            get { throw new NotImplementedException(); }
        }
    }
}