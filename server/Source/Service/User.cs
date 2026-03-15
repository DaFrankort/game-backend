namespace Server.Services;

using Server.Exceptions;
using Server.Models;

public class UserService()
{
    private readonly List<User> _users = [];
    private readonly Lock _lock = new();

    public IEnumerable<User> GetPaged(int page, int limit)
    {
        return _users.Skip((page - 1) * limit).Take(limit).ToList();
    }

    public User GetById(string id)
    {
        User user =
            _users.FirstOrDefault(user => user.Id == id) ?? throw new UserNotFoundException(id);
        return user;
    }

    public User GetByToken(string token)
    {
        User user =
            _users.FirstOrDefault(user => user.AuthToken == token)
            ?? throw new UserTokenNotFoundException(token);
        return user;
    }

    public User Create(string name)
    {
        string token = GenerateToken();
        User user = new(name, token);

        lock (_lock)
        {
            _users.Add(user);
        }

        return user;
    }

    public User Delete(User user)
    {
        if (user.LobbyId != null)
            throw new UserCanNotBeDeleted("User is still in a lobby.");

        lock (_lock)
        {
            _users.Remove(user);
        }

        return user;
    }

    private static string GenerateToken()
    {
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
    }
}
