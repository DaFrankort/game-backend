using System.ComponentModel.DataAnnotations;
using Server.Models;

public class CreateUserRequestDto
{
    [Required(ErrorMessage = "User name is required.")]
    [StringLength(
        20,
        MinimumLength = 3,
        ErrorMessage = "User name must be between 3 and 20 characters."
    )]
    [RegularExpression(
        @"^[a-zA-Z0-9 _-]+$",
        ErrorMessage = "User name can only contain letters, numbers, spaces, underscores, and hyphens."
    )]
    public string Name { get; set; } = "User";
}

public class CreateUserResponseDto(User user)
{
    public string Id { get; set; } = user.Id;
    public string Name { get; set; } = user.Name;
    public string AuthToken { get; set; } = user.AuthToken;
}
