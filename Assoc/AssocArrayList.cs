using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AdvancedStringHandlingInCSharp.Assoc
{
    [Serializable]
    public class AssocArrayList<T> : List<AssocArray<T>>
    {
        public readonly Func<string, string, bool> NameComparerFunc = DefaultCaseInsensitiveCompare;

        [XmlAttribute]
        public string Name;

        [XmlAttribute]
        public string Number;

        [XmlAttribute]
        public string Title;

        public AssocArrayList()
        {
        }

        public AssocArrayList(string name, Func<string, string, bool> nameComparerFunc)
        {
            Name = name;
            NameComparerFunc = nameComparerFunc ?? DefaultCaseInsensitiveCompare;
        }

        public AssocArrayList(AssocArrayList<T> source)
        {
            if (source == null)
                return;

            Name = source.Name;
            Number = source.Number;
            Title = source.Title;
            NameComparerFunc = source.NameComparerFunc ?? DefaultCaseInsensitiveCompare;
            foreach (var array in source)
            {
                var newArray = new AssocArray<T>(array.Name, this);
                newArray.AddRange(array
                    .Select(item => new AssocArrayItem<T>(item.Name, item.Value, item.Tag, newArray)));
                Add(newArray);
            }
        }

        public virtual AssocArray<T> this[string name]
        {
            get
            {
                var result = this
                    .FirstOrDefault(item => NameComparerFunc(item.Name, name));
                return result ?? Add(name);
            }
        }

        public static bool DefaultCaseInsensitiveCompare(string toSearch, string toFind)
        {
            if (toSearch.IsEmpty())
            {
                return false;
            }
            return toSearch.IndexOf(toFind, StringComparison.OrdinalIgnoreCase) > -1;
        }

        public virtual AssocArray<T> Add(string name)
        {
            var item = new AssocArray<T>(name, this);
            base.Add(item);
            return item;
        }

        public virtual bool Contains(string name)
        {
            return this
                .Any(item => string
                    .Compare(name, item.Name, StringComparison.InvariantCultureIgnoreCase)
                             == 0);
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            if (Name.IsNotEmpty() || Title.IsNotEmpty())
                result.AppendLine(Title ?? Name);
            foreach (var array in this)
            {
                if (array.Name.IsNotEmpty())
                    result.AppendLine("[" + array.Name + "]");
                foreach (var item in array)
                {
                    var tag = item.Tag.AsString();
                    if (tag.IsNotEmpty())
                        result.AppendLine("    " + item.Name + "=" + item.Value + " (" + tag + ")");
                    else
                        result.AppendLine("    " + item.Name + "=" + item.Value);
                }
            }
            return result.ToString();
        }
    }
}