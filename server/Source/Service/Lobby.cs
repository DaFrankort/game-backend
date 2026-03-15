namespace Server.Services;

using Server.Exceptions;
using Server.Models;

public class LobbyService(UserService userService)
{
    private readonly List<Lobby> _lobbies = [];
    private readonly UserService _userService = userService;

    public IEnumerable<Lobby> GetAll() => _lobbies;

    public IEnumerable<Lobby> GetPaged(int page, int limit)
    {
        return _lobbies.Skip((page - 1) * limit).Take(limit);
    }

    public Lobby? GetById(string id) => _lobbies.FirstOrDefault(lobby => lobby.Id == id);

    public Lobby Create(Lobby lobby)
    {
        _lobbies.Add(lobby);
        return lobby;
    }

    public Lobby AddMember(string lobbyId, string userId)
    {
        Lobby? lobby = GetById(lobbyId) ?? throw new LobbyNotFoundException(lobbyId);
        if (lobby.Members.Count >= lobby.MaxMembers)
            throw new LobbyFullException();

        User? user = _userService.GetById(userId) ?? throw new UserNotFoundException(userId);

        if (user.LobbyId == null)
            throw new UserInLobbyException(userId);
        user.LobbyId = lobbyId;

        lobby.Members.Add(user);
        return lobby;
    }

    public Lobby RemoveMember(string lobbyId, string userId)
    {
        Lobby? lobby = GetById(lobbyId) ?? throw new LobbyNotFoundException(lobbyId);
        User? user = _userService.GetById(userId) ?? throw new UserNotFoundException(userId);

        if (user.LobbyId == null)
            throw new UserNotInLobbyException(userId);
        user.LobbyId = null;

        lobby.Members.RemoveAll(u => u.Id == userId);
        return lobby;
    }
}
