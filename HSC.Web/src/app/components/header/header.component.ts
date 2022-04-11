import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { UserService } from 'src/app/services/auth/user.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  userName = '';
  constructor(private translateService: TranslateService, private userService: UserService) {}

  ngOnInit(): void {
    this.userName = this.userService.getUserName();
  }

  changeLanguage() {
    if (this.translateService.currentLang === 'en') this.translateService.use('hu');
    else {
      this.translateService.use('en');
    }
  }

  setUserName() {
    this.userService.setUserName(this.userName);
  }
}
