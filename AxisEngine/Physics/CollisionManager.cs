using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using AxisEngine;
using AxisEngine.AxisDebug;

namespace AxisEngine.Physics
{
    public class CollisionManager : IUpdateable, IEnumerable
    {
        private List<ICollidable> colliders = new List<ICollidable>();
        private DoubleKeyMap<ICollidable, bool> collisionMap = new DoubleKeyMap<ICollidable, bool>();

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
            get { return colliders.Count; }
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
        
        public void AddCollidable(ICollidable coll)
        {
            if (colliders.Contains(coll))
                throw new ArgumentException("The Collider is already in this collision manager.");

            // put an entry in the collision map
            foreach(ICollidable other in colliders)
            {
                collisionMap.Set(coll, other, false);
            }

            // add this entry to the list of colliders
            colliders.Add(coll);
        }

        public bool Contains(ICollidable coll)
        {
            return colliders.Contains(coll);
        }

        public IEnumerator GetEnumerator()
        {
            foreach (ICollidable coll in colliders)
                yield return coll;
        }

        public void Remove(ICollidable coll)
        {
            if (colliders.Contains(coll))
            {
                collisionMap.RemoveKey(coll);
                colliders.Remove(coll);
            }
        }

        //TODO: this needs to be tested
        public void Update(GameTime t)
        {
            if (Enabled)
            {
                foreach(Tuple<ICollidable, ICollidable> key in collisionMap.Keys)
                {
                    // unpack the colliders
                    ICollidable A = key.Item1;
                    ICollidable B = key.Item2;

                    // handle their collision status
                    if (collisionMap.Get(key))
                    {
                        // handle colliders that are collided
                        if (!A.Intersects(B))
                        {
                            // no longer intersecting
                            collisionMap.Set(key, false);
                            CollisionEventArgs args = new CollisionEventArgs(A, B, false);
                            A.RevokeCollision(args);
                            B.RevokeCollision(args);
                        }

                    }
                    else
                    {
                        // handle colliders that are not collided
                        if (A.Intersects(B))
                        {
                            // they are now colliding
                            collisionMap.Set(key, true);
                            CollisionEventArgs args = new CollisionEventArgs(A, B, true);
                            A.InvokeCollision(args);
                            B.InvokeCollision(args);
                        }
                    }
                }
            }
        }

        #region STATIC_METHODS
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
        #endregion STATIC_METHODS
    }
}