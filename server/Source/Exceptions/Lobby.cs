namespace Server.Exceptions
{
    public class LobbyNotFoundException(int Id) : Exception($"Lobby {Id} not found.") { }

    public class LobbyFullException() : Exception("The lobby you tried to join is full.") { }
}
