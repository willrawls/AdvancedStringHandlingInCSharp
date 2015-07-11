using System;

namespace AdvancedStringHandlingInCSharp.Assoc
{
    [Serializable]
    public class AssocArrayAccumulator : AssocArray<int>
    {
        private readonly object _mSyncRoot = new object();

        public AssocArrayAccumulator(string name)
        {
            Name = name;
        }

        public bool IsEqualTo(int threshold, int candidate)
        {
            lock (_mSyncRoot)
                if (candidate == threshold)
                {
                    this["Equal To " + threshold].Value++;
                    return true;
                }
            return false;
        }

        public bool IsHighest(int candidate)
        {
            lock (_mSyncRoot)
                if (candidate > this["Highest"].Value)
                {
                    this["Highest"].Value = candidate;
                    return true;
                }
            return false;
        }

        public bool IsLongest(int candidate)
        {
            lock (_mSyncRoot)
                if (candidate > this["Longest"].Value)
                {
                    this["Longest"].Value = candidate;
                    return true;
                }
            return false;
        }

        public bool IsOver(int threshold, int candidate)
        {
            lock (_mSyncRoot)
                if (candidate > threshold)
                {
                    this["Over " + threshold].Value++;
                    return true;
                }
            return false;
        }

        public bool Lowest(int candidate)
        {
            lock (_mSyncRoot)
                if (candidate < this["Lowest"].Value)
                {
                    this["Lowest"].Value = candidate;
                    return true;
                }
            return false;
        }

        public int Total(int toAdd)
        {
            lock (_mSyncRoot)
                this["Total"].Value += toAdd;
            return this["Total"].Value;
        }
    }
}