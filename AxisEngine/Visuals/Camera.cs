using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AxisEngine.Visuals
{
    public class Camera
    {
        // TODO: add tinting and filters
        // TODO: fix how Camera zooming works.

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
            Position = ScreenCenter;
        }

        /// <summary>
        /// The position of the upper-left of the viewport in the world
        /// </summary>
        public Vector2 AbsolutePosition
        {
            get { return absolutePosition; }
            set { absolutePosition = value; }
        }

        /// <summary>
        /// The position of the center of the viewport in the world
        /// </summary>
        public Vector2 Position
        {
            get { return absolutePosition + ScreenCenter; }
            set { absolutePosition = value - ScreenCenter; }
        }

        /// <summary>
        /// The position of the center of the viewport in the window's coordinates
        /// </summary>
        public Vector2 ScreenCenter
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
            foreach (IDrawManageable toDraw in drawManager)
            {
                if (toDraw.Visible && toDraw.IsViewableTo(this))
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