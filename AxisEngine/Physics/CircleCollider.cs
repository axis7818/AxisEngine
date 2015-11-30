using System;
using Microsoft.Xna.Framework;

namespace AxisEngine.Physics
{
    public class CircleCollider : WorldObject, ICollisionManageable
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

        public bool Intersects(Trigger trigger)
        {
            return trigger.Intersects(Bounds);
        }

        protected override void UpdateThis(GameTime t)
        {
            
        }
    }
}