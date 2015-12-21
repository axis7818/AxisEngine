using System;
using Microsoft.Xna.Framework;

namespace AxisEngine
{
    public class Circle
    {
        private Point Position;
        private int radius;

        public Circle(int x, int y, int radius)
        {
            Position = new Point(x, y);
            this.radius = radius;
        }

        public Circle(Point center, int radius)
        {
            Position = center;
            this.radius = radius;
        }

        public static Circle Empty
        {
            get { return new Circle(0, 0, 0); }
        }

        public int Bottom
        {
            get { return Position.Y + radius; }
        }

        public Point Center
        {
            get { return Position; }
        }

        public bool IsEmpty
        {
            get { return Position.X == 0 && Position.Y == 0 && radius == 0; }
        }

        public int Left
        {
            get { return Position.X - radius; }
        }

        public int Right
        {
            get { return Position.X + radius; }
        }

        public int Radius
        {
            get { return radius; }
        }

        public int Top
        {
            get { return Position.Y - radius; }
        }

        public static Circle Intersect(Circle A, Circle B)
        {
            Circle bigger;
            Circle smaller;
            if (A.radius > B.radius)
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
                return new Circle(smaller.Center, smaller.radius);

            Vector2 distance = new Vector2(A.Center.X - B.Center.X, A.Center.Y - B.Center.Y);
            Point center = new Point((A.Center.X + B.Center.X) / 2, (A.Center.Y + B.Center.Y) / 2);
            int radius = (int)Math.Abs(distance.Length() - A.radius - B.radius) / 2;
            return new Circle(center, radius);
        }

        public static bool operator !=(Circle A, Circle B)
        {
            return !(A == B);
        }

        public static bool operator ==(Circle A, Circle B)
        {
            return A.Center.X == B.Center.X && A.Center.Y == B.Center.Y && A.radius == B.radius;
        }

        public static Circle Union(Circle A, Circle B)
        {
            Circle bigger;
            Circle smaller;
            if (A.radius > B.radius)
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
                return new Circle(bigger.Center, bigger.radius);

            Vector2 distance = new Vector2(A.Center.X - B.Center.X, A.Center.Y - B.Center.Y);
            Point center = new Point((A.Center.X + B.Center.X) / 2, (A.Center.Y + B.Center.Y) / 2);
            int radius = (int)Math.Abs(distance.Length() + A.radius + B.radius) / 2;
            return new Circle(center, radius);
        }

        public bool Contains(Circle other)
        {
            Vector2 distance = new Vector2(Position.X - other.Position.X, Position.Y - other.Position.Y);
            return (distance.Length() + other.radius) <= radius;
        }

        public bool Contains(Point point)
        {
            return Contains(new Circle(point, 0));
        }

        public bool Contains(int x, int y)
        {
            return Contains(new Circle(x, y, 0));
        }

        public bool Containts(float x, float y)
        {
            return Contains(new Circle((int)x, (int)y, 0));
        }

        public void Inflate(int amount)
        {
            radius += amount;
            if (radius < 0)
                throw new ArgumentException("Radius cannot be less than 0");
        }

        public void Inflate(float amount)
        {
            radius += (int)amount;
            if (radius < 0)
                throw new ArgumentException("Radius cannot be less than 0");
        }

        public bool Intersects(Circle other)
        {
            Vector2 distance = new Vector2(Center.X - other.Center.X, Center.Y - other.Center.Y);
            return distance.Length() <= radius + other.radius;
        }

        public void Offset(Point amount)
        {
            Offset(amount.X, amount.Y);
        }

        public void Offset(Vector2 amount)
        {
            Offset(amount.X, amount.Y);
        }

        public void Offset(int x, int y)
        {
            Position.X += x;
            Position.Y += y;
        }

        public void Offset(float x, float y)
        {
            Offset((int)x, (int)y);
        }

        public override string ToString()
        {
            string result = "";
            result += "X: " + Position.X;
            result += " Y: " + Position.Y;
            result += " r: " + radius;
            return result;
        }

        public override bool Equals(object obj)
        {
            return obj is Circle ? this == (Circle)obj : false;
        }

        public override int GetHashCode()
        {
            return radius.GetHashCode() + Position.GetHashCode();
        }
    }
}