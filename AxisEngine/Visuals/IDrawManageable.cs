using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AxisEngine.Visuals
{
    public interface IDrawManageable
    {
        int DrawOrder { get; }

        bool Visible { get; }

        Rectangle DrawArea { get; }

        void Draw(SpriteBatch spriteBatch, Camera camera);

        event EventHandler<EventArgs> DrawOrderChanged;
    }
}