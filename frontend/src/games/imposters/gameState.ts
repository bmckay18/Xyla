type Role = "Imposter" | "Innocent"

export interface GameState {
    role: Role;
    word?: string;
    hint?: string;
}

let currentState: GameState | null = null;

export function getGameState(): GameState | null {
    return currentState;
}

export function setGameState(state: GameState): void {
    currentState = state;
}