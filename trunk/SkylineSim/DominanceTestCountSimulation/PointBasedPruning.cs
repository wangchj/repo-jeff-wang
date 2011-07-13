using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpatialIndex.GridRTree;

namespace DominanceTestCountSimulation
{
    public class PointBasedPruning
    {
        private List<PruningRegion> pruneRegs;
        private GridRTree rtree;
        private PrefSpec[] pref;
        private List<Point> skyline;
        private int domTestCount;
        private bool ran;

        public PointBasedPruning(GridRTree rtree, PrefSpec[] pref)
        {
            if (rtree == null || pref == null)
                throw new ArgumentNullException();
            
            pruneRegs = new List<PruningRegion>();
            skyline = new List<Point>();
            this.rtree = rtree;
            this.pref = pref;

            domTestCount = 0;
            ran = false;
        }

        public List<Point> ComputeSkyline()
        {
            ComputeSkyline(rtree.Root);
            return skyline;
        }

        private void ComputeSkyline(Node node)
        {
            ran = true;
            if (!Covered(node))
            {
                if (node is LeafNode)
                {
                    LeafNode leafNode = (LeafNode)node;
                    ComputeSkyline(leafNode.Points);
                }
                else
                {
                    IntNode intNode = (IntNode)node;
                    foreach (Node child in intNode.Children)
                    {
                        ComputeSkyline(child);
                    }
                }
            }
        }

        private bool Covered(Node node)
        {
            foreach (PruningRegion pruneReg in pruneRegs)
            {
                if (pruneReg.Covers(node.Bounds))
                    return true;
            }

            return false;
        }

        private void ComputeSkyline(List<Point> points)
        {
            for (int i = 0; i < points.Count; i++)
            {
                bool dominated = false;

                for (int j = 0; j < points.Count; j++)
                {
                    if (i == j)
                        continue;
                    domTestCount++;
                    if(Dominates(points[j], points[i], pref))
                    {
                        dominated = true;
                        break;
                    }
                }

                if (!dominated)
                {
                    PruningRegion pruneReg = 
                        new PruningRegion(points[i], pref);
                    RemoveCoveredPoints(pruneReg);
                    RemoveCoveredPruneRegion(pruneReg);
                    skyline.Add(points[i]);
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

        private void RemoveCoveredPoints(PruningRegion p)
        {
            List<int> removeList = new List<int>();
            
            //We need to test every point in skyline list.
            //domTestCount += skyline.Count;

            //Get a list of points dominated by p
            for (int i = 0; i < skyline.Count; i++)
                if (p.Covers(skyline[i]))
                    removeList.Add(i);

            //Remove the list of dominated points from skyline
            for (int i = removeList.Count - 1; i >= 0; i--)
                skyline.RemoveAt(removeList[i]);
        }

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

        public int DominanceCount
        {
            get
            {
                if (!ran)
                    ComputeSkyline(rtree.Root);
                return domTestCount;
            }
        }
    }
}
