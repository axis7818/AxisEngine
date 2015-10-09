using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AxisEngine.Physics
{
    /// <summary>
    /// Manages objects that can collide
    /// </summary>
    public class CollisionManager : IUpdateable, IEnumerable
    {
        /// <summary>
        /// Whether or not the manager is enabled
        /// </summary>
        private bool _enabled;

        /// <summary>
        /// The order that this object is updated in
        /// </summary>
        private int _updateOrder;

        /// <summary>
        /// the objects that the collision manager will manage
        /// </summary>
        private List<ICollisionManageable> ToManage;

        /// <summary>
        /// instantiates a Collision manager
        /// </summary>
        public CollisionManager()
        {
            Enabled = true;
            UpdateOrder = 0;

            ToManage = new List<ICollisionManageable>();
        }

        /// <summary>
        /// Fired when the Enabled property changes
        /// </summary>
        public event EventHandler<EventArgs> EnabledChanged;

        /// <summary>
        /// Fired when the UpdateOrder property changes
        /// </summary>
        public event EventHandler<EventArgs> UpdateOrderChanged;

        /// <summary>
        /// Whether or not the manager is enabled
        /// </summary>
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                if (EnabledChanged != null) EnabledChanged(this, new EventArgs());
            }
        }

        /// <summary>
        /// gets the number of items that are being managed
        /// </summary>
        public int Size
        {
            get
            {
                return ToManage.Count;
            }
        }

        /// <summary>
        /// The order that this object is updated in
        /// </summary>
        public int UpdateOrder
        {
            get { return _updateOrder; }
            set
            {
                _updateOrder = value;
                if (UpdateOrderChanged != null) UpdateOrderChanged(this, new EventArgs());
            }
        }

        /// <summary>
        /// checks whether or not two rectangles collide
        /// </summary>
        public static bool Collides(Rectangle r1, Rectangle r2)
        {
            return r1.Intersects(r2);
        }

        /// <summary>
        /// checks whether or not two circles collide
        /// </summary>
        public static bool Collides(Circle c1, Circle c2)
        {
            return c1.Intersects(c2);
        }

        /// <summary>
        /// checks whether or not a rectanlge and circle collide
        /// </summary>
        public static bool Collides(Rectangle r, Circle c)
        {
            // check if the center of the circle is in the rectangle
            if (r.Contains(c.Center))
                return true;

            // check if the circle's "outer box" doesn't intersect the rectangle
            Rectangle outerBounds = new Rectangle(new Point(c.Left, c.Top), new Point(c.Size * 2));
            if (!r.Intersects(outerBounds))
                return false;

            // check if the circle contains any of the rectangle's corners
            Point corner = new Point(r.Left, r.Top);
            if (c.Contains(corner))
                return true;
            corner.X = r.Right;
            if (c.Contains(corner))
                return true;
            corner.Y = r.Bottom;
            if (c.Contains(corner))
                return true;
            corner.X = r.Left;
            if (c.Contains(corner))
                return true;

            // check if the top edge intersects the circle
            if (Math.Abs(c.Center.Y - r.Top) <= 0)
                return true;

            // check if the right edge intersects the circle
            if (Math.Abs(c.Center.X - r.Right) <= 0)
                return true;

            // check if the bottom edge intersects the circle
            if (Math.Abs(c.Center.Y - r.Bottom) <= 0)
                return true;

            // check if the left edge intersects the circle
            if (Math.Abs(c.Center.X - r.Left) <= 0)
                return true;

            // if all else fails, return false
            return false;
        }

        /// <summary>
        /// Adds an ICollisionManageable to the Collision manager
        /// </summary>
        /// <param name="collisionManageable">the ICollisionManageable to add</param>
        public void Add(ICollisionManageable collisionManageable)
        {
            if (!ToManage.Contains(collisionManageable))
                ToManage.Add(collisionManageable);
            else
                throw new ArgumentException("collisionManageable already exists in the manager.");
        }

        /// <summary>
        /// whether or not the collision manager contains the ICollisionManageable
        /// </summary>
        /// <param name="coll">the ICollisionManageable to check for</param>
        public bool Contains(ICollisionManageable coll)
        {
            return ToManage.Contains(coll);
        }

        /// <summary>
        /// enumerates the CollisionManager to get the CollisionManageables
        /// </summary>
        public IEnumerator GetEnumerator()
        {
            foreach (ICollisionManageable coll in ToManage)
            {
                yield return coll;
            }
        }

        /// <summary>
        /// removes an ICollisionManageable from the Collision manager
        /// </summary>
        /// <param name="coll">the ICollisionManager to remove</param>
        public void Remove(ICollisionManageable coll)
        {
            ToManage.Remove(coll);
        }

        /// <summary>
        /// updates the collision manager
        /// </summary>
        /// <param name="t">the time since the last update</param>
        public void Update(GameTime t)
        {
            if (Enabled)
            {
                /* TODO: On Updating, make sure to undo any force on a Body that might push objects into eachother. */
            }
        }
    }
}