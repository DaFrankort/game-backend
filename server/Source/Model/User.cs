using System.Text.Json.Serialization;

namespace Server.Models;

public class User(string name, string authToken)
{
    public int Id { get; set; }
    public string Name { get; set; } = name;

    [JsonIgnore]
    public int? LobbyId { get; set; } = null;

    [JsonIgnore]
    public string AuthToken { get; private set; } = authToken;

    public bool ValidateToken(string token) => AuthToken == token;

    public bool InLobby() => LobbyId != null;
}
