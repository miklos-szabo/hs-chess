import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { Color } from 'chessground/types';
import { KeycloakService } from 'keycloak-angular';
import { Observable, Subject } from 'rxjs';
import { MatchFullDataDto, MatchService, MatchStartDto } from 'src/app/api/app.generated';
import { CountdownTimerComponent } from 'src/app/components/countdown-timer/countdown-timer.component';
import { EventService } from 'src/app/services/event.service';
import { MoveDto } from 'src/app/services/signalr/signalr-dtos';
import { SignalrService } from 'src/app/services/signalr/signalr.service';
import { BettingPopupComponent } from '../chess-board/betting-popup/betting-popup.component';
import { ChessBoardComponent } from '../chess-board/chess-board.component';

@Component({
  selector: 'app-chess-page',
  templateUrl: './chess-page.component.html',
  styleUrls: ['./chess-page.component.scss']
})
export class ChessPageComponent implements OnInit {
  matchId = '';
  matchData$: Observable<MatchFullDataDto>;
  startData = new MatchStartDto();
  orientation: Color | undefined = 'white';
  userName: string;
  potAfterBetting: number | undefined = undefined;
  history: string[] = [];

  @ViewChild('ownCd', { static: false })
  ownCountDown!: CountdownTimerComponent;

  @ViewChild('oppCd', { static: false })
  oppCountDown!: CountdownTimerComponent;

  moveMadeSubject: Subject<MoveDto> = new Subject<MoveDto>();
  timeRanOutSubject: Subject<boolean> = new Subject<boolean>();

  anyMoveMade = false;

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
    this.signalrService.moveReceivedEvent.subscribe((move) => {
      this.opponentMoveMade(move);
    });

    this.eventService.resumeGameEvent.subscribe(() => {
      this.bettingOver();
    });

    this.matchService.getMatchStartingData(this.matchId).subscribe((data) => {
      this.startData = data;
    });
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

    move.timeLeft = this.ownCountDown.currentTime;
    this.signalrService.sendMoveToServer(move, this.matchId);
  }

  opponentMoveMade(move: MoveDto) {
    this.oppCountDown.pause();
    if (move.timeLeft) {
      this.oppCountDown.currentTime = move.timeLeft;
    }
    this.ownCountDown.start();
    this.moveMadeSubject.next(move);
  }

  ownTimeRanOut() {
    this.timeRanOutSubject.next(false);
  }

  opponentTimeRanOut() {
    this.timeRanOutSubject.next(true);
  }

  bettingOver() {
    if (this.userName === this.startData.whiteUserName) {
      this.ownCountDown.start();
    } else {
      this.oppCountDown.start();
    }
  }

  updateHistory(history: string[]) {
    this.history = history;
  }

  getMoveNumberFromMove(moveNumber: number) {
    return Math.ceil(moveNumber / 2) + 1;
  }

  gameOver() {
    this.ownCountDown.stop();
    this.oppCountDown.stop();
  }
}
