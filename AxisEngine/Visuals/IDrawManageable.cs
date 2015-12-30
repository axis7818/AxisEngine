using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AxisEngine.Visuals
{
    public interface IDrawManageable
    {
        int DrawOrder { get; }

        bool Visible { get; }

        void Draw(SpriteBatch spriteBatch, Camera camera);

        bool IsViewableTo(Camera camera);

        event EventHandler<EventArgs> DrawOrderChanged;
    }
}