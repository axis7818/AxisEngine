using System;
using Microsoft.Xna.Framework;

namespace AxisEngine.Physics
{
    public class CircleCollider : WorldObject, ICollidable
    {
        private float Radius;

        public CircleCollider(float radius)
        {
            Radius = radius;
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

        public bool Intersects(ICollidable coll)
        {
            switch (coll.Type)
            {
                case ColliderType.BOX_COLLIDER:
                    return CollisionManager.Collides((coll as BoxCollider).Bounds, Bounds);
                case ColliderType.CIRCLE_COLLIDER:
                    return CollisionManager.Collides(Bounds, (coll as CircleCollider).Bounds);
                case ColliderType.TRIGGER:
                    return Intersects(coll as Trigger);
                default:
                    throw new InvalidOperationException("invalid collider type.");
            }
        }

        public bool Intersects(Trigger trigger)
        {
            return trigger.Intersects(Bounds);
        }

        protected override void UpdateThis(GameTime t)
        {
            
        }
    }
}