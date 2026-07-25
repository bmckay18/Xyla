import { appSettings } from "../../config";

export function setLobbyPageTitle(gameName: string): void {
    document.title = `${appSettings.name} | ${gameName}`;
}