using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace AxisEngine.Physics
{
    public class TimeManager
    {
        /// <summary>
        /// The factor to scale the passing of time by
        /// </summary>
        private float _timeMultiplier;

        /// <summary>
        /// The factor to scale the passing of time by
        /// </summary>
        public float TimeMultiplier
        {
            get
            {
                return TimeStopped ? 0 : _timeMultiplier;
            }
            set
            {
                if (value >= 0)
                    _timeMultiplier = value;
            }
        }

        /// <summary>
        /// starts and stoppes time depending on the value
        /// </summary>
        public bool TimeStopped = false;

        /// <summary>
        /// creates an instance of TimeManager and sets some values
        /// </summary>
        /// <param name="timeMultiplier">the factor to scale the passing of time by</param>
        public TimeManager(float timeMultiplier = 1)
        {
            TimeMultiplier = timeMultiplier;
        }

        /// <summary>
        /// Scales the game time accordingly
        /// </summary>
        /// <param name="t">the initial time since the last frame</param>
        /// <returns>a scaled time factor</returns>
        public GameTime ConvertTime(GameTime t)
        {
            TimeSpan elapsed = new TimeSpan((long)(t.ElapsedGameTime.Ticks * TimeMultiplier));
            return new GameTime(t.TotalGameTime, elapsed, t.IsRunningSlowly);
        }
    }
}
