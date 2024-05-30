using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BusinessLogicLayer
{
    public class LexicalAnalyzer
    {
        public List<Token> Tokenize(string input)
        {
            var tokens = new List<Token>();
            int position = 0;

            // Regular expressions for different token types
            Regex identifierRegex = new Regex(@"\b[a-zA-Z_]\w*\b");
            Regex numericLiteralRegex = new Regex(@"\b\d+(\.\d+)?\b");
            Regex stringLiteralRegex = new Regex(@"""(?:[^""]|"""")*""");
            Regex commaSeparatedRegex = new Regex(@"(?:^|,)\s*(?=(?:[^""]*""[^""]*"")*[^""]*$)");
            Regex spaceSeparatedRegex = new Regex(@"\s+");

            while (position < input.Length)
            {
                char currentChar = input[position];

                // Check for whitespace
                if (char.IsWhiteSpace(currentChar))
                {
                    position++;
                    continue;
                }

                // Check for identifiers
                Match identifierMatch = identifierRegex.Match(input, position);
                if (identifierMatch.Success)
                {
                    tokens.Add(new Token(TokenType.Identifier, identifierMatch.Value));
                    position += identifierMatch.Length;
                    continue;
                }

                // Check for numeric literals
                Match numericLiteralMatch = numericLiteralRegex.Match(input, position);
                if (numericLiteralMatch.Success)
                {
                    tokens.Add(new Token(TokenType.NumericLiteral, numericLiteralMatch.Value));
                    position += numericLiteralMatch.Length;
                    continue;
                }

                // Check for string literals
                Match stringLiteralMatch = stringLiteralRegex.Match(input, position);
                if (stringLiteralMatch.Success)
                {
                    tokens.Add(new Token(TokenType.StringLiteral, stringLiteralMatch.Value));
                    position += stringLiteralMatch.Length;
                    continue;
                }

                // Check for comma-separated values
                Match commaSeparatedMatch = commaSeparatedRegex.Match(input, position);
                if (commaSeparatedMatch.Success)
                {
                    string value = commaSeparatedMatch.Value.Trim(',', ' ');
                    tokens.Add(new Token(TokenType.CommaSeparatedValue, value));
                    position += commaSeparatedMatch.Length;
                    continue;
                }

                // Check for space-separated values
                Match spaceSeparatedMatch = spaceSeparatedRegex.Match(input, position);
                if (spaceSeparatedMatch.Success)
                {
                    string value = spaceSeparatedMatch.Value.Trim();
                    tokens.Add(new Token(TokenType.SpaceSeparatedValue, value));
                    position += spaceSeparatedMatch.Length;
                    continue;
                }

                position++;
            }

            return tokens;
        }

        public enum TokenType
        {
            Identifier,
            NumericLiteral,
            StringLiteral,
            CommaSeparatedValue,
            SpaceSeparatedValue
        }

        public class Token
        {
            public TokenType Type { get; set; }
            public string Value { get; set; }

            public Token(TokenType type, string value)
            {
                Type = type;
                Value = value;
            }
        }
    }
}
