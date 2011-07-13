using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpatialIndex.GridRTree;
using DominanceTestCountSimulation;

namespace TuningTimeSimulation
{
    public class PointBasedTuneTime
    {
        private List<PruningRegion> pruneRegs;
        private List<IndexEntry> wakeList;
        private GridRTree rtree;
        private int repLevel;
        private PrefSpec[] pref;

        public PointBasedTuneTime(GridRTree rtree, int repLevel,
            PrefSpec[] prefs)
        {
            if (rtree == null)
                throw new ArgumentNullException();
            if (repLevel < 0)
                throw new ArgumentException();

            this.pruneRegs = new List<PruningRegion>();
            this.wakeList = new List<IndexEntry>();
            this.rtree = rtree;
            this.repLevel = repLevel;

            pref = new PrefSpec[rtree.Root.Bounds.Dimensions];
            Array.Copy(prefs, pref, pref.Length);
        }

        public long GetTuningTime()
        {
            List<Bucket> program = DFDIPlanner.MakeProgram(rtree.Root, 0,
                repLevel);

            long tuneTime = 0;

            //Add the root to the wake up list.
            //TODO:What if root is not an index bucket?
            IndexBucket rootBucket= (IndexBucket)program[0];
            tuneTime += rootBucket.Size;
            foreach (IndexEntry entry in rootBucket.Entries)
                wakeList.Add(entry);

            for (int i = 1; i < program.Count; i++)
            {
                if (!WakeForIndex(i))
                    continue;

                //Tune into the bucket and increment tuning time.
                tuneTime += program[i].Size;

                if (program[i] is DataBucket)
                {
                    DataBucket db = (DataBucket)program[i];
                    ComputeSkyline(db.Data);
                }
                else
                {
                    IndexBucket ib = (IndexBucket)program[i];
                    foreach (IndexEntry entry in ib.Entries)
                        if (!Covered(entry))
                            wakeList.Add(entry);
                }
            }

            return tuneTime;
        }

        /// <summary>
        /// Check if index is in wakeList.
        /// </summary>
        /// <param name="i">The index</param>
        /// <returns>true if wakeList contains i</returns>
        private bool WakeForIndex(int i)
        {
            foreach (IndexEntry entry in wakeList)
                if (entry.Offset == i)
                    return true;
            return false;
        }

        private bool Covered(IndexEntry entry)
        {
            foreach (PruningRegion pruneReg in pruneRegs)
            {
                if (pruneReg.Covers(entry.MBR))
                    return true;
            }

            return false;
        }

        /***********************************************
         * Copied From PointBasedPruning.cs
         * ********************************************/

        private void ComputeSkyline(List<Point> points)
        {
            for (int i = 0; i < points.Count; i++)
            {
                bool dominated = false;

                for (int j = 0; j < points.Count; j++)
                {
                    if (i == j)
                        continue;
                    if (Dominates(points[j], points[i], pref))
                    {
                        dominated = true;
                        break;
                    }
                }

                if (!dominated)
                {
                    PruningRegion pruneReg =
                        new PruningRegion(points[i], pref);
                    //RemoveCoveredPoints(pruneReg);
                    RemoveCoveredPruneRegion(pruneReg);
                    RemoveCoveredWakeEntry(pruneReg);
                    //skyline.Add(points[i]);
                    pruneRegs.Add(pruneReg);
                }
            }
        }

        /// <summary>
        /// Determines if p1 is better than p2.
        /// </summary>
        /// <param name="p1">Point 1</param>
        /// <param name="p2">Point 2</param>
        /// <param name="pref">Preference specifiers</param>
        /// <returns></returns>
        public static bool Dominates(Point p1, Point p2, PrefSpec[] pref)
        {
            //At least one attribute is better
            bool better = false;

            for (int i = 0; i < p1.Dimensions; i++)
            {
                if (pref[i] == PrefSpec.Min)
                {
                    if (p1[i] < p2[i])
                        better = true;
                    else if (p2[i] < p1[i])
                        return false;
                }
                else
                {
                    if (p1[i] > p2[i])
                        better = true;
                    else if (p2[i] > p1[i])
                        return false;
                }
            }

            return better;
        }

        //private void RemoveCoveredPoints(PruningRegion p)
        //{
        //    List<int> removeList = new List<int>();

        //    //We need to test every point in skyline list.
        //    //domTestCount += skyline.Count;

        //    //Get a list of points dominated by p
        //    for (int i = 0; i < skyline.Count; i++)
        //        if (p.Covers(skyline[i]))
        //            removeList.Add(i);

        //    //Remove the list of dominated points from skyline
        //    for (int i = removeList.Count - 1; i >= 0; i--)
        //        skyline.RemoveAt(removeList[i]);
        //}

        private void RemoveCoveredPruneRegion(PruningRegion p)
        {
            List<int> removeList = new List<int>();

            //Get a list of points dominated by p
            for (int i = 0; i < pruneRegs.Count; i++)
                if (p.Covers(pruneRegs[i]))
                    removeList.Add(i);

            //Remove the list of dominated points from skyline
            for (int i = removeList.Count - 1; i >= 0; i--)
                pruneRegs.RemoveAt(removeList[i]);
        }

        private void RemoveCoveredWakeEntry(PruningRegion p)
        {
            List<int> removeList = new List<int>();

            //Get a list of points dominated by p
            for (int i = 0; i < wakeList.Count; i++)
                if (p.Covers(wakeList[i].MBR))
                    removeList.Add(i);

            //Remove the list of dominated points from skyline
            for (int i = removeList.Count - 1; i >= 0; i--)
                wakeList.RemoveAt(removeList[i]);
        }
    }
}
