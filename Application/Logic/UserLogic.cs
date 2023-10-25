using Application.DaoInterfaces;
using Application.LogicInterfaces;
using Domain.DTOs;
using Domain.Models;

namespace Application.Logic;

public class UserLogic : IUserLogic
{
    private readonly IUserDao UserDao;

    public UserLogic(IUserDao userDao)
    {
        UserDao = userDao;
    }

    public async Task<User> CreateAsync(UserCreationDto dto)
    {
        User? existing = await UserDao.GetByUsernameAsync(dto.Username);
        if (existing != null)
            throw new Exception("Username already taken!");

        ValidateData(dto);
        User toCreate = new User
        {
            Username = dto.Username,
            Password = dto.Password
        };

        User created = await UserDao.CreateAsync(toCreate);

        return created;
    }
    
    public Task<IEnumerable<User>> GetAsync(SearchUserParametersDto searchParameters)
    {
        return UserDao.GetAsync(searchParameters);
    }
    
    private static void ValidateData(UserCreationDto userToCreate)
    {
        string userName = userToCreate.Username;

        if (userName.Length < 3)
            throw new Exception("Username must bet at least 3 characters!");

        if (userName.Length > 15)
            throw new Exception("Username must be less than 16 characters!");
    }
}