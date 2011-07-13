using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpatialIndex.GridRTree;

namespace DominanceTestCountSimulation
{
    public class IndexBasedPruning
    {
        private List<PruningRegion> pruneRegs;
        private GridRTree rtree;
        private PrefSpec[] pref;
        private List<Point> skyline;
        private int domTestCount;
        private bool ran;

        public IndexBasedPruning(GridRTree rtree, PrefSpec[] pref)
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
            if (node.MBR != null && !Covered(node))
            {
                if (node is LeafNode)
                {
                    LeafNode leafNode = (LeafNode)node;
                    List<Point> s = ComputeSkyline(leafNode.Points);
                    if(node.MBR != null)
                        AddPruningRegion(
                            new PruningRegion(leafNode.MBR, pref), true);
                    skyline.AddRange(s);
                }
                else
                {
                    IntNode intNode = (IntNode)node;
                    foreach (Node child in intNode.Children)
                    {
                        ComputeSkyline(child);
                    }
                    if (node.MBR != null)
                        AddPruningRegion(
                            new PruningRegion(node.MBR, pref), false);
                }
            }
        }

        private bool Covered(Node node)
        {
            foreach (PruningRegion pruneReg in pruneRegs)
            {
                if (pruneReg.Covers(node.MBR))
                    return true;
            }

            return false;
        }

        private List<Point> ComputeSkyline(List<Point> points)
        {
            List<Point> result = new List<Point>();
            bool dom = false;

            for (int i = 0; i < points.Count; i++)
            {
                for (int j = 0; j < points.Count; j++)
                {
                    if (i == j)
                        continue;

                    domTestCount++;

                    if (Dominates(points[j], points[i], pref))
                    {
                        dom = true;
                        break;
                    }
                }

                if (!dom)
                    result.Add(points[i]);

                //if (!dominated)
                //{
                //    PruningRegion pruneReg = new PruningRegion(points[i]);
                //    RemoveCoveredPoints(pruneReg);
                //    RemoveCoveredPruneRegion(pruneReg);
                //    skyline.Add(points[i]);
                //    pruneRegs.Add(pruneReg);
                //}
            }

            return result;
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

        /// <summary>
        /// Adds a pruning region to the pruning region list and remove
        /// skyline points and pruning regions covered by this pruning region.
        /// </summary>
        /// <param name="p">The pruning region to be added.</param>
        private void AddPruningRegion(PruningRegion p, bool removePoints)
        {
            //Check if this pruning region is covered by another
            foreach (PruningRegion pruneReg in pruneRegs)
                if (pruneReg.Covers(p))
                    return;

            if (removePoints)
                RemoveCoveredPoints(p);
            RemoveCoveredPruneRegion(p);
            pruneRegs.Add(p);
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
