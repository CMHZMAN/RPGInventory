import axios from 'axios';

/**
 * Axios-instans med bas-URL och gemensamma headers.
 *
 * Varför en central instans istället för fetch() direkt?
 * - En plats att ändra bas-URL (t.ex. prod vs dev)
 * - Interceptors för global felhantering / auth-token
 * - Automatisk JSON-serialisering och Content-Type
 */
const apiClient = axios.create({
  baseURL: 'http://localhost:5115/api',
  headers: { 'Content-Type': 'application/json' },
});

// Response-interceptor: logga fel centralt i development
apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    const message = error.response?.data?.error ?? error.message;
    console.error('[API Error]', message);
    return Promise.reject(new Error(message));
  }
);

export default apiClient;
