namespace AxisEngine
{
    public class WorldChangingEventArgs
    {
        public World NewWorld;

        public World OldWorld;

        public WorldChangingEventArgs(World oldWolrd, World newWorld)
        {
            OldWorld = oldWolrd;
            NewWorld = newWorld;
        }
    }
}