namespace BehaviourTests.ScenarioContextExtensions;

internal static class TaskListExtension
{
    internal static void SetTaskListId(this ScenarioContext context, int taskListId)
    {
        context["taskListId"] = taskListId;
    }

    internal static int? GetTaskListId(this ScenarioContext context)
    {
        return context.TryGetValue("taskListId", out int res) ? res : null;
    }
}