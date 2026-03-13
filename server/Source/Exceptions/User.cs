namespace Server.Exceptions
{
    public class UserNotFoundException(int Id) : Exception($"User {Id} not found.") { }

    public class UserInLobbyException(int Id) : Exception($"User {Id} is already in a lobby.") { }

    public class UserNotInLobbyException(int Id) : Exception($"User {Id} is not in a lobby.") { }
}
