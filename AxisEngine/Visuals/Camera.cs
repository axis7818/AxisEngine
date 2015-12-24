using Microsoft.Xna.Framework;

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

        private string name;

        public Camera(string name)
        {
            this.name = name;
        }

        public string Name
        {
            get { return name; }
        }
    }
}
