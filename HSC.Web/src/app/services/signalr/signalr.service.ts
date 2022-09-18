import { EventEmitter, Injectable, Output } from '@angular/core';
import { Router } from '@angular/router';
import * as signalR from '@microsoft/signalr';
import { TranslateService } from '@ngx-translate/core';
import { KeycloakService } from 'keycloak-angular';
import { MatchFinderService, Result } from 'src/app/api/app.generated';
import { environment } from 'src/environments/environment';
import { NotificationService } from '../notification.service';
import { ChallengeDto, ChatMessageDto, MoveDto, TournamentOverDto } from './signalr-dtos';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  @Output() moveReceivedEvent: EventEmitter<MoveDto> = new EventEmitter();
  @Output() matchFoundEvent: EventEmitter<string> = new EventEmitter();
  @Output() challengeFoundEvent: EventEmitter<ChallengeDto> = new EventEmitter();
  @Output() foldReceivedEvent: EventEmitter<number> = new EventEmitter();
  @Output() checkReceivedEvent: EventEmitter<void> = new EventEmitter();
  @Output() callReceivedEvent: EventEmitter<void> = new EventEmitter();
  @Output() betReceivedEvent: EventEmitter<number> = new EventEmitter();
  @Output() friendRequestReceivedEvent: EventEmitter<string> = new EventEmitter();
  @Output() chatMessageReceivedEvent: EventEmitter<ChatMessageDto> = new EventEmitter();
  @Output() drawOfferReceivedEvent: EventEmitter<void> = new EventEmitter();
  @Output() matchEndedReceivedEvent: EventEmitter<Result> = new EventEmitter();
  @Output() tournamentOverEvent: EventEmitter<TournamentOverDto> = new EventEmitter();
  @Output() tournamentMessageReceivedEvent: EventEmitter<void> = new EventEmitter();
  @Output() updateStandingsEvent: EventEmitter<void> = new EventEmitter();
  @Output() tournamentStartedEvent: EventEmitter<void> = new EventEmitter();

  connection = new signalR.HubConnectionBuilder()
    .withUrl(environment.serverUrl + '/hubs/chesshub', { accessTokenFactory: () => this.keyCloak.getToken() })
    .build();

  constructor(
    private keyCloak: KeycloakService,
    private notificationService: NotificationService,
    private translateService: TranslateService,
    private matchFinderService: MatchFinderService,
    private router: Router
  ) {}

  connect() {
    if (this.connection.state === signalR.HubConnectionState.Connected) {
      return;
    }
    console.log('Connecting to SignalR on ' + environment.serverUrl);
    this.connection.start().then(() => {
      console.log('Connected to signalr!');
      this.connection.on('ReceiveMove', (move) => this.moveReceivedEvent.emit(move));
      this.connection.on('ReceiveMatchFound', (matchId) => this.matchFoundEvent.emit(matchId));
      this.connection.on('ReceiveChallenge', (challenge) => this.challengeFoundEvent.emit(challenge));
      this.connection.on('ReceiveFold', (amount) => this.foldReceivedEvent.emit(amount));
      this.connection.on('ReceiveCheck', () => this.checkReceivedEvent.emit());
      this.connection.on('ReceiveCall', () => this.callReceivedEvent.emit());
      this.connection.on('ReceiveBet', (amount) => this.betReceivedEvent.emit(amount));
      this.connection.on('ReceiveFriendRequest', (fromUser) => this.friendRequestReceivedEvent.emit(fromUser));
      this.connection.on('ReceiveMessage', (message) => this.chatMessageReceivedEvent.emit(message));
      this.connection.on('ReceiveDrawOffer', () => this.drawOfferReceivedEvent.emit());
      this.connection.on('ReceiveGameEnded', (result) => this.matchEndedReceivedEvent.emit(result));
      this.connection.on('ReceiveTournamentOver', (result) => this.tournamentOverEvent.emit(result));
      this.connection.on('ReceiveTournamentMessage', () => this.tournamentMessageReceivedEvent.emit());
      this.connection.on('ReceiveUpdateStandings', () => this.updateStandingsEvent.emit());
      this.connection.on('ReceiveTournamentStarted', () => this.tournamentStartedEvent.emit());
    });

    this.challengeFoundEvent.subscribe((challenge) => {
      this.receiveChallenge(challenge);
    });

    this.friendRequestReceivedEvent.subscribe((user) => {
      this.receiveFriendRequest(user);
    });

    this.tournamentOverEvent.subscribe((res) => {
      this.notificationService.info(
        this.translateService.instant('TournamentOverPopup.NotificationText') + ' ' + res.winnings + '$'
      );
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

  sendDrawOffer(toUserName: string) {
    this.connection.send('SendDrawOfferToUser', toUserName);
  }

  receiveFriendRequest(fromUser: string) {
    this.notificationService.infoWithAction(
      this.translateService.instant('Friends.RequestReceived', { userName: fromUser }),
      this.translateService.instant('Friends.RequestNotificationButton'),
      () => {
        this.router.navigateByUrl('/friends');
      }
    );
  }

  receiveChallenge(challenge: ChallengeDto) {
    this.notificationService.infoWithAction(
      this.translateService.instant('Challenge.Notification', { userName: challenge.userName }),
      this.translateService.instant('Challenge.NotificationButton'),
      () => {
        let subscription = this.matchFoundEvent.subscribe((id) => {
          subscription.unsubscribe();
          this.router.navigateByUrl(`/chess/${id}`);
        });
        this.matchFinderService.joinCustomGame(challenge.id).subscribe(() => {});
      }
    );
  }

  private matchFound(matchId: string) {
    this.matchFoundEvent.emit(matchId);
  }
}
