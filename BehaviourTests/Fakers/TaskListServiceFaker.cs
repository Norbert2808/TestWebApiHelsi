using Bogus;
using TestWebApp.Contracts;
using TestWebApp.Models.TaskList;
using TestWebApp.Models.User;

namespace BehaviourTests.Fakers;

internal static class TaskListServiceFaker
{
    internal static readonly Faker<AddTaskListCommand> RandomAddTaskListCommand = new Faker<AddTaskListCommand>()
        .CustomInstantiator(faker => new AddTaskListCommand
        {
            UserId = faker.Random.Int(1, 100),
            Name = faker.Random.Word()
        });
    
    internal static readonly Faker<UpdateTaskListCommand> RandomUpdateTaskListCommand = new Faker<UpdateTaskListCommand>()
        .CustomInstantiator(faker => new UpdateTaskListCommand
        {
            Id = faker.Random.Int(1, 100),
            UserId = faker.Random.Int(1, 100),
            Name = faker.Random.Word()
        });
    
    internal static readonly Faker<DeleteTaskListCommand> RandomDeleteTaskListCommand = new Faker<DeleteTaskListCommand>()
        .CustomInstantiator(faker => new DeleteTaskListCommand
        {
            Id = faker.Random.Int(1, 100),
            UserId = faker.Random.Int(1, 100)
        });
    
    internal static readonly Faker<GetByIdTaskListCommand> RandomGetByIdTaskListCommand = new Faker<GetByIdTaskListCommand>()
        .CustomInstantiator(faker => new GetByIdTaskListCommand
        {
            Id = faker.Random.Int(1, 100),
            UserId = faker.Random.Int(1, 100)
        });
    
    internal static readonly Faker<TaskListFullModel> RandomTaskListFullModel = new Faker<TaskListFullModel>()
        .CustomInstantiator(faker => new TaskListFullModel
        {
            Id = faker.Random.Int(1, 100),
            Name = faker.Random.Word(),
            Owner = RandomUserModel.Generate(),
            SharedUsers = RandomUserModel.Generate(faker.Random.Int(2, 5))
        });
    
    internal static readonly Faker<DeleteConnectionCommand> RandomDeleteConnectionCommand = new Faker<DeleteConnectionCommand>()
        .CustomInstantiator(faker => new DeleteConnectionCommand
        {
            Id = faker.Random.Int(1, 100),
            UserId = faker.Random.Int(1, 100),
            ConnectionUserId = faker.Random.Int(1, 100)
        });
    
    internal static readonly Faker<AddConnectionCommand> RandomAddConnectionCommand = new Faker<AddConnectionCommand>()
        .CustomInstantiator(_ => RandomDeleteConnectionCommand.Generate());

    private static readonly Faker<GetAllTaskListCommand> RandomGetAllTaskListCommand = new Faker<GetAllTaskListCommand>()
        .CustomInstantiator(faker => new GetAllTaskListCommand(
            pagination: new PaginationQueryRequest
            {
                Page = faker.Random.Int(1, 100),
                ItemsPerPage = faker.Random.Int(1, 100)
            },
            userId: faker.Random.Int(1, 100)));
    
    private static readonly Faker<TaskListModel> RandomTaskListModel = new Faker<TaskListModel>()
        .CustomInstantiator(faker => new TaskListModel
        {
            Id = faker.Random.Int(1, 100),
            Name = faker.Random.Word(),
            Owner = RandomUserModel.Generate()
        });
    
    private static readonly Faker<UserModel> RandomUserModel = new Faker<UserModel>()
        .CustomInstantiator(faker => new UserModel
        {
            Id = faker.Random.Int(1, 100),
            FullName = faker.Random.Word()
        });
    
    internal static TaskListModel GenerateTaskListModel(string name)
    {
        var model = RandomTaskListModel.Generate();
        model.Name = name;
        
        return model;
    }
    
    internal static TaskListFullModel GenerateTaskListFullModel(int? ownerId, int? taskListId)
    {
        var model = RandomTaskListFullModel.Generate();
        if (ownerId.HasValue)
        {
            model.Owner.Id = ownerId.Value;
        }
        if (taskListId.HasValue)
        {
            model.Id = taskListId.Value;
        }
        
        return model;
    }
    
    internal static GetAllTaskListCommand GenerateGetAllTaskListCommand(int itemsPerPage, int page)
    {
        var command = RandomGetAllTaskListCommand.Generate();
        command.Page = page;
        command.ItemsPerPage = itemsPerPage;
        
        return command;
    }
}