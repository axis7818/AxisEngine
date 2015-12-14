using System;

namespace AxisEngine.Physics
{
    public interface ICollidable
    {
        ColliderType Type { get; }

        bool Intersects(ICollidable coll);

        event EventHandler<CollisionEventArgs> CollisionStart;
        event EventHandler<CollisionEventArgs> CollisionEnd;
    }
}
