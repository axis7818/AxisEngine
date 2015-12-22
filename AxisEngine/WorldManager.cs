using System;
using System.Collections.Generic;

namespace AxisEngine
{
    public class WorldManager
    {
        private string _defaultWorld;
        private string _currentWorld;
        private Dictionary<string, World> _worlds;

        public WorldManager(World defaultWorld)
        {
            _worlds = new Dictionary<string, World>();
            _defaultWorld = defaultWorld.Name;           
            _currentWorld = _defaultWorld;
            AddWorld(defaultWorld);
            CurrentWorld.Initialize();
        }

        public event EventHandler<WorldChangingEventArgs> CurrentWorldChanging;
        public event EventHandler<EventArgs> ReadyToQuit;

        public World CurrentWorld
        {
            get { return _worlds[_currentWorld]; }
        }

        public void AddWorld(World world)
        {
            _worlds[world.Name] = world;
            world.EndWorld += WorldEndedHandler;
        }

        private void WorldEndedHandler(object sender, WorldChangingEventArgs args)
        {
            if (args.Quit)
            {
                if (ReadyToQuit != null)
                    ReadyToQuit(this, EventArgs.Empty);
                return;
            }

            if (!_worlds.ContainsKey(args.NewWorld))
                throw new InvalidOperationException("could not find world: " + args.NewWorld);

            if (CurrentWorldChanging != null)
                CurrentWorldChanging(this, args);

            if (args.Cancel)
                return;

            SetCurrentWorld(args.NewWorld);
        }

        private void SetCurrentWorld(string world)
        {
            CurrentWorld.Dispose();
            _currentWorld = world;
            CurrentWorld.Initialize();
        }
    }
}