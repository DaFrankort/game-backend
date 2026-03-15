import { useNavigate, useParams } from "react-router-dom";
import type { Lobby } from "../components/Types";
import { getCacheBearerToken } from "../components/Cache";
import { useEffect, useState } from "react";

export default function LobbyPage() {
    const { id } = useParams<{ id: string }>();
    const [lobby, setLobby] = useState<Lobby | null>(null);
    const navigate = useNavigate();
    const token = getCacheBearerToken();

    useEffect(() => {
        if (!id || !token) return;

        fetch(`${import.meta.env.VITE_BACKEND_URL}/api/lobby/${id}`, {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                Authorization: token,
            },
        })
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
            const res = await fetch(
                `${import.meta.env.VITE_BACKEND_URL}/api/lobby/${id}/members`,
                {
                    method: "DELETE",
                    headers: {
                        "Content-Type": "application/json",
                        Authorization: token,
                    },
                }
            );

            if (!res.ok) throw new Error(`Failed to leave lobby (${res.status})`);
            navigate("/", { replace: true });
        } catch (err) {
            alert(err);
        }
    };

    if (!lobby) {
        return <p>Loading lobby...</p>;
    }

    return (
        <div>
            <h1>{lobby.name}</h1>
            <button onClick={leaveLobby} className="leave-lobby-button">
                Leave Lobby
            </button>
            {lobby.members && (
                <>
                    <h2>Members ({lobby.members?.length ?? 0}/{lobby.maxMembers})</h2>
                    <ul>
                        {lobby.members.map((member) => (
                            <li key={member.id}>
                                {member.name}
                                {member.id === lobby.host.id && " (host)"}
                            </li>
                        ))}
                    </ul>
                </>
            )}
        </div>
    );
}