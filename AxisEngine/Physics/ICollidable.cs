using System;
using Microsoft.Xna.Framework;

namespace AxisEngine.Physics
{
    public interface ICollidable
    {
        ColliderType Type { get; }
        Vector2 Position { get; }

        event EventHandler<CollisionEventArgs> CollisionStart;
        event EventHandler<CollisionEventArgs> CollisionEnd;

        bool Intersects(ICollidable coll);
        void InvokeCollision(CollisionEventArgs args);
        void RevokeCollision(CollisionEventArgs args);
    }
}
