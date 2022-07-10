using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainFuckInterpreter
{
    internal static class Lexer
    {
        private static int current;
        private static int line;

        /// <summary>
        /// The Tokens that make up the sourceCode
        /// </summary>
        private static readonly List<Token> tokens = new();

        internal static IReadOnlyList<Token> ScanTokens(string sourceCode)
        {
            current = 0;
            line = 1;
            tokens.Clear();
            while (!IsAtEnd(sourceCode))
            {
                ScanToken(sourceCode);
            }
            return tokens;
        }

        private static void ScanToken(string sourceCode)
        {
            switch (sourceCode[current])
            {
                case '<':
                    tokens.Add(new Token(line, TokenType.LEFT_CHEVRON));
                    break;
                case '>':
                    tokens.Add(new Token(line, TokenType.RIGHT_CHEVRON));
                    break;
                case '+':
                    tokens.Add(new Token(line, TokenType.PLUS));
                    break;
                case '-':
                    tokens.Add(new Token(line, TokenType.MINUS));
                    break;
                case '.':
                    tokens.Add(new Token(line, TokenType.DOT));
                    break;
                case ',':
                    tokens.Add(new Token(line, TokenType.COMMA));
                    break;
                case '[':
                    tokens.Add(new Token(line, TokenType.LEFT_SQUARE_BRACKET));
                    break;
                case ']':
                    tokens.Add(new Token(line, TokenType.RIGHT_SQUARE_BRACKET));
                    break;
                case '\n':
                    line++;
                    break;
                default:
                    //Brainfuck ignores all characters that are not brainfuck instructions (+-<>[],.)
                    break;
            }
            current++;
        }

        private static char Advance(string sourceCode) => sourceCode[current++];

        private static bool IsAtEnd(string source) => current >= source.Length;
    }
}
