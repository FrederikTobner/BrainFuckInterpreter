using System.Security;
using System.Text;

namespace BrainFuckInterpreter
{
    internal static class Interpreter
    {
        private static int pointer;
        private static readonly int memorySize = 65535;
        private static readonly byte[] memory = new byte[memorySize];

        internal static void RunCommandPrompt()
        {
            for (; ; )
            {
                Console.WriteLine(">>");
                string? brainFuckProgram = Console.ReadLine();
                if (brainFuckProgram is null || brainFuckProgram is "")
                {
                    return;
                }
                Interpreter.Interpret(brainFuckProgram);
                Console.WriteLine();
            }
        }

        internal static void RunFromFile(string filePath)
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
            Interpreter.Interpret(sourceCode);

        }

        private static void Interpret(string sourceCode)
        {
            int cell = 0;
            for (int i = 0; i < sourceCode.Length; i++)
            {
                switch (sourceCode[i])
                {
                    case '>':
                        if (pointer == memorySize - 1)
                            pointer = 0;
                        else
                            pointer++;
                        break;
                    case '<':
                        if (pointer is 0)
                        {
                            pointer = memorySize - 1;
                        }
                        else
                        {
                            pointer--;
                        }
                        break;
                    case '+':
                        memory[pointer]++;
                        break;
                    case '-':
                        memory[pointer]--;
                        break;
                    case '.':
                        Console.Write((char)memory[pointer]);
                        break;
                    case ',':
                        memory[pointer] = (byte)Console.ReadKey().KeyChar;
                        break;
                    case '[':
                        if (memory[pointer] is 0)
                        {
                            i++;
                            while (cell > 0 || sourceCode[i] is not ']')
                            {
                                if (sourceCode[i] is '[')
                                    cell++;
                                else if (sourceCode[i] is ']')
                                    cell--;
                                i++;
                            }
                        }
                        break;
                    case ']':
                        if (memory[pointer] is not 0)
                        {
                            i--;
                            while (cell > 0 || sourceCode[i] is not '[')
                            {
                                if (sourceCode[i] is '[')
                                    cell++;
                                else if (sourceCode[i] is ']')
                                    cell--;
                                i--;
                            }
                            i--;
                        }
                        break;
                    default:
                        //Brainfuck ignores all characters that are not brainfuck instructions (+-<>[],.)
                        break;
                }
            }
        }
    }
}
