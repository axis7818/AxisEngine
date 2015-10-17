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
        /// checks if the given rectangle intersects the trigger
        /// </summary>
        public bool Intersects(Rectangle r)
        {
            // if the outer bounds doesn't collide, then there is no need to continue checking
            if (!CollisionManager.Collides(r, Bounds))
                return false;

            // if there are any collisions with the circles or rectangles
            return Rectangles.Any(rec => CollisionManager.Collides(rec, r)) ||
                Circles.Any(cir => CollisionManager.Collides(r, cir));
        }

        /// <summary>
        /// checks if the given circle intersects the trigger
        /// </summary>
        public bool Intersects(Circle c)
        {
            // if the outer bounds dont collide, then there is no need to continue checking
            if (!CollisionManager.Collides(Bounds, c))
                return false;

            // if there are any collisions with the circles or rectangles
            return Circles.Any(cir => CollisionManager.Collides(cir, c)) ||
                Rectangles.Any(r => CollisionManager.Collides(r, c));
        }

        /// <summary>
        /// returns true if the triggers intersect.
        /// </summary>
        /// <param name="trigger">the other trigger to check for an intersection with</param>
        public bool Intersects(Trigger trigger)
        {
            // get the bounding rectangles
            Rectangle thisBounds = Bounds;
            Rectangle otherBounds = trigger.Bounds;

            // if the bounds dont intersect, then return false
            if (!thisBounds.Intersects(otherBounds))
                return false;

            if (IsSimple)
            {
                if (trigger.IsSimple)
                {
                    // both are simple
                    return true;
                }
                else
                {
                    // this one is simple
                    return trigger.Rectangles.Any(r => thisBounds.Intersects(r)) || trigger.Circles.Any(c => CollisionManager.Collides(thisBounds, c));
                }
            }
            else
            {
                if (trigger.IsSimple)
                {
                    // other is simple
                    return Rectangles.Any(r => otherBounds.Intersects(r)) || Circles.Any(c => CollisionManager.Collides(otherBounds, c));
                }
                else
                {
                    // both aren't simple
                    return Rectangles.Any(R => trigger.Rectangles.Any(r => R.Intersects(r) ||     // compare these rectangles with those rectangles
                        trigger.Circles.Any(c => CollisionManager.Collides(R, c)))) ||            // compare these rectangles with those circles
                        Circles.Any(C => trigger.Circles.Any(c => C.Intersects(c)) ||             // compare these circles with those circles
                        trigger.Rectangles.Any(r => CollisionManager.Collides(r, C)));            // compare these circles with those rectangles

                }
            }
        }
    }
}
