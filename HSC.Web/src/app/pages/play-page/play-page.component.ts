import { Component, OnInit } from '@angular/core';
import { TestService } from 'src/app/api/app.generated';
import { SignalrService } from 'src/app/services/signalr/signalr.service';

@Component({
  selector: 'app-play-page',
  templateUrl: './play-page.component.html',
  styleUrls: ['./play-page.component.scss']
})
export class PlayPageComponent implements OnInit {
  constructor(private testService: TestService, private signalrService: SignalrService) {}
  message = '';

  ngOnInit(): void {
    this.signalrService.connect();
  }

  getTest() {
    this.signalrService.joinMatch('4ae1f29c-94ca-4055-8685-9995a766bab8');
  }
}
