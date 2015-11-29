using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AxisEngine;

namespace AxisEngine.Visuals
{
    public class TextSprite : WorldObject, IDrawManageable
    {
        private SpriteFont _spriteFont;
        private int _drawOrder;
        private bool _visible;
        
        public TextSprite(SpriteFont font, string text, Color color)
        {
            _spriteFont = font;
            Text = text;
            Color = color;

            _drawOrder = 0;
            _visible = true;
        }

        public event EventHandler<EventArgs> DrawOrderChanged;

        public event EventHandler<EventArgs> VisibleChanged;

        public SpriteFont SpriteFont
        {
            get { return _spriteFont; }
        }

        public string Text { get; set; }

        public Color Color { get; set; }

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

        public bool Visible
        {
            get { return _visible; }
            set
            {
                _visible = value;
                if (VisibleChanged != null)
                    VisibleChanged(this, EventArgs.Empty);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(SpriteFont, Text, Position, Color);
        }

        public static implicit operator string(TextSprite textSprite)
        {
            return textSprite.Text;
        }
    }
}
