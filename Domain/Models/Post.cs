namespace Domain.Models;

public class Post
{
    public int Id { get; set; }
    public User Owner { get; set; }
    public int OwnerId { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }

    public Post(int ownerId, string title, string body)
    {
        OwnerId = ownerId;
        Title = title;
        Body = body;
    }
    
    public Post(){}
}