using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpatialIndex.GridRTree;

namespace IndexPercentageSimulation
{
    public abstract class IndexPercentage
    {
        public const int BytesPerPtr = 4;
        public const int BytesPerDouble = 8;

        public static long GetSizeInBytes(Orthotope ortho)
        {
            return 2 * GetSizeInBytes(ortho.Min);
        }

        public static long GetSizeInBytes(Point p)
        {
            return BytesPerDouble * p.Dimensions;
        }

        public static long GetDataPointsInBytes(GridRTree rtree)
        {
            return GetDataPointsInBytes(rtree.Root);
        }

        public static long GetDataPointsInBytes(Node node)
        {
            long size = 0;

            if (node is LeafNode)
            {
                LeafNode leafNode = (LeafNode)node;
                size = leafNode.Points.Count *
                    leafNode.Bounds.Dimensions * BytesPerDouble;
            }
            else
            {
                IntNode intNode = (IntNode)node;
                foreach (Node n in intNode.Children)
                    size += GetDataPointsInBytes(n);
            }

            return size;
        }
    }
}
