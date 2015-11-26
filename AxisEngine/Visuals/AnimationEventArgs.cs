using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxisEngine.Visuals
{
    public class AnimationEventArgs : EventArgs
    {
        public Animation Animation;

        public AnimationEventArgs(Animation animation)
        {
            Animation = animation;
        }
    }
}
