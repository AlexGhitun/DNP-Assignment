using System.ComponentModel.DataAnnotations;
using Application.DaoInterfaces;
using Domain.Models;
using Microsoft.AspNetCore.Connections;

namespace WebAPI.Services;

public class AuthService : IAuthService
{
    // private IUserDao dao;
    // public AuthService(IUserDao dao)
    // {
    //     this.dao = dao;
    // }
    //
    // IList<User> users = new List<User>(); possible solution for adding it to json file and extracting it later from there
    
    private readonly IList<User> users = new List<User>
    {
        new User
        {
            Id = 36,
            Username = "trmo",
            Password = "1234",
            LoggedIn = "Yes"
                
        },
        new User
        {
            Id = 35,
            Username = "username",
            Password = "password",
            LoggedIn = "Yes"
        }
    };
    
    public Task<User> ValidateUser(string username, string password)
    {
        // foreach (var user in dao.GetAsync(null).Result)
        // {
        //     users.Add(user);
        //     Console.Write($"user.Username");
        // } possible solution for adding it to json file and extracting it later from there
        User? existingUser = users.FirstOrDefault(u =>
            u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

        if (existingUser == null)
        {
            throw new Exception("User not found");
        }

        if (!existingUser.Password.Equals(password))
        {
            throw new Exception("Password mismatch");
        }

        return Task.FromResult(existingUser);
    }
    
    public Task RegisterUser(User user)
    {
        if (string.IsNullOrEmpty(user.Username))
        {
            throw new ValidationException("Username cannot be null");
        }

        if (string.IsNullOrEmpty(user.Password))
        {
            throw new ValidationException("Password cannot be null");
        }
        // Do more user info validation here
        
        // save to persistence instead of list
        
        users.Add(user);

        return Task.CompletedTask;
    }
}