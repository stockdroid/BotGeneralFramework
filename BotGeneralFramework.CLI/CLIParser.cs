namespace BotGeneralFramework.CLI;

using BotGeneralFramework.Structs.CLI;
using Fastenshtein;

public static class CLIParser
{
  private readonly static List<Argument> _arguments = new List<Argument>();

  private static Argument getSimilar(string arg)
  {
    var distanceCalculator = new Levenshtein(arg);

    // create a list of tuples containing the argument and its distance
    var distances = new List<(Argument arg, int distance)>();
    _arguments.ForEach(
      argument => distances.Add((
        argument,
        distanceCalculator.DistanceFrom(argument.Name) +
        distanceCalculator.DistanceFrom(argument.LongForm)
      ))
    );

    // sort the list of tuples by distance and return the first argument
    distances.Sort((a, b) => a.distance.CompareTo(b.distance));
    return distances[0].arg;
  }
  private static bool validateArgument(Argument argument, Stack<string> args)
  {
    (bool validated, string? validationError) = argument.Validator is null ? (true, null) : argument.Validator(args);
    if (!validated) Console.WriteLine($"{argument.Name}: {validationError}");

    return validated;
  }
  private static Argument[] getArguments(string arg)
  {
    // Extract the argument name from the command-line argument
    var argName = arg.StartsWith("--") ? arg.Substring(2) : arg.Substring(1);

    // Find the argument(s) that match the name
    var matchingArgs = _arguments.Where(a =>
        arg.StartsWith("--") ? a.LongForm == argName : a.ShortForm == argName);

    if (!matchingArgs.Any())
    {
      // If no matching argument is found, suggest a similar one
      var similarArg = getSimilar(argName);
      Console.WriteLine($"Unknown argument: {argName}, did you mean {similarArg.Name} (--{similarArg.LongForm})?");
      Environment.Exit(1); // Exit with error code 1
    }

    return matchingArgs.ToArray();
  }
  private static Options parseNext(Stack<string> args, Options current)
  {
    // Check if there are no more command-line arguments
    if (args.Count == 0)
    {
      current.Parsed = true;
      return current;
    }

    // Get the next command-line argument
    var arg = args.Pop();

    // Check if the argument is a flag or an option
    if (!arg.StartsWith("-") && !arg.StartsWith("--"))
    {
      // If it's not a flag or option, assume it's a script filename
      if (current.MainModule is null)
      {
        args.Push(arg);
        arg = "--script";
      }
      else
      {
        // If the current module has already been set, this is an error
        throw new ArgumentException($"Unexpected argument: {arg}, did you mean to use --{arg}?");
      }
    }

    // Get the argument object that corresponds to the current command-line argument
    var arguments = getArguments(arg);
    var argument = arguments.First();

    // Validate the argument and exit with an error code if it's not valid
    if (!validateArgument(argument, args)) Environment.Exit(1);

    // Execute the argument's action and return the result
    var result = argument.Action(args, current);
    if (argument.Terminate) Environment.Exit(0);

    return result;
  }

  /// <summary>
  /// Parse the command line arguments
  /// </summary>
  /// <param name="args">The command line arguments</param>
  /// <returns>The parsed options</returns>
  public static Options Parse(string[] args)
  {
    if (_arguments.Count == 0) throw new InvalidOperationException("No arguments have been added to the parser");
    if (args.Length == 0)
    {
      Console.WriteLine("No arguments provided, use --help to see the available arguments");
      Environment.Exit(1);
    }

    var options = new Options();
    var stack = new Stack<string>(args.Reverse());
    while (!options.Parsed) options = parseNext(stack, options);
    return options;
  }
  /// <summary>
  /// Add an argument to the parser
  /// </summary>
  /// <param name="argument">The argument to add</param>
  public static void AddArguments(params Argument[] argument) =>
    _arguments.AddRange(argument);

  static CLIParser()
  {
    AddArguments(
      new Argument
      {
        Name = "Help",
        LongForm = "help",
        ShortForm = "h",
        Description = "Show this help message.",
        Terminate = true,
        Action = (args, current) =>
        {
          _arguments.ForEach(
            a =>
            {
              // Print the name of the argument, the long form and the short form if it exists
              var oldfg = Console.ForegroundColor;
              Console.ForegroundColor = ConsoleColor.White;
              Console.Write($"{a.Name} ({(a.ShortForm is not null ? $"-{a.ShortForm}, " : "")}--{a.LongForm}):");
              Console.ForegroundColor = oldfg;
              Console.CursorLeft = 40;

              // Print the description of the argument
              oldfg = Console.ForegroundColor;
              Console.ForegroundColor = ConsoleColor.DarkGray;
              Console.WriteLine($"{a.Description}");
              Console.ForegroundColor = oldfg;
            }
          );
          return current;
        }
      },
      new Argument
      {
        Name = "Script",
        LongForm = "script",
        ShortForm = "s",
        Validator = (args) =>
        {
          string filename = args.Peek();
          if (Path.Exists(filename))
          {
            if (Path.GetExtension(filename).EndsWith(".js")) return (true, null);
            else return (false, $"The file {filename} is not a JavaScript file.");
          }
          else return (false, $"The file {filename} does not exist.");
        },
        Action = (args, current) =>
        {
          current.MainModule = args.Pop();
          return current;
        },
        Description = "Set the script to run."
      },
      new Argument
      {
        Name = "Verbose",
        LongForm = "verb",
        ShortForm = "v",
        Action = (args, current) =>
        {
          current.Verbose = true;
          return current;
        },
        Description = "Enable verbose mode."
      },
      new Argument
      {
        Name = "Time",
        LongForm = "time",
        ShortForm = "t",
        Action = (args, current) =>
        {
          current.Time = true;
          return current;
        },
        Description = "Enable timing mode."
      }
    );
  }
}