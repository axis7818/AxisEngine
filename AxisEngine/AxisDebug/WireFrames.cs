using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AxisEngine;

namespace AxisEngine.AxisDebug
{
    public static class WireFrames
    {
        public static Color Color = Color.Yellow;
        public static GraphicsDevice GraphicsDevice = null;
        
        public static Texture2D BoxWireFrame(Rectangle rectangle)
        {
            if (GraphicsDevice == null)
                throw new MissingFieldException("Must set the GraphicsDevice field on WireFrames before any wire frames can be generated.");

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

            Texture2D result = new Texture2D(GraphicsDevice, width, height);
            result.SetData(data);
            return result;
        }
        
        public static Texture2D CircleWireFrame(Circle circle)
        {
            if (GraphicsDevice == null)
                throw new MissingFieldException("Must set the GraphicsDevice field on WireFrames before any wire frames can be generated.");

            int diameter = circle.Radius * 2;
            Color[] data = new Color[diameter * diameter];

            double delta = Math.Atan((double)1 / circle.Radius);
            for(double t = 0; t < 2 * Math.PI + delta; t += delta)
            {
                double y = circle.Radius * (1 - Math.Sin(t));
                int y_int = (int)AxisMath.Clamp(y, 0, diameter);
                double x = circle.Radius * (1 + Math.Cos(t));
                int x_int = (int)AxisMath.Clamp(x, 0, diameter);

                int i = y_int * diameter + x_int;

                data[i] = Color;
            }

            Texture2D result = new Texture2D(GraphicsDevice, diameter, diameter);
            result.SetData(data);
            return result;
        }
    }
}
