import type { LobbyState, Player } from "./lobbyState";
import { appSettings } from "../../config";

export async function initialiseLobbyState(lobbyId: string): Promise<LobbyState> {
    let response: Response;

    try {
        response = await fetch(`${appSettings.baseUrl}/api/lobbies/${lobbyId}`);
    }
    catch (exception) {
        throw new Error(`Error: ${exception}`);
    }

    if (!response) {
        throw new Error('Response cannot be null.')
    }

    if (!response.ok) {
        throw new Error(`Error: ${response.status}`);
    }

    let responseData: LobbyDetailsDto;

    try {
        responseData = await response.json() as LobbyDetailsDto;
    }
    catch (exception) {
        throw new Error(`Error: ${exception}`);
    }

    return {
        players: responseData.players,
        hostId: responseData.hostId
    };
}

interface LobbyDetailsDto {
    hostId: string;
    players: Player[];
}