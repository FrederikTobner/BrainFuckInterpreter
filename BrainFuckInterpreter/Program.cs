using BrainFuckInterpreter;

if (args.Length is 0)
    Interpreter.RunCommandPrompt();
else if (args.Length is 1)
    Interpreter.RunFromFile(args[0]);
else
    Console.WriteLine("usage: BrainFuckInterpreter [file]?");
