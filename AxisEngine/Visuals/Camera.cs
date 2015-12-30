using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AxisEngine.Visuals
{
    public class Camera
    {
        /* TODO: THINGS TO ADD
        ---
        panning
        tinting/filters?
        fade in/out
        bounds restrictions
        */

        public const string DEFAULT_NAME = "DEFAULT";

        public Vector2 Position = Vector2.Zero;
        public float Zoom = 1;

        private Viewport viewport;
        private string name;
        private bool enabled;

        public Camera(string name, Viewport viewport)
        {
            this.name = name;
            this.viewport = viewport;
            enabled = true;
        }

        public string Name
        {
            get { return name; }
        }

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public Viewport Viewport
        {
            get { return viewport; }
            set { viewport = value; }
        }

        public void Draw(DrawManager drawManager)
        {
            foreach(IDrawManageable toDraw in drawManager)
            {
                if(toDraw.Visible && toDraw.IsViewableTo(this))
                {
                    toDraw.Draw(drawManager.SpriteBatch, this);
                }
            }

#if DEBUG
            drawManager.DrawWireFrames();
#endif
        }
    }
}