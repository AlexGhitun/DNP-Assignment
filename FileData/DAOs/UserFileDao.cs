using Application.DaoInterfaces;
using Domain.DTOs;
using Domain.Models;

namespace FileData.DAOs.DAOs;

public class UserFileDao : IUserDao
{
    private readonly FileContext Context;

    public UserFileDao(FileContext context)
    {
        Context = context;
    }

    public Task<User> CreateAsync(User user)
    {
        int userId = 1;
        if (Context.Users.Any())
        {
            userId = Context.Users.Max(u => u.Id);
            userId++;
        }

        user.Id = userId;
        
        Context.Users.Add(user);
        Context.SaveChanges();

        return Task.FromResult(user);
    }
    
    public Task<User?> GetByUsernameAsync(string userName)
    {
        User? existing = Context.Users.FirstOrDefault(u =>
            u.Username.Equals(userName, StringComparison.OrdinalIgnoreCase)
        );
        return Task.FromResult(existing);
    }
    
    public Task<IEnumerable<User>> GetAsync(SearchUserParametersDto searchParameters)
    {
        IEnumerable<User> users = Context.Users.AsEnumerable();
        if (searchParameters.UsernameContains != null)
        {
            users = Context.Users.Where(u =>
                u.Username.Contains(searchParameters.UsernameContains, StringComparison.OrdinalIgnoreCase));
        }

        return Task.FromResult(users);
    }
    
    public Task<User?> GetByIdAsync(int id)
    {
        User? existing = Context.Users.FirstOrDefault(u =>
            u.Id == id
        );
        return Task.FromResult(existing);
    }
}