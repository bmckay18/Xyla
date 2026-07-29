import type { LobbyState, Player } from "./lobbyState";
import { appSettings } from "../../config";

export async function fetchLobbyState(lobbyId: string): Promise<LobbyState> {
    let response: Response;

    try {
        response = await fetch(`${appSettings.baseUrl}/api/lobbies/${lobbyId}`);

        if (!response.ok) {
            throw new Error(`${response.status}`);
        }

        const responseData = await response.json() as LobbyDetailsDto;

        return {
            players: responseData.players,
            hostId: responseData.hostId
        };
    }
    catch (exception) {
        throw new Error(`${exception}`);
    }
}

interface LobbyDetailsDto {
    hostId: string;
    players: Player[];
}