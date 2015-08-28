using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdvancedStringHandlingInCSharp
{
    public static class Strings
    {
        public static int AsInteger(this string target)
        {
            if (target.IsEmpty())
            {
                return 0;
            }
            int result;
            target = target.FirstNumbers().Trim();
            return Int32.TryParse(target, out result)
                ? result
                : 0;
        }

        public static string AsString(this object target)
        {
            if (target == null)
            {
                return string.Empty;
            }
            if (!(target is string))
            {
                return target.ToString();
            }
            return (string)target;
        }

        public static string AsString(this string[] target, string separator)
        {
            if (target.IsEmpty())
            {
                return string.Empty;
            }
            if (separator == null)
            {
                separator = string.Empty;
            }
            if (target.Length < 11)
            {
                var result = string.Empty;
                foreach (var s in target)
                {
                    if (result.Length > 0 && separator.Length > 0)
                    {
                        result += separator + s;
                    }
                    else
                    {
                        result += s;
                    }
                }
                return result;
            }
            var builder = new StringBuilder();
            foreach (var s in target)
            {
                if (builder.Length > 0 && separator.Length > 0)
                {
                    builder.Append(separator);
                }
                builder.Append(s);
            }
            return builder.ToString();
        }

        /// <summary>
        /// Returns true if target contains every value in items (case insensitive).
        /// Returns false if target or items is null or empty or no matches.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static bool ContainsAll(this string target, params string[] items)
        {
            return !target.IsEmpty()
                   && (!items.IsEmpty()
                       && items
                           .All(s => target
                               .IndexOf(s, StringComparison.OrdinalIgnoreCase) > -1));
        }

        /// <summary>
        /// Returns true if target contains any value in items (case insensitive).
        /// Returns false if target or items is null or empty or no matches.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static bool ContainsAny(this string target, params char[] items)
        {
            return !target.IsEmpty()
                   && (!items.IsEmpty()
                       && items
                           .Any(s => target
                               .IndexOf(s.ToString(), StringComparison.OrdinalIgnoreCase) > -1));
        }

        /// <summary>
        /// Returns true if target contains any value in items (case insensitive).
        /// Returns false if target or items is null or empty or no matches.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static bool ContainsAny(this string target, params string[] items)
        {
            if (target.IsEmpty())
            {
                return false;
            }
            if (items.IsEmpty())
            {
                return false;
            }
            return items.Any(s => s.IsNotEmpty() && target.IndexOf(s, StringComparison.OrdinalIgnoreCase) > -1);
        }

        /// <summary>
        /// Returns true if: (1) Both strings are empty or null. (2) Both strings are not null and are equal.
        /// NOTE: For this function, NULL == ""
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static bool Equate(this string s1, string s2)
        {
            return s1.IsEmpty() && s2.IsEmpty() || s1 == s2;
        }

        public static string FirstCharcters(this string target)
        {
            if (string.IsNullOrEmpty(target))
            {
                return string.Empty;
            }

            var startAt = 0;
            var endAt = 0;

            // Find the first character
            for (var i = 0; i < target.Length; i++)
            {
                if (!Char.IsLetter(target[i]))
                {
                    continue;
                }
                startAt = i;
                break;
            }

            // Find the last continues string of characters
            for (var i = startAt + 1; i < target.Length; i++)
            {
                if (Char.IsLetter(target[i]))
                {
                    continue;
                }
                endAt = i;
                break;
            }

            return target.Substring(startAt, endAt - startAt);
        }

        public static string FirstNumbers(this string target)
        {
            if (string.IsNullOrEmpty(target))
            {
                return string.Empty;
            }

            var startAt = 0;
            var endAt = 0;

            // Find the first character
            for (var i = 0; i < target.Length; i++)
            {
                if (Char.IsLetter(target[i]))
                {
                    continue;
                }
                startAt = i;
                break;
            }

            // Find the last continues string of characters
            for (var i = startAt + 1; i < target.Length; i++)
            {
                if (!Char.IsLetter(target[i]))
                {
                    continue;
                }
                endAt = i;
                break;
            }
            if (endAt == 0)
            {
                return target.Substring(startAt);
            }
            return target.Substring(startAt, endAt - startAt);
        }

        public static int FirstSequenceLength(this string target, char delimiter)
        {
            var strDelimiter = delimiter.ToString();
            if (target.IsEmpty() || target.IndexOf(strDelimiter, StringComparison.Ordinal) == -1)
            {
                return 0;
            }
            var firstIndex = target.IndexOf(strDelimiter, StringComparison.Ordinal);
            var secondIndex = firstIndex + 1;
            while (secondIndex < target.Length && target[secondIndex] == delimiter)
            {
                secondIndex++;
            }
            return secondIndex - firstIndex;
        }

        public static string InjectSpacesIntoCamelCase(this string target)
        {
            if (string.IsNullOrEmpty(target))
                return string.Empty;

            var sb = new StringBuilder(target);
            var lastUpper = -1;
            var lastUpperChar = ' ';
            for (var i = 1; i < sb.Length; i++)
            {
                if (!char.IsUpper(sb[i]))
                    continue;

                if (lastUpper != i - 1 || lastUpperChar == 'A')
                    sb.Insert(i, " ");
                lastUpper = i;
                lastUpperChar = sb[++i];
            }
            return sb.ToString();
        }

        /// <summary>
        /// Returns true if T is null or if T.ToString() == ""
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsEmptyWhenTrimmed(this string target)
        {
            return target == null || string.IsNullOrEmpty(target.SafeTrim());
        }

        /// <summary>
        /// Returns true if target is equal to any value in items (case insensitive).
        /// Returns false if target or items is null or empty or no matches.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static bool IsIn(this string target, IList<string> items)
        {
            if (target.IsEmpty())
            {
                return false;
            }
            if (items.IsEmpty())
            {
                return false;
            }
            return items.Any(s => string.Equals(target, s, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Returns true if target is not equal to any value in items (case insensitive).
        /// Returns false if target or items is null or empty.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static bool IsNotIn(this string target, params string[] items)
        {
            if (target.IsEmpty())
            {
                return false;
            }
            if (items.IsEmpty())
            {
                return false;
            }
            return !items.Any(s => string.Equals(target, s, StringComparison.OrdinalIgnoreCase));
        }

        public static bool IsNumeric(this char target, bool alreadyInsideANumber)
        {
            return target == '+'
                   || target == '-'
                   || target == '.'
                   || Char.IsDigit(target)
                   || (alreadyInsideANumber && target == 'e');
        }

        public static string Left(this string target, int length)
        {
            return string.IsNullOrEmpty(target) || length < 1
                ? string.Empty
                : (target.Length <= length
                    ? target
                    : target.Substring(0, length));
        }

        public static string[] Lines(this string target)
        {
            if (string.IsNullOrEmpty(target))
                return new[] { string.Empty };
            return target
                .Remove("\r", false, false)
                .Split(new[] { "\n" }, StringSplitOptions.None);
        }

        public static List<string> NonBlankLines(this string target, bool treatUnderscoresAsBlanks = false)
        {
            if (target.IsEmpty())
            {
                return new List<string>();
            }

            if (treatUnderscoresAsBlanks)
            {
                return new List<string>(target
                    .Remove("\r")
                    .Replace("_", " ")
                    .Trim()
                    .Split(new[] { '\n' }, StringSplitOptions
                        .RemoveEmptyEntries));
            }
            return new List<string>(target
                .Remove("\r")
                .Trim()
                .Split(new[] { '\n' }, StringSplitOptions
                    .RemoveEmptyEntries));
        }

        public static string Remove(this string target, string charactersToRemove, bool trimmed = true,
                                    bool removeExtraSpaces = true)
        {
            if (target.IsEmpty())
            {
                return string.Empty;
            }

            var builder = new StringBuilder(target);
            foreach (var c in charactersToRemove)
            {
                builder.Replace(c.ToString(), string.Empty);
            }

            if (removeExtraSpaces)
            {
                for (var i = 0; i < 10; i++)
                {
                    builder.Replace("  ", " ");
                }
            }

            return trimmed
                ? builder.ToString().Trim()
                : builder.ToString();
        }

        public static string Remove(this string target, params string[] stringsToRemove)
        {
            if (target.IsEmpty())
            {
                return string.Empty;
            }

            var builder = new StringBuilder(target);
            foreach (var s in stringsToRemove)
            {
                builder.Replace(s, string.Empty);
            }
            return builder.ToString();
        }

        public static string RemoveTagRefs(this string target)
        {
            if (target.IsEmpty())
            {
                return string.Empty;
            }

            var lines = target.Split('\n');
            for (var i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].TokenPartsBetween("tag_", " ");
            }
            var result = string.Join("\n", lines); //.Where(s => s.Length > 0).ToArray());
            if (target.Contains("\n") && target.ContainsAll("_Y_", "_N_")) // val_Y_val_N_     or like tag_mc_b1_val_Y_val_N_end
            {
                return target.FirstToken("\n").Trim() + "\nY  N";
            }
            //if (result == "Completed\n" && target.Contains("val_Y_val_N_")) return "Completed\nY  N";
            return result;
        }

        public static string ReplaceEach(this string target, string charactersToRemove,
                                         string replacementString = " ", bool trimmed = true,
                                         bool removeExtraSpaces = true)
        {
            if (target.IsEmpty())
            {
                return string.Empty;
            }

            var builder = new StringBuilder(target);
            foreach (var c in charactersToRemove)
            {
                builder.Replace(c.ToString(), replacementString);
            }

            if (removeExtraSpaces)
            {
                for (var i = 0; i < 10; i++)
                {
                    builder.Replace("  ", " ");
                }
            }

            return trimmed
                ? builder.ToString().Trim()
                : builder.ToString();
        }

        public static string SafeTrim(this string target)
        {
            if (target == null)
            {
                return string.Empty;
            }
            return target.Trim();
        }

        public static string[][] Splice(this string target, char delimiterOne = '|', char delimiterTwo = ',',
                                        StringSplitOptions options = StringSplitOptions.None, bool allowJagged = false)
        {
            if (target.IsEmpty() || delimiterOne == delimiterTwo)
            {
                return null;
            }
            var splitOne = target.Split(new[] { delimiterOne }, options);

            var rows = splitOne.Length;
            var columns = splitOne[0].TokenCount();
            if (rows == 0 || columns == 0)
            {
                return null;
            }

            var result = new string[rows][];
            var maxLength = 0;
            for (var i = 0; i < splitOne.Length; i++)
            {
                result[i] = splitOne[i].Split(new[] { delimiterTwo }, options);
                if (result[i].Length > maxLength)
                    maxLength = result[i].Length;
            }
            if (!allowJagged)
                for (var i = 0; i < result.Length; i++)
                {
                    if (result[i].Length < maxLength)
                        Array.Resize(ref result[i], maxLength);
                }
            return result;
        }

        public static string[] SplitAfterNumbers(this string target, bool autoTrim = true, bool reassembleIfWordFollowedByNumber = true)
        {
            if (string.IsNullOrEmpty(target))
                return new[] { string.Empty };
            if (target.Contains(" "))
                return null;

            bool inWord = !target[0].IsNumeric(false);
            if (inWord)
                return new[] { target };

            var results = new List<string>();
            var lastBreak = 0;

            for (var i = 0; i < target.Length; i++)
            {
                if (inWord)
                {
                    // ReSharper disable once InvertIf
                    if (target[i].IsNumeric(false))
                    {
                        results.Add(target.Substring(lastBreak, i - lastBreak));
                        inWord = false;
                        lastBreak = i;
                    }
                }
                else if (!target[i].IsNumeric(true))
                {
                    if (i <= 0
                        || i >= target.Length - 1
                        || target[i] != 'e'
                        || (target[i + 1] != '-' && target[i + 1] != '+'))
                    {
                        results.Add(target.Substring(lastBreak, i - lastBreak));
                        inWord = true;
                        lastBreak = i;
                    }
                }
            }

            if (lastBreak == 0)
            {
                results.Add(target);
            }
            else if (lastBreak < target.Length)
            {
                results.Add(target.Substring(lastBreak));
            }

            if (!autoTrim)
            {
                return results.ToArray();
            }

            for (var i = 0; i < results.Count; i++)
            {
                results[i] = results[i].Trim();
            }

            return results.ToArray();
        }

        public static string ToDebugInfo<T>(T toSerialize) where T : new()
        {
            return XmlSerializers.ToXml(toSerialize).XmlToReadable().Replace(Environment.NewLine, "\n");
        }

        /// <summary>
        /// Converts a string to a decimal rounding the value to X decimal places (where X is the value of decimals)
        /// Otherwise returns a defaultValue
        /// </summary>
        /// <param name="target"></param>
        /// <param name="decimals"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static decimal ToRoundedDecimal(this string target, int decimals = 2, decimal defaultValue = 0)
        {
            if (target.IsEmpty())
            {
                return defaultValue;
            }

            decimal result;
            if (Decimal.TryParse(target, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
            {
                result = Math.Round(result, decimals);
                return result;
            }
            return defaultValue;
        }

        public static string XmlToReadable(this string target, string nodeNameToTranslateIntoTable = null)
        {
            if (target.IsEmpty())
            {
                return string.Empty;
            }
            var sb = new StringBuilder(target);
            sb.Replace("</ ", string.Empty);
            sb.Replace("</", string.Empty);
            sb.Replace("<", string.Empty);
            sb.Replace(" />", string.Empty);
            sb.Replace("/>", string.Empty);
            sb.Replace(">", string.Empty);
            if (nodeNameToTranslateIntoTable.IsNotEmpty())
            {
                List<string> lines = sb.ToString()
                    .NonBlankLines()
                    .Where(l => l.Trim().StartsWith(nodeNameToTranslateIntoTable + " "))
                    .ToList();
                if (lines.IsNotEmpty())
                {
                    // Get column names
                    var tokens = lines[0].Trim().TokensAfterFirst(" ").Split('\"');
                    var columnNames = new List<string>();
                    var lengths = new List<int>();
                    for (var i = 0; i < tokens.Length; i += 2)
                    {
                        var columnName = tokens[i].Remove("=").Trim();
                        if (columnName.IsEmpty())
                            continue;

                        columnNames.Add(columnName.InjectSpacesIntoCamelCase());
                        lengths.Add(columnName.Length + 1);
                    }

                    // Get longest lengths
                    foreach (var line in lines)
                    {
                        tokens = line.Trim().TokensAfterFirst(" ").Split('\"');
                        for (var i = 1; i < tokens.Length; i += 2)
                        {
                            var value = tokens[i];
                            string columnName = tokens[i - 1].Remove("=").Trim().InjectSpacesIntoCamelCase();
                            var columnIndex = columnNames.FindIndex(x => x == columnName);
                            if (columnIndex == -1)
                            {
                                columnNames.Add(columnName);
                                lengths.Add(columnName.Length + 1);
                                columnIndex = lengths.Count - 1;
                            }
                            if (value.Length > lengths[columnIndex])
                                lengths[columnIndex] = value.Length + 1;
                        }
                    }

                    // Build the table
                    sb.Clear();
                    for (var j = 0; j < lengths.Count; j++)
                    {
                        var value = columnNames[j];
                        if (value.Length < lengths[j])
                            value += new string(' ', lengths[j] - value.Length);
                        sb.Append(value);
                        sb.Append(' ');
                    }
                    sb.Append("\n");
                    foreach (var line in lines)
                    {
                        tokens = line.Trim().TokensAfterFirst(" ").Split('\"');
                        for (var columnIndex = 0; columnIndex < columnNames.Count; columnIndex++)
                        {
                            var found = false;
                            for (var i = 1; i < tokens.Length; i += 2)
                            {
                                var value = tokens[i];
                                string columnName = tokens[i - 1].Remove("=").Trim().InjectSpacesIntoCamelCase();
                                if (columnNames[columnIndex] == columnName)
                                {
                                    if (value.Length < lengths[columnIndex])
                                        value += new string(' ', lengths[columnIndex] - value.Length);
                                    sb.Append(value);
                                    sb.Append(' ');
                                    found = true;
                                    break;
                                }
                            }
                            if (!found)
                                sb.Append(new string(' ', lengths[columnIndex] + 1));
                        }
                        sb.Append("\n");
                    }
                }
            }
            return sb.ToString();
        }

        /*
                /// <summary>
                /// Returns true if T is null or if T.ToString() == ""
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="target"></param>
                /// <returns></returns>
                public static bool IsEmpty<T>(this T target) where T : class
                {
                    return target == null || string.IsNullOrEmpty(target.ToString());
                }
        */

        /*
                /// <summary>
                /// Returns true if T is null or if T.ToString() == ""
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="target"></param>
                /// <returns></returns>
                public static bool IsEmptyWhenTrimmed<T>(this T target) where T : class
                {
                    return target == null || target.ToString().Trim().IsEmpty();
                }
        */
    }
}