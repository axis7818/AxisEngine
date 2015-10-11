using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            // check if the outer bounds intersect. If they do, continue checking. otherwise, return false
            if (!this.Bounds.Intersects(other.Bounds))
                return false;

            // compare rectangles with rectangles
            foreach (Rectangle R in Rectangles)
                foreach (Rectangle r in other.Rectangles)
                    if (R.Intersects(r))
                        return true;

            // compare circles with circles
            foreach (Circle C in Circles)
                foreach (Circle c in other.Circles)
                    if (C.Intersects(c))
                        return true;

            // compare rectangles with circles
            foreach (Rectangle R in Rectangles)
                foreach (Circle c in other.Circles)
                    if (CollisionManager.Collides(R, c))
                        return true;

            // compare circles with rectangles
            foreach (Circle C in Circles)
                foreach (Rectangle r in other.Rectangles)
                    if (CollisionManager.Collides(r, C))
                        return true;

            // return false if all else fails
            return false;
        }
    }
}
