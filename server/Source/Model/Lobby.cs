namespace Server.Models;

public class Lobby
{
    public int Id { get; set; }
    public string Name { get; set; } = "Lobby";
    public List<User> Members { get; set; } = [];
    public int MaxMembers { get; } = 2; // Currently not adjustable
}
