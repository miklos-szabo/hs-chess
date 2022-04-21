import { Component, OnInit } from '@angular/core';
import { TestService } from 'src/app/api/app.generated';

@Component({
  selector: 'app-play-page',
  templateUrl: './play-page.component.html',
  styleUrls: ['./play-page.component.scss']
})
export class PlayPageComponent implements OnInit {
  constructor(private testService: TestService) {}
  message = '';

  ngOnInit(): void {}

  getTest() {
    this.testService.getHello().subscribe((msg) => {
      this.message = msg;
    });
  }
}
