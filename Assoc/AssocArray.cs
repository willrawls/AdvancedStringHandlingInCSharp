using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

namespace AdvancedStringHandlingInCSharp.Assoc
{
    [Serializable]
    public class AssocArray<T> : List<AssocArrayItem<T>>
    {
        [XmlAttribute]
        public string Name;

        [XmlAttribute]
        public string SourceArea;

        [XmlIgnore]
        public readonly Func<AssocArrayItem<T>, T> OnNullValue;

        [XmlIgnore]
        public readonly AssocArrayList<T> Parent;

        public AssocArray()
        {
            OnNullValue = AssocArrayList<T>.DefaultCaseInsensitiveCompare;
        }

        public AssocArray(string name, AssocArrayList<T> parent)
        {
            Name = name;
            Parent = parent;
        }

        public AssocArray(string name, Func<AssocArrayItem<T>, T> onNullValue)
        {
            Name = name;
            OnNullValue = onNullValue;
        }

        public AssocArrayItem<T> this[string name]
        {
            get
            {
                var result = this.FirstOrDefault(item => _mNameComparerFunc(item.Name, name));
                return result ?? Add(name);
            }
        }

        public AssocArrayItem<T> Add(string name, T value = default(T), string tag = "", Func<AssocArrayItem<T>, T> onNullValue = null)
        {
            var item = new AssocArrayItem<T>(name, value, tag, onNullValue);
            base.Add(item);
            return item;
        }

        public bool Contains(string name)
        {
            return this.Any(item => _mNameComparerFunc(item.Name, name));
        }
    }
}