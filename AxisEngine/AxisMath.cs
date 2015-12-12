using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxisEngine
{
    public static class AxisMath
    {
        public static float Clamp(float val, float min, float max)
        {
            val = val < min ? min : val;
            val = val > max ? max : val;
            return val;
        }
    }
}
