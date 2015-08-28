using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace AdvancedStringHandlingInCSharp.Assoc
{
    [Serializable]
    public class AssocArray<T> : List<AssocArrayItem<T>>
    {
        [XmlIgnore]
        public readonly Func<AssocArrayItem<T>, T> OnNullValue;

        [XmlIgnore]
        public readonly AssocArrayList<T> Parent;

        [XmlAttribute]
        public string Name;

        [XmlAttribute]
        public string SourceArea;

        public AssocArray()
        {
        }

        public AssocArray(string name, AssocArrayList<T> parent)
        {
            Name = name;
            Parent = parent;
        }

        public AssocArray(string name, AssocArrayList<T> parent, Func<AssocArrayItem<T>, T> onNullValue)
        {
            Name = name;
            OnNullValue = onNullValue;
        }

        public AssocArrayItem<T> this[string name]
        {
            get
            {
                var result = this.FirstOrDefault(item => Parent.NameComparerFunc(item.Name, name));
                return result ?? Add(name);
            }
        }

        public AssocArrayItem<T> Add(string name, T value = default(T), string tag = "")
        {
            var item = new AssocArrayItem<T>(name, value, tag, this);
            base.Add(item);
            return item;
        }

        public bool Contains(string name)
        {
            return this.Any(item => Compare(item.Name, name));
        }

        private bool Compare(string s1, string s2)
        {
            if (Parent == null || Parent.NameComparerFunc == null)
            {
                return AssocArrayList<T>.DefaultCaseInsensitiveCompare(s1, s2);
            }
            return Parent.NameComparerFunc(s1, s2);
        }
    }
}