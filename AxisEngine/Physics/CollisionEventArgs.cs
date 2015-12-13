using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxisEngine.Physics
{
    public class CollisionEventArgs : EventArgs
    {
        Tuple<ICollidable, ICollidable> Involved;

        public CollisionEventArgs(ICollidable A, ICollidable B)
        {
            Involved = new Tuple<ICollidable, ICollidable>(A, B);
        }
    }
}
