export type LobbyList = {
  id: string;
  name: string;
  memberCount: number;
  maxMembers: number;
}[];

export interface Lobby {
  id: string;
  name: string;
  members: User[];
  maxMembers: number;
}

export interface User {
  id: string;
  name: string;
}

export interface AuthResponse extends User {
  authToken: string;
}
