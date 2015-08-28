using System;
using System.Collections.Generic;
using System.Text;

namespace AdvancedStringHandlingInCSharp
{
    /// <summary>
    ///     Provides simple methods for retrieving tokens from a string.
    ///     <para>
    ///         A token is a piece of a delimited string. For instance in the string "this is a test" when " " (a space) is
    ///         used as a delimiter, "this" is the first token and "test" is the last (4th) token.
    ///     </para>
    ///     <para>
    ///         Asking for a token beyond the end of a string returns a blank string. Asking for the zeroth or a negative
    ///         token returns a blank string.
    ///     </para>
    /// </summary>
    public static class StringTokens
    {
        /// <summary>Returns the first delimited token in the indicated string</summary>
        /// <param name="target">The string to target</param>
        /// <param name="delimiter">The token delimiter</param>
        /// <example>
        ///     <code>
        ///  string x = Token.First("this is a test", " a ");
        ///  // x = "this is"
        ///  </code>
        /// </example>
        public static string FirstToken(this string target, string delimiter = " ")
        {
            return TokenAt(target, 1, delimiter);
        }

        /// <summary>Returns the first delimited token in the indicated string</summary>
        /// <param name="target">The string to target</param>
        /// <param name="delimiter">The token delimiter</param>
        /// <example>
        ///     <code>
        ///  string x = Token.First("this is a test", " a ");
        ///  // x = "this is"
        ///  </code>
        /// </example>
        public static string LastToken(this string target, string delimiter = " ")
        {
            return TokenAt(target, target.TokenCount(delimiter), delimiter);
        }

        /// <summary>Returns a single delimited token from a string</summary>
        /// <param name="target">The string to target</param>
        /// <param name="tokenNumber">The token to return</param>
        /// <param name="delimiter">The token delimiter</param>
        /// <param name="compare"></param>
        public static string TokenAt(this string target, 
            int tokenNumber = 1, 
            string delimiter = " ", 
            StringComparison compare = StringComparison.OrdinalIgnoreCase)
        {
            // Negative, 0 token or empty delimiter mean an empty token
            if (tokenNumber < 1
                || string.IsNullOrEmpty(target)
                || string.IsNullOrEmpty(delimiter))
                return string.Empty;

            var index = 0;
            var lastIndex = 0;

            // Find the beginning of the Nth token
            for (var i = 1; i < tokenNumber; i++)
            {
                index = target.IndexOf(delimiter, lastIndex, compare);
                if (index == -1)
                {
                    return string.Empty;
                }
                lastIndex = index + delimiter.Length;
            }

            //  Extract the Nth token
            index = target.IndexOf(delimiter, lastIndex, compare);
            if (index > 0)
            {
                return target.Substring(lastIndex, index - lastIndex);
            }
            return target.Substring(lastIndex);
        }

        public static string ReplaceTokenBetween(this string target,
            string leftDelimiter,
            string rightDelimiter,
            string replacementText)
        {
            if (string.IsNullOrEmpty(target)
                || string.IsNullOrEmpty(leftDelimiter)
                || string.IsNullOrEmpty(rightDelimiter))
                return string.Empty;

            string firstPart = target.TokenAt(1, leftDelimiter) + leftDelimiter;
            string lastPart = rightDelimiter + target.TokensAfterFirst(leftDelimiter).TokensAfterFirst(rightDelimiter);
            if(replacementText.IsNotEmpty())
                return firstPart + replacementText + lastPart;
            return firstPart + lastPart;
        }

        public static string TokenBetween(this string target, 
            string leftDelimiter, string rightDelimiter)
        {
            if (string.IsNullOrEmpty(target)
                || string.IsNullOrEmpty(leftDelimiter)
                || string.IsNullOrEmpty(rightDelimiter)) 
                return string.Empty;

            var partOne = TokenAt(target, 2, leftDelimiter);
            return TokenAt(partOne, 1, rightDelimiter);
        }

        /// <summary>Returns the number of tokens in a string</summary>
        /// <param name="target">The string to target</param>
        /// <param name="delimiter">The token delimiter</param>
        /// <example>
        ///     <code>
        ///  int x = Token.Count("this is a test", "is");
        ///  // x = 2;
        ///  x = Token.Count("this is a test", " ");
        ///  // x = 4;
        ///  </code>
        /// </example>
        public static int TokenCount(
            this string target, 
            string delimiter = " ", 
            StringComparison compare = StringComparison.OrdinalIgnoreCase)
        {
            //  Empty input string means no tokens
            if (string.IsNullOrEmpty(target))
                return 0;

            //  Empty delimiter strings means only one token equal to the string
            if (string.IsNullOrEmpty(delimiter))
                return 1;

            var tokensSoFar = 0;
            var lastAt = 0;

            var currTokenLocation = 0;
            while(currTokenLocation != -1)
            {
                tokensSoFar++;
                currTokenLocation = target.IndexOf(delimiter, lastAt, compare);
                lastAt = currTokenLocation + delimiter.Length;
            }
            return tokensSoFar;
        }

