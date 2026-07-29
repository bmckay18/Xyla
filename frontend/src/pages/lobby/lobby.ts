import { startImposters } from "../../games/imposters/imposters";
import { fetchLobbyState } from "./fetchLobbyState";
import { renderPlayers } from "./lobbyState";
import type { LobbyState } from "./lobbyState";

let lobbyState: LobbyState;

const params = new URLSearchParams(window.location.search);
const lobbyId = params.get("lobbyId");

if (!lobbyId) {
    throw new Error('No lobby ID provided.');
}

lobbyState = await fetchLobbyState(lobbyId);

renderPlayers(lobbyState);

startImposters();