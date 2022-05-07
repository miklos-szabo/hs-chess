import { Component, OnInit } from '@angular/core';
import { SignalrService } from 'src/app/services/signalr/signalr.service';

@Component({
  selector: 'app-play-page',
  templateUrl: './play-page.component.html',
  styleUrls: ['./play-page.component.scss']
})
export class PlayPageComponent implements OnInit {
  constructor(private signalrService: SignalrService) {}

  ngOnInit(): void {
    this.signalrService.connect();
  }
}
