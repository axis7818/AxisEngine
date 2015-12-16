using System;

namespace AxisEngine.Physics
{
    public interface ICollidable
    {
        ColliderType Type { get; }

        event EventHandler<CollisionEventArgs> CollisionStart;
        event EventHandler<CollisionEventArgs> CollisionEnd;

        bool Intersects(ICollidable coll);
        void InvokeCollision(CollisionEventArgs args);
        void RevokeCollision(CollisionEventArgs args);
    }
}
