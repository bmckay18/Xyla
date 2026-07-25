import { setGameState } from "./gameState";

export function getNewImpostersGamestate() {
    //Make call to backend to get new game state
    
    setGameState({
        role: "Innocent",
        word: "Hey"
    });
}