import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { charactersApi, itemsApi } from '../api/rpgApi';

/**
 * React Query-hooks – ett lager mellan API-anrop och komponenter.
 *
 * Varför React Query?
 * - Automatisk caching: samma data hämtas inte igen i onödan
 * - Loading/error-states utan useState-boilerplate
 * - Automatisk refetch efter mutations (invalidateQueries)
 * - Optimistic updates möjliga utan extra kod
 *
 * QueryKeys är strängar/arrays som identifierar cache-poster.
 * Konvention: ['resurs'] för listor, ['resurs', id] för enstaka.
 */

// ── Query keys ─────────────────────────────────────────────────
export const QUERY_KEYS = {
  characters:          ['characters']          as const,
  character: (id: string) => ['characters', id] as const,
  items:               ['items']               as const,
};

// ── Characters: Queries ───────────────────────────────────────
export function useCharacters() {
  return useQuery({
    queryKey: QUERY_KEYS.characters,
    queryFn:  charactersApi.getAll,
  });
}

export function useCharacter(id: string) {
  return useQuery({
    queryKey: QUERY_KEYS.character(id),
    queryFn:  () => charactersApi.getById(id),
    enabled:  !!id, // Kör inte queryn om id är tomt
  });
}

// ── Characters: Mutations ─────────────────────────────────────
export function useCreateCharacter() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: ({ name, characterClass }: { name: string; characterClass: number }) =>
      charactersApi.create(name, characterClass),
    // onSuccess: ogiltigförklara cache för listan → React Query hämtar om
    onSuccess: () => qc.invalidateQueries({ queryKey: QUERY_KEYS.characters }),
  });
}

export function useLevelUp() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => charactersApi.levelUp(id),
    onSuccess: (updated) => {
      // Uppdatera both listan och den enskilda karaktären i cache
      qc.invalidateQueries({ queryKey: QUERY_KEYS.characters });
      qc.setQueryData(QUERY_KEYS.character(updated.id), updated);
    },
  });
}

export function useAddItem() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: ({ characterId, itemId }: { characterId: string; itemId: string }) =>
      charactersApi.addItem(characterId, itemId),
    onSuccess: (updated) => {
      qc.invalidateQueries({ queryKey: QUERY_KEYS.characters });
      qc.setQueryData(QUERY_KEYS.character(updated.id), updated);
    },
  });
}

export function useRemoveItem() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: ({ characterId, itemId }: { characterId: string; itemId: string }) =>
      charactersApi.removeItem(characterId, itemId),
    onSuccess: (updated) => {
      qc.invalidateQueries({ queryKey: QUERY_KEYS.characters });
      qc.setQueryData(QUERY_KEYS.character(updated.id), updated);
    },
  });
}

export function useDeleteCharacter() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => charactersApi.delete(id),
    onSuccess: () => qc.invalidateQueries({ queryKey: QUERY_KEYS.characters }),
  });
}

// ── Items ─────────────────────────────────────────────────────
export function useItems() {
  return useQuery({
    queryKey: QUERY_KEYS.items,
    queryFn:  itemsApi.getAll,
  });
}
