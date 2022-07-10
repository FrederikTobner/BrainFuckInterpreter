namespace BrainFuckInterpreter;

internal class Token
{
    /// <summary>
    /// The line in the file where the Token occured
    /// </summary>
    public int Line { get; init; }

    /// <summary>
    /// The Type of the Token (e.g. </>/,/./[/])
    /// </summary>
    public TokenType TokenType { get; init; }

    public Token(int Line, TokenType TokenType)
    {
        this.Line = Line;
        this.TokenType = TokenType;
    }

    /// <summary>
    /// Converts the Token to a String
    /// </summary>
    public override string ToString() => this.TokenType.ToString() + " in line " + this.Line;
}
