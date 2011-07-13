using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpatialIndex.GridRTree;

namespace DominanceTestCountSimulation
{
    public class PruningRegion
    {
        private Point pivot;
        PrefSpec[] pref;

        public PruningRegion(Point pivot, PrefSpec[] prefs)
        {
            if (pivot == null || prefs == null)
                throw new ArgumentNullException();

            this.pivot = pivot.Copy();
            pref = new PrefSpec[pivot.Dimensions];
            Array.Copy(prefs, pref, prefs.Length);
        }

        public PruningRegion(Orthotope o, PrefSpec[] prefs)
        {
            if (o == null || prefs == null)
                throw new ArgumentNullException();
            pivot = MakePivot(o, prefs);
            pref = new PrefSpec[pivot.Dimensions];
            Array.Copy(prefs, pref, prefs.Length);
        }

        public Point Pivot
        {
            get { return pivot; }
        }

        public PrefSpec[] prefs
        {
            get { return pref; }
        }

        /// <summary>
        /// Determines if this pruning region contains a point.
        /// </summary>
        /// <param name="p">A point</param>
        /// <returns>true if the point is inside or at a border of this
        /// pruning region; false otherwise.</returns>
        public bool Covers(Point p)
        {
            for (int i = 0; i < p.Dimensions; i++)
            {
                if (pref[i] == PrefSpec.Min && p[i] < pivot[i])
                    return false;
                if (pref[i] == PrefSpec.Max && p[i] > pivot[i])
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Determine if this pruning region covers (dominates) an
        /// orthotope.
        /// </summary>
        /// <param name="o">An orthotope</param>
        /// <param name="pref">An array of preference specifier for
        /// each dimension.</param>
        /// <returns>true if the orthotope is completely inside this
        /// pruning region; false otherwise.</returns>
        public bool Covers(Orthotope o)
        {
            //Find the dominant corner of the orthotope
            Point p = MakePivot(o, pref);

            //Check if the dominant point is covered by this pruning region
            return Covers(p);
        }

        public bool Covers(PruningRegion p)
        {
            return Covers(p.Pivot);
        }

        /// <summary>
        /// Find the dominant corner (pivot) of the orthotope and make a
        /// new instance of Point.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="pref"></param>
        /// <returns></returns>
        public static Point MakePivot(Orthotope o, PrefSpec[] pref)
        {
            Point p = new Point(o.Dimensions);
            for (int i = 0; i < o.Dimensions; i++)
            {
                if (pref[i] == PrefSpec.Min)
                    p[i] = o.Min[i];
                else
                    p[i] = o.Max[i];
            }
            return p;
        }

        
    }
}
