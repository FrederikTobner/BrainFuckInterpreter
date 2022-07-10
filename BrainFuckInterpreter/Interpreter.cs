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

    internal static void Run(string[] args)
    {
        if (args.Length is 0)
            RunCommandPrompt();
        else if (args.Length is 1)
            RunFromFile(args[0]);
        else
            Console.WriteLine("usage: BrainFuckInterpreter [file]?");

    }

    //Run a brainfuck program from the command propmt
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
            if (sourceCode is null || sourceCode is "")
            {
                return;
            }
            IReadOnlyList<Token> brainFuckProgram = Lexer.ScanTokens(sourceCode);
            Interpret(brainFuckProgram);
            Console.WriteLine();
        }
    }

    //Run a brainfuck program from a file
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
        IReadOnlyList<Token> brainFuckProgram = Lexer.ScanTokens(sourceCode);
        Interpreter.Interpret(brainFuckProgram);

    }

    //Interprets a brainfuck program
    private static void Interpret(IReadOnlyList<Token> brainFuckProgram)
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
                        while (cell > 0 || brainFuckProgram[i].TokenType is not TokenType.RIGHT_SQUARE_BRACKET)
                        {
                            if (brainFuckProgram[i].TokenType is TokenType.LEFT_SQUARE_BRACKET)
                                cell++;
                            else if (brainFuckProgram[i].TokenType is TokenType.RIGHT_SQUARE_BRACKET)
                                cell--;
                            i++;
                        }
                    }
                    break;
                case TokenType.RIGHT_SQUARE_BRACKET:
                    if (memory[pointer] is not 0)
                    {
                        i--;
                        while (cell > 0 || brainFuckProgram[i].TokenType is not TokenType.LEFT_SQUARE_BRACKET)
                        {
                            if (brainFuckProgram[i].TokenType is TokenType.RIGHT_SQUARE_BRACKET)
                                cell++;
                            else if (brainFuckProgram[i].TokenType is TokenType.LEFT_SQUARE_BRACKET)
                                cell--;
                            i--;
                        }
                    }
                    break;
                default:
                    //Brainfuck ignores all characters that are not brainfuck instructions (+-<>[],.)
                    break;
            }
        }
    }
}
