using TestWebApp.Models.User;

namespace TestWebApp.Services;

public interface IUserService
{
    Task<UserModel> CreateAsync(AddUserCommand user, CancellationToken cancellationToken);
    
    Task<UserModel?> UpdateAsync(UpdateUserCommand user, CancellationToken cancellationToken);
    
    Task<UserModel?> GetByIdAsync(int id, CancellationToken cancellationToken);
    
    Task<IReadOnlyCollection<UserModel>> GetAllAsync(CancellationToken cancellationToken);

    Task DeleteAsync(int id, CancellationToken cancellationToken);
}