//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.Xna.Framework;

//namespace AxisEngine.Physics
//{
//    /// <summary>
//    /// a rectange that acts as a trigger
//    /// </summary>
//    public class BoxTrigger : WorldObject
//    {
//        /// <summary>
//        /// The width of the trigger
//        /// </summary>
//        private float Width;

//        /// <summary>
//        /// the height of the trigger
//        /// </summary>
//        private float Height;

//        /// <summary>
//        /// instantiates a BoxTrigger
//        /// </summary>
//        /// <param name="width">the width of the trigger</param>
//        /// <param name="height">the height of the trigger</param>
//        public BoxTrigger(float width, float height)
//        {
//            Width = width;
//            Height = height;
//        }
        
//        /// <summary>
//        /// the Rectangle object that this trigger represents
//        /// </summary>
//        public Rectangle Bounds
//        {            
//            get
//            {
//                return new Rectangle((int)Position.X, (int)Position.Y, (int)Width, (int)Height);
//            }
//        }

//        /// <summary>
//        /// centers the trigger with its owner
//        /// </summary>
//        public void Center()
//        {
//            Position = new Vector2(-Width * 0.5f, -Height * 0.5f);
//        }
//    }
//}
