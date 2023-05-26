using TestWebApp.Models.User;
using TestWebApp.Repositories;

namespace TestWebApp.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<UserModel> CreateAsync(AddUserCommand user, CancellationToken cancellationToken)
        => await _userRepository.CreateAsync(user, cancellationToken);

    public async Task<UserModel?> UpdateAsync(UpdateUserCommand user, CancellationToken cancellationToken)
        => await _userRepository.UpdateAsync(user, cancellationToken);

    public async Task<UserModel?> GetByIdAsync(int id, CancellationToken cancellationToken)
        => await _userRepository.GetByIdAsync(id, cancellationToken);

    public async Task<IReadOnlyCollection<UserModel>> GetAllAsync(CancellationToken cancellationToken)
        => await _userRepository.GetAllAsync(cancellationToken);

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        => await _userRepository.DeleteAsync(id, cancellationToken);
}