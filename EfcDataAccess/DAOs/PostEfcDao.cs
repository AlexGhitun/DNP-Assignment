using Application.DaoInterfaces;
using Domain.DTOs;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EfcDataAccess.DAOs;

public class PostEfcDao : IPostDao
{
    private readonly PostContext context;

    public PostEfcDao(PostContext context)
    {
        this.context = context;
    }
    
    public async Task<Post> CreateAsync(Post post)
    {
        EntityEntry<Post> added = await context.Posts.AddAsync(post);
        await context.SaveChangesAsync();
        return added.Entity;
    }

    public async Task<IEnumerable<Post>> GetAsync(SearchPostParametersDto searchParams)
    {
        IQueryable<Post> query = context.Posts.Include(post => post.Owner).AsQueryable();
        
        if (!string.IsNullOrEmpty(searchParams.Username))
        {
            // we know username is unique, so just fetch the first
            query = query.Where(todo =>
                todo.Owner.Username.ToLower().Equals(searchParams.Username.ToLower()));
        }
        
        if (searchParams.UserId != null)
        {
            query = query.Where(t => t.Owner.Id == searchParams.UserId);
        }
        
        if (!string.IsNullOrEmpty(searchParams.TitleContains))
        {
            query = query.Where(t =>
                t.Title.ToLower().Contains(searchParams.TitleContains.ToLower()));
        }

        List<Post> result = await query.ToListAsync();
        return result;
    }

    public async Task UpdateAsync(Post post)
    {
        context.Posts.Update(post);
        await context.SaveChangesAsync();
    }

    public async Task<Post?> GetByIdAsync(int postId)
    {
        Post? found = await context.Posts
            .AsNoTracking()
            .Include(post => post.Owner)
            .SingleOrDefaultAsync(post => post.Id == postId);
        return found;
    }

    public async Task DeleteAsync(int id)
    {
        Post? existing = await GetByIdAsync(id);
        if (existing == null)
        {
            throw new Exception($"Post with id {id} not found");
        }

        context.Posts.Remove(existing);
        await context.SaveChangesAsync();
    }
}