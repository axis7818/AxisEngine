using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AxisEngine.Physics
{
    public class CollisionManager : IUpdateable, IEnumerable
    {
        private bool _enabled;

        private int _updateOrder;

        private List<ICollisionManageable> ToManage;

        private List<Trigger> Triggers;

        public CollisionManager()
        {
            Enabled = true;
            UpdateOrder = 0;

            ToManage = new List<ICollisionManageable>();
            Triggers = new List<Trigger>();
        }

        public event EventHandler<EventArgs> EnabledChanged;

        public event EventHandler<EventArgs> UpdateOrderChanged;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                if (EnabledChanged != null) EnabledChanged(this, new EventArgs());
            }
        }

        public int Size
        {
            get
            {
                return ToManage.Count;
            }
        }

        public int UpdateOrder
        {
            get { return _updateOrder; }
            set
            {
                _updateOrder = value;
                if (UpdateOrderChanged != null) UpdateOrderChanged(this, new EventArgs());
            }
        }

        public static bool Collides(Rectangle r1, Rectangle r2)
        {
            return r1.Intersects(r2);
        }

        public static bool Collides(Circle c1, Circle c2)
        {
            return c1.Intersects(c2);
        }

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

        public void AddCollisionManageable(ICollisionManageable collisionManageable)
        {
            if (!ToManage.Contains(collisionManageable))
                ToManage.Add(collisionManageable);
            else
                throw new ArgumentException("collisionManageable already exists in the manager.");
        }

        public void AddTrigger(Trigger trigger)
        {
            if (!Triggers.Contains(trigger))
                Triggers.Add(trigger);
            else
                throw new ArgumentException("trigger already exists in the manager.");
        }

        public bool Contains(ICollisionManageable coll)
        {
            return ToManage.Contains(coll);
        }

        public bool Contains(Trigger trigger)
        {
            return Triggers.Contains(trigger);
        }

        public IEnumerator GetEnumerator()
        {
            foreach (ICollisionManageable coll in ToManage)
            {
                yield return coll;
            }
        }

        public IEnumerator GetEnumeratedTriggers()
        {
            foreach (Trigger trig in Triggers)
            {
                yield return trig;
            }
        }

        public void Remove(ICollisionManageable coll)
        {
            ToManage.Remove(coll);
        }

        public void Remove(Trigger trigger)
        {
            Triggers.Remove(trigger);
        }

        public void Update(GameTime t)
        {
            if (Enabled)
            {
                /* TODO: On Updating, make sure to undo any force on a Body that might push objects into eachother. */
                /* TODO: On Updating, make sure to check for overlaps with triggers. */
            }
        }
    }
}