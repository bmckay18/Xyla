import { startImposters } from "../../games/imposters/imposters";
import { fetchLobbyState } from "./fetchLobbyState";
import { renderPlayers } from "./lobbyState";
import { connection, startSignalR } from "../../core/connection";
import type { LobbyState, Player } from "./lobbyState";

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

setupSignalRNotifications();
await startSignalR();
await connection.invoke('JoinLobby');


lobbyState = await fetchLobbyState(lobbyId, playerId);

renderPlayers(lobbyState);

startImposters();

function setupSignalRNotifications(): void {
    connection.on("PlayerJoined", (player: Player) => {
        
        const playerAlreadyExists = lobbyState.players.some(p => p.id === player.id);

        if (playerAlreadyExists) {
            return;
        }
        
        lobbyState.players.push(player);
        renderPlayers(lobbyState);
    })
}