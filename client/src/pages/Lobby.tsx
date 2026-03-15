import { useNavigate, useParams } from "react-router-dom";
import type { Lobby } from "../utils/Types";
import { getCacheBearerToken, getCurrentUserId } from "../utils/Cache";
import { useEffect, useState } from "react";
import { fetchApi } from "../utils/Api";

export default function LobbyPage() {
  const { id } = useParams<{ id: string }>();
  const [lobby, setLobby] = useState<Lobby | null>(null);
  const navigate = useNavigate();
  const token = getCacheBearerToken();
  const currentUserId = getCurrentUserId();

  useEffect(() => {
    if (!id || !token) return;

    fetchApi(`/api/lobby/${id}`, "GET")
      .then((res) => {
        if (!res.ok) {
          throw new Error(`Failed to fetch lobby (${res.status})`);
        }
        return res.json();
      })
      .then((data: Lobby) => setLobby(data))
      .catch(() => {
        alert("Could not find that lobby, rerouting!");
        navigate("/", { replace: true });
      });
  }, [id, navigate, token]);

  const leaveLobby = async () => {
    if (!id || !token) return;

    try {
      const res = await fetchApi(`/api/lobby/${id}/members`, "DELETE");
      if (!res.ok) {
        alert(`Failed to leave lobby (${res.status})`);
        return;
      }
      navigate("/", { replace: true });
    } catch (err) {
      alert(err);
    }
  };

  const kickUser = async (userId: string) => {
    if (!id || !token) return;

    try {
      const res = await fetchApi(
        `/api/lobby/${id}/members/${userId}`,
        "DELETE",
      );

      if (!res.ok) {
        alert(`Failed to remove user (${res.status})`);
        return;
      }

      if (lobby) {
        setLobby({
          ...lobby,
          members: lobby.members?.filter((m) => m.id !== userId) ?? [],
        });
      }

      if (userId === currentUserId) {
        navigate("/", { replace: true });
      }
    } catch (err) {
      alert(err);
    }
  };

  if (!lobby) {
    navigate("/", { replace: true });
    return;
  }

  const isHost = currentUserId === lobby.host.id;
  console.log("Current user:", currentUserId);
  console.log("Lobby host:", lobby.host.id);
  console.log("Is host?", isHost);
  console.log("Members:", lobby.members);

  return (
    <div>
      <h1>{lobby.name}</h1>
      <button onClick={leaveLobby} className="leave-lobby-button">
        Leave Lobby
      </button>

      {lobby.members && (
        <>
          <h2>
            Members ({lobby.members.length}/{lobby.maxMembers})
          </h2>
          <ul>
            {lobby.members.map((member) => (
              <li key={member.id}>
                {member.name}
                {member.id === lobby.host.id && " (host)"}

                {/* Show Kick button if current user is host and not the host themselves */}
                {isHost && member.id !== lobby.host.id && (
                  <button
                    onClick={() => {
                      if (
                        window.confirm(
                          `Are you sure you want to kick ${member.name}?`,
                        )
                      ) {
                        kickUser(member.id);
                      }
                    }}
                    style={{
                      marginLeft: "0.5rem",
                      backgroundColor: "#ff4d4f",
                      color: "white",
                      border: "none",
                      borderRadius: "3px",
                      cursor: "pointer",
                      padding: "0.2rem 0.5rem",
                    }}
                  >
                    Kick
                  </button>
                )}
              </li>
            ))}
          </ul>
        </>
      )}
    </div>
  );
}
