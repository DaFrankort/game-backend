public class CreateUserRequestDto
{
    public string Name { get; set; } = "User";
}

public class CreateUserResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string AuthToken { get; set; } = string.Empty;
}
