using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpatialIndex.GridRTree;

namespace TuningTimeSimulation
{
    public class IndexEntry
    {
        private Orthotope mbr;
        private int offset;

        public IndexEntry(Orthotope mbr, int offset)
        {
            this.mbr = mbr.Copy();
            this.offset = offset;
        }

        public Orthotope MBR
        {
            get { return mbr; }
        }

        public int Offset
        {
            get { return offset; }
        }
    }
}
