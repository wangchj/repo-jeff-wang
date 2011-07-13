using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpatialIndex.GridRTree;
using IndexPercentageSimulation;

namespace TuningTimeSimulation
{
    class DataBucket : Bucket
    {
        private List<Point> data;

        public DataBucket(List<Point> dataList)
        {
            data = new List<Point>();
            data.AddRange(dataList);
        }

        public List<Point> Data
        {
            get { return data; }
        }

        public override long Size
        {
            get
            {
                if (data.Count == 0)
                    return 0;
                return IndexPercentage.GetSizeInBytes(data[0]) *
                    data.Count;
            }
        }
    }
}
