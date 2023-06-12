import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Observable, Subject } from 'rxjs';
import * as signalR from '@microsoft/signalr';


@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public scenes: SceneModel[] = [];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<SceneModel[]>('https://localhost:7133/' + 'scenes').subscribe(result => {
      this.scenes = result;
    }, error => console.error(error));

    var signalR = new SignalrService();
    signalR.startConnection();

  }
}

export class SignalrService {

  private hubConnection: any;

  public startConnection() {

    var connection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7133/hub',{
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
      })
      .withAutomaticReconnect()
      .build();


    connection.start()
      .then(() => { 
        console.log("connection established");

      })
      .catch((err: any) => {
        console.log("error occured" + err);
        
      });

    return new Promise((resolve, reject) => {
      this.hubConnection = new HubConnectionBuilder()
        .withUrl("https://localhost:7133/Hub", { skipNegotiation: true,
          transport: signalR.HttpTransportType.WebSockets}).build();

      this.hubConnection.start()
        .then(() => {
          console.log("connection established");
          return resolve(true);
        })
        .catch((err: any) => {
          console.log("error occured" + err);
          reject(err);
        });
    });
  }
}

interface SceneModel {
  
  name: string;
}
