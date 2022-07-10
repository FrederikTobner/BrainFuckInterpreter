namespace BrainFuckInterpreter;

internal class Token
{
    public int Line { get; init; }

    public TokenType TokenType { get; init; }

    public Token(int Line, TokenType TokenType)
    {
        this.Line = Line;
        this.TokenType = TokenType;
    }

    public override string ToString() => this.TokenType.ToString() + " in line " + this.Line;
}
