import { Component, Inject } from '@angular/core';
import { Subject } from 'rxjs';
import { SceneModel } from './SceneModel';
import { ScenesService } from './ScenesService';




@Component({
  selector: 'scenes',
  templateUrl: './scenes.component.html'
})
export class ScenesComponent {
  public scenes: SceneModel[] = [];

  constructor(service: ScenesService) {
    service.scenePromise.then((model) => {
      this.scenes = model;
    });
  }

}
