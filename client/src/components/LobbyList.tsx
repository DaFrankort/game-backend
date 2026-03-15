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
      .catch(console.error);
  }, [loggedIn, token]);

  if (!loggedIn) {
    return <p>Please log in to view lobbies.</p>;
  }

  return (
    <ul className="lobby-list">
      {Array.isArray(lobbies) && lobbies.length > 0 ? (
        lobbies.map((lobby) => (
          <li key={lobby.id} className="lobby-item">
            <b>{lobby.name}</b>
            <p>{lobby.memberCount}/{lobby.maxMembers} Users</p>
          </li>
        ))
      ) : (
        <li>No lobbies available</li>
      )}
    </ul>
  );
}
