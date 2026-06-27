type ApiResponse<T> = {
  success: boolean;
  message: string;
  data: T | null;
  errors?: Record<string, string[]> | null;
};

class ApiError extends Error {
  status: number;
  errors?: Record<string, string[]>;
  constructor(status: number, message: string, errors?: Record<string, string[]>) {
    super(message);
    this.status = status;
    this.errors = errors;
  }
}

function getAuth(): string | null {
  return sessionStorage.getItem('auth');
}

function clearAuth(): void {
  sessionStorage.removeItem('auth');
  sessionStorage.removeItem('user');
}

function setAuth(auth: string, user: string): void {
  sessionStorage.setItem('auth', auth);
  sessionStorage.setItem('user', user);
}

function getStoredUser<T>(): T | null {
  const raw = sessionStorage.getItem('user');
  if (!raw) return null;
  try { return JSON.parse(raw) as T; } catch { return null; }
}

async function request<T>(
  method: string,
  path: string,
  body?: unknown,
): Promise<ApiResponse<T>> {
  const headers: Record<string, string> = { 'Content-Type': 'application/json' };
  const auth = getAuth();
  if (auth) headers['Authorization'] = `Basic ${auth}`;

  const res = await fetch(path.startsWith('/api/') ? path : `/api${path}`, {
    method,
    headers,
    body: body ? JSON.stringify(body) : undefined,
  });

  if (res.status === 401) {
    clearAuth();
    window.location.href = '/login';
    throw new ApiError(401, 'Unauthorized');
  }

  const json = await res.json() as ApiResponse<T>;
  if (!json.success) {
    throw new ApiError(res.status, json.message, json.errors ?? undefined);
  }
  return json;
}

const api = {
  get: <T>(path: string) => request<T>('GET', path),
  post: <T>(path: string, body?: unknown) => request<T>('POST', path, body),
  put: <T>(path: string, body?: unknown) => request<T>('PUT', path, body),
  delete: <T>(path: string) => request<T>('DELETE', path),
};

export { api, getAuth, setAuth, clearAuth, getStoredUser, ApiError };
export type { ApiResponse };
