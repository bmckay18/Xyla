import { getGameState } from "./gameState";
import type { GameState } from "./gameState";

export function renderImposters(): void {
    const state = getGameState();

    if (!state) {
        return;
    }

    setupLobby(state);
}

function setupLobby(state: GameState): void {
    const gameSection = document.getElementsByClassName('game-section')[0];

    if (!gameSection) {
        return;
    }

    gameSection.innerHTML = '';

    const roleElement = document.createElement('h2');
    roleElement.innerText = `Role: ${state.role}`;

    const wordElement = document.createElement('p');

    gameSection.appendChild(roleElement);
    gameSection.appendChild(wordElement);
    
    if (state.hint) {
        wordElement.innerText = `Hint: ${state.hint}`;
    }
    else if (state.word) {
        wordElement.innerText = `Word: ${state.word}`;
    }
}

