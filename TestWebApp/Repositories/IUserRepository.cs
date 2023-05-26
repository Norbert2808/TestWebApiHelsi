using TestWebApp.Models.User;

namespace TestWebApp.Repositories;

public interface IUserRepository
{
    Task<UserModel> CreateAsync(AddUserCommand command, CancellationToken cancellationToken);
    
    Task<UserModel?> UpdateAsync(UpdateUserCommand command, CancellationToken cancellationToken);
    
    Task<UserModel?> GetByIdAsync(int id, CancellationToken cancellationToken);
    
    Task<IReadOnlyCollection<UserModel>> GetAllAsync(CancellationToken cancellationToken);

    Task DeleteAsync(int id, CancellationToken cancellationToken);
}