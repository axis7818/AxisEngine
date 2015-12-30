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

        private Vector2 absolutePosition;
        public float Zoom = 1;

        private Viewport viewport;
        private string name;
        private bool enabled;

        public Camera(string name, Viewport viewport)
        {
            this.name = name;
            this.viewport = viewport;
            enabled = true;
            Position = Vector2.Zero;
        }

        public Vector2 Position
        {
            get
            {
                Vector2 offset = new Vector2(viewport.Width / 2, viewport.Height / 2);
                return absolutePosition + offset;
            }
            set
            {
                Vector2 offset = new Vector2(viewport.Width / 2, viewport.Height / 2);
                absolutePosition = value - offset;
            }
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
        }
    }
}