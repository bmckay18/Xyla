import { startImposters } from "../../games/imposters/imposters";
import { fetchLobbyState } from "./fetchLobbyState";
import { renderPlayers } from "./lobbyState";
import type { LobbyState } from "./lobbyState";

let lobbyState: LobbyState;

const params = new URLSearchParams(window.location.search);
const lobbyId = params.get("lobbyId");
const playerId = sessionStorage.getItem('playerId');

if (!lobbyId) {
    throw new Error('No lobby ID provided.');
}

if (!playerId) {
    throw new Error('No player ID provided.')
}

lobbyState = await fetchLobbyState(lobbyId, playerId);

renderPlayers(lobbyState);

startImposters();