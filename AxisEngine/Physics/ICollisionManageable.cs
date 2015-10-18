using Microsoft.Xna.Framework;

namespace AxisEngine.Physics
{
    /// <summary>
    /// allows an object to be managed by a CollisionManager
    /// </summary>
    public interface ICollisionManageable
    {
        /// <summary>
        /// checks whether or not there is an overlap with a trigger
        /// </summary>
        /// <param name="trigger">the trigger to check for overlap with</param>
        bool Intersects(Trigger trigger);

        /// <summary>
        /// the center of the CollisionMangeable 
        /// </summary>
        Point CenterPoint { get; }
    }
}