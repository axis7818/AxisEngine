using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AxisEngine.Physics
{
    public class BoxCollider : WorldObject, ICollidable
    {
        private Point Dimensions;
        private Texture2D _wireFrame = null;

        public BoxCollider(Point dimensions)
        {
            Dimensions = dimensions;
        }

        public Texture2D WireFrame
        {
            get { return _wireFrame; }
            set { _wireFrame = value; }
        }

        public Rectangle Bounds
        {
            get { return new Rectangle(Position.ToPoint(), Dimensions); }
        }

        public Point CenterPoint
        {
            get { return new Point((int)(Position.X + Dimensions.X * 0.5f), (int)(Position.Y + Dimensions.Y * 0.5f)); }
        }

        public ColliderType Type
        {
            get { return ColliderType.BOX_COLLIDER; }
        }

        public event EventHandler<CollisionEventArgs> CollisionStart;
        public event EventHandler<CollisionEventArgs> CollisionEnd;

        public void Center()
        {
            Position = new Vector2(BasePosition.X - Dimensions.X * 0.5f, BasePosition.Y - Dimensions.Y * 0.5f);
        }

        public bool Intersects(ICollidable coll)
        {
            switch (coll.Type)
            {
                case ColliderType.BOX_COLLIDER:
                    return CollisionManager.Collides(Bounds, (coll as BoxCollider).Bounds);
                case ColliderType.CIRCLE_COLLIDER:
                    return CollisionManager.Collides(Bounds, (coll as CircleCollider).Bounds);
                default:
                    throw new InvalidOperationException("Incompatable Collider Type.");
            }
        }

        protected override void UpdateThis(GameTime t)
        {
            
        }

        public void InvokeCollision(CollisionEventArgs args)
        {
            if (CollisionStart != null)
                CollisionStart(this, args);    
        }

        public void RevokeCollision(CollisionEventArgs args)
        {
            if (CollisionEnd != null)
                CollisionEnd(this, args);
        }
    }
}