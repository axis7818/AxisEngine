using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AxisEngine.Physics
{
    public class CollisionManager : IUpdateable, IEnumerable
    {
        private List<BoxCollider> _boxColliders = new List<BoxCollider>();
        private List<CircleCollider> _circleColliders = new List<CircleCollider>();
        private List<Trigger> _triggers = new List<Trigger>();

        private bool _enabled = true;
        private int _updateOrder = 0;
        
        public CollisionManager() { }
        
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

        public int Count
        {
            get { return _circleColliders.Count + _boxColliders.Count + _triggers.Count; }
        }

        public int UpdateOrder
        {
            get { return _updateOrder; }
            set
            {
                _updateOrder = value;
                if (UpdateOrderChanged != null)
                    UpdateOrderChanged(this, new EventArgs());
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
        
        public void AddCollidable(ICollidable coll)
        {
            switch (coll.Type)
            {
                case ColliderType.BOX_COLLIDER:
                    BoxCollider box = coll as BoxCollider;
                    if (_boxColliders.Contains(box))
                        throw new ArgumentException("The BoxCollider is already in this CollisionManager.");
                    _boxColliders.Add(box);
                    return;
                case ColliderType.CIRCLE_COLLIDER:
                    CircleCollider cir = coll as CircleCollider;
                    if (_circleColliders.Contains(cir))
                        throw new ArgumentException("The CircleCollider is already in this CollisionManager.");
                    _circleColliders.Add(cir);
                    return;
                case ColliderType.TRIGGER:
                    Trigger trig = coll as Trigger;
                    if (_triggers.Contains(trig))
                        throw new ArgumentException("The Trigger is already in this CollisionManager.");
                    _triggers.Add(trig);
                    return;
                default:
                    throw new InvalidOperationException("Invalid Collider type.");
            }
        }

        public bool Contains(ICollidable coll)
        {
            switch (coll.Type)
            {
                case ColliderType.BOX_COLLIDER:
                    return _boxColliders.Contains(coll as BoxCollider);
                case ColliderType.CIRCLE_COLLIDER:
                    return _circleColliders.Contains(coll as CircleCollider);
                case ColliderType.TRIGGER:
                    return _triggers.Contains(coll as Trigger);
                default:
                    throw new InvalidOperationException("Invalid Collider Type.");
            }
        }

        public IEnumerator GetEnumerator()
        {
            foreach (BoxCollider box in _boxColliders)
                yield return box;
            foreach (CircleCollider cir in _circleColliders)
                yield return cir;
            foreach (Trigger trig in _triggers)
                yield return trig;
        }

        public void Remove(ICollidable coll)
        {
            switch (coll.Type)
            {
                case ColliderType.BOX_COLLIDER:
                    _boxColliders.Remove(coll as BoxCollider);
                    return;
                case ColliderType.CIRCLE_COLLIDER:
                    _circleColliders.Remove(coll as CircleCollider);
                    return;
                case ColliderType.TRIGGER:
                    _triggers.Remove(coll as Trigger);
                    return;
                default:
                    throw new InvalidOperationException("Invalid ColliderType");
            }
        }

        public void Update(GameTime t)
        {
            if (Enabled)
            {

            }
        }
    }
}