        /// <summary>
        ///     Like TokenBetween but treats left and right delimiter as the beginning of the delimiter.
        ///     So for:
        ///     ":x :y :z".TokenPartsBetween(":", " ")
        ///     We would get a blank string.
        ///     For:
        ///     ":x_y_z_1 fred :george".TokenPartsBetween(":", " ")
        ///     We would get "fred".
        /// </summary>
        /// <param name="target">The string to target</param>
        /// <param name="leftDelimiterPart">The first part of the left delimiter to look for</param>
        /// <param name="rightDelimiterPart">The first part of the right delimiter to look for</param>
        /// <returns></returns>
        public static string TokenPartsBetween(this string target, string leftDelimiterPart, string rightDelimiterPart)
        {
            if (target.IsEmpty()) return string.Empty;
            if (leftDelimiterPart.IsEmpty() || rightDelimiterPart.IsEmpty()) return target;

            var sb = new StringBuilder();
            var parts = target.Tokens(leftDelimiterPart, true);
            sb.Append(parts[0]);
            for (var i = 1; i < parts.Length; i++)
            {
                sb.Append(parts[i].TokensAfterFirst(rightDelimiterPart));
            }
            return sb.ToString();
        }

        /// <summary>Returns all the tokens in a string</summary>
        /// <param name="target">The string to target</param>
        /// <param name="delimiter">The token delimiter</param>
        /// <param name="excludeDelimiter">True if the resulting array should not include the delimiting text</param>
        public static string[] Tokens(this string target, string delimiter = " ", bool excludeDelimiter = true)
        {
            //  Empty delimiter strings means only one token equal to the string
            if (delimiter.Length < 1) return new[] {target};
            //  Empty input string means no tokens
            if (target.IsEmpty()) return new[] {string.Empty};

            var lastAt = 0;
            var tokenCount = target.TokenCount(delimiter);
            if (tokenCount == 1) return new[] {target};

            var tokens = new string[tokenCount];
            for (var i = 0; i < tokenCount - 1; i++)
            {
                var currTokenLocation = target.IndexOf(delimiter, lastAt, StringComparison.OrdinalIgnoreCase);
                //  Character position of the first delimiter string
                tokens[i] = target.Substring(lastAt,
                    currTokenLocation + (excludeDelimiter ? 0 : delimiter.Length) - lastAt);
                lastAt = currTokenLocation + delimiter.Length;
            }
            if (lastAt < target.Length)
                tokens[tokenCount - 1] = target.Substring(lastAt);
            else
                tokens[tokenCount - 1] = string.Empty;
            return tokens;
        }


        /// <summary>Returns all tokens after the indicated token.</summary>
        /// <param name="target">The string to target</param>
        /// <param name="tokenNumber">The token number to return after</param>
        /// <param name="delimiter">The token delimiter</param>
        /// <param name="compare"></param>
        /// <example>
        ///     <code>
        ///  string x = Token.After("this is a test", 2, " ");
        ///  // x = "a test"
        ///  </code>
        /// </example>
        public static string TokensAfter(this string target, 
            int tokenNumber = 1, 
            string delimiter = " ",
            StringComparison compare = StringComparison.OrdinalIgnoreCase)
        {
            if (tokenNumber < 1)                
                return target;

            if (string.IsNullOrEmpty(target)
                || string.IsNullOrEmpty(delimiter))
                return string.Empty;

            var lastIndex = 0;
            for (var i = 0; i < tokenNumber; i++)
            {
                var index = target.IndexOf(delimiter, lastIndex, compare);
                if (index == -1)
                {
                    return string.Empty;
                }
                lastIndex = index + delimiter.Length;
            }
            return target.Substring(lastIndex);
        }

        /// <summary>Returns everything after the first delimited token from a string</summary>
        /// <param name="target">The string to target</param>
        /// <param name="delimiter">The token delimiter</param>
        public static string TokensAfterFirst(this string target, string delimiter = " ")
        {
            return TokensAfter(target, 1, delimiter);
        }

        /// <summary>Returns all tokens before the indicated token.</summary>
        /// <param name="target">The string to target</param>
        /// <param name="tokenNumber">The token number to return before</param>
        /// <param name="delimiter">The token delimiter</param>
        /// <example>
        ///     <code>
        ///  string x = Token.Before("this is a test", 3, " ");
        ///  // x = "this is"
        ///  </code>
        /// </example>
        public static string TokensBefore(this string target, 
            int tokenNumber = 2, 
            string delimiter = " ", 
            StringComparison compare = StringComparison.OrdinalIgnoreCase)
        {
            if (tokenNumber < 1
                || string.IsNullOrEmpty(target)
                || string.IsNullOrEmpty(delimiter))
                return string.Empty;

            if (tokenNumber == 2)
            {
                return TokenAt(target, 1, delimiter, compare);
            }

            var index = 0;
            var lastIndex = 0;
            for (var i = 1; i < tokenNumber; i++)
            {
                index = target.IndexOf(delimiter, lastIndex, compare);
                if (index == -1)
                {
                    return target;
                }
                lastIndex = index + delimiter.Length;
            }
            if(index > 0)
                return target.Substring(0, index);
            return string.Empty;
        }

        /// <summary>Returns everything before the last delimited token from a string</summary>
        /// <param name="target">The string to target</param>
        /// <param name="delimiter">The token delimiter</param>
        public static string TokensBeforeLast(this string target, string delimiter = " ")
        {
            return TokensBefore(target, TokenCount(target, delimiter), delimiter);
        }
    }
}