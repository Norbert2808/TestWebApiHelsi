namespace BehaviourTests.ScenarioContextExtensions;

internal static class CommandExtensions
{
    internal static void SetCommand<T>(this ScenarioContext context, T command)
        where T : class
    {
        context["command"] = command;
    }

    internal static T? GetCommand<T>(this ScenarioContext context)
        where T : class
    {
        return context.TryGetValue("command", out T res) ? res : null;
    }

    internal static void SetCommandResult<T>(this ScenarioContext context, T commandResult)
        where T : class?
    {
        context["command_result"] = commandResult;
    }

    internal static T? GetCommandResult<T>(this ScenarioContext context)
        where T : class
    {
        return context.TryGetValue("command_result", out T res) ? res : null;
    }
}