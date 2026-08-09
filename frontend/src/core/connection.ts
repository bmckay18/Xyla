import * as signalR from "@microsoft/signalr";
import { appSettings } from "../config";

export const connection = new signalR.HubConnectionBuilder()
    .withUrl(`${appSettings.baseUrl}/gameHub`, {
        accessTokenFactory: () => {
            const token = sessionStorage.getItem('authToken');

            if (!token) {
                throw new Error('Not authenticated');
            }

            return token;
        }
    })
    .withAutomaticReconnect()
    .build();

export async function startSignalR(): Promise<void> {
    if (connection.state === signalR.HubConnectionState.Disconnected) {
        await connection.start();
    }
};