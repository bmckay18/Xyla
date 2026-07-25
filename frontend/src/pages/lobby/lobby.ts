import { startImposters } from "../../games/imposters/imposters";
import { renderPlayers } from "./lobbyState";
import type { LobbyState } from "./lobbyState";

const mockLobbyState: LobbyState = {
    players: [
        {
            id: "1",
            name: "James"
        },
        {
            id: "4",
            name: "John"
        }
    ],
    hostId: "4",
    currentPlayerId: "4"
}

renderPlayers(mockLobbyState);

startImposters();