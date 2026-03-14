export type LobbyList = {
    id: number;
    name: string;
    userCount: number;
}[];

export interface Lobby {
    id: number;
    name: string;
    users: User[];
}

export interface User {
    id: number;
    name: string;
}