﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AxisEngine;

namespace AxisEngine.AxisDebug
{
    public static class WireFrames
    {
        public static Color Color = Color.Red;
        
        public static Texture2D BoxWireFrame(Rectangle rectangle, GraphicsDevice graphicsDevice)
        {
            int width = rectangle.Width, height = rectangle.Height;
            Color[] data = new Color[width * height];
            for(int i = 0; i < width; i++)
            {
                // draw the top line
                data[i] = Color;
                // draw the bottom line
                int offset = (height - 1) * width;
                data[offset + i] = Color;
            }
            for(int i = 0; i < height; i++)
            {
                // draw the left line
                data[i * width] = Color;
                // draw the right line
                data[(i + 1) * width - 1] = Color;
            }

            Texture2D result = new Texture2D(graphicsDevice, width, height);
            result.SetData(data);
            return result;
        }

        public static Texture2D CircleWireFrame(Circle circle, GraphicsDevice graphicsDevice)
        {
            throw new NotImplementedException();
        }
    }
}
