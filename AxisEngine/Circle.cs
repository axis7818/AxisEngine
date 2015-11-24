using System;

//TODO: test the circle and make sure everything works

namespace Microsoft.Xna.Framework
{
    public class Circle
    {
        private Point Position;

        private int Radius;

        public Circle(int x, int y, int radius)
        {
            Position = new Point(x, y);
            Radius = radius;
        }

        public Circle(Point center, int radius)
        {
            Position = center;
            Radius = radius;
        }

        public static Circle Empty
        {
            get
            {
                return new Circle(0, 0, 0);
            }
        }

        public int Bottom
        {
            get
            {
                return Position.Y + Radius;
            }
        }

        public Point Center
        {
            get
            {
                return Position;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return Position.X == 0 && Position.Y == 0 && Radius == 0;
            }
        }

        public int Left
        {
            get
            {
                return Position.X - Radius;
            }
        }

        public int Right
        {
            get
            {
                return Position.X + Radius;
            }
        }

        public int Size
        {
            get
            {
                return Radius;
            }
        }

        public int Top
        {
            get
            {
                return Position.Y - Radius;
            }
        }

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

        public static bool operator !=(Circle A, Circle B)
        {
            return !(A == B);
        }

        public static bool operator ==(Circle A, Circle B)
        {
            return A.Center.X == B.Center.X && A.Center.Y == B.Center.Y && A.Radius == B.Radius;
        }

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

        public bool Contains(Circle other)
        {
            Vector2 distance = new Vector2(Position.X - other.Position.X, Position.Y - other.Position.Y);
            return (distance.Length() + other.Radius) <= Radius;
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
            Radius += amount;
            if (Radius < 0)
                throw new ArgumentException("Radius cannot be less than 0");
        }

        public void Inflate(float amount)
        {
            Radius += (int)amount;
            if (Radius < 0)
                throw new ArgumentException("Radius cannot be less than 0");
        }

        public bool Intersects(Circle other)
        {
            Vector2 distance = new Vector2(Center.X - other.Center.X, Center.Y - other.Center.Y);
            return distance.Length() <= Radius + other.Radius;
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
            result += " r: " + Radius;
            return result;
        }

        public override bool Equals(object obj)
        {
            return obj is Circle ? this == (Circle)obj : false;
        }

        public override int GetHashCode()
        {
            return Radius.GetHashCode() + Position.GetHashCode();
        }
    }
}