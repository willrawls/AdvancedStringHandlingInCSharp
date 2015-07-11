using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace AdvancedStringHandlingInCSharp
{
    public static class NotEmpties
    {
        /*
                /// <summary>
                /// Check the given array IsNot empty or not
                /// </summary>
                public static bool IsNotEmpty(this Array obj)
                {
                    return ((obj != null) && (obj.Length != 0));
                }
        */

        /// <summary>
        /// Check the given ArrayList IsNot empty or not
        /// </summary>
        public static bool IsNotEmpty(this ArrayList obj)
        {
            return (obj != null) && (obj.Count != 0);
        }

        /// <summary>
        /// Check the given BitArray IsNot empty or not
        /// </summary>
        public static bool IsNotEmpty(this BitArray obj)
        {
            return (obj != null) && (obj.Count != 0);
        }

        /// <summary>
        /// Check the given CollectionBase IsNot empty or not
        /// </summary>
        public static bool IsNotEmpty(this CollectionBase obj)
        {
            return (obj != null) && (obj.Count != 0);
        }

        /// <summary>
        /// Check the given DictionaryBase IsNot empty or not
        /// </summary>
        public static bool IsNotEmpty(this DictionaryBase obj)
        {
            return (obj != null) && (obj.Count != 0);
        }

        /// <summary>
        /// Check the given Hashtable IsNot empty or not
        /// </summary>
        public static bool IsNotEmpty(this Hashtable obj)
        {
            return (obj != null) && (obj.Count != 0);
        }

        /// <summary>
        /// Check the given Queue IsNot empty or not
        /// </summary>
        public static bool IsNotEmpty(this Queue obj)
        {
            return (obj != null) && (obj.Count != 0);
        }

        /// <summary>
        /// Check the given Queue IsNot empty or not
        /// </summary>
        public static bool IsNotEmpty<T>(this Queue<T> obj)
        {
            return (obj != null) && (obj.Count != 0);
        }

        /// <summary>
        /// Check the given ReadOnlyCollectionBase IsNot empty or not
        /// </summary>
        public static bool IsNotEmpty(this ReadOnlyCollectionBase obj)
        {
            return (obj != null) && (obj.Count != 0);
        }

        public static bool IsNotEmpty(this IDataParameterCollection obj)
        {
            return (obj != null) && (obj.Count != 0);
        }

        /// <summary>
        /// Check the given SortedList IsNot empty or not
        /// </summary>
        public static bool IsNotEmpty(this SortedList obj)
        {
            return (obj != null) && (obj.Count != 0);
        }

        /// <summary>
        /// Check the given Stack IsNot empty or not
        /// </summary>
        public static bool IsNotEmpty(this Stack obj)
        {
            return (obj != null) && (obj.Count != 0);
        }

        /// <summary>
        /// Check the given generic IsNot empty or not
        /// </summary>
        public static bool IsNotEmpty<T>(this ICollection<T> obj)
        {
            return (obj != null) && (obj.Count != 0);
        }

        /// <summary>
        /// Equivalent to string.IsNullOrEmpty() but as an extension to string
        /// </summary>
        public static bool IsNotEmpty(this string target)
        {
            return !string.IsNullOrEmpty(target);
        }
    }
}