using System;
using System.Collections.Generic;
using System.Linq;

namespace AdvancedStringHandlingInCSharp
{
    public static class StringLists
    {
        public static List<string> ReconnectAfter(this List<string> target, string reconnectString, string joiner)
        {
            if (target == null)
                return new List<string>();
            if (target.Count == 0)
                return target;

            for (var i = 0; i < target.Count - 1; i++)
            {
                if (target[i] != reconnectString)
                    continue;
                var suffix = target[i + 1];
                if(suffix.IsNotEmpty())
                    target[i] += joiner + suffix;
                target.RemoveAt(i + 1);
            }
            return target;
        }

        public static bool AllMustHaveAndArePresent(this IList<string> target, IList<string> mustHaves)
        {
            if (mustHaves.IsEmpty()) return true;
            if (target.IsEmpty()) return false;
            var mustHavesNotFound = new List<string>(mustHaves);
            foreach (var item in target)
            {
                if (!mustHaves.Any(item.Contains)) return false;
                foreach (var currMustHave in mustHaves)
                {
                    if (item.Contains(currMustHave) && mustHavesNotFound.Contains(currMustHave))
                        mustHavesNotFound.Remove(currMustHave);
                }
            }
            return mustHavesNotFound.Count == 0;
        }

        /// <summary>
        /// All phrases in mustNotHaves must not appear in any item in target.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="mustNotHaves"></param>
        /// <returns></returns>
        public static bool AllMustNotHaveAny(this IList<string> target, IList<string> mustNotHaves)
        {
            return mustNotHaves.IsEmpty()
                   || (target.IsEmpty()
                       || target.All(item => !mustNotHaves.Any(item.Contains)));
        }

        public static string[] SplitAfterNumbers(this IList<string> target, bool autoTrim = true, bool reassembleIfWordFollowedByNumber = true)
        {
            if (target == null)
                return new string[] { };
            if (target.Count == 0)
                return target.ToArray();

            var parts = new List<string>(target);
            for (var index = 0; index < parts.Count; index++)
            {
                var part = parts[index];
                var subParts = part.SplitAfterNumbers(autoTrim, reassembleIfWordFollowedByNumber);
                if (subParts.Length <= 1)
                    continue;

                parts[index] = subParts[0];
                for (var i = 1; i < subParts.Length; i++)
                {
                    parts.Insert(index + i, subParts[i]);
                }
            }
            return parts.ToArray();
        }

        public static string[] SplitAroundAny(this IList<string> target, IList<string> splitPoints)
        {
            if (target == null)
                return new string[] { };
            if (target.Count == 0)
                return target.ToArray();

            var parts = new List<string>(target);
            for (var index = 0; index < parts.Count; index++)
            {
                var tryAgain = false;
                do
                {
                    var part = parts[index];
                    if (!part.IsNotEmpty())
                    {
                        index++;
                        continue;
                    }

                    tryAgain = false;
                    foreach (var splitPoint in splitPoints)
                    {
                        if (part == splitPoint || !part.Contains(splitPoint))
                            continue;
                        if (splitPoints.Contains(part))
                            continue;

                        var subParts = part.Split(new[] { splitPoint }, StringSplitOptions.None);
                        parts[index] = subParts[0];
                        var nextIndex = index + 1;
                        for (var i = 1; i < subParts.Length; i++)
                        {
                            parts.Insert(nextIndex++, splitPoint);
                            parts.Insert(nextIndex++, subParts[i]);
                        }
                        tryAgain = true;
                        break;
                    }
                }
                while (tryAgain);
            }
            return parts.ToArray();
        }
    }
}