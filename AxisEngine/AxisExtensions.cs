using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace AxisEngine
{
    public static class AxisExtensions
    {
        public static Vector2 PointToVector2(this Point p)
        {
            return new Vector2(p.X, p.Y);
        }
    }
}
