/**
 * TypeScript-typer som matchar API:ets C# record-DTOs exakt.
 *
 * Varför egna typer istället för att lita på `any`?
 * - Kompilatorn fångar stavfel och strukturändringar direkt
 * - Intellisense/autocomplete fungerar i hela appen
 * - Kontraktet mellan frontend och backend är dokumenterat i kod
 */

export interface InventoryItem {
  characterItemId: string;
  itemId: string;
  name: string;
  description: string;
  type: string;
  isEquipped: boolean;
  strengthBonus: number;
  intelligenceBonus: number;
  agilityBonus: number;
  defenseBonus: number;
}

export interface Character {
  id: string;
  name: string;
  level: number;
  currentHealth: number;
  maxHealth: number;
  strength: number;
  intelligence: number;
  agility: number;
  defense: number;
  class: string;
  isAlive: boolean;
  inventory: InventoryItem[];
}

export interface Item {
  id: string;
  name: string;
  description: string;
  type: string;
  strengthBonus: number;
  intelligenceBonus: number;
  agilityBonus: number;
  defenseBonus: number;
}

export type CharacterClass =
  | 'Warrior'
  | 'Mage'
  | 'Rogue'
  | 'Paladin'
  | 'Ranger';

export const CHARACTER_CLASSES: { label: string; value: number }[] = [
  { label: 'Warrior',  value: 1 },
  { label: 'Mage',     value: 2 },
  { label: 'Rogue',    value: 3 },
  { label: 'Paladin',  value: 4 },
  { label: 'Ranger',   value: 5 },
];

export interface AuthResponse {
  token: string;
  username: string;
  expiresAt: string;
}
