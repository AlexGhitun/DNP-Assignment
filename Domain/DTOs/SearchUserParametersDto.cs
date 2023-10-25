namespace Domain.DTOs;

public class SearchUserParametersDto
{
    public String? UsernameContains { get; }

    public SearchUserParametersDto(string? usernameContains)
    {
        UsernameContains = usernameContains;
    }
}