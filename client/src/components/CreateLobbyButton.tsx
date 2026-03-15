import { useState } from "react";
import { getCacheBearerToken, isLoggedIn } from "./Cache";

export default function CreateLobbyButton() {
  const [lobbyName, setLobbyName] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

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
      const res = await fetch(`${import.meta.env.VITE_BACKEND_URL}/api/lobby`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: token,
        },
        body: JSON.stringify({ name: lobbyName.trim() }),
      });

      if (!res.ok) {
        throw new Error(`Failed to create lobby. Status: ${res.status}`);
      }

      setLobbyName("");
      window.location.reload(); // Crude solution, but refreshes the lobby-list.
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
