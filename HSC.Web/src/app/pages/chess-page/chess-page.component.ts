import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { Color } from 'chessground/types';
import { KeycloakService } from 'keycloak-angular';
import { Observable, Subject, Subscription } from 'rxjs';
import { MatchFullDataDto, MatchService, MatchStartDto, Result } from 'src/app/api/app.generated';
import { CountdownTimerComponent } from 'src/app/components/countdown-timer/countdown-timer.component';
import { EventService } from 'src/app/services/event.service';
import { MoveDto } from 'src/app/services/signalr/signalr-dtos';
import { SignalrService } from 'src/app/services/signalr/signalr.service';
import { BettingPopupComponent } from '../chess-board/betting-popup/betting-popup.component';
import { HistoryMoveDto } from '../chess-board/chess-board.component';

@Component({
  selector: 'app-chess-page',
  templateUrl: './chess-page.component.html',
  styleUrls: ['./chess-page.component.scss']
})
export class ChessPageComponent implements OnInit, OnDestroy {
  matchId = '';
  matchData$: Observable<MatchFullDataDto>;
  startData = new MatchStartDto();
  orientation: Color | undefined = 'white';
  userName: string;
  potAfterBetting: number | undefined = undefined;
  history: HistoryMoveDto[] = [];
  hasOngoingDrawOffer = false;

  @ViewChild('ownCd', { static: false })
  ownCountDown!: CountdownTimerComponent;

  @ViewChild('oppCd', { static: false })
  oppCountDown!: CountdownTimerComponent;

  moveMadeSubject: Subject<MoveDto> = new Subject<MoveDto>();
  matchEndedSubject: Subject<Result> = new Subject<Result>();
  setFenSubject: Subject<string> = new Subject<string>();

  initialFen = 'rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR';
  currentlySelectedMove = -1;
  anyMoveMade = false;

  moveReceivedSubscription!: Subscription;
  resumeGameSubscription!: Subscription;
  drawOfferReceivedSubscription!: Subscription;
  matchEndedSubscription!: Subscription;

  constructor(
    private route: ActivatedRoute,
    private matchService: MatchService,
    private signalrService: SignalrService,
    private keycloak: KeycloakService,
    private dialog: MatDialog,
    private eventService: EventService
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
  }

  ngOnDestroy(): void {
    this.moveReceivedSubscription.unsubscribe();
    this.resumeGameSubscription.unsubscribe();
    this.drawOfferReceivedSubscription.unsubscribe();
    this.matchEndedSubscription.unsubscribe();
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
    this.setFenSubject.next(this.initialFen);
  }

  moveBack() {
    if (this.currentlySelectedMove === -1) return;
    this.currentlySelectedMove--;
    if (this.currentlySelectedMove === -1) {
      this.setFenSubject.next(this.initialFen);
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
}
