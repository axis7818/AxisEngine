using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxisEngine.Physics
{
    public class CollisionEventArgs : EventArgs
    {
        public ICollidable A;
        public ICollidable B;
        public bool IsColliding;

        public CollisionEventArgs(ICollidable A, ICollidable B, bool isColliding)
        {
            this.A = A;
            this.B = B;
            IsColliding = isColliding;
        }
    }
}
