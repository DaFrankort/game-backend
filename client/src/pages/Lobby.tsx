import { useParams } from "react-router-dom";
import type { Lobby } from "../components/Types";
import { getCacheBearerToken } from "../components/Cache";
import { useEffect, useState } from "react";

export default function LobbyPage() {
    const { id } = useParams<{ id: string }>();
    const [lobby, setLobby] = useState<Lobby | null>(null);
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
            .catch((err) => alert(err));

    }, [id, token]);

    if (!lobby) {
        return <p>Loading lobby...</p>;
    }

    console.log(lobby);


    return (
        <div>
            <h1>{lobby.name}</h1>
            <b>Hosted by: {lobby.host.name}</b>
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