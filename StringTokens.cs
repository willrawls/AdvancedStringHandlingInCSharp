using System;
using System.Text;

namespace AdvancedStringHandlingInCSharp
{
    /// <summary>Provides simple methods for retrieving tokens from a string.
    /// <para>A token is a piece of a delimited string. For instance in the string "this is a test" when " " (a space) is used as a delimiter, "this" is the first token and "test" is the last (4th) token.</para>
    /// <para>Asking for a token beyond the end of a string returns a blank string. Asking for the zeroth or a negative token returns a blank string.</para>
    /// </summary>
    public static class StringTokens
    {
        /// <summary>Returns the first delimited token in the indicated string</summary>
        /// <param name="target">The string to target</param>
        /// <param name="delimiter">The token delimiter</param>
        ///
        /// <example>
        /// <code>
        /// string x = Token.First("this is a test", " a ");
        /// // x = "this is"
        /// </code>
        /// </example>
        public static string FirstToken(this string target, string delimiter = " ")
        {
            return TokenAt(target, delimiter, 1);
        }

        /// <summary>Returns the first delimited token in the indicated string</summary>
        /// <param name="target">The string to target</param>
        /// <param name="delimiter">The token delimiter</param>
        ///
        /// <example>
        /// <code>
        /// string x = Token.First("this is a test", " a ");
        /// // x = "this is"
        /// </code>
        /// </example>
        public static string LastToken(this string target, string delimiter = " ")
        {
            return TokenAt(target, delimiter, target.TokenCount(delimiter));
        }

        /// <summary>Returns a single delimited token from a string</summary>
        /// <param name="target">The string to target</param>
        /// <param name="tokenNumber">The token to return</param>
        /// <param name="delimiter">The token delimiter</param>
        public static string TokenAt(this string target, int tokenNumber = 1, string delimiter = " ")
        {
            return target.TokenAt(delimiter, tokenNumber);
        }

        /// <summary>Returns a single delimited token from a string</summary>
        /// <param name="target">The string to target</param>
        /// <param name="tokenNumber">The token to return</param>
        /// <param name="delimiter">The token delimiter</param>
        ///
        public static string TokenAt(this string target, string delimiter = " ", int tokenNumber = 1)
        {
            int currTokenLocation;
            var delimiterLength = delimiter.Length;

            //  Negative or zeroth token or empty delimiter strings mean an empty token
            if (tokenNumber < 1 || delimiterLength < 1 || string.IsNullOrEmpty(target))
            {
                return string.Empty;
            }

            //  Quickly extract the first token
            if (tokenNumber == 1)
            {
                currTokenLocation = target.IndexOf(delimiter, StringComparison.OrdinalIgnoreCase);
                if (currTokenLocation > 0)
                {
                    return target.Substring(0, currTokenLocation);
                }
                if (currTokenLocation == 0)
                {
                    return string.Empty;
                }
                return target;
            }
            //  Find the Nth token
            while (tokenNumber > 1)
            {
                currTokenLocation = target.IndexOf(delimiter, StringComparison.OrdinalIgnoreCase);
                if (currTokenLocation == -1)
                {
                    return string.Empty;
                }
                target = target.Substring(currTokenLocation + delimiterLength);
                tokenNumber -= 1;
            }
            //  Extract the Nth token (Which is the next token at this point)
            currTokenLocation = target.IndexOf(delimiter, StringComparison.OrdinalIgnoreCase);
            if (currTokenLocation > 0)
            {
                return target.Substring(0, currTokenLocation);
            }
            return target;
        }

        public static string TokenBetween(this string target, string leftDelimiter, string rightDelimiter)
        {
            return string.IsNullOrEmpty(target)
                ? string.Empty
                : TokenAt(TokenAt(target, leftDelimiter, 2), rightDelimiter, 1);
        }

        /// <summary>Returns the number of tokens in a string</summary>
        /// <param name="target">The string to target</param>
        /// <param name="delimiter">The token delimiter</param>
        ///
        /// <example>
        /// <code>
        /// int x = Token.Count("this is a test", "is");
        /// // x = 2;
        /// x = Token.Count("this is a test", " ");
        /// // x = 4;
        /// </code>
        /// </example>
        public static int TokenCount(this string target, string delimiter = " ")
        {
            var delimiterLength = delimiter.Length;

            //  Empty delimiter strings means only one token equal to the string
            if (delimiterLength < 1) return 1;
            //  Empty input string means no tokens
            if (string.IsNullOrEmpty(target)) return 0;

            var tokensSoFar = 0;
            var lastAt = 0;
            do
            {
                var currTokenLocation = target.IndexOf(delimiter, lastAt, StringComparison.OrdinalIgnoreCase); //  Character position of the first delimiter string
                if (currTokenLocation == -1) return ++tokensSoFar;
                tokensSoFar += 1;
                lastAt = currTokenLocation + delimiterLength;
                //target = target.Substring(currTokenLocation + delimiterLength);
            }
            while (true);
        }

