import { EventEmitter, Injectable, Output } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { KeycloakService } from 'keycloak-angular';
import { ChatMessageDto, MoveDto } from './signalr-dtos';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  @Output() moveReceivedEvent: EventEmitter<MoveDto> = new EventEmitter();
  @Output() matchFoundEvent: EventEmitter<string> = new EventEmitter();
  @Output() foldReceivedEvent: EventEmitter<void> = new EventEmitter();
  @Output() checkReceivedEvent: EventEmitter<void> = new EventEmitter();
  @Output() callReceivedEvent: EventEmitter<void> = new EventEmitter();
  @Output() betReceivedEvent: EventEmitter<number> = new EventEmitter();
  @Output() friendRequestReceivedEvent: EventEmitter<string> = new EventEmitter();
  @Output() chatMessageReceivedEvent: EventEmitter<ChatMessageDto> = new EventEmitter();

  connection = new signalR.HubConnectionBuilder()
    .withUrl('https://localhost:5000/hubs/chesshub', { accessTokenFactory: () => this.keyCloak.getToken() })
    .build();

  constructor(private keyCloak: KeycloakService) {}

  connect() {
    this.connection.start().then(() => {
      console.log('Connected to signalr!');
      this.connection.on('ReceiveMove', (move) => this.moveReceivedEvent.emit(move));
      this.connection.on('ReceiveMatchFound', (matchId) => this.matchFoundEvent.emit(matchId));
      this.connection.on('ReceiveFold', () => this.foldReceivedEvent.emit());
      this.connection.on('ReceiveCheck', () => this.checkReceivedEvent.emit());
      this.connection.on('ReceiveCall', () => this.callReceivedEvent.emit());
      this.connection.on('ReceiveBet', (amount) => this.betReceivedEvent.emit(amount));
      this.connection.on('ReceiveFriendRequest', (fromUser) => this.friendRequestReceivedEvent.emit(fromUser));
      this.connection.on('ReceiveChatMessage', (message) => this.betReceivedEvent.emit(message));
    });
  }

  joinMatch(matchId: string) {
    this.connection.send('JoinMatch', matchId);
  }

  leaveMatch(matchId: string) {
    this.connection.send('LeaveMatch', matchId);
  }

  sendMoveToServer(move: MoveDto, matchId: string) {
    this.connection.send('SendMoveToServer', move, matchId);
  }

  private matchFound(matchId: string) {
    this.matchFoundEvent.emit(matchId);
  }
}
