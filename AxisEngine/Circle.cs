using System;

//TODO: test the circle and make sure everything works

namespace Microsoft.Xna.Framework
{
    /// <summary>
    /// defines a circle in a coordinate system and contains the functionality of MonoGame's Rectangle class
    /// </summary>
    public class Circle
    {
        /// <summary>
        /// the position of the circle on a screen grid
        /// </summary>
        private Point Position;

        /// <summary>
        /// the radius of the circle
        /// </summary>
        private int Radius;

        /// <summary>
        /// instantiates a circle
        /// </summary>
        /// <param name="x">the x position of the center</param>
        /// <param name="y">the y position of the center</param>
        /// <param name="radius">the radius of the circle</param>
        public Circle(int x, int y, int radius)
        {
            Position = new Point(x, y);
            Radius = radius;
        }

        /// <summary>
        /// instantiates a circle
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        public Circle(Point center, int radius)
        {
            Position = center;
            Radius = radius;
        }

        /// <summary>
        /// returns a circle with position (0, 0) and radius of 0
        /// </summary>
        public static Circle Empty
        {
            get
            {
                return new Circle(0, 0, 0);
            }
        }

        /// <summary>
        /// gets the x coordinate of the bottom of the circle
        /// </summary>
        public int Bottom
        {
            get
            {
                return Position.Y + Radius;
            }
        }

        /// <summary>
        /// gets the coordinates of the center of the circle
        /// </summary>
        public Point Center
        {
            get
            {
                return Position;
            }
        }

