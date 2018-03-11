using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace newton
{
    public static class MathHelper
    {
        public static Point AddPoints(Point theFirstPoint, Point theSecondPoint)
        {
            return new Point(theFirstPoint.X + theSecondPoint.X, theFirstPoint.Y + theSecondPoint.Y);
        }

        public static Point SubPoints(Point theFirstPoint, Point theSecondPoint)
        {
            return new Point(theFirstPoint.X - theSecondPoint.X, theFirstPoint.Y - theSecondPoint.Y);
        }

        public static Point GetDiffVector(Point theFirstPoint, Point theSecondPoint)
        {
            var aDeltaX = theFirstPoint.X - theSecondPoint.X;
            var aDeltaY = theFirstPoint.Y - theSecondPoint.Y;
            return new Point(aDeltaX, aDeltaY);
        }

        public static double GetDistance(Point theFirstPoint, Point theSecondPoint)
        {
            var aDiff = GetDiffVector(theFirstPoint, theSecondPoint);
            return Math.Sqrt(aDiff.X * aDiff.X + aDiff.Y * aDiff.Y);
        }
    }
}
