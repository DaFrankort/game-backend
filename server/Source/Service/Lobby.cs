namespace Server.Services;

using Server.Models;

public class LobbyService(UserService userService)
{
    private readonly List<Lobby> _lobbies = [];
    private readonly UserService _userService = userService;

    public IEnumerable<Lobby> GetAll() => _lobbies;

    public Lobby? GetById(int id) => _lobbies.FirstOrDefault(l => l.Id == id);

    public Lobby Create(Lobby lobby)
    {
        lobby.Id = _lobbies.Count > 0 ? _lobbies.Max(l => l.Id) + 1 : 1;
        _lobbies.Add(lobby);
        return lobby;
    }

    public bool AddUser(int lobbyId, int userId)
    {
        Lobby? lobby = GetById(lobbyId);
        User? user = _userService.GetById(userId);
        if (lobby == null || user == null)
            return false;

        if (user.InLobby())
            return false;
        user.LobbyId = lobbyId;

        lobby.Users.Add(user);
        return true;
    }

    public bool RemoveUser(int lobbyId, int userId)
    {
        Lobby? lobby = GetById(lobbyId);
        User? user = _userService.GetById(userId);
        if (lobby == null || user == null)
            return false;

        if (!user.InLobby())
            return false;
        user.LobbyId = null;

        return lobby.Users.RemoveAll(u => u.Id == userId) > 0;
    }
}
