using System.Text.Json.Serialization;

namespace Domain.Models;

public class Root
{
    public List<User> Users { get; set; }
}

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    [JsonIgnore]
    public ICollection<Post> Posts { get; set; }

    public User()
    {
        
    }
}