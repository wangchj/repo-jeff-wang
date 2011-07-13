using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpatialIndex.GridRTree
{
    /// <summary>
    /// A general point for two and higher dimensions.
    /// </summary>
    [Serializable]
    public class Point
    {
        private double[] coords;

        /// <summary>
        /// Constructs a point with d dimensions.
        /// </summary>
        /// <param name="d">The number of dimension.</param>
        public Point(int d)
        {
            if (d < 2)
                throw new ArgumentOutOfRangeException();
            coords = new double[d];
        }

        /// <summary>
        /// Constructs an instance of Point with give coordinates.
        /// Elements is not copied.
        /// </summary>
        /// <param name="coords">The coordinates</param>
        public Point(double[] coords)
        {
            if (coords == null)
                throw new ArgumentNullException();
            if (coords.Length < 2)
                throw new ArgumentOutOfRangeException();
            this.coords = coords;
        }

        /// <summary>
        /// Gets or sets individual coordinate starting with 0.
        /// </summary>
        /// <param name="i">Non-negative value less than dimensions - 1</param>
        /// <returns></returns>
        public double this[int i]
        {
            get { return coords[i]; }
            set { coords[i] = value; }
        }

        /// <summary>
        /// Gets the number of dimension.
        /// </summary>
        public int Dimensions
        {
            get { return coords.Length; }
        }

        public double distance(Point p)
        {
            return distance(this, p);
        }

        /// <summary>
        /// Calcualte distance between two points.
        /// </summary>
        /// <remarks>
        /// Currently this method does not check if two points have the
        /// same number of dimensions. If points have different dimensions
        /// exception will be thrown.
        /// </remarks>
        /// <param name="p1">Point 1</param>
        /// <param name="p2">Point 2</param>
        /// <returns></returns>
        public static double distance(Point p1, Point p2)
        {
            double squareSum = 0;
            for (int i = 0; i < p1.Dimensions; i++)
            {
                squareSum += Math.Pow(p1[i] - p2[i], 2);
            }
            return Math.Sqrt(squareSum);
        }

        /// <summary>
        /// Two points are equal if all coordinates are equal.
        /// </summary>
        /// <param name="obj">An instance of Point class</param>
        /// <returns>true if the two objects have the same number of
        /// dimensions and value of all coordinate are equal, false
        /// otherwise</returns>
        public override bool Equals(object obj)
        {
            Point p = (Point)obj;
            if (p.Dimensions != Dimensions)
                return false;

            for (int i = 0; i < Dimensions; i++)
            {
                if (p[i] != coords[i])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            for (int i = 0; i < coords.Length; i++)
            {
                s.Append(coords[i]);
                if (i < coords.Length - 1)
                    s.Append(',');
            }
            return s.ToString();
        }

        /// <summary>
        /// Make a new instance of Point with same coordinates (deep copy).
        /// </summary>
        /// <returns>A deep copy of this instance.</returns>
        public Point Copy()
        {
            double[] newCoords = new double[coords.Length];
            Array.Copy(coords, newCoords, coords.Length);
            return new Point(newCoords);
        }
    }
}