        /// <summary>
        /// gets whether or not the circle has radius of 0 and position of (0, 0)
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return Position.X == 0 && Position.Y == 0 && Radius == 0;
            }
        }

        /// <summary>
        /// gets the x coordinate of the Left side of the circle
        /// </summary>
        public int Left
        {
            get
            {
                return Position.X - Radius;
            }
        }

        /// <summary>
        /// gets the x coordinate of the right side of the circle
        /// </summary>
        public int Right
        {
            get
            {
                return Position.X + Radius;
            }
        }

        /// <summary>
        /// gets the "size" (radius) of the circle
        /// </summary>
        public int Size
        {
            get
            {
                return Radius;
            }
        }

        /// <summary>
        /// gets the y coordinate of the top of the circle
        /// </summary>
        public int Top
        {
            get
            {
                return Position.Y - Radius;
            }
        }

        /// <summary>
        /// returns a circle (of minimum size) that intersects both the given circle
        /// </summary>
        public static Circle Intersect(Circle A, Circle B)
        {
            Circle bigger;
            Circle smaller;
            if (A.Radius > B.Radius)
            {
                bigger = B;
                smaller = A;
            }
            else
            {
                bigger = A;
                smaller = B;
            }

            if (bigger.Contains(smaller))
                return new Circle(smaller.Center, smaller.Radius);

            Vector2 distance = new Vector2(A.Center.X - B.Center.X, A.Center.Y - B.Center.Y);
            Point center = new Point((A.Center.X + B.Center.X) / 2, (A.Center.Y + B.Center.Y) / 2);
            int radius = (int)Math.Abs(distance.Length() - A.Radius - B.Radius) / 2;
            return new Circle(center, radius);
        }

        /// <summary>
        /// checks if two circles dont have the same values
        /// </summary>
        public static bool operator !=(Circle A, Circle B)
        {
            return !(A == B);
        }

        /// <summary>
        /// checks if two circles have the same values
        /// </summary>
        public static bool operator ==(Circle A, Circle B)
        {
            return A.Center.X == B.Center.X && A.Center.Y == B.Center.Y && A.Radius == B.Radius;
        }

        /// <summary>
        /// returns a circle (of minimum size) that completely contains both of the given circles
        /// </summary>
        public static Circle Union(Circle A, Circle B)
        {
            Circle bigger;
            Circle smaller;
            if (A.Radius > B.Radius)
            {
                bigger = A;
                smaller = B;
            }
            else
            {
                bigger = B;
                smaller = A;
            }

            if (bigger.Contains(smaller))
                return new Circle(bigger.Center, bigger.Radius);

            Vector2 distance = new Vector2(A.Center.X - B.Center.X, A.Center.Y - B.Center.Y);
            Point center = new Point((A.Center.X + B.Center.X) / 2, (A.Center.Y + B.Center.Y) / 2);
            int radius = (int)Math.Abs(distance.Length() + A.Radius + B.Radius) / 2;
            return new Circle(center, radius);
        }

        /// <summary>
        /// checks whether or not the given circle is within this circle
        /// </summary>
        /// <param name="other">the circle to check if its in this circle</param>
        public bool Contains(Circle other)
        {
            Vector2 distance = new Vector2(Position.X - other.Position.X, Position.Y - other.Position.Y);
            return (distance.Length() + other.Radius) <= Radius;
        }

        /// <summary>
        /// checks if a given point is in this circle
        /// </summary>
        /// <param name="point">the point to check</param>
        public bool Contains(Point point)
        {
            return Contains(new Circle(point, 0));
        }

        /// <summary>
        /// checks if the given coordinates are in this circle
        /// </summary>
        /// <param name="x">the x coordinate to check</param>
        /// <param name="y">the y coordinate to check</param>
        public bool Contains(int x, int y)
        {
            return Contains(new Circle(x, y, 0));
        }

        /// <summary>
        /// checks if the given coordinates are in this circle
        /// </summary>
        /// <param name="x">the x coordinate to check</param>
        /// <param name="y">the y coordinate to check</param>
        public bool Containts(float x, float y)
        {
            return Contains(new Circle((int)x, (int)y, 0));
        }

        /// <summary>
        /// increases the radius by the given amount
        /// </summary>
        /// <param name="amount">the amount to increase the radius by</param>
        public void Inflate(int amount)
        {
            Radius += amount;
            if (Radius < 0)
                throw new ArgumentException("Radius cannot be less than 0");
        }

        /// <summary>
        /// increases the radius by the given amount
        /// </summary>
        /// <param name="amount">the amount to increase the radius by</param>
        public void Inflate(float amount)
        {
            Radius += (int)amount;
            if (Radius < 0)
                throw new ArgumentException("Radius cannot be less than 0");
        }

        /// <summary>
        /// checks if a given cricle intersects this circle
        /// </summary>
        public bool Intersects(Circle other)
        {
            Vector2 distance = new Vector2(Center.X - other.Center.X, Center.Y - other.Center.Y);
            return distance.Length() <= Radius + other.Radius;
        }

        /// <summary>
        /// shifts the circle's position by the given amount
        /// </summary>
        /// <param name="amount"></param>
        public void Offset(Point amount)
        {
            Offset(amount.X, amount.Y);
        }

        /// <summary>
        /// shifts the circle's position by the given amount
        /// </summary>
        public void Offset(Vector2 amount)
        {
            Offset(amount.X, amount.Y);
        }

        /// <summary>
        /// shifts the circle's position by the given amount
        /// </summary>
        public void Offset(int x, int y)
        {
            Position.X += x;
            Position.Y += y;
        }

        /// <summary>
        /// shifts the circle's position by the given amount
        /// </summary>
        public void Offset(float x, float y)
        {
            Offset((int)x, (int)y);
        }

        /// <summary>
        /// a string representation of the circle "X: [X] Y: [Y] r: [Radius]"
        /// </summary>
        public override string ToString()
        {
            string result = "";
            result += "X: " + Position.X;
            result += " Y: " + Position.Y;
            result += " r: " + Radius;
            return result;
        }

        /// <summary>
        /// compares another object with this one
        /// </summary>
        /// <param name="obj">the other object to compare to</param>
        /// <returns>Whether or not the object is a Circle with the same parameters as this</returns>
        public override bool Equals(object obj)
        {
            return obj is Circle ? this == (Circle)obj : false;
        }

        /// <summary>
        /// gets a hash code for the object
        /// </summary>
        public override int GetHashCode()
        {
            return Radius.GetHashCode() + Position.GetHashCode();
        }
    }
}