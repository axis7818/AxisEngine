using System;
using Microsoft.Xna.Framework;

namespace AxisEngine.Physics
{
    public class BoxCollider : WorldObject, ICollisionManageable
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

        public void Center()
        {
            Position = new Vector2(BasePosition.X - Dimensions.X * 0.5f, BasePosition.Y - Dimensions.Y * 0.5f);
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