namespace AxisEngine
{
    public static class AxisMath
    {
        public static float Clamp(float val, float min, float max)
        {
            if (val < min)
                return min;
            if (val > max)
                return max;
            return val;
        }

        public static double Clamp(double val, double min, double max)
        {
            if (val < min)
                return min;
            if (val > max)
                return max;
            return val;
        }

        public static bool Between(int val, int min, int max)
        {
            return val >= min && val <= max;
        }
    }
}
