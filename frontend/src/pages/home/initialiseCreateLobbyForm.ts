import { appSettings } from "../../config";

export async function initialiseCreateLobbyForm(): Promise<void> {
    const form = document.getElementById('create-lobby-form') as HTMLFormElement;
    const submitButton = document.getElementById('create-lobby-button');

    if (!form || !submitButton) {
        return;
    }

    form.addEventListener('submit', async (e) => {
        e.preventDefault();

        submitButton.setAttribute('loading','');

        const data = new FormData(form);
        const hostName = data.get('displayName')?.toString();
        const password = data.get('password')?.toString();

        if (!hostName) {
            console.error('Display name is required.');
            submitButton.removeAttribute('loading');
            return;
        }

        const requestBody: CreateLobbyRequest = {
            hostName: hostName,
            password: password || null
        };

        let response: Response | null;

        response = await makeCreateLobbyRequest(requestBody);

        if (!response) {
            submitButton.removeAttribute('loading');
            return;
        }

        if (!response.ok) {
            console.error(`${response.status}`);
            submitButton.removeAttribute('loading');
            return;
        }

        let responseData: LobbyDto;

        try {
            responseData = await response.json() as LobbyDto;
        }
        catch (exception) {
            console.error(`${exception}`);
            return;
        }
        finally {
            submitButton.removeAttribute('loading');
        }

        sessionStorage.setItem("playerId", responseData.playerId);

        window.location.href = `/lobby.html?lobbyId=${responseData.lobbyId}`;
    })
}

async function makeCreateLobbyRequest(data: CreateLobbyRequest): Promise<Response | null> {
    try {
        const response = await fetch(`${appSettings.baseUrl}/api/lobbies`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(data)
            });

        return response;
    }
    catch(exception) {
        console.error(`${exception}`);
        return null;
    }
}

interface CreateLobbyRequest {
    hostName: string;
    password: string | null;
}

interface LobbyDto {
    lobbyId: string;
    playerId: string;
}