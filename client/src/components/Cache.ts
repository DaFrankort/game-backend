import type { AuthResponse } from "./Types";

export function addCacheUser(data: AuthResponse) {
  localStorage.setItem("authToken", data.authToken);
}

export function delCacheUser() {
  localStorage.removeItem("authToken");
}

export function getCacheBearerToken(): string | null {
  const auth = localStorage.getItem("authToken");
  if (auth) return `Bearer ${auth}`;
  return null;
}

export function isLoggedIn(): boolean {
  return getCacheBearerToken() != null;
}
