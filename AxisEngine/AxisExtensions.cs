using Microsoft.Xna.Framework;

namespace AxisEngine
{
    public static class AxisExtensions
    {
        public static Vector2 ToVector2(this Point p)
        {
            return new Vector2(p.X, p.Y);
        }
    }
}
