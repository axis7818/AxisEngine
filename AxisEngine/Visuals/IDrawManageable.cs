using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AxisEngine.Visuals
{
    public interface IDrawManageable
    {
        int DrawOrder { get; }

        Color Color { get; }

        Vector2 Origin { get; }

        Vector2 DrawPosition { get; }

        float Rotation { get; }

        Vector2 Scale { get; }

        SpriteEffects SpriteEffect { get; }

        Texture2D Texture { get; }

        float LayerDepth { get; }

        Rectangle? SourceRectangle { get; }

        Rectangle? DestinationRectangle { get; }

        bool Visible { get; }
    }
}