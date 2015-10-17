using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace AxisEngine.Physics
{
    /// <summary>
    /// an interface that will test whether overlaps occur
    /// </summary>
    public class Trigger : WorldObject
    {
        /// <summary>
        /// whether or not to just use the outer bounds for collision detection
        /// </summary>
        private bool _simple;

        /// <summary>
        /// the circles that are part of the trigger
        /// </summary>
        private List<Circle> Circles;

        /// <summary>
        /// the rectangles that are part of the trigger
        /// </summary>
        private List<Rectangle> Rectangles;

        /// <summary>
        /// the dimensions of the outer bounds
        /// </summary>
        private Point BoundsDimensions;

        /// <summary>
        /// the outer bounds of the trigger
        /// </summary>
        public Rectangle Bounds
        {
            get
            {
                return new Rectangle(new Point((int)Position.X, (int)Position.Y), BoundsDimensions);
            }
        }

        /// <summary>
        /// gets whether or not the trigger is simple
        /// </summary>
        public bool IsSimple
        {
            get
            {
                return _simple;
            }
        }

        /// <summary>
        /// creates a new Trigger instance
        /// </summary>
        /// <param name="boundDimensions">the x and y dimensions to be used for the outer bounding box</param>
        /// <param name="circles">the circles that will be used to build the trigger shape</param>
        /// <param name="rectangles">the rectangles that will be used to build the trigger shape</param>
        public Trigger(Point boundDimensions, IEnumerable<Circle> circles = null, IEnumerable<Rectangle> rectangles = null)
        {
            BoundsDimensions = boundDimensions;
            Circles = circles.ToList();
            Rectangles = rectangles.ToList();
            _simple = Circles == null && Rectangles == null;
        }

        /// <summary>
        /// centers the trigger with its parent
        /// </summary>
        public void Center()
        {
            Position = new Vector2(-Bounds.Width * 0.5f, -Bounds.Height * 0.5f);
        }

        /// <summary>
        /// returns true if the triggers intersect.
        /// </summary>
        /// <param name="other">the other trigger to check for an intersection with</param>
        public bool Intersects(Trigger other)
        {
            // get the bounding rectangles
            Rectangle thisBounds = Bounds;
            Rectangle otherBounds = other.Bounds;

            // if the bounds dont intersect, then return false
            if (!thisBounds.Intersects(otherBounds))
                return false;

            if (IsSimple)
            {
                if (other.IsSimple)
                {
                    // both are simple
                    return true;
                }
                else
                {
                    // this one is simple
                    return other.Rectangles.Any(r => thisBounds.Intersects(r)) || other.Circles.Any(c => CollisionManager.Collides(thisBounds, c));
                }
            }
            else
            {
                if (other.IsSimple)
                {
                    // other is simple
                    return Rectangles.Any(r => otherBounds.Intersects(r)) || Circles.Any(c => CollisionManager.Collides(otherBounds, c));
                }
                else
                {
                    // both aren't simple
                    return Rectangles.Any(R => other.Rectangles.Any(r => R.Intersects(r) ||     // compare these rectangles with those rectangles
                        other.Circles.Any(c => CollisionManager.Collides(R, c)))) ||            // compare these rectangles with those circles
                        Circles.Any(C => other.Circles.Any(c => C.Intersects(c)) ||             // compare these circles with those circles
                        other.Rectangles.Any(r => CollisionManager.Collides(r, C)));            // compare these circles with those rectangles

                }
            }
        }
    }
}
