using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxisEngine.Physics
{
    public class CollisionEventArgs : EventArgs
    {
        public ICollidable Other;
        public bool IsColliding;

        public CollisionEventArgs(ICollidable other, bool isColliding)
        {
            Other = other;
            IsColliding = isColliding;
        }
    }
}
