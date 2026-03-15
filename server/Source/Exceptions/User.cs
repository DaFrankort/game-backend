namespace Server.Exceptions
{
    public class UserNotFoundException(string Id) : Exception($"User '{Id}' not found.") { }

    public class UserTokenNotFoundException(string token)
        : Exception($"User with token '{token}' not found.") { }

    public class UserInLobbyException(string Id)
        : Exception($"User {Id} is already in a lobby.") { }

    public class UserNotInLobbyException(string Id) : Exception($"User {Id} is not in a lobby.") { }

    public class UserCanNotBeDeleted(string reason) : Exception($"Can't remove user: {reason}") { }
}
