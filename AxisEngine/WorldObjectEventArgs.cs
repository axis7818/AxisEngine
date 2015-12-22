using System;

namespace AxisEngine
{
    public class WorldObjectEventArgs : EventArgs
    {
        public WorldObject WorldObject;

        public WorldObjectEventArgs(WorldObject worldObject)
        {
            WorldObject = worldObject;
        }
    }
}