import type { Character, Item } from '../types/api.types';
import apiClient from './apiClient';

/**
 * Alla API-anrop samlade per domän.
 * Komponenterna anropar dessa funktioner – aldrig axios direkt.
 * Det gör det enkelt att byta HTTP-bibliotek utan att röra komponenterna.
 */

// ── Characters ───────────────────────────────────────────────
export const charactersApi = {
  getAll: () =>
    apiClient.get<Character[]>('/characters').then((r) => r.data),

  getById: (id: string) =>
    apiClient.get<Character>(`/characters/${id}`).then((r) => r.data),

  create: (name: string, characterClass: number) =>
    apiClient
      .post<Character>('/characters', { name, class: characterClass })
      .then((r) => r.data),

  updateName: (id: string, newName: string) =>
    apiClient
      .put<Character>(`/characters/${id}/name`, { newName })
      .then((r) => r.data),

  levelUp: (id: string) =>
    apiClient
      .post<Character>(`/characters/${id}/levelup`)
      .then((r) => r.data),

  addItem: (characterId: string, itemId: string) =>
    apiClient
      .post<Character>(`/characters/${characterId}/inventory/${itemId}`)
      .then((r) => r.data),

  removeItem: (characterId: string, itemId: string) =>
    apiClient
      .delete<Character>(`/characters/${characterId}/inventory/${itemId}`)
      .then((r) => r.data),

  delete: (id: string) =>
    apiClient.delete(`/characters/${id}`),
};

// ── Items ─────────────────────────────────────────────────────
export const itemsApi = {
  getAll: () =>
    apiClient.get<Item[]>('/items').then((r) => r.data),

  getById: (id: string) =>
    apiClient.get<Item>(`/items/${id}`).then((r) => r.data),
};
