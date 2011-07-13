using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpatialIndex.GridRTree
{
    [Serializable]
    public abstract class Node
    {
        protected Orthotope bounds;
        protected Orthotope mbr;
        protected int height;

        public Orthotope Bounds
        {
            get { return bounds; }
        }

        public Orthotope MBR
        {
            get { return mbr; }
        }

        public int Height { get { return height; } }

        public abstract bool Insert(Point point);

    }

    [Serializable]
    public class IntNode : Node
    {
        /// <summary>
        /// Child nodes.
        /// </summary>
        private List<Node> children;

        /// <summary>
        /// Create a new internal node with list of children.
        /// </summary>
        /// <param name="nodes">A list of nodes to be added to children
        /// list.</param>
        public IntNode(List<Node> nodes)
        {
            if (bounds == null)
                bounds = nodes[0].Bounds.Copy();

            children = new List<Node>();

            foreach (Node node in nodes)
                AddChild(node);
        }

        public List<Node> Children { get { return children; } }

        /// <summary>
        ///
        /// </summary>
        /// <param name="child"></param>
        /// <exception cref="NullReferenceException">
        /// If node is null
        /// </exception>
        public void AddChild(Node child)
        {
            if (child.Height + 1 > height)
                height = child.Height + 1;

            if (child.MBR != null)
            {
                if (mbr == null)
                    mbr = child.MBR.Copy();
                else
                    mbr.Add(child.MBR);
            }

            bounds.Add(child.Bounds);
            children.Add(child);
        }

        public override bool Insert(Point point)
        {
            if (!bounds.Contains(point))
                return false;
            foreach (Node child in children)
            {
                if (child.Bounds.Contains(point))
                {
                    if (child.Insert(point))
                    {
                        if (mbr == null)
                            mbr = child.MBR.Copy();
                        else
                            mbr.Add(child.MBR);

                        return true;
                    }
                }
            }
            return false;
        }
    }

    [Serializable]
    public class LeafNode : Node
    {
        private List<Point> points;

        public LeafNode(Orthotope bounds)
        {
            if (bounds == null)
                throw new ArgumentNullException();
            this.bounds = bounds.Copy();
            this.height = 0;
            points = new List<Point>();
        }

        public List<Point> Points { get { return points; } }

        public override bool Insert(Point point)
        {
            if (!bounds.Contains(point))
                return false;
            
            if (mbr == null)
                mbr = new Orthotope(point, point);
            else
                mbr.Add(point);

            points.Add(point);
            return true;
        }
    }
}
