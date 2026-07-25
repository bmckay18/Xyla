import { appSettings } from "../../config"

export function setHomepageTitle(): void {
    document.title = `${appSettings.name} | Home`;
}