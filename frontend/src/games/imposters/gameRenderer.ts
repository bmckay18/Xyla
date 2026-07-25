import { getGameState } from "./gameState";
import { setPageTitle } from "../../pages/lobby/setPageTitle";
import type { GameState } from "./gameState";
import { getNewImpostersGamestate } from "./startImposters";

export function renderImposters(): void {
    const state = getGameState();

    if (!state) {
        return;
    }

    setPageTitle('Imposters');
    setupLobby(state);
    setupSettings();
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

function setupNewGame(): void {
    getNewImpostersGamestate();

    const state = getGameState();

    if (!state) {
        return;
    }

    setupLobby(state);
}

function setupSettings(): void {
    const gameSettings = document.getElementById('game-settings');

    if (!gameSettings) {
        return;
    }

    const startGameButton = document.createElement('wa-button');
    startGameButton.textContent = 'Start Game';
    startGameButton.setAttribute('variant', 'brand');

    startGameButton.addEventListener('click', setupNewGame);

    gameSettings.appendChild(startGameButton);
}