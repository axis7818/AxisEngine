using Microsoft.Xna.Framework;

namespace AxisEngine.Physics
{
    /// <summary>
    /// a circular collider
    /// </summary>
    public class CircleCollider : WorldObject, ICollisionManageable
    {
        /// <summary>
        /// the radius of the circle
        /// </summary>
        private float Radius;

        /// <summary>
        /// creates a new CircleCollider with the given radius
        /// </summary>
        public CircleCollider(float radius)
        {
            Radius = radius;
        }

        /// <summary>
        /// the circle that represents the collider
        /// </summary>
        public Circle Bounds
        {
            get
            {
                return new Circle(Position.ToPoint(), (int)Radius);
            }
        }

        /// <summary>
        /// gets the point in the game world that represents the center of the collider
        /// </summary>
        public Point CenterPoint
        {
            get
            {
                return Position.ToPoint();
            }
        }

        /// <summary>
        /// determines if the circle collider overlaps with the given trigger
        /// </summary>
        /// <param name="trigger">the trigger to check against</param>
        public bool Intersects(Trigger trigger)
        {
            return trigger.Intersects(Bounds);
        }
    }
}