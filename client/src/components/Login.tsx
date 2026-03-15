import { useState } from "react";
import type { User } from "./Types";
import { addCacheUser, delCacheUser, isLoggedIn } from "./Cache";

interface AuthResponse extends User {
  authToken: string;
}

export default function Login() {
  const [name, setName] = useState("");
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);
  const loggedIn = isLoggedIn();

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

      addCacheUser(data.id, data.authToken);
      window.location.reload();
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
    } catch (err: any) {
      setError(err.message || "Unknown error");
    } finally {
      setLoading(false);
    }
  };

  if (loggedIn) {
    const handleLogout = () => {
      delCacheUser();
      window.location.reload();
    };
    return (
      <div style={{ display: "flex", alignItems: "center", gap: "10px" }}>
        <p>Name</p>
        <button type="submit" disabled={loading} onClick={handleLogout}>
          {loading ? "Logging out..." : "Log-out"}
        </button>
      </div>
    );
  } else {
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
}
