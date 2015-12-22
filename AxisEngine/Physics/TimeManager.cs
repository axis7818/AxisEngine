using Microsoft.Xna.Framework;
using System;

namespace AxisEngine.Physics
{
    public class TimeManager
    {
        public bool TimeStopped = false;

        private float _timeMultiplier;

        public float TimeMultiplier
        {
            get { return TimeStopped ? 0 : _timeMultiplier; }
            set
            {
                if (value >= 0)
                    _timeMultiplier = value;
            }
        }
        
        public TimeManager(float timeMultiplier = 1)
        {
            TimeMultiplier = timeMultiplier;
        }

        public GameTime ConvertTime(GameTime t)
        {
            TimeSpan elapsed = new TimeSpan((long)(t.ElapsedGameTime.Ticks * TimeMultiplier));
            return new GameTime(t.TotalGameTime, elapsed, t.IsRunningSlowly);
        }
    }
}