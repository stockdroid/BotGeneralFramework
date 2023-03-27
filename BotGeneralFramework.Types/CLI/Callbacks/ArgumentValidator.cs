namespace BotGeneralFramework.Structs.CLI;

public delegate (bool success, string? error) ArgumentValidator(Stack<string> args);