using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AxisEngine.Debug
{
    public static class Grid
    {
        /// <summary>
        /// The number of pixels between each grid line
        /// </summary>
        public static int GridSpacing = 10;

        /// <summary>
        /// The number of minor grid lines between major grid lines
        /// </summary>
        public static int MajorGridSpacing = 5;

        /// <summary>
        /// The color to draw the Major grid lines
        /// </summary>
        public static Color MajorGridColor = Color.LimeGreen;

        /// <summary>
        /// The color to draw the minor grid lines
        /// </summary>
        public static Color MinorGridColor = Color.LightSeaGreen;

        /// <summary>
        /// The visibility of the grid
        /// </summary>
        private static bool _visible = false;

        /// <summary>
        /// The visibility of the grid
        /// </summary>
        public static bool Visible
        {
            get
            {
                return _visible;
            }
            set
            {
                _visible = value;
                if(VisibleChanged != null)
                {
                    VisibleChanged(null, new EventArgs());
                }
            }
        }

        /// <summary>
        /// fired when visible changes
        /// </summary>
        public static event EventHandler<EventArgs> VisibleChanged;

        /// <summary>
        /// draws the grid
        /// </summary>
        public static void Draw(GraphicsDevice graphicsDevice)
        {
            if (Visible)
            {
                SpriteBatch gridSpriteBatch = new SpriteBatch(graphicsDevice);
                Texture2D line = new Texture2D(graphicsDevice, 1, 1);
                line.SetData(new Color[] { Color.White });

                gridSpriteBatch.Begin();

                DrawGrid(gridSpriteBatch, line, GridSpacing, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);

                gridSpriteBatch.End();
            }
        }

        private static void DrawGrid(SpriteBatch spriteBatch, Texture2D texture, int gridSpacing, int screenWidth, int screenHeight)
        {
            // Draw vertical lines
            int count = 0;
            for(int i = 0; i < screenWidth + 1; i += GridSpacing, count++)
            {
                // get the color, start, and end position
                Color color = count % MajorGridSpacing == 0 ? MajorGridColor : MinorGridColor;
                Vector2 start = new Vector2(i, 0);
                Vector2 end = new Vector2(i, screenHeight);

                // draw the line
                DrawLine(spriteBatch, texture, color, start, end);
            }

            // Draw horizontal lines
            count = 0; 
            for(int i = 0; i < screenHeight + 1; i += GridSpacing, count++)
            {
                // get the color, start, and end position
                Color color = count % MajorGridSpacing == 0 ? MajorGridColor : MinorGridColor;
                Vector2 start = new Vector2(0, i);
                Vector2 end = new Vector2(screenWidth, i);

                // draw the line
                DrawLine(spriteBatch, texture, color, start, end);
            }
        }

        /// <summary>
        /// draws a line
        /// </summary>
        private static void DrawLine(SpriteBatch spriteBatch, Texture2D texture, Color color, Vector2 start, Vector2 end)
        {
            Vector2 edge = end - start;
            float angle = (float)Math.Atan2(edge.Y, edge.X);

            spriteBatch.Draw(texture,
                new Rectangle(
                    (int)start.X,
                    (int)start.Y,
                    (int)edge.Length(),
                    1),
                null,
                color,
                angle,
                Vector2.Zero,
                SpriteEffects.None,
                0);
        }
    }
}
