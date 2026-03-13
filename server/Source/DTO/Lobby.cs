namespace Server.DTO
{
    public class CreateLobbyRequestDto
    {
        public string Name { get; set; } = "Lobby";
    }

    public class JoinLobbyRequestDto
    {
        public int LobbyId { get; set; }
        public int UserId { get; set; }
    }

    public class LobbySummaryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int UserCount { get; set; }
    }
}
