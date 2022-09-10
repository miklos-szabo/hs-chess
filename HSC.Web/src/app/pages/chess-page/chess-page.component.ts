import { AfterViewChecked, ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { Color } from 'chessground/types';
import { KeycloakService } from 'keycloak-angular';
import { Observable, Subject, Subscription } from 'rxjs';
import {
  ChatMessageDto,
  ChatService,
  MatchFullDataDto,
  MatchService,
  MatchStartDto,
  Result,
  TournamentPlayerDto,
  TournamentService
} from 'src/app/api/app.generated';
import { CountdownTimerComponent } from 'src/app/components/countdown-timer/countdown-timer.component';
import { EventService } from 'src/app/services/event.service';
import { MoveDto, TournamentOverDto } from 'src/app/services/signalr/signalr-dtos';
import { SignalrService } from 'src/app/services/signalr/signalr.service';
import { BettingPopupComponent } from '../chess-board/betting-popup/betting-popup.component';
import { HistoryMoveDto } from '../chess-board/chess-board.component';
import { TournamentOverPopupComponent } from '../chess-board/tournament-over-popup/tournament-over-popup.component';

@Component({
  selector: 'app-chess-page',
  templateUrl: './chess-page.component.html',
  styleUrls: ['./chess-page.component.scss']
})
export class ChessPageComponent implements OnInit, OnDestroy, AfterViewChecked {
  matchId = '';
  matchData$: Observable<MatchFullDataDto>;
  startData = new MatchStartDto();
  orientation: Color | undefined = 'white';
  userName: string;
  potAfterBetting: number | undefined = undefined;
  history: HistoryMoveDto[] = [];
  hasOngoingDrawOffer = false;
  writtenMessage = '';
  messages: ChatMessageDto[] = [];
  tournamentPlayers: TournamentPlayerDto[] = [];
  tournamentId: number | undefined = undefined;

  @ViewChild('ownCd', { static: false })
  ownCountDown!: CountdownTimerComponent;

  @ViewChild('oppCd', { static: false })
  oppCountDown!: CountdownTimerComponent;

  moveMadeSubject: Subject<MoveDto> = new Subject<MoveDto>();
  matchEndedSubject: Subject<Result> = new Subject<Result>();
  setFenSubject: Subject<string> = new Subject<string>();
  flipBoardSubject: Subject<void> = new Subject<void>();

  currentlySelectedMove = -1;
  anyMoveMade = false;

  moveReceivedSubscription!: Subscription;
  resumeGameSubscription!: Subscription;
  drawOfferReceivedSubscription!: Subscription;
  matchEndedSubscription!: Subscription;
  chatMessageSubscription!: Subscription;
  tournamentStandingsSubscription!: Subscription;
  tournamentOverSubscription!: Subscription;

  constructor(
    private route: ActivatedRoute,
    private matchService: MatchService,
    private signalrService: SignalrService,
    private keycloak: KeycloakService,
    private dialog: MatDialog,
    private eventService: EventService,
    private cdRef: ChangeDetectorRef,
    private tournamentService: TournamentService,
    private chatService: ChatService
  ) {
    this.matchId = this.route.snapshot.params.matchId;
    this.matchData$ = this.matchService.getMatchData(this.matchId);
    this.userName = this.keycloak.getUsername();
  }

  ngOnInit(): void {
    this.signalrService.joinMatch(this.matchId);
    this.moveReceivedSubscription = this.signalrService.moveReceivedEvent.subscribe((move) => {
      this.opponentMoveMade(move);
    });

    this.resumeGameSubscription = this.eventService.resumeGameEvent.subscribe(() => {
      this.bettingOver();
    });

    this.matchService.getMatchStartingData(this.matchId).subscribe((data) => {
      this.startData = data;
    });

    this.drawOfferReceivedSubscription = this.signalrService.drawOfferReceivedEvent.subscribe(() => {
      this.drawOfferReceived();
    });

    this.matchEndedSubscription = this.signalrService.matchEndedReceivedEvent.subscribe((result) => {
      this.matchEnded(result);
    });

    this.chatMessageSubscription = this.signalrService.chatMessageReceivedEvent.subscribe(() => {
      this.getChatMessages();
    });

    this.tournamentStandingsSubscription = this.signalrService.updateStandingsEvent.subscribe(() => {
      this.getStandings();
    });

    this.tournamentOverSubscription = this.signalrService.tournamentOverEvent.subscribe((dto) => {
      this.gameOver;
      this.openTournamentOverPopup(dto);
      this.moveReceivedSubscription.unsubscribe();
    });

    this.matchData$.subscribe((data) => {
      this.tournamentId = data.tournamentId;

      this.getChatMessages();
      if (this.tournamentId) {
        this.getStandings();
      }
    });
  }

  ngOnDestroy(): void {
    this.moveReceivedSubscription.unsubscribe();
    this.resumeGameSubscription.unsubscribe();
    this.drawOfferReceivedSubscription.unsubscribe();
    this.matchEndedSubscription.unsubscribe();
    this.tournamentOverSubscription.unsubscribe();
    this.chatMessageSubscription.unsubscribe();
    this.tournamentStandingsSubscription.unsubscribe();
  }

  ngAfterViewChecked(): void {
    this.cdRef.detectChanges();
  }

  getColorFromData(data: MatchFullDataDto): Color | undefined {
    if (this.userName === data.whiteUserName) {
      this.orientation = 'white';
      return 'white';
    }
    if (this.userName === data.blackUserName) {
      this.orientation = 'black';
      return 'black';
    } else return undefined;
  }

  startBetting(data: MatchFullDataDto) {
    let color = this.getColorFromData(data);
    if (color === 'white') {
      this.ownCountDown.stop();
    } else {
      this.oppCountDown.stop();
    }
    if (color === undefined) return;
    const dialogRef = this.dialog.open(BettingPopupComponent, {
      data: {
        isCurrentPlayerStarting: color === 'white',
        startingBet: data.minimumBet,
        maximumBet: data.maximumBet,
        otherUserName: this.userName === data.whiteUserName ? data.blackUserName : data.whiteUserName,
        matchId: this.matchId
      },
      hasBackdrop: false,
      width: '350px',
      minHeight: '150px',
      position: { top: '200px', left: '50px' }
    });
    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.potAfterBetting = result;
      }
    });
  }

  ownMoveMade(move: MoveDto) {
    this.ownCountDown.pause();
    this.oppCountDown.start();
    this.hasOngoingDrawOffer = false;

    move.timeLeft = this.ownCountDown.currentTime;
    this.signalrService.sendMoveToServer(move, this.matchId);
  }

  opponentMoveMade(move: MoveDto) {
    this.moveToLast();
    this.oppCountDown.pause();
    if (move.timeLeft) {
      this.oppCountDown.currentTime = move.timeLeft;
    }
    this.ownCountDown.start();
    this.moveMadeSubject.next(move);
  }

  ownTimeRanOut() {
    this.gameOver();
    if (this.userName === this.startData.whiteUserName) {
      this.matchService
        .matchOver(this.matchId, Result.BlackWonByTimeOut, this.startData.blackUserName)
        .subscribe(() => {});
      this.matchEndedSubject.next(Result.BlackWonByTimeOut);
    } else {
      this.matchService
        .matchOver(this.matchId, Result.WhiteWonByTimeout, this.startData.whiteUserName)
        .subscribe(() => {});
      this.matchEndedSubject.next(Result.WhiteWonByTimeout);
    }
  }

  bettingOver() {
    if (this.userName === this.startData.whiteUserName) {
      this.ownCountDown.start();
    } else {
      this.oppCountDown.start();
    }
  }

  updateHistory(history: HistoryMoveDto) {
    this.history.push(history);
    this.currentlySelectedMove = this.history.length - 1;
  }

  getMoveNumberFromMove(moveNumber: number) {
    return Math.ceil(moveNumber / 2) + 1;
  }

  gameOver() {
    this.ownCountDown.stop();
    this.oppCountDown.stop();
  }

  drawOfferReceived() {
    this.hasOngoingDrawOffer = true;
  }

  draw() {
    if (this.hasOngoingDrawOffer) {
      this.gameOver();
      this.matchService.matchOver(this.matchId, Result.DrawByAgreement, '').subscribe(() => {});
      this.matchEndedSubject.next(Result.DrawByAgreement);
    } else {
      this.signalrService.sendDrawOffer(
        this.startData.whiteUserName === this.userName ? this.startData.blackUserName! : this.startData.whiteUserName!
      );
    }
  }

  resign() {
    this.gameOver();
    if (this.userName === this.startData.whiteUserName) {
      this.matchService
        .matchOver(this.matchId, Result.BlackWonByResignation, this.startData.blackUserName)
        .subscribe(() => {});
      this.matchEndedSubject.next(Result.BlackWonByResignation);
    } else {
      this.matchService
        .matchOver(this.matchId, Result.WhiteWonByResignation, this.startData.whiteUserName)
        .subscribe(() => {});
      this.matchEndedSubject.next(Result.WhiteWonByResignation);
    }
  }

  matchEnded(result: Result) {
    this.gameOver();
    this.matchEndedSubject.next(result);
  }

  moveToStart() {
    if (this.currentlySelectedMove === -1) return;
    this.currentlySelectedMove = -1;
    this.setFenSubject.next('rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR');
  }

  moveBack() {
    if (this.currentlySelectedMove === -1) return;
    this.currentlySelectedMove--;
    if (this.currentlySelectedMove === -1) {
      this.setFenSubject.next('rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR');
    } else {
      this.setFenSubject.next(this.history[this.currentlySelectedMove].fen);
    }
  }

  moveForward() {
    if (this.currentlySelectedMove === this.history.length - 1 || this.history.length === 0) return;
    this.currentlySelectedMove++;
    this.setFenSubject.next(this.history[this.currentlySelectedMove].fen);
  }

  moveToLast() {
    if (this.currentlySelectedMove === this.history.length - 1 || this.history.length === 0) return;
    this.currentlySelectedMove = this.history.length - 1;
    this.setFenSubject.next(this.history[this.history.length - 1].fen);
  }

  moveToSpecificPosition(moveIndex: number) {
    if (this.currentlySelectedMove === moveIndex) return;
    this.currentlySelectedMove = moveIndex;
    this.setFenSubject.next(this.history[moveIndex].fen);
  }

  flipBoard() {
    this.flipBoardSubject.next();
    this.orientation = this.orientation === 'white' ? 'black' : 'white';
    this.cdRef.detectChanges();
  }

  loadFullHistory(history: HistoryMoveDto[]) {
    this.history = history;
    this.currentlySelectedMove = this.history.length - 1;
  }

  getStandings() {
    this.tournamentService.getStandings(this.tournamentId!).subscribe((st) => {
      this.tournamentPlayers = st;
    });
  }

  getChatMessages() {
    this.chatService
      .getChatMessages(
        this.userName === this.startData.whiteUserName ? this.startData.blackUserName : this.startData.whiteUserName,
        50,
        0
      )
      .subscribe((res) => {
        this.messages = res;
      });
  }

  sendChatMessage() {
    this.chatService
      .sendChatMessage(
        this.userName === this.startData.whiteUserName ? this.startData.blackUserName : this.startData.whiteUserName,
        this.writtenMessage
      )
      .subscribe(() => {
        this.getChatMessages();
        this.writtenMessage = '';
      });
  }

  openTournamentOverPopup(dto: TournamentOverDto) {
    this.dialog.open(TournamentOverPopupComponent, {
      data: {
        dto: dto,
        tournamentId: this.tournamentId
      },
      width: '350px',
      backdropClass: 'cdk-overlay-transparent-backdrop'
    });
  }
}
