import { useState, useEffect } from "react";
import type { LobbyList } from "./Types";
import { getCacheBearerToken, isLoggedIn } from "./Cache";
import "./LobbyList.css";

export default function LobbyListComponent() {
  const [lobbies, setLobbies] = useState<LobbyList>([]);
  const token = getCacheBearerToken();
  const loggedIn = isLoggedIn();

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

  if (!loggedIn) {
    return <p>Please log in to view lobbies.</p>;
  }

  return (
    <table className="lobby-list">
      <thead>
        <tr className="lobby-item">
          <th>Lobby Name</th>
          <th>Host</th>
          <th>Members</th>
        </tr>
      </thead>
      <tbody>
        {Array.isArray(lobbies) && lobbies.length > 0 ? (
          lobbies.map((lobby) => (
            <tr key={lobby.id} className="lobby-item">
              <td>{lobby.name}</td>
              <td>{lobby.hostUserName}</td>
              <td>
                {lobby.memberCount}/{lobby.maxMembers}
              </td>
            </tr>
          ))
        ) : (
          <tr>
            <td colSpan={3}>No lobbies available</td>
          </tr>
        )}
      </tbody>
    </table>
  );
}
