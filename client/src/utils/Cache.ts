import type { AuthResponse } from "./Types";

export function addCacheUser(data: AuthResponse) {
  localStorage.setItem("authToken", data.authToken);
  localStorage.setItem("userId", data.id);
}

export function delCacheUser() {
  localStorage.removeItem("authToken");
  localStorage.removeItem("userId");
}

export function getCurrentUserId(): string | null {
  return localStorage.getItem("userId");
}

export function getCacheBearerToken(): string | null {
  const auth = localStorage.getItem("authToken");
  if (auth) return `Bearer ${auth}`;
  return null;
}

export function isLoggedIn(): boolean {
  return getCacheBearerToken() != null && getCurrentUserId() != null;
}
