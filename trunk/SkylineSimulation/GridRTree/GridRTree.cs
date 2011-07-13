using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpatialIndex.GridRTree
{
    /// <summary>
    /// Point-based Grid RTree
    /// </summary>
    [Serializable]
    public class GridRTree
    {
        Node root;

        #region Properties

        public Node Root { get { return root; } }
        public int Height { get { return root.Height; } }

        #endregion

        #region Tree Building

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="branchFactor"></param>
        public GridRTree(string path, int branchFactor)
        {
            Object[] data = LoadDataPoints(path);
            Orthotope bounds = (Orthotope)data[0];
            List<Point> points = (List<Point>)data[1];
            
            //Adjust bounds (Necessary?)
            
            //Build tree
            BuildTree(points, bounds, branchFactor);

            //Insert points
            foreach (Point point in points)
            {
                root.Insert(point);
            }
        }

        /// <summary>
        /// Load data points and their minimal bounding rectangle (MBR).
        /// </summary>
        /// <param name="path">The path of the input file.</param>
        /// <returns>The first element contains the MBR of the data points
        /// and the second is the list of data points.</returns>
        private Object[] LoadDataPoints(string path)
        {
            List<Point> points = new List<Point>();
            Orthotope bound = null;

            //Load data points from file.
            using (StreamReader fileReader = new StreamReader(path))
            {
                for (string line = fileReader.ReadLine();
                    line != null;
                    line = fileReader.ReadLine())
                {
                    line = line.Trim();
                    //Create a new point
                    string[] coordStr = line.Split(new char[] {' ', '\t'});
                    Point point = new Point(coordStr.Length);
                    for (int i = 0; i < coordStr.Length; i++)
                    {
                        point[i] = double.Parse(coordStr[i]);
                    }

                    //Add point to the list
                    points.Add(point);

                    //Add point to the bound
                    if (bound == null)
                        bound = new Orthotope(point, point);
                    else
                        bound.Add(point);
                }

                Object[] result = { bound, points };
                return result;
            }
        }

        private void BuildTree(List<Point> records, Orthotope bounds,
            int branchFactor)
        {
            int dimensions = records[0].Dimensions;
            int height = ApproxTreeHeight(records.Count, branchFactor);
            
            //Assign partitions to each dimension.
            int[] dimensionPartitions = GetPartitionPerDimension(
                GetPartitionPowerFactor(dimensions, height),
                branchFactor);
            //Get cell width for each dimension
            double[] cellWidths = GetPartitionWidth(bounds, dimensionPartitions);
            //Make Leaves
            List<Node> leaves = MakeLeaves(bounds, dimensionPartitions,
                cellWidths);

            //Build the tree bottom up from leaves.
            List<Node> nodes = leaves;
            while (true)
            {
                if (nodes.Count == 1)
                {
                    root = nodes[0];
                    break;
                }
                List<Node> newNodes = new List<Node>();
                for (int i = 0; i < (int)(nodes.Count / branchFactor); i++)
                {
                    IntNode node = new IntNode(
                        nodes.GetRange(i * branchFactor, branchFactor));
                    newNodes.Add(node);
                }
                nodes = newNodes;
            }
        }

        private List<Node> MakeLeaves(Orthotope dataBound,
            int[] dimenPartCount, double[] dimenPartWidth)
        {
            int dimensions = dimenPartCount.Length;

            //tracker tracks the current cell to be created.
            //It's a counter for each dimension, but starts with 0.
            int[] tracker = new int[dimensions];
            List<Node> cells = new List<Node>();
            while (true)
            {
                //Create a cell.
                Point min = new Point(dimensions);
                Point max = new Point(dimensions);
                for (int d = 0; d < dimensions; d++)
                {
                    min[d] = dataBound.Min[d] + dimenPartWidth[d] * tracker[d];
                    max[d] = dataBound.Min[d] + dimenPartWidth[d] * (tracker[d] + 1);
                }

                cells.Add(new LeafNode(new Orthotope(min, max)));

                //Increase the counter to the next cell.
                if (!IncrementCellTracker(tracker, dimenPartCount))
                    break;
            }
            return cells;
        }


        private int ApproxTreeHeight(int recCount, int b)
        {
            //Estimate the number levels of the index tree.
            return (int)Math.Ceiling(Math.Log(recCount, b)) - 1;
        }

        private double[] GetPartitionWidth(Orthotope dataBound,
            int[] dimenPartCount)
        {
            //Calculate width of each partition for each dimension
            double[] widths = new double[dataBound.Dimensions];
            for (int i = 0; i < dataBound.Dimensions; i++)
            {
                widths[i] = dataBound.Width(i) / dimenPartCount[i];
            }

            return widths;           
        }

        /// <summary>
        /// Increase the tracker.
        /// </summary>
        /// <param name="tracker">An array of tracker for each
        /// dimension.</param>
        /// <param name="dimensionPartitions">An array of count of
        /// partitions for each dimension.</param>
        /// <returns>true if tracker was updated successfuly; false 
        /// otherwise.</returns>
        private bool IncrementCellTracker(int[] tracker,
            int[] dimensionPartitions)
        {
            for (int i = 0; i < tracker.Length; i++)
            {
                if (tracker[i] + 1 < dimensionPartitions[i])
                {
                    tracker[i]++;
                    return true;
                }
                else
                {
                    //reset tracker for this dimension
                    //and increase the tracker for next dimension during
                    //next iteration.
                    tracker[i] = 0;
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dimensions"></param>
        /// <param name="treeHeight"></param>
        /// <returns></returns>
        private int[] GetPartitionPowerFactor(int dimensions, int treeHeight)
        {
            if (dimensions < 2)
                throw new ArgumentException(
                    "Dimensions must be greater than 1.");
            //if (treeHeight < 1)
            //    throw new ArgumentException(
            //        "Tree height has to be greater than 0.");

            int[] powerFactor = new int[dimensions];
            
            //Initialize all power factor to 0
            for (int i = 0; i < dimensions; i++)
                powerFactor[i] = 0;

            int quotient = (int)(treeHeight / dimensions);
            int remainder = treeHeight % dimensions;

            //Assign quatient to each dimension
            if (quotient > 0)
                for (int i = 0; i < dimensions; i++)
                    powerFactor[i] = quotient;

            //Assign remainder height, first come first serve
            for (int i = 0; i < remainder; i++)
                powerFactor[i]++;

            return powerFactor;
        }

        /// <summary>
        /// Calculate the number of partitions for each dimension based
        /// on branching factor.
        /// </summary>
        /// <param name="powerFactor">
        /// The power to be raised for each dimension
        /// </param>
        /// <param name="branchFactor">
        /// The branching factor of the tree.
        /// </param>
        /// <returns>Partitions for each dimension.</returns>
        private int[] GetPartitionPerDimension(int[] powerFactor,
            int branchFactor)
        {
            int[] part = new int[powerFactor.Length];
            for (int i = 0; i < part.Length; i++)
            {
                part[i] = (int)Math.Pow(branchFactor, powerFactor[i]);
            }
            return part;
        }

        #endregion
    }
}
