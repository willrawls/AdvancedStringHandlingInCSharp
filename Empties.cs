using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace AdvancedStringHandlingInCSharp
{
    public static class Empties
    {
        /*
                /// <summary>
                /// Check the given array is empty or not
                /// </summary>
                public static bool IsEmpty(this Array obj)
                {
                    return ((obj == null) || (obj.Length == 0));
                }
        */

        /// <summary>
        /// Check the given ArrayList is empty or not
        /// </summary>
        public static bool IsEmpty(this ArrayList obj)
        {
            return ((obj == null) || (obj.Count == 0));
        }

        /// <summary>
        /// Check the given BitArray is empty or not
        /// </summary>
        public static bool IsEmpty(this BitArray obj)
        {
            return ((obj == null) || (obj.Count == 0));
        }

        /// <summary>
        /// Check the given CollectionBase is empty or not
        /// </summary>
        public static bool IsEmpty(this CollectionBase obj)
        {
            return ((obj == null) || (obj.Count == 0));
        }

        /// <summary>
        /// Check the given DictionaryBase is empty or not
        /// </summary>
        public static bool IsEmpty(this DictionaryBase obj)
        {
            return ((obj == null) || (obj.Count == 0));
        }

        /// <summary>
        /// Check the given Hashtable is empty or not
        /// </summary>
        public static bool IsEmpty(this Hashtable obj)
        {
            return ((obj == null) || (obj.Count == 0));
        }

        /// <summary>
        /// Check the given Queue is empty or not
        /// </summary>
        public static bool IsEmpty(this Queue obj)
        {
            return ((obj == null) || (obj.Count == 0));
        }

        /// <summary>
        /// Check the given Queue is empty or not
        /// </summary>
        public static bool IsEmpty<T>(this Queue<T> obj)
        {
            return ((obj == null) || (obj.Count == 0));
        }

        /// <summary>
        /// Check the given ReadOnlyCollectionBase is empty or not
        /// </summary>
        public static bool IsEmpty(this ReadOnlyCollectionBase obj)
        {
            return ((obj == null) || (obj.Count == 0));
        }

        public static bool IsEmpty(this IDataParameterCollection obj)
        {
            return ((obj == null) || (obj.Count == 0));
        }

        /// <summary>
        /// Check the given SortedList is empty or not
        /// </summary>
        public static bool IsEmpty(this SortedList obj)
        {
            return ((obj == null) || (obj.Count == 0));
        }

        /// <summary>
        /// Check the given Stack is empty or not
        /// </summary>
        public static bool IsEmpty(this Stack obj)
        {
            return ((obj == null) || (obj.Count == 0));
        }

        /// <summary>
        /// Check the given generic is empty or not
        /// </summary>
        public static bool IsEmpty<T>(this ICollection<T> obj)
        {
            return ((obj == null) || (obj.Count == 0));
        }

        /// <summary>
        /// Equivalent to string.IsNullOrEmpty() but as an extension to string
        /// </summary>
        public static bool IsEmpty(this string target)
        {
            return string.IsNullOrEmpty(target);
        }
    }
}