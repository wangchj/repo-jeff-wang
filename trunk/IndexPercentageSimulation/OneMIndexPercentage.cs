using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpatialIndex.GridRTree;

namespace IndexPercentageSimulation
{
    public class OneMIndexPercentage : OneTimeIndexPercentage
    {
        public static double GetPercentage(GridRTree rtree, int m)
        {
            long indexBytes = m * GetIndexInBytes(rtree.Root);
            long dataBytes = GetDataPointsInBytes(rtree);
            return (double)indexBytes / (indexBytes + dataBytes);
        }
    }
}
