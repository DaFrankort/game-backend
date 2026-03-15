namespace Server.Services;

using Server.Exceptions;
using Server.Models;

public class UserService
{
    private readonly List<User> _users = [];

    public IEnumerable<User> GetAll() => _users;

    public IEnumerable<User> GetPaged(int page, int limit)
    {
        return _users.Skip((page - 1) * limit).Take(limit);
    }

    public User GetById(string id)
    {
        User user =
            _users.FirstOrDefault(lobby => lobby.Id == id) ?? throw new UserNotFoundException(id);
        return user;
    }

    public User GetByToken(string token)
    {
        User user =
            _users.FirstOrDefault(user => user.AuthToken == token)
            ?? throw new UserNotFoundException(token);
        return user;
    }

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
