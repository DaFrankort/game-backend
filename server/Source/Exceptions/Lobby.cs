namespace Server.Exceptions
{
    public class LobbyNotFoundException(string Id) : Exception($"Lobby {Id} not found.") { }

    public class LobbyFullException() : Exception("The lobby you tried to join is full.") { }

    public class LobbyCantRemoveException()
        : Exception("You don't have permission to kick that user.") { }
}
