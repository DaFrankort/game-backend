namespace Server.Models;

public class Lobby
{
    public int Id { get; set; }
    public string Name { get; set; } = "Lobby";
    public List<User> Users { get; set; } = [];
}
