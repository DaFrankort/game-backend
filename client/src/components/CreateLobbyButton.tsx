import { useState } from "react";
import { getCacheBearerToken, isLoggedIn } from "../utils/Cache";
import { useNavigate } from "react-router-dom";
import type { Lobby } from "../utils/Types";
import { fetchApi } from "../utils/Api";

export default function CreateLobbyButton() {
  const [lobbyName, setLobbyName] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  const loggedIn = isLoggedIn();
  const token = getCacheBearerToken();

  if (!loggedIn) return null;

  const handleCreateLobby = async () => {
    if (!token) {
      setError("Not authenticated");
      return;
    }
    if (!lobbyName.trim()) {
      setError("Lobby name cannot be empty");
      return;
    }

    setLoading(true);
    setError(null);

    try {
      const res = await fetchApi("/api/lobby", "POST", {
        name: lobbyName.trim(),
      });
      if (!res.ok) {
        alert(`Failed to create lobby. Status: ${res.status}`);
        return;
      }

      const data: Lobby = await res.json();
      navigate(`/lobby/${data.id}`);
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
    } catch (err: any) {
      setError(err.message || "Something went wrong");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <input
        type="text"
        value={lobbyName}
        onChange={(e) => setLobbyName(e.target.value)}
        placeholder="Enter lobby name"
        disabled={loading}
        style={{ marginRight: "8px" }}
        required
      />
      <button onClick={handleCreateLobby} disabled={loading}>
        {loading ? "Creating..." : "Create Lobby"}
      </button>
      {error && <p style={{ color: "red" }}>{error}</p>}
    </div>
  );
}
