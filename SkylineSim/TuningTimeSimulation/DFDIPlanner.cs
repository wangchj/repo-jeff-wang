using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpatialIndex.GridRTree;

namespace TuningTimeSimulation
{
    public class DFDIPlanner
    {
        //private List<Bucket> program;
        //private int offset;

        //DFDIPlanner()
        //{
        //    program = new List<Bucket>();
        //    offset = 0;
        //}

        public static List<Bucket> MakeProgram(Node node, int offset,
            int repLevel)
        {
            List<Bucket> result = new List<Bucket>();
            if (node is LeafNode)
            {
                LeafNode leaf = (LeafNode)node;
                DataBucket b = new DataBucket(leaf.Points);
                result.Add(b);
            }
            else
            {
                IntNode intNode = (IntNode)node;
                for (int i = 0; i < intNode.Children.Count; i++)
                {
                    if (i == 0)
                    {
                        result.Add(MakeIndexBucket(intNode, 0,
                            offset, repLevel));
                    }
                    else if (repLevel != 0)
                    {
                        result.Add(MakeIndexBucket(intNode, i,
                            result.Count + offset, repLevel));
                    }

                    result.AddRange(MakeProgram(intNode.Children[i],
                        result.Count + offset, repLevel - 1));
                }
            }

            return result;
        }

        /// <summary>
        /// Make an IndexBucket and calculate the pointer for each entry.
        /// </summary>
        /// <param name="node">The node to make the bucket of</param>
        /// <param name="start">Index of entry of this node to be used to
        /// start this bucket.</param>
        /// <param name="offset"></param>
        /// <param name="rep"></param>
        /// <returns></returns>
        public static IndexBucket MakeIndexBucket(IntNode node, int start,
            int offset, int rep)
        {
            IndexBucket bucket =  new IndexBucket();
            int entry = 0;
            for (int i = start; i < node.Children.Count; i++)
            {
                int index = GetAlpha(node, entry);
                if (rep > 0)
                    index += entry;
                if (rep > 1)
                    index += GetBeta(node.Children.Count, rep, entry);
                index += (offset + 1);

                bucket.AddEntry(new IndexEntry(node.Children[i].Bounds,
                    index));

                entry++;
            }

            return bucket;
        }

        public static int GetAlpha(IntNode node, int entry)
        {
            if (entry == 0)
                return 0;

            int branchFactor = node.Children.Count;
            int sum = 0;
            for (int i = 0; i <= node.Children[0].Height; i++)
                sum += (int)Math.Pow(branchFactor, i);
            return sum * entry;
        }

        public static int GetBeta(int b, int rep, int entry)
        {
            if (entry == 0)
                return 0;

            int sum = 0;
            for (int i = 0; i <= rep - 2; i++)
                sum += (int)Math.Pow(b, i);
            return entry * sum * (b - 1);
        }
    }
}
