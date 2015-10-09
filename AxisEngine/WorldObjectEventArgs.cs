using System;

namespace AxisEngine
{
    /// <summary>
    /// Event arguments pertaining to a world object
    /// </summary>
    public class WorldObjectEventArgs : EventArgs
    {
        /// <summary>
        /// the world object that the event pertains to
        /// </summary>
        public WorldObject WorldObject;
    }
}