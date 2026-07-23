export function setGameName(name: string) {
    const nameHeader = document.getElementById('game-name');

    if (!nameHeader) {
        return
    }

    nameHeader.innerText = `Game: ${name}`;
}