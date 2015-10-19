namespace AxisEngine
{
    /// <summary>
    /// event arguments with details about worlds changing
    /// </summary>
    public class WorldChangingEventArgs
    {
        /// <summary>
        /// the world that is being switched to
        /// </summary>
        public World NewWorld;

        /// <summary>
        /// the world that is being switched away from
        /// </summary>
        public World OldWorld;

        /// <summary>
        /// creates a new WorldEventArgs object
        /// </summary>
        /// <param name="world">the world this event is associated with</param>
        public WorldChangingEventArgs(World oldWolrd, World newWorld)
        {
            OldWorld = oldWolrd;
            NewWorld = newWorld;
        }
    }
}