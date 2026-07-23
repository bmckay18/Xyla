import { setGameState } from "./gameState";
import { renderImposters } from "./gameRenderer";
import { setGameName } from "../core/setGameName";

export function impostersGame(): void {
    setGameState({
        playerId: "123",
        role: "Johnny",
        hint: "Hey"
    });

    setGameName("Imposters");
    renderImposters();
}