using Microsoft.EntityFrameworkCore;
using TestWebApp.Data;
using TestWebApp.Mappers;
using TestWebApp.Models.User;

namespace TestWebApp.Repositories;

public class PostgresqlUserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public PostgresqlUserRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<UserModel> CreateAsync(AddUserCommand command, CancellationToken cancellationToken)
    {
        var createdUser = await _context.Users.AddAsync(command.ToPostgresqlEntity(), cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return createdUser.Entity.ToUserModel();
    }

    public async Task<UserModel?> UpdateAsync(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FindAsync(new object?[] { command.Id }, cancellationToken: cancellationToken);
        if (user is null)
        {
            return null;
        }

        user.FullName = command.FullName;
        await _context.SaveChangesAsync(cancellationToken);

        return user.ToUserModel();
    }

    public async Task<UserModel?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FindAsync(new object?[] { id }, cancellationToken: cancellationToken);

        return user?.ToUserModel();
    }

    public async Task<IReadOnlyCollection<UserModel>> GetAllAsync(CancellationToken cancellationToken)
    {
        var users = await _context.Users.ToListAsync(cancellationToken);

        return users.Select(it => it.ToUserModel()).ToList();
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FindAsync(new object?[] { id }, cancellationToken: cancellationToken);
        if (user is null)
        {
            return;
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
