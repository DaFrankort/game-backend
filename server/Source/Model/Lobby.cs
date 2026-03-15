namespace Server.Models;

public class Lobby(string name)
{
    public string Id { get; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = name;
    public List<User> Members { get; set; } = [];
    public int MaxMembers { get; } = 2; // Currently not adjustable
}
