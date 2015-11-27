using System;
using System.Collections.Generic;

namespace AxisEngine
{
    public class WorldManager
    {
        private const string DEFAULT = "Default";
        private string _currentWorld;
        private Dictionary<string, World> _worlds;

        WorldManager(World defaultWorld)
        {
            _worlds = new Dictionary<string, World>();
            _currentWorld = DEFAULT;
            _worlds[DEFAULT] = defaultWorld;
            CurrentWorld.Initialize();
        }

        public event EventHandler<WorldChangingEventArgs> CurrentWorldChanging;

        public World CurrentWorld
        {
            get { return _worlds[_currentWorld]; }
        }

        public void SetCurrentWorld(string world)
        {
            // check for valid world
            if (!_worlds.ContainsKey(world))
                throw new ArgumentException("World Manager does not have the world: " + world);

            // fire the world changing event
            WorldChangingEventArgs args = new WorldChangingEventArgs(CurrentWorld, _worlds[world]);
            if (CurrentWorldChanging != null)
                CurrentWorldChanging(this, args);

            // check if the change was cancelled
            if (args.Cancel)
                return;

            // change the world
            CurrentWorld.Dispose();
            _currentWorld = world;
            CurrentWorld.Initialize();
        }
    }
}