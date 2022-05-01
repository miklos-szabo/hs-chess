import { EventEmitter, Injectable, Output } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class EventService {
  @Output() friendSelectedEvent: EventEmitter<string> = new EventEmitter();
  @Output() resumeGameEvent: EventEmitter<void> = new EventEmitter();

  constructor() {}

  friendSelected(username: string) {
    this.friendSelectedEvent.emit(username);
  }

  resumeGame() {
    this.resumeGameEvent.emit();
  }
}
