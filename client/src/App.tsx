import "./App.css";
import LobbyList from "./components/LobbyList";
import Login from "./components/Login";
import CreateLobbyButton from "./components/CreateLobbyButton";

function App() {
  return (
    <div>
      <div className="lobby-list-header">
        <h1>Join a lobby!</h1>
        <CreateLobbyButton></CreateLobbyButton>
        <Login></Login>
      </div>
      <div className="card">
        <LobbyList></LobbyList>
      </div>
    </div>
  );
}

export default App;
