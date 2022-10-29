import { EventEmitter, Injectable, Output } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class EngineService {
  constructor() {}

  worker: Worker | undefined = undefined;
  @Output() messageRecievedEvent: EventEmitter<string> = new EventEmitter();

  startEngine() {
    console.log('Starting engine...');
    this.worker = new Worker('stockfish.js', { type: 'module' });
    this.worker.onmessage = ({ data }) => {
      this.messageRecievedEvent.emit(data);
    };
    console.log('Engine started!');
  }

  sendMessage(message: string) {
    this.worker!.postMessage(message);
  }
}
