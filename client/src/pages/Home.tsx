import LobbyList from "../components/LobbyList";
import Login from "../components/Login";
import CreateLobbyButton from "../components/CreateLobbyButton";

export default function HomePage() {
    return (
        <div>
            <div className="lobby-list-header">
                <h1>Join a lobby!</h1>
                <CreateLobbyButton />
                <Login />
            </div>
            <div className="card">
                <LobbyList />
            </div>
        </div>
    );
}