import { Component, OnInit } from '@angular/core';
import { AccountService } from 'src/app/api/app.generated';
import { SignalrService } from 'src/app/services/signalr/signalr.service';
import { ThemeService } from 'src/app/services/theme.service';

@Component({
  selector: 'app-play-page',
  templateUrl: './play-page.component.html',
  styleUrls: ['./play-page.component.scss']
})
export class PlayPageComponent implements OnInit {
  constructor(
    private signalrService: SignalrService,
    private accountService: AccountService,
    private themeService: ThemeService
  ) {}

  ngOnInit(): void {
    this.signalrService.connect();
    this.accountService.usesLightTheme().subscribe((lightTheme) => {
      this.themeService.setDarkTheme(!lightTheme);
    });
    this.accountService.createUserIfDoesntExist().subscribe(() => {});
  }
}
