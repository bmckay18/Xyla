import { appSettings } from "../../config";

export interface LobbyState {
    players: Player[];
    hostId: string;
}

export interface Player {
    id: string;
    name: string;
}

function isHost(state: LobbyState): boolean {
    const playerId = sessionStorage.getItem('playerId');

    if (!playerId) {
        throw new Error('Player ID does not exist.');
    }

    return playerId === state.hostId;
}

export function renderPlayers(state: LobbyState): void {
    const tbody = document.getElementById('player-table-body');

    if (!tbody) {
        return;
    }

    tbody.innerHTML = '';

    state.players.forEach((player) => {
        const row = document.createElement('tr');

        const nameCell = document.createElement('td');
        nameCell.textContent = player.name;

        row.appendChild(nameCell);

        if (isHost(state) && player.id !== state.hostId) {
            const kickButtonCell = document.createElement('td');
            const kickButton = document.createElement('wa-button');

            kickButton.setAttribute('variant','danger');
            kickButton.setAttribute('size', 's');

            kickButton.textContent = 'Kick';

            kickButton.addEventListener("click", async (e) => {
                e.preventDefault();

                await kickPlayer(player.id);
            });

            kickButtonCell.appendChild(kickButton);
            row.appendChild(kickButtonCell);
        }     
        
        tbody.appendChild(row);
    });
}

async function kickPlayer(kickedPlayerId: string): Promise<void> {
    const jwt = sessionStorage.getItem('authToken');
    
    const response = await fetch(`${appSettings.baseUrl}/api/lobbies/kick`, {
        method: "POST",
        headers: {
            "Authorization": `Bearer ${jwt}`,
            "Content-Type": "application/json"
        },
        body: JSON.stringify({kickedPlayerId: kickedPlayerId})
    });

    if (!response.ok) {
        throw new Error(`${response.status}`);
    }
}