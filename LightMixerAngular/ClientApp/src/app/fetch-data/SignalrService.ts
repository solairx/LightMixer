import * as signalR from '@microsoft/signalr';
import { HubConnection, HubConnectionBuilder, Subject } from '@microsoft/signalr';
import { environment } from '../../environments/environment';


export class TrackInfo {
  isBeat: boolean = false;
  trackInfo: string = '';
}

export class SignalrService {

  private hubConnection: any;
  private $allFeed: Subject<TrackInfo> = new Subject<TrackInfo>();


  public get AllFeedObservable(): Subject<TrackInfo> {
    return this.$allFeed;
  }

  public startConnection() {
    return new Promise((resolve, reject) => {
      var conn = environment.apiUrl + "hub";
      this.hubConnection = new HubConnectionBuilder()
        .withUrl(conn, {
          skipNegotiation: true,
          transport: signalR.HttpTransportType.WebSockets
        }).build();

      this.hubConnection.start()
        .then(() => {
          console.log("connection established");
          return resolve(true);
        })
        .catch((err: any) => {
          console.log("error occured " + err);
          reject(err);
        });
    });
  }

  public listenToAllFeeds() {
    (<HubConnection>this.hubConnection).on("TrackInfo", (data: TrackInfo) => {
      this.$allFeed.next(data);
    });
  }
}
