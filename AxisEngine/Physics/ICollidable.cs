using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AxisEngine.Physics
{
    public interface ICollidable
    {
        ColliderType Type { get; }
        Vector2 Position { get; }
        Texture2D WireFrame { get; set; }

        event EventHandler<CollisionEventArgs> CollisionStart;
        event EventHandler<CollisionEventArgs> CollisionEnd;

        bool Intersects(ICollidable coll);
        void InvokeCollision(CollisionEventArgs args);
        void RevokeCollision(CollisionEventArgs args);
    }
}
