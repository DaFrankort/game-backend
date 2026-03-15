namespace Server.Services;

using Server.Exceptions;
using Server.Models;

public class LobbyService(UserService userService)
{
    private readonly List<Lobby> _lobbies = [];
    private readonly object _lock = new();
    private readonly UserService _userService = userService;

    public IEnumerable<Lobby> GetPaged(int page, int limit)
    {
        return _lobbies.Skip((page - 1) * limit).Take(limit).ToList();
    }

    public Lobby GetById(string id)
    {
        Lobby lobby =
            _lobbies.FirstOrDefault(lobby => lobby.Id == id)
            ?? throw new LobbyNotFoundException(id);
        return lobby;
    }

    public Lobby Create(Lobby lobby, User host)
    {
        if (host.LobbyId != null)
            throw new UserInLobbyException(host.Id);

        lock (_lock)
        {
            _lobbies.Add(lobby);
            AddMember(lobby.Id, host.Id);
        }

        return lobby;
    }

    private Lobby DeleteLobby(Lobby lobby)
    {
        foreach (User member in lobby.Members)
        {
            member.LobbyId = null;
        }
        _lobbies.Remove(lobby);
        return lobby;
    }

    public Lobby Delete(string id, User invoker)
    {
        Lobby lobby = GetById(id);
        if (lobby.Host.Id != invoker.Id)
            throw new LobbyCantDeleteException();

        lock (_lock)
        {
            return DeleteLobby(lobby);
        }
    }

    public Lobby AddMember(string lobbyId, string userId)
    {
        Lobby lobby = GetById(lobbyId) ?? throw new LobbyNotFoundException(lobbyId);
        User user = _userService.GetById(userId) ?? throw new UserNotFoundException(userId);

        lock (_lock)
        {
            if (lobby.Members.Count >= lobby.MaxMembers)
                throw new LobbyFullException();
            if (user.LobbyId != null)
                throw new UserInLobbyException(userId);

            user.LobbyId = lobbyId;
            lobby.Members.Add(user);
        }

        return lobby;
    }

    public Lobby RemoveMember(string lobbyId, string userId, User invoker)
    {
        Lobby lobby = GetById(lobbyId) ?? throw new LobbyNotFoundException(lobbyId);
        User target = _userService.GetById(userId) ?? throw new UserNotFoundException(userId);

        lock (_lock)
        {
            if ((invoker.Id != lobby.Host.Id) && (target.Id != invoker.Id))
                throw new LobbyCantRemoveUserException();
            if (target.LobbyId == null)
                throw new UserNotInLobbyException(userId);

            target.LobbyId = null;
            lobby.Members.RemoveAll(user => user.Id == userId);

            if (target.Id == lobby.Host.Id)
            {
                if (lobby.Members.Count > 0)
                    lobby.Host = lobby.Members[0];
                else
                    DeleteLobby(lobby);
            }
        }

        return lobby;
    }
}
