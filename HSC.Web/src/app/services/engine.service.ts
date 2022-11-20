import { EventEmitter, Injectable, Output } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class EngineService {
  constructor() {}

  worker: Worker | undefined = undefined;
  @Output() messageRecievedEvent: EventEmitter<string> = new EventEmitter();
  @Output() EngineReadyevent: EventEmitter<void> = new EventEmitter();

  startEngine() {
    console.log('Starting engine...');
    this.worker = new Worker('stockfish.js', { type: 'module' });
    this.worker.onmessage = ({ data }) => {
      this.messageRecievedEvent.emit(data);
      this.messageReceived(data);
    };
    console.log('Engine started!');
  }

  messageReceived(data: string) {
    if (data === 'uciok') {
      this.sendMessage('isready');
    }
    if (data === 'readyok') {
      this.EngineReadyevent.emit();
    }
  }

  sendMessage(message: string) {
    this.worker!.postMessage(message);
  }
}
