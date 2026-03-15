namespace Server.Services;

using Server.Exceptions;
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

    public Lobby AddMember(int lobbyId, int userId)
    {
        Lobby? lobby = GetById(lobbyId) ?? throw new LobbyNotFoundException(lobbyId);
        if (lobby.Members.Count >= lobby.MaxMembers)
            throw new LobbyFullException();

        User? user = _userService.GetById(userId) ?? throw new UserNotFoundException(userId);

        if (user.InLobby())
            throw new UserInLobbyException(userId);
        user.LobbyId = lobbyId;

        lobby.Members.Add(user);
        return lobby;
    }

    public Lobby RemoveMember(int lobbyId, int userId)
    {
        Lobby? lobby = GetById(lobbyId) ?? throw new LobbyNotFoundException(lobbyId);
        User? user = _userService.GetById(userId) ?? throw new UserNotFoundException(userId);

        if (!user.InLobby())
            throw new UserNotInLobbyException(userId);
        user.LobbyId = null;

        lobby.Members.RemoveAll(u => u.Id == userId);
        return lobby;
    }
}