        /// <summary>
        /// Like TokenBetween but treats left and right delimiter as the beginning of the delimiter.
        /// So for "tag_x tag_y tag_z".TokenPartsBetween("tag_", " ") we would get a blank string.
        /// For "tag_x_y_z_1 fred tag_george".TokenPartsBetween("tag_", " ") we would get "fred".
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
            string[] parts = target.Tokens(leftDelimiterPart, true);
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
        public static string[] Tokens(this string target, string delimiter = " ", bool excludeDelimiter = false)
        {
            //  Empty delimiter strings means only one token equal to the string
            if (delimiter.Length < 1) return new[] { target };
            //  Empty input string means no tokens
            if (target.IsEmpty()) return new[] { string.Empty };

            var lastAt = 0;
            var tokenCount = target.TokenCount(delimiter);
            if (tokenCount == 1) return new[] { target };

            var tokens = new string[tokenCount];
            for (var i = 0; i < tokenCount - 1; i++)
            {
                var currTokenLocation = target.IndexOf(delimiter, lastAt, StringComparison.OrdinalIgnoreCase); //  Character position of the first delimiter string
                tokens[i] = target.Substring(lastAt, currTokenLocation + (excludeDelimiter ? 0 : delimiter.Length) - lastAt);
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
        ///
        /// <example>
        /// <code>
        /// string x = Token.After("this is a test", 2, " ");
        /// // x = "a test"
        /// </code>
        /// </example>
        public static string TokensAfter(this string target, int tokenNumber = 1, string delimiter = " ")
        {
            return target.TokensAfter(delimiter, tokenNumber);
        }

        /// <summary>Returns all tokens after the indicated token.</summary>
        /// <param name="target">The string to target</param>
        /// <param name="tokenNumber">The token number to return after</param>
        /// <param name="delimiter">The token delimiter</param>
        ///
        /// <example>
        /// <code>
        /// string x = Token.After("this is a test", 2, " ");
        /// // x = "a test"
        /// </code>
        /// </example>
        public static string TokensAfter(this string target, string delimiter = " ", int tokenNumber = 1)
        {
            int currTokenLocation; //  Character position of the first delimiter string
            var delimiterLength = delimiter.Length;
            if (tokenNumber < 1 || delimiterLength < 1) //  Negative or zeroth token or empty delimiter strings mean an empty token
            {
                return target;
            }
            if (tokenNumber == 1) //  Quickly extract the first token
            {
                currTokenLocation = target.IndexOf(delimiter, StringComparison.OrdinalIgnoreCase);
                if (currTokenLocation > 1)
                {
                    return target.Substring(currTokenLocation + delimiterLength);
                }
                if (currTokenLocation == -1)
                {
                    return string.Empty;
                }
                return target.Substring(currTokenLocation + delimiterLength);
            }
            do
            {
                currTokenLocation = target.IndexOf(delimiter, StringComparison.OrdinalIgnoreCase);
                if (currTokenLocation == -1)
                {
                    return string.Empty;
                }
                target = target.Substring(currTokenLocation + delimiterLength);
                tokenNumber -= 1;
            }
            while (tokenNumber > 1); //  Extract the Nth token (Which is the next token at this point)

            currTokenLocation = target.IndexOf(delimiter, StringComparison.OrdinalIgnoreCase);

            if (currTokenLocation > 0)
            {
                return target.Substring(currTokenLocation + delimiterLength);
            }
            return string.Empty;
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
        ///
        /// <example>
        /// <code>
        /// string x = Token.Before("this is a test", 3, " ");
        /// // x = "this is"
        /// </code>
        /// </example>
        public static string TokensBefore(this string target, int tokenNumber, string delimiter)
        {
            var delimiterLength = delimiter.Length; //  Length of the delimiter string
            if (tokenNumber < 2 || delimiterLength < 1) //  First, Zeroth, or Negative tokens or empty delimiter strings mean an empty string returned
            {
                return string.Empty;
            }
            if (tokenNumber == 2)
            {
                return TokenAt(target, 1, delimiter); //  Quickly extract the first token
            }

            //  Find the Nth token
            var lastIndex = 0;
            var tokensLeft = tokenNumber;
            do
            {
                var currTokenLocation = target.IndexOf(delimiter, lastIndex, StringComparison.OrdinalIgnoreCase); //  Character position of the first delimiter string
                if (currTokenLocation == -1 || tokensLeft-- == 1)
                {
                    if (lastIndex < 1) return string.Empty;
                    if (currTokenLocation > -1)
                        return target.Substring(0, currTokenLocation);
                    return target.Substring(0, lastIndex - 1);
                }
                lastIndex = currTokenLocation + delimiterLength;
            }
            while (true);
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