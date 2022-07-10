namespace BrainFuckInterpreter;

internal class RunTimeError : ApplicationException
{
    public RunTimeError(string message) : base(message)
    {

    }
}
