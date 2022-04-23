import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { KeycloakEventType, KeycloakService } from 'keycloak-angular';
import { AccountService } from './api/app.generated';
import { SignalrService } from './services/signalr/signalr.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'hsc-web';

  constructor(
    translateService: TranslateService,
    signalrService: SignalrService,
    keycloak: KeycloakService,
    accountService: AccountService
  ) {
    translateService.addLangs(['en', 'hu']);
    translateService.setDefaultLang('en');
    translateService.use('en');

    keycloak.keycloakEvents$.subscribe({
      next: (e) => {
        if (e.type == KeycloakEventType.OnTokenExpired) {
          keycloak.updateToken(300);
        } else if (e.type == KeycloakEventType.OnAuthSuccess) {
          signalrService.connect();
          accountService.createUserIfDoesntExist().subscribe(() => {});
        }
      }
    });
  }
}
