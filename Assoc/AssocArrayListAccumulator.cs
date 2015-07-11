using System;
using System.Linq;

namespace AdvancedStringHandlingInCSharp.Assoc
{
    [Serializable]
    public class AssocArrayListAccumulator : AssocArrayList<int>
    {
        public AssocArrayListAccumulator()
        {
        }

        public new AssocArrayAccumulator this[string name]
        {
            get
            {
                var result = this.FirstOrDefault(item => NameComparerFunc(item.Name, name)) as AssocArrayAccumulator;
                return result ?? Add(name);
            }
        }

        public new AssocArrayAccumulator Add(string name, Func<string, string, bool> nameComparerFunc = null)
        {
            var item = new AssocArrayAccumulator(name);
            base.Add(item);
            return item;
        }
    }
}