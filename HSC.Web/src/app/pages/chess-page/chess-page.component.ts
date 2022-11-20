import { AfterViewChecked, ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { ChessInstance } from 'chess.js';
import { Color } from 'chessground/types';
import { KeycloakService } from 'keycloak-angular';
import { Observable, Subject, Subscription } from 'rxjs';
import {
  AnalysedGameDto,
  AnalysisService,
  BestMovesDto,
  ChatMessageDto,
  ChatService,
  EngineLineDto,
  GameToBeAnalysedDto,
  MatchFullDataDto,
  MatchService,
  MatchStartDto,
  Result,
  TournamentPlayerDto,
  TournamentService
} from 'src/app/api/app.generated';
import { CountdownTimerComponent } from 'src/app/components/countdown-timer/countdown-timer.component';
import { EngineService } from 'src/app/services/engine.service';
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
  ChessReq: any = require('chess.js');
  private chess: ChessInstance = new this.ChessReq();

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
  analysis: AnalysedGameDto | undefined = undefined;
  currentMoveAnalysis: BestMovesDto | undefined = undefined;
  isCurrentlyAnalysing = false;
  serverAnalysisChosen = false;
  browserAnalysisChosen = false;
  browserAnalysis: BestMovesDto | undefined = undefined;
  currentDepth = 0;

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
  setFenSubscription!: Subscription;

  constructor(
    private route: ActivatedRoute,
    private matchService: MatchService,
    private signalrService: SignalrService,
    private keycloak: KeycloakService,
    private dialog: MatDialog,
    private eventService: EventService,
    private cdRef: ChangeDetectorRef,
    private tournamentService: TournamentService,
    private chatService: ChatService,
    private analysisService: AnalysisService,
    private engineService: EngineService
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

    this.engineService.startEngine();
    this.engineService.EngineReadyevent.subscribe(() => {
      this.engineReady();
    });
    this.engineService.messageRecievedEvent.subscribe((m) => {
      this.browserStockfishMessageReceived(m);
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
    this.setAnalysisPosition(this.currentlySelectedMove);
    this.positionChangedAnalyzeBrowser();
  }

  moveBack() {
    if (this.currentlySelectedMove === -1) return;
    this.currentlySelectedMove--;
    if (this.currentlySelectedMove === -1) {
      this.setFenSubject.next('rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR');
    } else {
      this.setFenSubject.next(this.history[this.currentlySelectedMove].fen);
    }
    this.setAnalysisPosition(this.currentlySelectedMove);
    this.positionChangedAnalyzeBrowser();
  }

  moveForward() {
    if (this.currentlySelectedMove === this.history.length - 1 || this.history.length === 0) return;
    this.currentlySelectedMove++;
    this.setFenSubject.next(this.history[this.currentlySelectedMove].fen);
    this.setAnalysisPosition(this.currentlySelectedMove);
    this.positionChangedAnalyzeBrowser();
  }

  moveToLast() {
    if (this.currentlySelectedMove === this.history.length - 1 || this.history.length === 0) return;
    this.currentlySelectedMove = this.history.length - 1;
    this.setFenSubject.next(this.history[this.history.length - 1].fen);
    this.setAnalysisPosition(this.currentlySelectedMove);
    this.positionChangedAnalyzeBrowser();
  }

  moveToSpecificPosition(moveIndex: number) {
    if (this.currentlySelectedMove === moveIndex) return;
    this.currentlySelectedMove = moveIndex;
    this.setFenSubject.next(this.history[moveIndex].fen);
    this.setAnalysisPosition(this.currentlySelectedMove);
    this.positionChangedAnalyzeBrowser();
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

  getAnalysis() {
    this.serverAnalysisChosen = true;
    this.isCurrentlyAnalysing = true;
    var dto = new GameToBeAnalysedDto();
    dto.matchId = this.matchId;
    dto.fens = this.history.map((h) => h.fen);
    this.analysisService.getAnalysis(dto).subscribe((data) => {
      //this.analysis = data;
      this.analysis = this.convertAnalysisToSan(data);
      this.isCurrentlyAnalysing = false;
      this.setAnalysisPosition(this.currentlySelectedMove);
    });
  }

  setAnalysisPosition(moveIndex: number) {
    if (!this.analysis) return;
    if (moveIndex === -1) this.currentMoveAnalysis = undefined;

    this.currentMoveAnalysis = this.analysis.bestMoves?.find((m) => m.moveNumber == moveIndex);
  }

  convertAnalysisToSan(analysis: AnalysedGameDto): AnalysedGameDto {
    analysis.bestMoves?.forEach((bm) => {
      // First load the fen into the board
      this.chess.load(this.history[bm.moveNumber].fen);

      // Make the moves on the board
      this.chess.move(bm.firstBest!.move!, { sloppy: true });
      let continuationMoves = bm.firstBest!.continuation!.split(' ');
      continuationMoves.forEach((m) => {
        this.chess.move(m, { sloppy: true });
      });

      // Read the history with the SAN values
      let history = this.chess.history({ verbose: true });
      let sanHistory = history.map((h) => h.san);
      bm.firstBest!.move = sanHistory[0];
      bm.firstBest!.continuation = sanHistory.slice(1).join(' ');
      this.chess.reset();

      // Second best move
      this.chess.load(this.history[bm.moveNumber].fen);
      this.chess.move(bm.secondBest!.move!, { sloppy: true });
      continuationMoves = bm.secondBest!.continuation!.split(' ');
      continuationMoves.forEach((m) => {
        this.chess.move(m, { sloppy: true });
      });
      history = this.chess.history({ verbose: true });
      sanHistory = history.map((h) => h.san);
      bm.secondBest!.move = sanHistory[0];
      bm.secondBest!.continuation = sanHistory.slice(1).join(' ');
      this.chess.reset();

      // Third best move
      this.chess.load(this.history[bm.moveNumber].fen);
      this.chess.move(bm.thirdBest!.move!, { sloppy: true });
      continuationMoves = bm.thirdBest!.continuation!.split(' ');
      continuationMoves.forEach((m) => {
        this.chess.move(m, { sloppy: true });
      });
      history = this.chess.history({ verbose: true });
      sanHistory = history.map((h) => h.san);
      bm.thirdBest!.move = sanHistory[0];
      bm.thirdBest!.continuation = sanHistory.slice(1).join(' ');
      this.chess.reset();
    });

    return analysis;
  }

  browserAnalysisClicked() {
    this.browserAnalysisChosen = true;
    this.engineService.sendMessage('uci');
  }

  browserStockfishMessageReceived(m: string) {
    console.log(m);
    if (m.startsWith('info depth') && !m.includes('lowerbound') && !m.includes('upperbound')) {
      let lineParts = m.split(' ');
      if (+lineParts[2] < 14) return;
      console.log('working!');
      let isBlack = this.history[this.currentlySelectedMove].fen.split(' ')[1] == 'b';
      this.browserAnalysis = new BestMovesDto();
      let bestMove = new EngineLineDto();
      if (lineParts[8] == 'cp') {
        let advantage: number = +lineParts[9] / 100;
        var sidedAdvantage = isBlack ? advantage * -1 : advantage;
        bestMove.eval = sidedAdvantage.toFixed(2);
      } else {
        bestMove.eval = `M${Math.abs(+lineParts[9])}`;
      }

      bestMove.move = lineParts[17];
      bestMove.continuation = lineParts.slice(18, -2).join(' ');

      // Convert to SAN
      let analChess: ChessInstance = new this.ChessReq();
      analChess.load(this.history[this.currentlySelectedMove].fen);

      // Make the moves on the board
      analChess.move(bestMove.move, { sloppy: true });
      let continuationMoves = bestMove.continuation!.split(' ');
      continuationMoves.forEach((m) => {
        analChess.move(m, { sloppy: true });
      });

      // Read the history with the SAN values
      let history = analChess.history({ verbose: true });
      let sanHistory = history.map((h) => h.san);
      bestMove.move = sanHistory[0];
      bestMove.continuation = sanHistory.slice(1).join(' ');

      this.currentDepth = +lineParts[2];
      this.browserAnalysis.firstBest = bestMove;
    }
  }

  engineReady() {
    this.engineService.sendMessage('position fen ' + this.history[this.currentlySelectedMove].fen);
    this.engineService.sendMessage('go depth 24');
  }

  positionChangedAnalyzeBrowser() {
    if (this.browserAnalysisChosen) {
      this.engineService.sendMessage('stop');
      this.engineService.sendMessage('ucinewgame');
      this.engineService.sendMessage('position fen ' + this.history[this.currentlySelectedMove].fen);
      this.engineService.sendMessage('go depth 24');
    }
  }
}
