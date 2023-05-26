using TestWebApp.Contracts.User;
using TestWebApp.Data.Entities;
using TestWebApp.Models.User;

namespace TestWebApp.Mappers;

public static class UserMapper
{
    public static AddUserCommand ToAddCommand(this AddUserRequest user)
    {
        return new AddUserCommand
        {
            FullName = user.FullName
        };
    }
    
    public static User ToPostgresqlEntity(this AddUserCommand user)
    {
        return new User
        {
            FullName = user.FullName
        };
    }
    
    public static UserModel ToUserModel(this User user)
    {
        return new UserModel
        {
            Id = user.Id,
            FullName = user.FullName
        };
    }
    
    public static UserResponse ToResponse(this UserModel user)
    {
        return new UserResponse
        {
            Id = user.Id,
            FullName = user.FullName
        };
    }
    
    public static UpdateUserCommand ToUpdateCommand(this UpdateUserRequest user)
    {
        return new UpdateUserCommand
        {
            Id = user.Id,
            FullName = user.FullName
        };
    }
}
