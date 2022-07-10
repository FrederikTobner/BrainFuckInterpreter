namespace BrainFuckInterpreter;

internal static class Lexer
{
    /// <summary>
    /// CurrentPosition in the sourcecode
    /// </summary>
    private static int current;

    /// <summary>
    /// The current line in the sourcecode
    /// </summary>
    private static int line;

    /// <summary>
    /// The Tokens that make up the sourceCode
    /// </summary>
    private static readonly List<Token> tokens = new();

    /// <summary>
    /// Scans the sourceCode and returns them as a linear sequence of Tokens
    /// </summary>
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

    /// <summary>
    /// Scans the next Token 🔍
    /// </summary>
    private static void ScanToken(string sourceCode)
    {
        switch (Advance(sourceCode))
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
    }


    /// <summary>
    /// Advances a postion further in the sourceCode (one character) and returns the character at the previous position
    /// </summary>
    private static char Advance(string sourceCode) => sourceCode[current++];

    /// <summary>
    /// Indicates whether the Lexer has reached the end of the file
    /// </summary>
    private static bool IsAtEnd(string source) => current >= source.Length;
}
