import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { TranslateService } from '@ngx-translate/core';
import { KeycloakService } from 'keycloak-angular';
import { AccountService, UserMenuDto } from 'src/app/api/app.generated';
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
  constructor(
    private translateService: TranslateService,
    private keycloak: KeycloakService,
    private accountService: AccountService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.keycloak.isLoggedIn().then((res) => {
      this.isAuthed = res;
      if (res) this.userName = this.keycloak.getUsername();
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
