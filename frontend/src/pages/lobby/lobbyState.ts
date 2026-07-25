export interface LobbyState {
    players: Player[];
    hostId: string;
    currentPlayerId: string;
}

export interface Player {
    id: string;
    name: string;
}

export function isHost(state: LobbyState): boolean {
    return state.currentPlayerId === state.hostId;
}

export function renderPlayers(state: LobbyState): void {
    const tbody = document.getElementById('player-table-body');

    if (!tbody) {
        return;
    }

    tbody.innerHTML = '';

    state.players.forEach((player) => {
        const row = document.createElement('tr');

        const iconCell = document.createElement('td');
        
        if (player.id === state.hostId) {
            iconCell.textContent = 'Host';
        }

        const nameCell = document.createElement('td');
        nameCell.textContent = player.name;

        const kickButtonCell = document.createElement('td');
        const kickButton = document.createElement('wa-button');

        kickButton.setAttribute('variant','danger');
        kickButton.setAttribute('size', 's');

        kickButton.textContent = 'Kick';

        kickButtonCell.appendChild(kickButton);

        row.appendChild(iconCell);
        row.appendChild(nameCell);
        row.appendChild(kickButtonCell);

        tbody.appendChild(row);
    })
}