namespace Domain.DTOs;

public class PostBasicDto
{
    public int Id { get; set; }
    public string OwnerName { get; }
    public string Title { get; }
    public string Body { get; }
    
    public PostBasicDto(int id, string ownerName, string title, string body)
    {
        Id = id;
        OwnerName = ownerName;
        Title = title;
        Body = body;
    }
}