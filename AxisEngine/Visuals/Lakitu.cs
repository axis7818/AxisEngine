using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AxisEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AxisEngine.Visuals
{
    /// <summary>
    /// A WorldObject that holds a Camera. This gives a Camera all of the functionality of a WorldObject and adds additional functionality.
    /// </summary>
    public class Lakitu : WorldObject
    {
        /* TODO: THINGS TO ADD
        Panning/Zooming
        tinting/filters?
        fade in/out
        ---
        May need to add some functions to the Camera to allow for Lakitu to control certain parameters
        */

        private Camera camera;

        public Lakitu(Camera camera)
        {
            this.camera = camera;
        }

        protected override void UpdateThis(GameTime t)
        {
                
        }
    }
}
