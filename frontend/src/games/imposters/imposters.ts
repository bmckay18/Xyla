import { renderImposters } from "./gameRenderer";
import { setGameName } from "../core/setGameName";
import { getNewImpostersGamestate } from "./startImposters";

export function startImposters(): void {
    getNewImpostersGamestate();

    setGameName("Imposters");
    renderImposters();
}