using Microsoft.Xna.Framework;

namespace AxisEngine.Physics
{
    public interface ICollisionManageable
    {
        bool Intersects(Trigger trigger);

        Point CenterPoint { get; }
    }
}