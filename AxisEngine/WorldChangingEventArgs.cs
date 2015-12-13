using System;

namespace AxisEngine
{
    public class WorldChangingEventArgs : EventArgs
    {
        public string NewWorld;
        public string OldWorld;
        public bool Cancel;

        public WorldChangingEventArgs(string oldWolrd, string newWorld)
        {
            OldWorld = oldWolrd;
            NewWorld = newWorld;
            Cancel = false;
        }
    }
}