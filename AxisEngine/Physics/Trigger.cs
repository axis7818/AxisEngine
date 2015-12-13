using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System;

namespace AxisEngine.Physics
{
    public class Trigger : WorldObject, ICollidable
    {
        private bool _simple;
        private List<Circle> Circles;
        private List<Rectangle> Rectangles;
        private Point BoundsDimensions;
                
        public Trigger(Point boundDimensions, IEnumerable<Circle> circles = null, IEnumerable<Rectangle> rectangles = null)
        {
            BoundsDimensions = boundDimensions;
            Circles = circles.ToList();
            Rectangles = rectangles.ToList();
            _simple = Circles == null && Rectangles == null;
        }

        public Rectangle Bounds
        {
            get { return new Rectangle(new Point((int)Position.X, (int)Position.Y), BoundsDimensions); }
        }

        public bool IsSimple
        {
            get { return _simple; }
        }

        public ColliderType Type
        {
            get { return ColliderType.TRIGGER; }
        }

        public void Center()
        {
            Position = new Vector2(-Bounds.Width * 0.5f, -Bounds.Height * 0.5f);
        }

        public bool Intersects(Rectangle r)
        {
            // if the outer bounds doesn't collide, then there is no need to continue checking
            if (!CollisionManager.Collides(r, Bounds))
                return false;

            // if there are any collisions with the circles or rectangles
            if (Rectangles.Any(rec => CollisionManager.Collides(rec, r)))
                return true;
            if (Circles.Any(cir => CollisionManager.Collides(r, cir)))
                return true;
            return false;
        }

        public bool Intersects(Circle c)
        {
            // if the outer bounds dont collide, then there is no need to continue checking
            if (!CollisionManager.Collides(Bounds, c))
                return false;

            // if there are any collisions with the circles or rectangles
            if (Circles.Any(cir => CollisionManager.Collides(cir, c)))
                return true;
            if (Rectangles.Any(r => CollisionManager.Collides(r, c)))
                return true;
            return false;
        }

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

        protected override void UpdateThis(GameTime t)
        {
            
        }

        public bool Intersects(ICollidable coll)
        {
            switch (coll.Type)
            {
                case ColliderType.BOX_COLLIDER:
                    return (coll as BoxCollider).Intersects(this);
                case ColliderType.CIRCLE_COLLIDER:
                    return (coll as CircleCollider).Intersects(this);
                case ColliderType.TRIGGER:
                    return Intersects(coll as Trigger);
                default:
                    throw new InvalidCastException("invalid collider type.");
            }
        }
    }
}