import { useEffect, useState } from "react";
import type { AuthResponse, User } from "./Types";
import {
  addCacheUser,
  delCacheUser,
  getCacheBearerToken,
  isLoggedIn,
} from "./Cache";
import { useNavigate } from "react-router-dom";

export default function Login() {
  const [name, setName] = useState("");
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);
  const [currentUser, setCurrentUser] = useState<User | null>(null);
  const navigate = useNavigate();
  const loggedIn = isLoggedIn();

  useEffect(() => {
    if (!loggedIn) return;

    const fetchUser = async () => {
      try {
        const token = getCacheBearerToken();
        if (!token) {
          handleLogout();
          return;
        }

        const res = await fetch(
          `${import.meta.env.VITE_BACKEND_URL}/api/user/me`,
          {
            method: "GET",
            headers: {
              Authorization: token,
            },
          },
        );

        if (!res.ok) {
          handleLogout();
          return;
        }

        const data: User = await res.json();
        if (data.lobbyId) navigate(`/lobby/${data.lobbyId}`);

        setCurrentUser(data);
      } catch (err) {
        console.error(err);
        handleLogout();
      }
    };

    fetchUser();
  }, [loggedIn, navigate]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setLoading(true);

    try {
      const res = await fetch(`${import.meta.env.VITE_BACKEND_URL}/api/user`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ name }),
      });

      if (!res.ok) throw new Error("Failed to login");
      const data: AuthResponse = await res.json();

      addCacheUser(data);

      setTimeout(() => {
        window.location.reload();
      }, 50);
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
    } catch (err: any) {
      setError(err.message || "Unknown error");
    } finally {
      setLoading(false);
    }
  };

  const handleLogout = () => {
    delCacheUser();
    window.location.reload();
  };

  if (loggedIn && currentUser) {
    return (
      <div style={{ display: "flex", alignItems: "center", gap: "10px" }}>
        <p>{currentUser.name}</p>
        <button type="button" disabled={loading} onClick={handleLogout}>
          {loading ? "Logging out..." : "Log-out"}
        </button>
      </div>
    );
  }

  return (
    <form onSubmit={handleSubmit}>
      <input
        type="text"
        value={name}
        onChange={(e) => setName(e.target.value)}
        placeholder="Enter your name"
        required
      />
      <button type="submit" disabled={loading}>
        {loading ? "Logging in..." : "Login"}
      </button>
      {error && <p style={{ color: "red" }}>{error}</p>}
    </form>
  );
}
