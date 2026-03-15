export type LobbyList = {
  id: string;
  name: string;
  hostUserName: string;
  memberCount: number;
  maxMembers: number;
}[];

export interface Lobby {
  id: string;
  name: string;
  host: User;
  members: User[];
  maxMembers: number;
}

export interface User {
  id: string;
  name: string;
  lobbyId: string | null;
}

export interface AuthResponse extends User {
  authToken: string;
}
