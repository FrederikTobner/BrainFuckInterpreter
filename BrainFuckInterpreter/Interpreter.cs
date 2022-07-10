using System.Security;
using System.Text;

namespace BrainFuckInterpreter;

internal static class Interpreter
{
    /// <summary>
    /// Pointer of the brainfuck program
    /// </summary>
    private static int pointer;

    /// <summary>
    /// Memory size
    /// </summary>
    private static readonly int memorySize = 1048576;

    /// <summary>
    /// Memory for the brainfuck program
    /// </summary>
    private static readonly byte[] memory = new byte[memorySize];

    /// <summary>
    /// Starts the interpreter from file or as command prompt, if no file is specified
    /// </summary>
    /// <param name="args">The arguments provided by the user when the interpreter was started</param>
    internal static void RunInterpreter(string[] args)
    {
        if (args.Length is 0)
            RunCommandPrompt();
        else if (args.Length is 1)
            RunFromFile(args[0]);
        else
            Console.WriteLine("usage: BrainFuckInterpreter [file]?");

    }

    /// <summary>
    /// Executes a BrainFuck program from the Command prompt
    /// </summary>
    private static void RunCommandPrompt()
    {
        for (; ; )
        {
            Console.WriteLine(">>");
            string? sourceCode = null;
            try { sourceCode = Console.ReadLine(); }
            catch (Exception)
            {
                Environment.Exit(0);
            }
            if (sourceCode is null || string.Empty.Equals(sourceCode))
            {
                return;
            }
            Run(sourceCode);
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Executes a BrainFuck program from a file
    /// </summary>
    private static void RunFromFile(string filePath)
    {
        byte[]? file = null;
        try
        {
            file = File.ReadAllBytes(filePath);
        }
        catch (Exception exception)
        {
            switch (exception)
            {
                case PathTooLongException:
                    Console.WriteLine("The specified path, file name, or both exceed the system-defined maximum length");
                    break;
                case DirectoryNotFoundException:
                    Console.WriteLine("The specified path is invalid (for example, it is on an unmapped drive)");
                    break;
                case IOException:
                    Console.WriteLine("An I/O error occurred while opening the file");
                    break;
                case UnauthorizedAccessException:
                    Console.WriteLine("This operation is not supported on the current platform. -or- path specified is a directory. -or- The caller does not have the required permission");
                    break;
                case NotSupportedException:
                    Console.WriteLine("path is in an invalid format.");
                    break;
                case SecurityException:
                    Console.WriteLine("The caller does not have the required permission.");
                    break;
                default:
                    Console.WriteLine("The file couldn't be found.");
                    break;
            }
            Environment.Exit(65);
        }
        string? sourceCode = null;
        try
        {
            sourceCode = Encoding.UTF8.GetString(file);
        }
        catch (ArgumentException)
        {
            Console.WriteLine("Couldn't endcode the file with using UTF-8");
            Environment.Exit(70);
        }
        Run(sourceCode);
    }

    /// <summary>
    /// Executes a BrainFuck program
    /// </summary>
    /// <param name="sourceCode">The sourcecode of the program that shall be executed</param>
    private static void Run(string sourceCode)
    {
        IReadOnlyList<Token> brainFuckProgram = Lexer.ScanTokens(sourceCode);
        try
        {
            Interpreter.RunProgram(brainFuckProgram);
        }
        catch (RunTimeError runTimeError)
        {
            System.Console.WriteLine(runTimeError.Message);
            Environment.Exit(70);
        }
    }


    /// <summary>
    /// Executes the Program written in Brainfuck
    /// </summary>
    /// <param name="program">The program that is executed</param>
    private static void RunProgram(IReadOnlyList<Token> brainFuckProgram)
    {
        int cell = 0;
        for (int i = 0; i < brainFuckProgram.Count; i++)
        {
            switch (brainFuckProgram[i].TokenType)
            {
                case TokenType.RIGHT_CHEVRON:
                    if (pointer == memorySize - 1)
                        pointer = 0;
                    else
                        pointer++;
                    break;
                case TokenType.LEFT_CHEVRON:
                    if (pointer is 0)
                    {
                        pointer = memorySize - 1;
                    }
                    else
                    {
                        pointer--;
                    }
                    break;
                case TokenType.PLUS:
                    memory[pointer]++;
                    break;
                case TokenType.MINUS:
                    memory[pointer]--;
                    break;
                case TokenType.DOT:
                    Console.Write((char)memory[pointer]);
                    break;
                case TokenType.COMMA:
                    memory[pointer] = (byte)Console.ReadKey().KeyChar;
                    break;
                case TokenType.LEFT_SQUARE_BRACKET:
                    if (memory[pointer] is 0)
                    {
                        i++;
                        while (cell is not 0 || brainFuckProgram[i].TokenType is not TokenType.RIGHT_SQUARE_BRACKET)
                        {
                            switch (brainFuckProgram[i].TokenType)
                            {
                                case TokenType.LEFT_SQUARE_BRACKET:
                                    cell++;
                                    break;
                                case TokenType.RIGHT_SQUARE_BRACKET:
                                    cell--;
                                    break;
                                default:
                                    break;
                            }
                            i++;
                            if (i >= brainFuckProgram.Count)
                            {
                                throw new RunTimeError("Expect ]");
                            }

                        }
                    }
                    break;
                case TokenType.RIGHT_SQUARE_BRACKET:
                    if (memory[pointer] is not 0)
                    {
                        i--;
                        while (cell is not 0 || brainFuckProgram[i].TokenType is not TokenType.LEFT_SQUARE_BRACKET)
                        {

                            switch (brainFuckProgram[i].TokenType)
                            {
                                case TokenType.RIGHT_SQUARE_BRACKET:
                                    cell++;
                                    break;
                                case TokenType.LEFT_SQUARE_BRACKET:
                                    cell--;
                                    break;
                                default:
                                    break;
                            }
                            i--;
                            if (i < 0)
                            {
                                throw new RunTimeError("Expect [");
                            }

                        }
                    }
                    break;
                default:
                    throw new NotImplementedException("Tokentype: " + brainFuckProgram[i].TokenType + " is not implemented");
            }
        }
    }
}
