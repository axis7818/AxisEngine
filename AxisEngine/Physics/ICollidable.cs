using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxisEngine.Physics
{
    public interface ICollidable
    {
        ColliderType Type { get; }

        bool Intersects(ICollidable coll);
    }
}
