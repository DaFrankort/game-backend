using System.ComponentModel.DataAnnotations;
using Server.Models;

namespace Server.DTO
{
    public class CreateLobbyRequestDto
    {
        [Required(ErrorMessage = "Lobby name is required.")]
        [StringLength(
            20,
            MinimumLength = 3,
            ErrorMessage = "Lobby name must be between 3 and 20 characters."
        )]
        [RegularExpression(
            @"^[a-zA-Z0-9_-]+$",
            ErrorMessage = "User name can only contain letters, numbers, underscores, and hyphens."
        )]
        public string Name { get; set; } = "Lobby";
    }

    public class LobbySummaryDto(Lobby lobby)
    {
        public string Id { get; set; } = lobby.Id;
        public string Name { get; set; } = lobby.Name;
        public int MemberCount { get; set; } = lobby.Members.Count;
        public int MaxMembers { get; set; } = lobby.MaxMembers;
    }
}
