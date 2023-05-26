using Microsoft.EntityFrameworkCore;
using TestWebApp.Data;
using TestWebApp.Mappers;
using TestWebApp.Models.TaskList;

namespace TestWebApp.Repositories;

public class PostgresqlTaskListRepository : ITaskListRepository
{
    private readonly ApplicationDbContext _context;

    public PostgresqlTaskListRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<TaskListModel> CreateAsync(AddTaskListCommand command, CancellationToken cancellationToken)
    {
        var createdUser = await _context.TaskLists.AddAsync(command.ToPostgresqlEntity(), cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return createdUser.Entity.ToTaskListModel();
    }

    public async Task<TaskListModel?> UpdateAsync(UpdateTaskListCommand command, CancellationToken cancellationToken)
    {
        var oldTaskList = await _context.TaskLists.FindAsync(new object?[] { command.Id }, cancellationToken: cancellationToken);
        if (oldTaskList is null)
        {
            return null;
        }

        oldTaskList.Name = command.Name;
        await _context.SaveChangesAsync(cancellationToken);

        return oldTaskList.ToTaskListModel();
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var taskList = await _context.TaskLists.FindAsync(new object?[] { id }, cancellationToken: cancellationToken);
        if (taskList is null)
        {
            return;
        }

        _context.TaskLists.Remove(taskList);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<TaskListFullModel?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var taskList = await _context.TaskLists.Include(it => it.SharedUsers)
            .Include(it => it.Owner)
            .FirstOrDefaultAsync(it => it.Id == id, cancellationToken);

        return taskList?.ToTaskListFullModel();
    }

    public async Task<IReadOnlyCollection<TaskListFullModel>> GetAllAsync(CancellationToken cancellationToken)
    {
        var taskLists = await _context.TaskLists.Include(it => it.SharedUsers)
            .Include(it => it.Owner)
            .ToListAsync(cancellationToken);

        return taskLists.Select(it => it.ToTaskListFullModel()).ToList();
    }

    public async Task AddConnectionAsync(AddConnectionCommand command, CancellationToken cancellationToken)
    {
        var userTaskLists = await _context.UserTaskLists.ToListAsync(cancellationToken);
        if (userTaskLists.Any(it => it.UserId == command.ConnectionUserId && it.TaskListId == command.Id))
        {
            return;
        }

        await _context.UserTaskLists.AddAsync(command.ToPostgresqlEntity(), cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteConnectionAsync(DeleteConnectionCommand command, CancellationToken cancellationToken)
    {
        var userTaskLists = await _context.UserTaskLists.ToListAsync(cancellationToken);
        var connection = userTaskLists
            .FirstOrDefault(it => it.UserId == command.ConnectionUserId && it.TaskListId == command.Id);
        if (connection is null)
        {
            return;
        }

        _context.UserTaskLists.Remove(connection);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
