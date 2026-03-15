import { useState, useEffect } from "react";
import type { LobbyList } from "../utils/Types";
import { isLoggedIn } from "../utils/Cache";
import "./LobbyList.css";
import { useNavigate } from "react-router-dom";
import { fetchApi } from "../utils/Api";

export default function LobbyListComponent() {
  const [lobbies, setLobbies] = useState<LobbyList>([]);
  const loggedIn = isLoggedIn();
  const navigate = useNavigate();

  useEffect(() => {
    if (!loggedIn) return;
    fetchApi("/api/lobby", "GET")
      .then((res) => {
        if (!res.ok) {
          throw new Error(`HTTP error! Status: ${res.status}`);
        }
        return res.json();
      })
      .then((data: LobbyList) => setLobbies(Array.isArray(data) ? data : []))
      .catch((err) => alert(err));
  }, [loggedIn]);

  async function joinLobby(lobbyId: string) {
    if (!loggedIn) return;

    try {
      const res = await fetchApi(`/api/lobby/${lobbyId}/members`, "POST");

      if (!res.ok) {
        alert(`Failed to join lobby: ${res.status}`);
        return;
      }

      navigate(`/lobby/${lobbyId}`);
    } catch (err) {
      alert(err);
    }
  }

  if (!loggedIn) {
    return <p>Please log in to view lobbies.</p>;
  }

  const getBody = () => {
    if (!Array.isArray(lobbies) || lobbies.length === 0) {
      return (
        <tr>
          <td colSpan={3}>No lobbies available</td>
        </tr>
      );
    }

    return lobbies.map((lobby) => {
      const joinable = lobby.memberCount < lobby.maxMembers;

      return (
        <tr
          key={lobby.id}
          className={`lobby-item ${joinable ? "clickable" : "full"}`}
          onClick={joinable ? () => joinLobby(lobby.id) : undefined}
        >
          <td>{lobby.name}</td>
          <td>{lobby.hostUserName}</td>
          <td>
            {lobby.memberCount}/{lobby.maxMembers}
          </td>
        </tr>
      );
    });
  };

  return (
    <table className="lobby-list">
      <thead>
        <tr className="lobby-item">
          <th>Lobby Name</th>
          <th>Host</th>
          <th>Members</th>
        </tr>
      </thead>
      <tbody>{getBody()}</tbody>
    </table>
  );
}
