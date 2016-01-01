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

        private Vector2 absolutePosition; // this is the position of the upper left corner of the viewport in the world
        public float Zoom = 1;

        private Viewport viewport;
        private string name;
        private bool enabled;

        public Camera(string name, Viewport viewport)
        {
            this.name = name;
            this.viewport = viewport;
            enabled = true;
            Position = ScreenCenter;
        }

        public Vector2 AbsolutePosition // this is the position of the upper-right of the viewport in the world
        {
            get { return absolutePosition; }
            set { absolutePosition = value; }
        }

        public Vector2 Position // this is the position of the center of the viewport in the world
        {
            get { return absolutePosition + ScreenCenter; }
            set { absolutePosition = value - ScreenCenter; }
        }

        public Vector2 ScreenCenter // this is the position of the center of the viewport in the window's coordinates
        {
            get { return new Vector2(viewport.Width / 2, viewport.Height / 2); }
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

        public Vector2 WorldPointToViewportPoint(Vector2 worldPoint)
        {
            return worldPoint - absolutePosition;
        }
        
        public Vector2 ViewportPointToWorldPoint(Vector2 viewportPoint)
        {
            return viewportPoint + absolutePosition;
        }
    }
}