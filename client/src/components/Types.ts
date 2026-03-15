export type LobbyList = {
  id: number;
  name: string;
  memberCount: number;
  maxMembers: number;
}[];

export interface Lobby {
  id: number;
  name: string;
  members: User[];
  maxMembers: number;
}

export interface User {
  id: number;
  name: string;
}

export interface AuthResponse extends User {
  authToken: string;
}
