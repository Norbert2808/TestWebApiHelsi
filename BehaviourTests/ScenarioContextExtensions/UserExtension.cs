namespace BehaviourTests.ScenarioContextExtensions;

internal static class UserExtension
{
    internal static void SetUserId(this ScenarioContext context, int userId)
    {
        context["userId"] = userId;
    }

    internal static int? GetUserId(this ScenarioContext context)
    {
        return context.TryGetValue("userId", out int res) ? res : null;
    }
}