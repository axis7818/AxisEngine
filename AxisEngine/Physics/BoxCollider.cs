using Microsoft.Xna.Framework;

namespace AxisEngine.Physics
{
    /// <summary>
    /// a rectangle collider
    /// </summary>
    public class BoxCollider : WorldObject, ICollisionManageable
    {
        /// <summary>
        /// the dimensions of the collider
        /// </summary>
        private Point Dimensions;

        /// <summary>
        /// the rectangle that represents the collider
        /// </summary>
        public Rectangle Bounds
        {
            get
            {
                return new Rectangle(Position.ToPoint(), Dimensions);
            }
        }

        /// <summary>
        /// gets the point in the game world that represents the center of the collider
        /// </summary>
        public Point CenterPoint
        {
            get
            {
                return new Point((int)(Position.X + Dimensions.X * 0.5f), (int)(Position.Y + Dimensions.Y * 0.5f));
            }
        }

        /// <summary>
        /// creates a new Box Collider with the given dimensions
        /// </summary>
        /// <param name="dimensions"></param>
        public BoxCollider(Point dimensions)
        {
            Dimensions = dimensions;
        }

        /// <summary>
        /// centers the collider with the dimensions
        /// </summary>
        public void Center()
        {
            Position = new Vector2(BasePosition.X - Dimensions.X * 0.5f, BasePosition.Y - Dimensions.Y * 0.5f);
        }

        /// <summary>
        /// determines if the Box Collider overlaps with the given trigger
        /// </summary>
        /// <param name="trigger">the trigger to check against</param>
        public bool Intersects(Trigger trigger)
        {
            return trigger.Intersects(Bounds);
        }
    }
}
