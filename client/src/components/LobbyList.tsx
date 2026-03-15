import { useState, useEffect } from "react";
import type { LobbyList } from "./Types";
import { getCacheBearerToken, isLoggedIn } from "./Cache";
import "./LobbyList.css";
import { useNavigate } from "react-router-dom";

export default function LobbyListComponent() {
  const [lobbies, setLobbies] = useState<LobbyList>([]);
  const token = getCacheBearerToken();
  const loggedIn = isLoggedIn();
  const navigate = useNavigate();

  useEffect(() => {
    if (!loggedIn || !token) return;
    const headers: Record<string, string> = {
      "Content-Type": "application/json",
      Authorization: token,
    };

    fetch(`${import.meta.env.VITE_BACKEND_URL}/api/lobby`, {
      method: "GET",
      headers,
    })
      .then((res) => {
        if (!res.ok) {
          throw new Error(`HTTP error! Status: ${res.status}`);
        }
        return res.json();
      })
      .then((data: LobbyList) => setLobbies(Array.isArray(data) ? data : []))
      .catch((err) => alert(err));
  }, [loggedIn, token]);

  async function joinLobby(lobbyId: string) {
    if (!token) return;

    try {
      const res = await fetch(
        `${import.meta.env.VITE_BACKEND_URL}/api/lobby/${lobbyId}/members`,
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
            Authorization: token,
          },
        }
      );

      if (!res.ok) {
        throw new Error(`Failed to join lobby: ${res.status}`);
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
          className={`lobby-item ${joinable ? "clickable" : ""}`}
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
