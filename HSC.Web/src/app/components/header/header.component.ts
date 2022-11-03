import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { TranslateService } from '@ngx-translate/core';
import { KeycloakService } from 'keycloak-angular';
import { Subscription } from 'rxjs';
import { AccountService, UserMenuDto } from 'src/app/api/app.generated';
import { EventService } from 'src/app/services/event.service';
import { SignalrService } from 'src/app/services/signalr/signalr.service';
import { ThemeService } from 'src/app/services/theme.service';
import { MenuComponent } from './menu/menu.component';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  userName = '';
  isAuthed = false;
  userdata = new UserMenuDto();

  balanceChangedSubscription!: Subscription;

  constructor(
    private translateService: TranslateService,
    private keycloak: KeycloakService,
    private accountService: AccountService,
    private dialog: MatDialog,
    private signalrService: SignalrService,
    private themeService: ThemeService,
    private eventService: EventService
  ) {}

  ngOnInit(): void {
    this.keycloak.isLoggedIn().then((res) => {
      this.isAuthed = res;
      if (res) {
        this.userName = this.keycloak.getUsername();
        this.accountService.createUserIfDoesntExist().subscribe(() => {
          this.signalrService.connect();
          this.accountService.getUserMenuData().subscribe((data) => {
            this.userdata = data;
          });
          this.accountService.usesLightTheme().subscribe((lightTheme) => {
            this.themeService.setDarkTheme(!lightTheme);
          });
        });
      }
    });

    this.balanceChangedSubscription = this.eventService.balanceChangedEvent.subscribe(() => {
      this.accountService.getUserMenuData().subscribe((data) => {
        this.userdata = data;
      });
    });
  }

  settingsClicked() {
    this.dialog.open(MenuComponent, {
      data: this.userdata,
      backdropClass: 'cdk-overlay-transparent-backdrop',
      width: '220px',
      position: { right: '20px', top: '80px' }
    });
  }
}
