namespace Server.Exceptions
{
    public class UserNotFoundException(string Id) : Exception($"User {Id} not found.") { }

    public class UserInLobbyException(string Id)
        : Exception($"User {Id} is already in a lobby.") { }

    public class UserNotInLobbyException(string Id) : Exception($"User {Id} is not in a lobby.") { }
}
