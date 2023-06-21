import { Component,  Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IStreamSubscriber } from '@microsoft/signalr';
import { SignalrService, TrackInfo } from './SignalrService';
import { environment } from '../../environments/environment';




@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public scenes: SceneModel[] = [];
  allFeedSubscription: any;
  IsBeat: boolean = false;
  BeatColor = '#F44336';
  NonBeatColor = '#000000';
  Color = '#000000'
  TrackName = 'NA'

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    
    http.get<SceneModel[]>(environment.apiUrl + 'scenes')
      .subscribe(result => {
      this.scenes = result;
    }, error => console.error(error));

    var signalR = new SignalrService();
    signalR.startConnection().then(() => {
      signalR.listenToAllFeeds();
    });

    const subscriber: IStreamSubscriber<TrackInfo> = {
      next: (trackInfo: TrackInfo) => {
        this.ProcessNewTrackInfo(trackInfo);
      },
      error: function (err: any): void {
        console.log('Error');
      },
      complete: function (): void {
        console.log('Done');
      }
    };

    this.allFeedSubscription = signalR.AllFeedObservable
      .subscribe(subscriber);

  }

    private ProcessNewTrackInfo(trackInfo: TrackInfo) {
        console.log('Received TrackInfo:', trackInfo);
        this.IsBeat = trackInfo.isBeat;
        if (trackInfo.trackInfo != this.TrackName) {
            this.TrackName = trackInfo.trackInfo;
        }
        if (this.IsBeat) {
            this.Color = this.BeatColor;
        }
        else {
            this.Color = this.NonBeatColor;
        }
    }
}

interface SceneModel {

  name: string;
}
