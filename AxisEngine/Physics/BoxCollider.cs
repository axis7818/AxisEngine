using System;
using Microsoft.Xna.Framework;

namespace AxisEngine.Physics
{
    public class BoxCollider : WorldObject, ICollidable
    {
        private Point Dimensions;

        public BoxCollider(Point dimensions)
        {
            Dimensions = dimensions;
        }

        public Rectangle Bounds
        {
            get { return new Rectangle(Position.ToPoint(), Dimensions); }
        }

        public Point CenterPoint
        {
            get { return new Point((int)(Position.X + Dimensions.X * 0.5f), (int)(Position.Y + Dimensions.Y * 0.5f)); }
        }

        public ColliderType Type
        {
            get { return ColliderType.BOX_COLLIDER; }
        }

        public void Center()
        {
            Position = new Vector2(BasePosition.X - Dimensions.X * 0.5f, BasePosition.Y - Dimensions.Y * 0.5f);
        }

        public bool Intersects(ICollidable coll)
        {
            switch (coll.Type)
            {
                case ColliderType.BOX_COLLIDER:
                    return CollisionManager.Collides(Bounds, (coll as BoxCollider).Bounds);
                case ColliderType.CIRCLE_COLLIDER:
                    return CollisionManager.Collides(Bounds, (coll as CircleCollider).Bounds);
                case ColliderType.TRIGGER:
                    return Intersects(coll as Trigger);
                default:
                    throw new InvalidOperationException("Incompatable Collider Type.");
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