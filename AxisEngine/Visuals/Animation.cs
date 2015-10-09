using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace AxisEngine.Visuals
{
    /// <summary>
    /// A collection of animation frames that are strung together to form the animation
    /// </summary>
    public class Animation : List<AnimationFrame>
    {
        /// <summary>
        /// The amount of time to complete the animation
        /// </summary>
        public float Length;

        /// <summary>
        /// the index of the frame to be displayed
        /// </summary>
        private int CurrentFrame = 0;

        /// <summary>
        /// a timer to count the time (in milliseconds) that the animation has been played. The timer resets after the animation loops
        /// </summary>
        private float Timer = 0.0f;

        /// <summary>
        /// initializes an animation
        /// </summary>
        /// <param name="length">the amount of time in seconds that it takes to complete the animation</param>
        public Animation(float length)
        {
            Length = length;
        }

        /// <summary>
        /// gets the frame that is currently being displayed
        /// </summary>
        public AnimationFrame Frame
        {
            get
            {
                return this[CurrentFrame];
            }
        }

        /// <summary>
        /// Checks to see if the frames on the animation have the correct indexes for the available frames
        /// </summary>
        /// <param name="numberOfFrames">the number of frames that the animation has access to</param>
        /// <returns>whether or not the each animation frame has the correct indexing</returns>
        public bool HasValidFrames(int numberOfFrames)
        {
            return this.All(frame => frame.Index >= 0 && frame.Index <= numberOfFrames);
        }

        /// <summary>
        /// updates the animation
        /// </summary>
        /// <param name="t">the time elapsed since the last update</param>
        public void Update(GameTime t)
        {
            // update the timer
            Timer += t.ElapsedGameTime.Milliseconds;
            Timer %= Length * 1000;

            // get the current frame
            int frame = (int)(Timer * Count / (Length * 1000));
            CurrentFrame = frame;
        }

        /// <summary>
        /// Resets the animation to the first frame
        /// </summary>
        public void Reset()
        {
            Timer = 0.0f;
            CurrentFrame = 0;
        }
    }
}