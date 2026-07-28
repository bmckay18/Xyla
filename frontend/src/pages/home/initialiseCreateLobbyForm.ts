import { appSettings } from "../../config";

export async function initialiseCreateLobbyForm(): Promise<void> {
    const form = document.getElementById('create-lobby-form') as HTMLFormElement;
    const submitButton = document.getElementById('create-lobby-form');

    if (!form || !submitButton) {
        return;
    }

    form.addEventListener('submit', async (e: SubmitEvent) => {
        e.preventDefault();

        const data = new FormData(form);
        const hostName = data.get('displayName')?.toString();
        const password = data.get('password')?.toString();

        if (!hostName) {
            throw new Error('Display name is required.');
        }

        const requestBody: CreateLobbyRequest = {
            hostName: hostName,
            password: password || null
        };

        const response = await fetch(`${appSettings.baseUrl}/api/lobbies`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(requestBody)
        });

        if (!response.ok) {
            throw new Error(`Error: ${response.status}`);
        }

        const responseData = await response.json() as LobbyDto;

        console.log(responseData); //debugging
    })
}

interface CreateLobbyRequest {
    hostName: string,
    password: string | null
}

interface LobbyDto {
    Id: string,
    Host: Player,
    Players: Player[]
}

interface Player {
    Id: string,
    Name: string
}