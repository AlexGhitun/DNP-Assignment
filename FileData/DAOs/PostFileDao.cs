using Application.DaoInterfaces;
using Domain.DTOs;
using Domain.Models;

namespace FileData.DAOs.DAOs;

public class PostFileDao : IPostDao
{
    private readonly FileContext Context;

    public PostFileDao(FileContext context)
    {
        Context = context;
    }

    public Task<Post> CreateAsync(Post post)
    {
        int id = 1;
        if (Context.Posts.Any())
        {
            id = Context.Posts.Max(p => p.Id);
            id++;
        }

        post.Id = id;
        
        Context.Posts.Add(post);
        Context.SaveChanges();

        return Task.FromResult(post);
    }

    public Task<IEnumerable<Post>> GetAsync(SearchPostParametersDto searchParameters)
    {
        IEnumerable<Post> posts = Context.Posts.AsEnumerable();
        if (!string.IsNullOrEmpty(searchParameters.Username))
        {
            posts = Context.Posts.Where(post =>
                post.Owner.Username.Equals(searchParameters.Username, StringComparison.OrdinalIgnoreCase));
        }

        if (searchParameters.UserId != null)
        {
            posts = posts.Where(p => p.Owner.Id == searchParameters.UserId);
        }

        if (!string.IsNullOrEmpty(searchParameters.TitleContains))
        {
            posts = posts.Where(p =>
                p.Title.Contains(searchParameters.TitleContains, StringComparison.OrdinalIgnoreCase));
        }

        return Task.FromResult(posts);
    }

    public Task<Post?> GetByIdAsync(int id)
    {
        Post? existing = Context.Posts.FirstOrDefault(p =>
            p.Id == id
        );
        return Task.FromResult(existing);
    }

    public Task UpdateAsync(Post toUpdate)
    {
        Post? existing = Context.Posts.FirstOrDefault(post => post.Id == toUpdate.Id);
        if (existing == null)
        {
            throw new Exception($"Post with id {toUpdate.Id} does not exist!");
        }

        Context.Posts.Remove(existing);
        Context.Posts.Add(toUpdate);
        
        Context.SaveChanges();

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        Post? existing = Context.Posts.FirstOrDefault(post => post.Id == id);
        if (existing == null)
        {
            throw new Exception($"Post with id {id} does not exist!");
        }

        Context.Posts.Remove(existing);
        Context.SaveChanges();
        
        return Task.CompletedTask;
    }
}