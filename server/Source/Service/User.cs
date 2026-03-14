namespace Server.Services;

using Server.Models;

public class UserService
{
    private readonly List<User> _users = [];

    public IEnumerable<User> GetAll() => _users;

    public User? GetById(int id) => _users.FirstOrDefault(l => l.Id == id);

    public User? GetByToken(string token) => _users.FirstOrDefault(u => u.AuthToken == token);

    public User Create(string Name)
    {
        string token = GenerateToken();
        User user = new(Name, token) { Id = _users.Count > 0 ? _users.Max(l => l.Id) + 1 : 1 };
        _users.Add(user);
        return user;
    }

    private static string GenerateToken()
    {
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
    }
}
