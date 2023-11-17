using Application.LogicInterfaces;
using Domain.DTOs;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostLogic PostLogic;

    public PostsController(IPostLogic postLogic)
    {
        PostLogic = postLogic;
    }

    [HttpPost]
    public async Task<ActionResult<Post>> CreateAsync([FromBody] PostCreationDto dto)
    {
        try
        {
            Post post = await PostLogic.CreateAsync(dto);
            return Created($"/posts/{post.Id}", $"The post with the title {post.Title} by user {post.OwnerId}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Post>>> GetAsync([FromQuery] string? username, int? userId,
        string? titleContains)
    {
        try
        {
            SearchPostParametersDto parameters = new(username, userId, titleContains);
            IEnumerable<Post> posts = await PostLogic.GetAsync(parameters);
            return Ok(posts);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500,e.Message);
        }
    }

    [HttpPatch]
    public async Task<ActionResult> UpdateAsync([FromBody] PostUpdateDto dto)
    {
        try
        {
            await PostLogic.UpdateAsync(dto);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAsync([FromRoute] int id)
    {
        try
        {
            await PostLogic.DeleteAsync(id);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PostBasicDto>> GetById([FromRoute] int id)
    {
        try
        {
            PostBasicDto result = await PostLogic.GetByIdAsync(id);
            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}