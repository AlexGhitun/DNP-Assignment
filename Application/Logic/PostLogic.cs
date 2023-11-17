using Application.DaoInterfaces;
using Application.LogicInterfaces;
using Domain.DTOs;
using Domain.Models;

namespace Application.Logic;

public class PostLogic : IPostLogic
{
    private readonly IPostDao postDao;
    private readonly IUserDao userDao;

    public PostLogic(IPostDao postDao, IUserDao userDao)
    {
        this.postDao = postDao;
        this.userDao = userDao;
    }

    public async Task<Post> CreateAsync(PostCreationDto dto)
    {
        User? user = await userDao.GetByIdAsync(dto.OwnerId);
        if (user == null)
        {
            throw new Exception($"User with id {dto.OwnerId} was not found.");
        }

        Post post = new Post(user.Id, dto.Title, dto.Body);

        ValidatePost(post);

        Post created = await postDao.CreateAsync(post);
        return created;
    }

    public Task<IEnumerable<Post>> GetAsync(SearchPostParametersDto searchParameters)
    {
        return postDao.GetAsync(searchParameters);
    }

    public async Task UpdateAsync(PostUpdateDto dto)
    {
        Post? existing = await postDao.GetByIdAsync(dto.Id);

        if (existing == null)
        {
            throw new Exception($"Post with ID {dto.Id} not found!");
        }

        User? user = null;
        if (dto.OwnerId != null)
        {
            user = await userDao.GetByIdAsync((int)dto.OwnerId);
            if (user == null)
            {
                throw new Exception($"User with id {dto.OwnerId} was not found.");
            }
        }

        User userToUse = user ?? existing.Owner;
        string titleToUse = dto.Title ?? existing.Title;
        string bodyToUse = dto.Body ?? existing.Body;

        Post updated = new(userToUse.Id, titleToUse, bodyToUse)
        {
            Id = existing.Id
        };
        
        ValidatePost(updated);

        await postDao.UpdateAsync(updated);
    }

    public async Task DeleteAsync(int id)
    {
        Post? post = await postDao.GetByIdAsync(id);
        if (post == null)
        {
            throw new Exception($"Post with ID {id} was not found!");
        }

        await postDao.DeleteAsync(id);
    }

    public async Task<PostBasicDto> GetByIdAsync(int id)
    {
        Post? post = await postDao.GetByIdAsync(id);
        if (post == null)
        {
            throw new Exception($"Post with id {id} not found");
        }

        return new PostBasicDto(post.Id, post.Owner.Username, post.Title, post.Body);
    }
    
    private void ValidatePost(Post post)
    {
        if (string.IsNullOrEmpty(post.Title))
        {
            throw new Exception("Title cannot be empty.");
        }

        if (string.IsNullOrEmpty(post.Body))
        {
            throw new Exception("Body cannot be empty.");
        }
        //other validation stuff if needed
    }
}