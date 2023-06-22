import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { BehaviorSubject } from 'rxjs';
import { SceneModel } from "./SceneModel";

@Injectable({ providedIn: 'root' })
export class ScenesService {

    public scenes = new BehaviorSubject<SceneModel[]>([]);
    public scenePromise: Promise<SceneModel[]>;


    constructor(http: HttpClient) {
        this.scenePromise = new Promise<SceneModel[]>((resolved, rejected) => {
            http.get<SceneModel[]>(environment.apiUrl + 'scenes')
                .subscribe(result => {
                    resolved(result);
                    this.scenes.next(result);
                }, error => console.error(error));
        });
    }
}
