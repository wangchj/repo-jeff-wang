using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndexPercentageSimulation;

namespace TuningTimeSimulation
{
    public class IndexBucket : Bucket
    {
        private List<IndexEntry> entries;

        public IndexBucket()
        {
            entries = new List<IndexEntry>();
        }

        /// <summary>
        /// Create a new list and make shallow copies of index entries.
        /// </summary>
        /// <param name="entries">The index entries</param>
        public IndexBucket(List<IndexEntry> entries)
        {
            this.entries = new List<IndexEntry>();
            this.entries.AddRange(entries);
        }

        /// <summary>
        /// Adds an entry to this index bucket, without making a copy.
        /// </summary>
        /// <param name="entry">An index entry</param>
        public void AddEntry(IndexEntry entry)
        {
            entries.Add(entry);
        }

        /// <summary>
        /// Returns the number entries.
        /// </summary>
        public int Count
        {
            get { return entries.Count; }
        }

        public List<IndexEntry> Entries
        {
            get { return entries; }
        }

        public override long Size
        {
            get
            {
                if (entries.Count == 0)
                    return 0;

                return (IndexPercentage.GetSizeInBytes(entries[0].MBR) +
                    IndexPercentage.BytesPerPtr) * entries.Count;
            }
        }
    }
}
