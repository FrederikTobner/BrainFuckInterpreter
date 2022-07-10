namespace BrainFuckInterpreter
{
    internal enum TokenType
    {
        /// <summary>
        /// &gt;
        /// </summary>
        LEFT_CHEVRON,

        /// <summary>
        /// &lt;
        /// </summary>
        RIGHT_CHEVRON,

        /// <summary>
        /// +
        /// </summary>
        PLUS,

        /// <summary>
        /// -
        /// </summary>
        MINUS,

        /// <summary>
        /// .
        /// </summary>
        DOT,

        /// <summary>
        /// ,
        /// </summary>
        COMMA,

        /// <summary>
        /// [
        /// </summary>
        LEFT_SQUARE_BRACKET,

        /// <summary>
        /// ]
        /// </summary>
        RIGHT_SQUARE_BRACKET,
    }
}
