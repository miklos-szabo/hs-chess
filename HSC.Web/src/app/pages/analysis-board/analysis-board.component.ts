import { Component, OnInit } from '@angular/core';
import { EngineService } from 'src/app/services/engine.service';

@Component({
  selector: 'app-analysis-board',
  templateUrl: './analysis-board.component.html',
  styleUrls: ['./analysis-board.component.scss']
})
export class AnalysisBoardComponent implements OnInit {
  lastMessage = '';
  command = '';

  constructor(private engineService: EngineService) {}

  ngOnInit(): void {
    this.engineService.messageRecievedEvent.subscribe((msg) => {
      this.messageReceived(msg);
    });
  }

  start() {
    this.engineService.startEngine();
  }

  send() {
    this.engineService.sendMessage(this.command);
  }

  messageReceived(message: string) {
    this.lastMessage = message;
  }
}
