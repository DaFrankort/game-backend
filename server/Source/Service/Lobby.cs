namespace Server.Services;

using Server.Models;

public class LobbyService
{
    private readonly List<Lobby> _lobbies = [];

    public IEnumerable<Lobby> GetAll() => _lobbies;

    public Lobby? GetById(int id) => _lobbies.FirstOrDefault(l => l.Id == id);

    public Lobby Create(Lobby lobby)
    {
        lobby.Id = _lobbies.Count > 0 ? _lobbies.Max(l => l.Id) + 1 : 1;
        _lobbies.Add(lobby);
        return lobby;
    }
}
