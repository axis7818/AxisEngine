using AxisEngine.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AxisEngine.Visuals
{
    public class DrawManager : IEnumerable
    {
        private int _drawOrder = 0;
        private bool _visible = true;
        private bool drawing = false;
        private List<IDrawManageable> thingsToDraw = new List<IDrawManageable>();
        private SpriteBatch spriteBatch;

        // fields used for drawing wireframes
        private CollisionManager collisionManager = null;
        private bool _drawWireFrames = false;

        public DrawManager(GraphicsDevice graphicsDevice)
        {
            spriteBatch = new SpriteBatch(graphicsDevice);
        }
        
        public event EventHandler<EventArgs> VisibleChanged;
        public event EventHandler<EventArgs> DrawOrderChanged;

        public bool Visible
        {
            get { return _visible; }
            set
            {
                _visible = value;
                OnVisibleChanged();
            }
        }

        public int DrawOrder
        {
            get { return _drawOrder; }
            set
            {
                _drawOrder = value;
                if (DrawOrderChanged != null)
                    DrawOrderChanged(this, EventArgs.Empty);
            }
        }

        public int Size
        {
            get { return thingsToDraw.Count; }
        }

        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }

        private void OnVisibleChanged()
        {
            if (VisibleChanged != null)
                VisibleChanged(this, EventArgs.Empty);

            if (!_visible && drawing)
            {
                EndDraw();
            }
        }

        public void AddDrawable(IDrawManageable toDraw)
        {
            thingsToDraw.Add(toDraw);
            SortByDrawOrder();
        }

        public bool Contains(IDrawManageable test)
        {
            return thingsToDraw.Contains(test);
        }

        public void StartDraw()
        {
            spriteBatch.Begin();
            drawing = true;
        }

        public void EndDraw()
        {
            spriteBatch.End();
            drawing = false;
        }

//        public virtual void Draw(Camera camera)
//        {
//            if (Visible)
//            {
//                spriteBatch.Begin(); //TODO give drawmanager fields to populate spriteBatch.Begin() with.

//                // draw all of the IDrawManageables to the screen
//                foreach (IDrawManageable toDraw in _thingsToDraw)
//                {
//                    if (toDraw.Visible && toDraw.IsViewableTo(camera))
//                    {
//                        toDraw.Draw(spriteBatch, camera);
//                    }
//                }

//#if DEBUG
//                // if the draw manager is set to draw the wire frames from a CollisionManager, then draw them
//                if (_drawWireFrames && collisionManager != null)
//                {
//                    foreach (ICollidable coll in collisionManager)
//                    {
//                        if (coll.WireFrame != null)
//                        {
//                            Vector2 drawPosition = coll.Position;
//                            if (coll.Type == ColliderType.CIRCLE_COLLIDER)
//                            {
//                                // circle colliders need to be shifted since circles are located at their 
//                                // center, but the Texture2D is drawn from the upper left corner
//                                int offset = (coll as CircleCollider).Bounds.Radius;
//                                drawPosition -= new Vector2(offset, offset);
//                            }
//                            spriteBatch.Draw(coll.WireFrame, drawPosition, Color.White);
//                        }
//                    }
//                }
//#endif

//                spriteBatch.End();
//            }
//        }

        public void DrawWireFrames()
        {
            if(_drawWireFrames && collisionManager != null)
            {
                foreach(ICollidable coll in collisionManager)
                {
                    if(coll.WireFrame != null)
                    {
                        Vector2 drawPosition = coll.Position;
                        if(coll.Type == ColliderType.CIRCLE_COLLIDER)
                        {
                            int offset = (coll as CircleCollider).Bounds.Radius;
                            drawPosition -= new Vector2(offset, offset);
                        }
                        spriteBatch.Draw(coll.WireFrame, drawPosition, Color.White);
                    }
                }
            }
        }

        public void EnableDrawWireFrames(CollisionManager collisionManager)
        {
            _drawWireFrames = true;
            this.collisionManager = collisionManager;
        }

        public void StopDrawingWireFrames()
        {
            _drawWireFrames = false;
            collisionManager = null;
        }

        public void Remove(IDrawManageable toRemove)
        {
            thingsToDraw.Remove(toRemove);
        }
        
        private void SortByDrawOrder()
        {
            thingsToDraw = thingsToDraw.OrderBy(x => x.DrawOrder).ToList();
        }

        public IEnumerator GetEnumerator()
        {
            foreach (IDrawManageable dr in thingsToDraw)
            {
                yield return dr;
            }
        }
    }
}