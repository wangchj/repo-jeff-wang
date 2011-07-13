using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SpatialIndex.GridRTree;

namespace IndexPercentageSimulation
{
    public class OneTimeIndexPercentage : IndexPercentage
    {
        public static double GetPercentage(GridRTree rtree)
        {
            long indexBytes = GetIndexInBytes(rtree);
            long dataBytes = GetDataPointsInBytes(rtree);
            return (double)indexBytes / (indexBytes + dataBytes);
        }

        public static long GetIndexInBytes(GridRTree rtree)
        {
            return GetIndexInBytes(rtree.Root);
        }

        /// <summary>
        /// Get the size of the index tree rooted with node.
        /// </summary>
        /// <param name="node">The root node of the tree.</param>
        /// <returns>The size of the tree in byte.</returns>
        public static long GetIndexInBytes(Node node)
        {
            long size = 0;
            size = GetNodeInBytes(node);

            if (node is IntNode)
            {
                IntNode intNode = (IntNode)node;
                foreach (Node n in intNode.Children)
                    size += GetIndexInBytes(n);
            }
            return size;
        }

        /// <summary>
        /// Get the size of a node. Child nodes are not be accounted for.
        /// </summary>
        /// <param name="node">The node</param>
        /// <returns>The size of the bound plus the size the pointeres.
        /// </returns>
        public static long GetNodeInBytes(Node node)
        {
            long size = GetSizeInBytes(node.Bounds);
            if (node is LeafNode)
            {
                LeafNode leaf = (LeafNode)node;
                size += leaf.Points.Count * BytesPerPtr;
            }
            else
            {
                IntNode intNode = (IntNode)node;
                size += intNode.Children.Count * BytesPerPtr;
            }
            return size;
        }   
    }
}
