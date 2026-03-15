import { useParams } from "react-router-dom";

export default function LobbyPage() {
    const { id } = useParams();

    return (
        <div>
            <h1>Lobby {id}</h1>
        </div>
    );
}