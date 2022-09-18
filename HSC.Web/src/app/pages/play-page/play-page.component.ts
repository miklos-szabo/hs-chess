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
  constructor() {}

  ngOnInit(): void {}
}
