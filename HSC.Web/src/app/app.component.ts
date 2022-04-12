import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { SignalrService } from './services/signalr/signalr.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'hsc-web';

  constructor(translateService: TranslateService, signalrService: SignalrService) {
    translateService.addLangs(['en', 'hu']);
    translateService.setDefaultLang('en');
    translateService.use('en');

    signalrService.connect();
  }
}
