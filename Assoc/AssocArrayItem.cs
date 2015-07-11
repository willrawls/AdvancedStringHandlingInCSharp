using System;
using System.Xml.Serialization;

namespace AdvancedStringHandlingInCSharp.Assoc
{
    [Serializable]
    public class AssocArrayItem<T>
    {
        [XmlAttribute]
        public readonly string Name;

        [XmlAttribute]
        public int Index;

        [XmlAttribute]
        public string Tag;

        [XmlIgnore]
        public AssocArray<T> Parent;

        // ReSharper disable once StaticMemberInGenericType
        private static int _mNextIndex;

        private T _mValue;

        public AssocArrayItem(AssocArray<T> parent)
        {
            Parent = parent;
            Index = _mNextIndex++;
        }

        public AssocArrayItem(string name, T value, string tag, AssocArray<T> parent)
        {
            if (name.IsEmpty())
            {
                throw new AssocArrayException("AssocArrayItem: Name cannot be blank or null.");
            }

            Name = name;
            _mValue = value;
            Tag = tag;
            Parent = parent;
            Index = _mNextIndex++;
        }

        public AssocArrayItem()
        {
        }

        public static int NextIndex { get { return _mNextIndex; } }

        public T Value
        {
            get
            {
                if (_mValue == null && OnNullValue != null)
                {
                    _mValue = OnNullValue(this);
                }
                return _mValue;
            }
            set { _mValue = value; }
        }

        public override string ToString()
        {
            return Name.AsString() + "= " + Value.AsString();
        }

        public void ValueAndTag(T value, string tag)
        {
            Value = value;
            Tag = tag;
        }
    }
}