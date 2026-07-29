import { startImposters } from "../../games/imposters/imposters";
import { initialiseLobbyState } from "./initialiseLobbyState";
import { renderPlayers } from "./lobbyState";
import type { LobbyState } from "./lobbyState";

let lobbyState: LobbyState;

const params = new URLSearchParams(window.location.search);
const lobbyId = params.get("lobbyId");

if (!lobbyId) {
    throw new Error('No lobby ID provided.');
}

lobbyState = await initialiseLobbyState(lobbyId);

renderPlayers(lobbyState);

startImposters();