using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpatialIndex.GridRTree;

namespace IndexPercentageSimulation
{
    public class DFDIIndexPercentage : IndexPercentage
    {
        public static double GetPercentage(GridRTree rtree, int repLevel)
        {
            long indexBytes = GetIndexInBytes(rtree.Root, 1, repLevel);
            long dataBytes = GetDataPointsInBytes(rtree);
            return (double)indexBytes / (indexBytes + dataBytes);
        }

        /// <summary>
        /// Get the size of the index tree rooted with node.
        /// </summary>
        /// <param name="node">The root node of the tree.</param>
        /// <param name="level"></param>
        /// <param name="repLevel"></param>
        /// <returns>The size of the tree in byte.</returns>
        public static long GetIndexInBytes(Node node, int level, int repLevel)
        {
            long size = 0;
            size = GetNodeInBytes(node, level, repLevel);

            if (node is IntNode)
            {
                IntNode intNode = (IntNode)node;
                foreach (Node n in intNode.Children)
                    size += GetIndexInBytes(n, level + 1, repLevel);
            }
            return size;
        }

        /// <summary>
        /// Get the size of a node. Child nodes are not be accounted for.
        /// </summary>
        /// <param name="node">The node</param>
        /// <returns>The size of the bound plus the size the pointeres.
        /// </returns>
        public static long GetNodeInBytes(Node node, int level, int repLevel)
        {
            long size = 0;
            if (node is LeafNode)
            {
                LeafNode leaf = (LeafNode)node;
                size += GetSizeInBytes(node.Bounds);
                size += leaf.Points.Count * BytesPerPtr;
            }
            else
            {
                IntNode intNode = (IntNode)node;
                //Check to see if this node is to be replicated
                if (level <= repLevel)
                {
                    int n = intNode.Children.Count;
                    int s = (n + 1) * n / 2;
                    size += s * GetSizeInBytes(node.Bounds);
                    size += s * BytesPerPtr;
                }
                else
                {
                    size += GetSizeInBytes(node.Bounds);
                    size += intNode.Children.Count * BytesPerPtr;
                }
            }
            return size;
        }
    }
}
