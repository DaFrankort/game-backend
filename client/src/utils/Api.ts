import { getCacheBearerToken } from "./Cache";

type HttpMethod =
  | "GET"
  | "POST"
  | "PUT"
  | "DELETE"
  | "PATCH"
  | "OPTIONS"
  | "HEAD";

export async function fetchApi(
  endpoint: string,
  method: HttpMethod,
  body: object | null = null,
  withAuth: boolean = true,
): Promise<Response> {
  let url = import.meta.env.VITE_BACKEND_URL;
  if (url.endsWith("/")) {
    url = url.slice(0, -1);
  }

  if (!endpoint.startsWith("/")) endpoint = `/${endpoint}`;

  const headers: Record<string, string> = {
    "Content-Type": "application/json",
  };

  if (withAuth) {
    const token = getCacheBearerToken();
    if (token) headers["Authorization"] = token;
  }

  return fetch(`${url}${endpoint}`, {
    method,
    headers,
    body: body ? JSON.stringify(body) : undefined,
  });
}
