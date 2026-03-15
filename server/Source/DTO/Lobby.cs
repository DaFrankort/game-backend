using Server.Models;

namespace Server.DTO
{
    public class CreateLobbyRequestDto
    {
        public string Name { get; set; } = "Lobby";
    }

    public class LobbySummaryDto(Lobby lobby)
    {
        public int Id { get; set; } = lobby.Id;
        public string Name { get; set; } = lobby.Name;
        public int MemberCount { get; set; } = lobby.Members.Count;
        public int MaxMembers { get; set; } = lobby.MaxMembers;
    }
}
