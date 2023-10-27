using Domain.DTOs;
using Domain.Models;

namespace HttpClients.ClientInterfaces;

public interface IPostService
{
    Task CreateAsync(PostCreationDto dto);

    Task<ICollection<Post>> GetAsync(
        string? userName,
        int? userId,
        string? titleContains
    );

    Task UpdateAsync(PostUpdateDto dto);

    Task<PostBasicDto> GetByIdAsync(int id);

    Task DeleteAsync(int id);
}