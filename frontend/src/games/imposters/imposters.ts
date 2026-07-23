import { setGameState } from "./gameState";
import { renderImposters } from "./gameRenderer";
import { setGameName } from "../core/setGameName";

export function impostersGame(): void {
    setGameState({
        role: "Innocent",
        word: "Hey"
    });

    setGameName("Imposters");
    renderImposters();
}