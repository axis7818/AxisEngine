using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AxisEngine.Physics
{
    public class CircleCollider : WorldObject, ICollidable
    {
        private float Radius;
        private Texture2D _wireFrame = null;

        public CircleCollider(float radius)
        {
            Radius = radius;
        }

        public Texture2D WireFrame
        {
            get { return _wireFrame; }
            set { _wireFrame = value; }
        }

        public Circle Bounds
        {
            get { return new Circle(Position.ToPoint(), (int)Radius); }
        }

        public Point CenterPoint
        {
            get { return Position.ToPoint(); }
        }

        public ColliderType Type
        {
            get { return ColliderType.CIRCLE_COLLIDER; }
        }

        public event EventHandler<CollisionEventArgs> CollisionStart;
        public event EventHandler<CollisionEventArgs> CollisionEnd;

        public bool Intersects(ICollidable coll)
        {
            switch (coll.Type)
            {
                case ColliderType.BOX_COLLIDER:
                    return CollisionManager.Collides((coll as BoxCollider).Bounds, Bounds);
                case ColliderType.CIRCLE_COLLIDER:
                    return CollisionManager.Collides(Bounds, (coll as CircleCollider).Bounds);
                default:
                    throw new InvalidOperationException("invalid collider type.");
            }
        }

        protected override void UpdateThis(GameTime t)
        {
            
        }

        public void InvokeCollision(CollisionEventArgs args)
        {
            if (CollisionStart != null)
                CollisionStart(this, args);
        }

        public void RevokeCollision(CollisionEventArgs args)
        {
            if (CollisionEnd != null)
                CollisionEnd(this, args);
        }
    }
}