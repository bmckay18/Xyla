import { appSettings } from "../../config";
import type { LobbyDto } from "./initialiseCreateLobbyForm";

export async function initialiseJoinLobbyForm(): Promise<void> {
    const form = document.getElementById('join-lobby-form') as HTMLFormElement;
    const submitButton = document.getElementById('join-lobby-button');

    if (!form || !submitButton) {
        return;
    }

    form.addEventListener("submit", async (e) => {
        e.preventDefault();

        submitButton.setAttribute('loading', '');

        const data = new FormData(form);
        const displayName = data.get('displayName')?.toString();
        const lobbyId = data.get('lobbyId')?.toString();
        const password = data.get('password')?.toString();

        if (!displayName || !lobbyId) {
            console.error('Display name and lobby ID must have values.')
            submitButton.removeAttribute('loading');
            return;
        }

        const requestBody: JoinLobbyRequest = {
            displayName: displayName,
            lobbyId: lobbyId,
            password: password || null
        };

        const response: Response | null = await makeJoinLobbyRequest(requestBody);

        if (!response) {
            console.error("An error occurred.");
            submitButton.removeAttribute('loading');
            return;
        }

        if (response.status == 401) {
            alert('Invalid password.');
            submitButton.removeAttribute('loading');
            return;
        }

        if (!response.ok) {
            const message = await response.text();

            console.error(`Http ${response.status}: ${message}`);
            submitButton.removeAttribute('loading');

            alert(message);
            return;
        }

        let responseData: LobbyDto;

        try {
            responseData = await response.json() as LobbyDto
        }
        catch (exception) {
            console.error(`${exception}`);
            return
        }
        finally {
            submitButton.removeAttribute('loading');
        }

        sessionStorage.setItem("playerId", responseData.playerId);
        sessionStorage.setItem("authToken", responseData.jwt);
        window.location.href = `/lobby.html?lobbyId=${responseData.lobbyId}`;
    });
}

async function makeJoinLobbyRequest(body: JoinLobbyRequest): Promise<Response | null> {
    try {
        const response = await fetch(`${appSettings.baseUrl}/api/lobbies/join`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(body)
        });

        return response;
    }
    catch (exception) {
        console.error(`${exception}`);
        return null;
    }
}

interface JoinLobbyRequest {
    displayName: string;
    lobbyId: string;
    password: string | null;
};