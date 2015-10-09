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
        /// creates a new Trigger instance
        /// </summary>
        /// <param name="boundDimensions">the x and y dimensions to be used for the outer bounding box</param>
        /// <param name="circles">the circles that will be used to build the trigger shape</param>
        /// <param name="rectangles">the rectangles that will be used to build the trigger shape</param>
        public Trigger(Point boundDimensions, IEnumerable<Circle> circles, IEnumerable<Rectangle> rectangles)
        {
            BoundsDimensions = boundDimensions;
            Circles = circles.ToList();
            Rectangles = rectangles.ToList();
        }

        /// <summary>
        /// centers the trigger with its parent
        /// </summary>
        public void Center()
        {
            Position = new Vector2(-Bounds.Width * 0.5f, -Bounds.Height * 0.5f);
        }
    }
}
