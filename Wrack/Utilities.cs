using System;
using Microsoft.Xna.Framework;

namespace WrackEngine
{
    public static class Utilities
    {
        private static Random random = new Random();

        public static int RandomInt(int min, int max)
        {
            return random.Next(min, max);
        }

        public static int RandomInt(int max)
        {
            return random.Next(max);
        }

        public static float Random()
        {
            return (float)random.NextDouble();
        }

        public static void Limit(ref int x, int min, int max)
        {
            if (x < min) x = min;
            if (x > max) x = max;
        }

        public static void Limit(ref float x, float min, float max)
        {
            if (x < min) x = min;
            if (x > max) x = max;
        }

        public static Vector2 GetPenetrationVector(Vector2Rectangle vr1, Vector2Rectangle vr2)
        {
            Vector2 max1 = vr1.Position + vr1.Size;
            Vector2 max2 = vr2.Position + vr2.Size;

            if (vr1.Position.X < max2.X &&
                max1.X > vr2.Position.X &&
                vr1.Position.Y < max2.Y &&
                max1.Y > vr2.Position.Y)
            {

            }
            else
            {
                return Vector2.Zero;
            }

            Vector2 mtd = new Vector2();

            float left = vr2.Position.X - max1.X;
            float right = max2.X - vr1.Position.X;
            float top = vr2.Position.Y - max1.Y;
            float bottom = max2.Y - vr1.Position.Y;

            if (left > 0 || right < 0) { return Vector2.Zero; }
            if (top > 0 || bottom < 0) { return Vector2.Zero; }

            if (Math.Abs(left) < right) { mtd += new Vector2(left, 0); } else { mtd += new Vector2(right, 0); }
            if (Math.Abs(top) < bottom) { mtd += new Vector2(0, top); } else { mtd += new Vector2(0, bottom); }

            return mtd;
        }

        public static bool AreIntersecting(Vector2Rectangle vr1, Vector2Rectangle vr2)
        {
            Vector2 max1 = vr1.Position + vr1.Size;
            Vector2 max2 = vr2.Position + vr2.Size;
            return vr1.Position.X < max2.X && max1.X > vr2.Position.X &&
                vr1.Position.Y < max2.Y && max1.Y > vr2.Position.Y;
        }

        public static Vector2 FloorVector(Vector2 v)
        {
            return new Vector2((int)v.X, (int)v.Y);
        }

        public static void GetLongestLeg(ref Vector2 v)
        {
            if (Math.Abs(v.X) > Math.Abs(v.Y))
            {
                v = new Vector2(v.X, 0);
            }
            else
            {
                v = new Vector2(0, v.Y);
            }
        }

        public static void GetShortestLeg(ref Vector2 v)
        {
            if (Math.Abs(v.X) < Math.Abs(v.Y))
            {
                v = new Vector2(v.X, 0);
            }
            else
            {
                v = new Vector2(0, v.Y);
            }
        }

        public static void FlipVector(ref Vector2 v)
        {
            float q = v.Y;
            v.X = v.Y;
            v.Y = q;
        }

        public static Rectangle VectorsToRectangle(Vector2 pos, Vector2 size)
        {
            return new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y);
        }

        public static Rectangle TranslateRectangle(Rectangle rect, Vector2 translationVector)
        {
            rect.X += (int)translationVector.X;
            rect.Y += (int)translationVector.Y;
            return rect;
        }

        public static bool PointIsInRect(Vector2 point, Rectangle r)
        {
            return (point.X >= r.X && point.X <= r.X + r.Width && point.Y >= r.Y && point.Y <= r.Y + r.Height);
        }
    }

    public struct Vector2Rectangle
    {
        public Vector2 Position;
        public Vector2 Size;

        public Vector2Rectangle(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = size;
        }

        public Vector2Rectangle(IntVector2 position, Vector2 size)
        {
            Position = new Vector2(position.X, position.Y);
            Size = size;
        }

        public Vector2Rectangle(int x, int y, int w, int h)
        {
            Position = new Vector2(x, y);
            Size = new Vector2(w, h);
        }

        public Vector2Rectangle(float x, float y, float w, float h)
        {
            Position = new Vector2(x, y);
            Size = new Vector2(w, h);
        }

        public Vector2Rectangle(Vector2 position, IntVector2 size)
        {
            Position = position;
            Size = new Vector2(size.X, size.Y);
        }

        public Vector2Rectangle(Rectangle r)
        {
            Position = new Vector2(r.X, r.Y);
            Size = new Vector2(r.Width, r.Height);
        }

        public Rectangle GetRect()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
        }
    }

    public struct IntVector2
    {
        public int X;
        public int Y;

        public IntVector2(int x, int y)
        {
            X = x;
            Y = y;
        }

        public IntVector2(float x, float y)
        {
            X = (int)x;
            Y = (int)y;
        }

        public IntVector2(Vector2 v) : this(v.X, v.Y) { }
    }
}
