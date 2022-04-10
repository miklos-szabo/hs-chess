import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  constructor(private translateService: TranslateService) {}

  ngOnInit(): void {}

  changeLanguage() {
    if (this.translateService.currentLang === 'en') this.translateService.use('hu');
    else {
      this.translateService.use('en');
    }
  }
}
