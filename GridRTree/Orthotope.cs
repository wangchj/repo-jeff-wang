using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpatialIndex.GridRTree
{
    [Serializable]
    public class Orthotope
    {
        private Point min;
        private Point max;


        /// <summary>
        /// Construct an Orthotope from two corners.
        /// </summary>
        /// <param name="min">The min corner</param>
        /// <param name="max">The max corner</param>
        /// <remarks>
        /// This method does not make a deep copy of the parameters.
        /// </remarks>
        public Orthotope(Point min, Point max)
        {
            if (min == null || max == null)
                throw new ArgumentNullException();
            if (min.Dimensions != max.Dimensions)
                throw new ArgumentException();

            //Check the validaty of the input
            for (int i = 0; i < min.Dimensions; i++)
            {
                if (min[i] > max[i])
                    throw new ArgumentException();
            }

            this.min = min.Copy();
            this.max = max.Copy();
        }

        public Point Min
        {
            get { return min; }
        }

        public Point Max
        {
            get { return max; }
        }

        /// <summary>
        /// Returns the number of dimension of this orthotope.
        /// </summary>
        public int Dimensions
        {
            get { return min.Dimensions; }
        }

        /// <summary>
        ///  Determine whether this rectangle intersects the passed rectangle.
        /// </summary>
        /// <param name="o">The rectangle that might intersect this rectangle</param>
        /// <returns>true if the rectangles intersect, false if they do
        /// not intersect</returns>
        public bool Intersects(Orthotope o)
        {
            // Every dimension must intersect. If any dimension
            // does not intersect, return false immediately.
            for (int i = 0; i < Dimensions; i++)
            {
                if (max[i] < o.min[i] || min[i] > o.max[i])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Determine whether this rectangle contains the passed rectangle
        /// </summary>
        /// <param name="o">The rectangle that might be contained by this
        /// rectangle</param>
        /// <returns>true if this rectangle contains the passed 
        /// rectangle, false if it does not</returns>
        public bool Contains(Orthotope o)
        {
            for (int i = 0; i < Dimensions; i++)
            {
                if (max[i] < o.max[i] || min[i] > o.min[i])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Determine whether a point falls within this orthotope.
        /// </summary>
        /// <param name="p">A point</param>
        /// <returns>true if the coordinates of the point falls within the
        /// bounds of this orthotope; false otherwise.</returns>
        /// <remarks>
        /// For each dimension, d, if p[d] >= this.min[i] and 
        /// p[d] &lt;= this.max[i], the true is returned.
        /// </remarks>
        public bool Contains(Point p)
        {
            for (int i = 0; i < Dimensions; i++)
                if (p[i] < min[i] || p[i] > max[i])
                    return false;
            return true;
        }

        /// <summary>
        /// Determine whether this rectangle is contained by the passed
        /// rectangle
        /// </summary>
        /// <param name="o">The rectangle that might contain this
        /// rectangle</param>
        /// <returns>true if the passed rectangle contains this rectangle,
        /// false if it does not</returns>
        public bool ContainedBy(Orthotope o)
        {
            for (int i = 0; i < Dimensions; i++)
            {
                if (max[i] > o.max[i] || min[i] < o.min[i])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Return the distance between this rectangle and the passed point.
        /// If the rectangle contains the point, the distance is zero.
        /// </summary>
        /// <param name="p">Point to find the distance to</param>
        /// <returns>Distance between this orthotope and the passed point.
        /// </returns>
        /// <remarks>The passed point should have the same dimension as
        /// this orthotope or exception will be thrown.</remarks>
        public double Distance(Point p)
        {
            double distanceSquared = 0;
            for (int i = 0; i < Dimensions; i++)
            {
                double greatestMin = Math.Max(min[i], p[i]);
                double leastMax = Math.Min(max[i], p[i]);
                if (greatestMin > leastMax)
                {
                    distanceSquared += ((greatestMin - leastMax) * (greatestMin - leastMax));
                }
            }
            return Math.Sqrt(distanceSquared);
        }

        /// <summary>
        /// Return the distance between this rectangle and the passed
        /// rectangle. If the rectangles overlap, the distance is zero.
        /// </summary>
        /// <param name="o">Rectangle to find the distance to</param>
        /// <returns>Distance between this rectangle and the passed
        /// rectangle</returns>
        /// <remarks>The passed rectangle should have the same dimension
        /// as this orthotope or exception will be thrown.</remarks>
        public double Distance(Orthotope o)
        {
            double distanceSquared = 0;
            for (int i = 0; i < Dimensions; i++)
            {
                double greatestMin = Math.Max(min[i], o.min[i]);
                double leastMax = Math.Min(max[i], o.max[i]);
                if (greatestMin > leastMax)
                {
                    distanceSquared += ((greatestMin - leastMax) * (greatestMin - leastMax));
                }
            }
            return Math.Sqrt(distanceSquared);
        }

        /// <summary>
        /// Calculate the volume by which this orthotope would be enlarged 
        /// if added to the passed orthotope. Neither orthotope is altered.
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public double Enlargement(Orthotope o)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates the volume of this orthotope. If dimension = 2,
        /// this calculates the area.
        /// </summary>
        public double Volume
        {
            get
            {
                double vol = 0;
                for (int i = 0; i < Dimensions; i++)
                {
                    double diff = max[i] - min[i];
                    if (i == 0)
                        vol = diff;
                    else
                        vol *= diff;
                }
                return vol;
            }
        }

        /// <summary>
        /// Returns a deep copy of this instance.
        /// </summary>
        /// <returns>A new instance of Orthotope.</returns>
        public Orthotope Copy()
        {
            return new Orthotope(min.Copy(), max.Copy());
        }

        /// <summary>
        /// Computes the union of this orthotope and the passed orthotope,
        /// storing the result in this orthotope.
        /// </summary>
        /// <param name="o">Orthotope to add to this one</param>
        /// <remarks>The passed orthotope should have the same number
        /// of dimensions.</remarks>
        public void Add(Orthotope o)
        {
            for (int i = 0; i < Dimensions; i++)
            {
                if (o.min[i] < min[i])
                {
                    min[i] = o.min[i];
                }
                if (o.max[i] > max[i])
                {
                    max[i] = o.max[i];
                }
            }
        }

        /// <summary>
        /// Expand the bound of this orthotope to include a point. If the
        /// point is contained in the bound, nothing happens.
        /// </summary>
        /// <param name="p">The point to include.</param>
        public void Add(Point p)
        {
            for (int i = 0; i < Dimensions; i++)
            {
                if (p[i] < min[i])
                    min[i] = p[i];
                else if (p[i] > max[i])
                    max[i] = p[i];
            }
        }

        /// <summary>
        /// Width of this box of a specific dimension. Width is calcuated
        /// by Max[d] - Min[d].
        /// </summary>
        /// <param name="d">The dimension</param>
        /// <returns>The width of box in dimension d.</returns>
        public double Width(int d)
        {
            return max[d] - min[d];
        }

        /// <summary>
        /// Equal if all the bounds are equal.
        /// </summary>
        /// <param name="obj">The orthotope to compare with.</param>
        /// <returns>true if both objects have the same number of
        /// dimensions and all the bounds are equal; false otherwise.
        /// </returns>
        public override bool Equals(object obj)
        {
            Orthotope o = (Orthotope)obj;
            if (o.Dimensions != o.Dimensions)
                return false;
            return min.Equals(o.Min) && max.Equals(o.Max);
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder(min.ToString());
            s.Append('|');
            s.Append(max.ToString());
            return s.ToString();
        }
    }
}
