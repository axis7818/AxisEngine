using System;
using System.Collections.Generic;

namespace AxisEngine
{
    /// <summary>
    /// an object to facilitate the transitions between Worlds
    /// </summary>
    public static class WorldManager
    {
        /// <summary>
        /// the world currently activated
        /// </summary>
        private static World _currentWorld;

        /// <summary>
        /// the worlds within the manager
        /// </summary>
        private static Dictionary<string, World> Worlds;

        /// <summary>
        /// initializes the WorldManager
        /// </summary>
        static WorldManager()
        {
            Worlds = new Dictionary<string, World>();
        }

        /// <summary>
        /// fires when the current world is changing
        /// </summary>
        public static event EventHandler<WorldChangingEventArgs> CurrentWorldChanged;

        /// <summary>
        /// the world currently activated
        /// </summary>
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

        /// <summary>
        /// handles the CurrentWorldChanged event
        /// </summary>
        /// <param name="sender">the object that triggered the event</param>
        /// <param name="args">the arguments associated with the event</param>
        private static void OnCurrentWorldChanging(World oldWorld, World newWorld)
        {
            if (CurrentWorldChanged != null)
                CurrentWorldChanged(null, new WorldChangingEventArgs(oldWorld, newWorld));
        }
    }
}