using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpatialIndex.GridRTree;
using DominanceTestCountSimulation;

namespace TuningTimeSimulation
{
    public class IndexBasedTuneTime
    {
        private List<PruningRegion> pruneRegs;
        private List<IndexEntry> wakeList;
        private GridRTree rtree;
        private int repLevel;
        private PrefSpec[] pref;

        public IndexBasedTuneTime(GridRTree rtree, int repLevel,
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

                if (program[i] is IndexBucket)
                {
                    IndexBucket ib = (IndexBucket)program[i];
                    foreach (IndexEntry entry in ib.Entries)
                    {
                        if (!Covered(entry))
                        {
                            List<PruningRegion> pr = MakePruneRegs(entry);
                            foreach (PruningRegion p in pr)
                            {
                                RemoveCoveredPruneRegion(p);
                                RemoveCoveredWakeEntry(p);
                            }
                            pruneRegs.AddRange(pr);
                            wakeList.Add(entry);
                        }
                    }
                }
            }

            return tuneTime;
        }

        private List<PruningRegion> MakePruneRegs(IndexEntry entry)
        {
            int dimension = entry.MBR.Dimensions;
            Orthotope bounds = entry.MBR;
            List<PruningRegion> pruneRegs = new List<PruningRegion>();
            for (int i = 0; i < dimension; i++)
            {
                Point pivot = new Point(dimension);

                for (int j = 0; j < dimension; j++)
                {
                    if (i == j)
                    {
                        if (pref[j] == PrefSpec.Min)
                            pivot[j] = bounds.Max[j];
                        else
                            pivot[j] = bounds.Min[j];
                    }
                    else
                    {
                        if (pref[j] == PrefSpec.Min)
                            pivot[j] = bounds.Min[j];
                        else
                            pivot[j] = bounds.Max[j];
                    }
                }

                PruningRegion pruneReg = new PruningRegion(pivot, pref);
                
                //Check if the new pruning region is covered
                bool covered = false;
                foreach (PruningRegion p in this.pruneRegs)
                {
                    if (p.Covers(pruneReg))
                    {
                        covered = true;
                        break;
                    }
                }

                if(!covered)
                    pruneRegs.Add(pruneReg);
            }

            return pruneRegs;
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
