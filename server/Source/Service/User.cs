namespace Server.Services;

using Server.Models;

public class UserService
{
    private readonly List<User> _users = [];

    public IEnumerable<User> GetAll() => _users;

    public User? GetById(string id) => _users.FirstOrDefault(lobby => lobby.Id == id);

    public User? GetByToken(string token) => _users.FirstOrDefault(u => u.AuthToken == token);

    public User Create(string name)
    {
        string token = GenerateToken();
        User user = new(name, token);
        _users.Add(user);
        return user;
    }

    private static string GenerateToken()
    {
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
    }
}
