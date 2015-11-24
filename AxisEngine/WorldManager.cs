using System;
using System.Collections.Generic;

namespace AxisEngine
{
    public static class WorldManager
    {
        private static World _currentWorld;
        private static Dictionary<string, World> Worlds;

        static WorldManager()
        {
            Worlds = new Dictionary<string, World>();
        }

        public static event EventHandler<WorldChangingEventArgs> CurrentWorldChanged;

        public static World CurrentWorld
        {
            get
            {
                return _currentWorld;
            }
            private set
            {
                OnCurrentWorldChanging(_currentWorld, value);
                _currentWorld = value;
            }
        }

        private static void OnCurrentWorldChanging(World oldWorld, World newWorld)
        {
            if (CurrentWorldChanged != null)
                CurrentWorldChanged(null, new WorldChangingEventArgs(oldWorld, newWorld));
        }
    }
}