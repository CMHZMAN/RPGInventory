import axios from 'axios';

const TOKEN_KEY = 'rpg_token';

export const tokenStorage = {
  get: ()         => localStorage.getItem(TOKEN_KEY),
  set: (t: string) => localStorage.setItem(TOKEN_KEY, t),
  clear: ()        => localStorage.removeItem(TOKEN_KEY),
};

const apiClient = axios.create({
  baseURL: 'http://localhost:5115/api',
  headers: { 'Content-Type': 'application/json' },
});

// Bifoga JWT automatiskt på varje request om den finns
apiClient.interceptors.request.use((config) => {
  const token = tokenStorage.get();
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

// Logga fel centralt; rensa token vid 401 (utgången/ogiltig)
apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      tokenStorage.clear();
      window.location.href = '/login';
    }
    const message = error.response?.data?.error ?? error.message;
    console.error('[API Error]', message);
    return Promise.reject(new Error(message));
  }
);

export default apiClient;
