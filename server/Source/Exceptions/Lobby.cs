namespace Server.Exceptions
{
    public class LobbyNotFoundException(int Id) : Exception($"Lobby {Id} not found.") { }
}
