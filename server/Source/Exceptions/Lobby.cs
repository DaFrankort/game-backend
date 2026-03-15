namespace Server.Exceptions
{
    public class LobbyNotFoundException(string Id) : Exception($"Lobby {Id} not found.") { }

    public class LobbyFullException() : Exception("The lobby you tried to join is full.") { }
}
