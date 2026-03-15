using Server.Models;

public class CreateUserRequestDto
{
    public string Name { get; set; } = "User";
}

public class CreateUserResponseDto(User user)
{
    public string Id { get; set; } = user.Id;
    public string Name { get; set; } = user.Name;
    public string AuthToken { get; set; } = user.AuthToken;
}
