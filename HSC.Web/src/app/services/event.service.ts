import { EventEmitter, Injectable, Output } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class EventService {
  @Output() friendSelectedEvent: EventEmitter<string> = new EventEmitter();
  @Output() resumeGameEvent: EventEmitter<void> = new EventEmitter();
  @Output() boardThemeChangedEvent: EventEmitter<string> = new EventEmitter();

  constructor() {}

  friendSelected(username: string) {
    this.friendSelectedEvent.emit(username);
  }

  resumeGame() {
    this.resumeGameEvent.emit();
  }

  boardThemeChanged(theme: string) {
    this.boardThemeChangedEvent.emit(theme);
  }
}
