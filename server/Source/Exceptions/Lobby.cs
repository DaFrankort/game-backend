namespace Server.Exceptions
{
    public class LobbyNotFoundException(string Id) : Exception($"Lobby {Id} not found.") { }

    public class LobbyFullException() : Exception("The lobby you tried to join is full.") { }

    public class LobbyCantRemoveUserException()
        : Exception("You don't have permission to kick that user.") { }

    public class LobbyCantDeleteException()
        : Exception("You don't have permission to remove this lobby.") { }
}
