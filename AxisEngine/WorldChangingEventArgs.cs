using System;

namespace AxisEngine
{
    public class WorldChangingEventArgs : EventArgs
    {
        public string NewWorld;
        public string OldWorld;
        public bool Cancel = false;
        public bool Quit = false;

        public WorldChangingEventArgs(string oldWolrd, string newWorld)
        {
            OldWorld = oldWolrd;
            NewWorld = newWorld;
        }

        public static WorldChangingEventArgs QuittingArgs
        {
            get
            {
                WorldChangingEventArgs result = new WorldChangingEventArgs(null, null);
                result.Quit = true;
                return result;
            }
        }
    }
}