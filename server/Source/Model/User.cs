using System.Text.Json.Serialization;

namespace Server.Models;

public class User(string name, string authToken)
{
    public string Id { get; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = name;

    public string? LobbyId { get; set; } = null;

    [JsonIgnore]
    public string AuthToken { get; private set; } = authToken;

    public bool ValidateToken(string token) => AuthToken == token;
}
