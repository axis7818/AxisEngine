using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AxisEngine.Visuals;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AxisEngine
{
    public static class WorldExtensions
    {
        public static Camera[,] SetSplitScreenGrid(this World world, int rows, int columns, int border = 0)
        {
            if (rows < 1 || columns < 1)
                throw new ArgumentException("The number of rows and columns for the splitscreen grid must be positive.");

            if (border < 0)
                throw new ArgumentException("The border must be atleast 0.");

            // get the width and height of each splitscreen cell
            int width = world.DefaultCamera.Viewport.Width;
            width -= border * (columns + 1);
            width /= columns;
            int height = world.DefaultCamera.Viewport.Height;
            height -= border * (rows + 1);
            height /= rows;
            Point cellSize = new Point(width, height);

            if (width <= 0 || height <= 0)
                throw new InvalidOperationException("The number of rows/columns, border width, and window size do not make a valid splitscreen configuration.");

            // disable all the other cameras
            foreach(Camera c in world.GetCameras())
            {
                c.Enabled = false;
            }
            
            // make the splitscreen cameras
            Camera[,] result = new Camera[columns, rows];
            for(int i = 0; i < columns; i++)
            {
                for(int j = 0; j < rows; j++)
                {
                    Point location = new Point(i * (width + border) + border, j * (height + border) + border);
                    Viewport v = new Viewport(location.X, location.Y, cellSize.X, cellSize.Y);
                    Camera c = new Camera("SplitScreen_" + i + "_" + j, v);
                    world.AddCamera(c);
                    result[i, j] = c;
                }
            }


            return result;
        }   
    }
}
