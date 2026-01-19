import { Injectable, signal } from '@angular/core';
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import { Environment } from '../../environment/environment.dev';
import { ColaboratorModel } from '../../pages/colaborator/models/colaborator.model';

@Injectable({
  providedIn: 'root',
})
export class SignalrService {
  hubUrl = Environment.hubUrl;
  hubConnection?: HubConnection;
  colaboratorHub = signal<ColaboratorModel | null>(null);
  colaboratorUpdateStatusHub = signal<string | null>(null);
  colaboratorCompleteUpdateHub = signal<string | null>(null);

  createHubConnection() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl, {
        withCredentials: true,
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch((error) => console.log(error));

    this.hubConnection.on('CreateCollaboratorNotification', (colaborator: ColaboratorModel) => {
      this.colaboratorHub.set(colaborator);
    });

    this.hubConnection.on('StatusUpdateCollaboratorNotification', (status: string ) => {
      this.colaboratorUpdateStatusHub.set(status);
    });

    this.hubConnection.on('CompleteUpdateCollaboratorNotification', (status: string ) => {
      this.colaboratorCompleteUpdateHub.set(status);
    });
  }

  stopHubConnection() {
    if (this.hubConnection?.state === HubConnectionState.Connected) {
      this.hubConnection.stop().catch((error) => console.log(error));
    }
  }

  isConnected(): boolean {
    return this.hubConnection?.state === HubConnectionState.Connected;
  }
}
