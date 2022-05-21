import { isDataSource } from '@angular/cdk/collections';
import { Component, HostBinding, Inject, OnInit, ViewChild } from '@angular/core';
import { MatOptionSelectionChange } from '@angular/material/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSelect } from '@angular/material/select';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { KeycloakService } from 'keycloak-angular';
import { Observable } from 'rxjs';
import { AccountService, UserMenuDto } from 'src/app/api/app.generated';
import { ThemeService } from 'src/app/services/theme.service';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss']
})
export class MenuComponent implements OnInit {
  isUsingRealMoney: boolean;
  languages: string[] = [];
  currentLanguage = '';

  isCurrentlyDarkTheme = false;

  @HostBinding('class.light-theme') isLightTheme: boolean = false;

  constructor(
    public dialogRef: MatDialogRef<MenuComponent>,
    @Inject(MAT_DIALOG_DATA) public data: UserMenuDto,
    private translateService: TranslateService,
    private accountService: AccountService,
    private keycloak: KeycloakService,
    private router: Router,
    private themeService: ThemeService
  ) {
    this.isUsingRealMoney = !data.isUsingPlayMoney;
    this.languages = this.translateService.getLangs();
    this.currentLanguage = this.translateService.currentLang;
  }

  ngOnInit(): void {
    this.isCurrentlyDarkTheme = this.themeService.isCurrentlyDarkTheme;
    this.isLightTheme = this.isCurrentlyDarkTheme;

    this.themeService.isDarkTheme.subscribe((darkTheme) => {
      this.isLightTheme = !darkTheme;
    });
  }

  realMoneySwitchChanged() {
    this.accountService.changeRealMoney(this.isUsingRealMoney).subscribe(() => {});
  }

  changeLanguage(lang: string) {
    this.translateService.use(lang);
  }

  async logout() {
    await this.keycloak.logout(window.location.origin);
  }

  toggleDarkTheme(darkTheme: boolean) {
    this.themeService.setDarkTheme(darkTheme);
    this.accountService.setLightTheme(!darkTheme).subscribe(() => {});
  }
}
