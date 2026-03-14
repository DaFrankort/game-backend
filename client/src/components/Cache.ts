export function addCacheUser(id: number, auth: string) {
    localStorage.setItem("userId", id.toString());
    localStorage.setItem("authToken", auth);
}

export function getCacheUserId(): number | null {
    const id = localStorage.getItem("userId");
    if (id) return parseInt(id);
    return null;
}

export function getCacheBearerToken(): string | null {
    const auth = localStorage.getItem("authToken");
    if (auth) return `Bearer ${auth}`;
    return null;
}