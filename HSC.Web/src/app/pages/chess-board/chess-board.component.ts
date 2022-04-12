import { Component, Input, OnInit } from '@angular/core';
import { Chessground } from 'chessground';
import { ChessInstance, Move } from 'chess.js';
import { Color, Key, PiecesDiff, Role } from 'chessground/types';
import { Api } from 'chessground/api';
import { MatDialog } from '@angular/material/dialog';
import { PromotionPickerComponent } from './promotion-picker/promotion-picker.component';
import { SignalrService } from 'src/app/services/signalr/signalr.service';
import { MatchService, MatchStartDto, Result } from 'src/app/api/app.generated';
import { EndPopupComponent } from './end-popup/end-popup.component';

@Component({
  selector: 'app-chess-board',
  templateUrl: './chess-board.component.html',
  styleUrls: ['./chess-board.component.scss']
})
export class ChessBoardComponent implements OnInit {
  ChessReq: any = require('chess.js');
  private chess: ChessInstance = new this.ChessReq();
  private cg!: Api;

  @Input()
  matchId = '';

  @Input()
  color: Color | undefined = undefined;

  @Input()
  matchData: MatchStartDto = new MatchStartDto();

  constructor(private dialog: MatDialog, private signalrService: SignalrService, private matchService: MatchService) {}

  ngOnInit(): void {
    this.cg = Chessground(document.getElementById('board')!, {
      turnColor: 'white',
      orientation: this.color,
      movable: {
        color: this.color,
        free: false,
        dests: this.getDestinations(this.chess)
      },
      draggable: {
        showGhost: true
      }
    });
    this.cg.set({
      movable: {
        events: {
          after: this.thisPlayerMoved()
        }
      }
    });

    this.signalrService.moveReceivedEvent.subscribe((move) => {
      this.otherPlayerMoved(move.origin, move.destination, move.promotion);
    });
  }

  getDestinations(chess: ChessInstance): Map<Key, Key[]> {
    const dests = new Map();
    chess.SQUARES.forEach((s) => {
      const moves = chess.moves({ square: s, verbose: true });
      if (moves.length)
        dests.set(
          s,
          moves.map((m) => m.to)
        );
    });
    return dests;
  }

  thisPlayerMoved() {
    return (orig: any, dest: any) => {
      this.checkPromotionAndSendMove(orig, dest);
    };
  }

  otherPlayerMoved(orig: any, dest: any, promotion: string) {
    if (promotion) {
      this.makeOtherPlayersPromotionMove(orig, dest, promotion);
    } else {
      this.makeOtherPlayersMove(orig, dest);
    }
  }

  holyHell(m: Move) {
    let pawnToTake: any = m?.to.charAt(0)! + m?.from.charAt(1);
    this.cg.setPieces(new Map([[pawnToTake, undefined]]));
  }

  checkPromotionAndSendMove(orig: any, dest: any) {
    if (
      ((orig.charAt(1) === '2' && dest.charAt(1) === '1') || (orig.charAt(1) === '7' && dest.charAt(1) === '8')) &&
      this.chess.get(orig)?.type === 'p'
    ) {
      // Promotion
      const dialogRef = this.dialog.open(PromotionPickerComponent, {
        data: {
          color: this.toGuiColor(this.chess)
        }
      });

      dialogRef.afterClosed().subscribe((result) => {
        this.makeAndSendPromotionMove(orig, dest, result.piece);
      });
    } else {
      this.makeAndSendMove(orig, dest);
    }
  }

  makeAndSendMove(orig: any, dest: any) {
    let moveResult = this.chess.move({ from: orig, to: dest });
    this.cg.set({
      turnColor: this.toGuiColor(this.chess)
    });
    if (moveResult?.flags === 'e') this.holyHell(moveResult);

    this.signalrService.sendMoveToServer({ origin: orig, destination: dest, promotion: '' }, this.matchId);

    if (this.chess.game_over()) {
      const result = this.getGameOverReason();
      this.openEndPopup(result);
      this.matchService.matchOver(this.matchId, result, this.getUserNameOfNextPlayer());
      this.cg.stop();
    }
  }

  makeAndSendPromotionMove(orig: any, dest: any, promotion: any) {
    let moveResult = this.chess.move({
      from: orig,
      to: dest,
      promotion: promotion
    });
    let x: PiecesDiff;
    this.cg.setPieces(
      new Map([[dest, { color: this.toOtherGuiColor(this.chess), role: this.toGuiPiece(promotion), promoted: true }]])
    );
    this.cg.set({
      turnColor: this.toGuiColor(this.chess)
    });
    if (moveResult?.flags === 'e') this.holyHell(moveResult);

    this.signalrService.sendMoveToServer({ origin: orig, destination: dest, promotion: promotion }, this.matchId);

    if (this.chess.game_over()) {
      const result = this.getGameOverReason();
      this.openEndPopup(result);
      this.matchService.matchOver(this.matchId, result, this.getUserNameOfNextPlayer());
      this.cg.stop();
    }
  }

  makeOtherPlayersMove(orig: any, dest: any) {
    let m = this.chess.move({ from: orig, to: dest });
    this.cg.move(orig, dest);
    this.cg.set({
      turnColor: this.toGuiColor(this.chess),
      movable: {
        dests: this.getDestinations(this.chess)
      }
    });
    if (m?.flags === 'e') this.holyHell(m);

    if (this.chess.game_over()) {
      const result = this.getGameOverReason();
      this.openEndPopup(result);
      this.cg.stop();
    }
  }

  makeOtherPlayersPromotionMove(orig: any, dest: any, promotion: any) {
    let m = this.chess.move({ from: orig, to: dest, promotion: promotion });
    this.cg.move(orig, dest);
    this.cg.setPieces(
      new Map([[dest, { color: this.toOtherGuiColor(this.chess), role: this.toGuiPiece(promotion), promoted: true }]])
    );
    this.cg.set({
      turnColor: this.toGuiColor(this.chess),
      movable: {
        dests: this.getDestinations(this.chess)
      }
    });
    if (m?.flags === 'e') this.holyHell(m);

    if (this.chess.game_over()) {
      const result = this.getGameOverReason();
      this.openEndPopup(result);
      this.cg.stop();
    }
  }

  toGuiPiece(enginePiece: any): Role {
    switch (enginePiece) {
      case 'q':
        return 'queen';
      case 'r':
        return 'rook';
      case 'b':
        return 'bishop';
      case 'n':
        return 'knight';
      case 'k':
        return 'king';
      case 'p':
        return 'pawn';
      default:
        return 'pawn';
    }
  }

  toGuiColor(chess: any): Color {
    return chess.turn() === 'w' ? 'white' : 'black';
  }

  toOtherGuiColor(chess: any): Color {
    return chess.turn() === 'w' ? 'black' : 'white';
  }

  getGameOverReason(): Result {
    let whitesTurn = this.chess.turn() === 'w';

    if (this.chess.in_checkmate()) return whitesTurn ? Result.BlackWonByCheckmate : Result.WhiteWonByCheckmate;
    else if (this.chess.insufficient_material()) return Result.DrawByInsufficientMaterial;
    else if (this.chess.in_threefold_repetition()) return Result.DrawByThreefoldRepetition;
    else return Result.DrawByStalemate;
  }

  openEndPopup(result: Result) {
    this.dialog.open(EndPopupComponent, {
      data: {
        color: this.color,
        result: result
      }
    });
  }

  getUserNameOfNextPlayer(): string | undefined {
    return this.chess.turn() === 'w' ? this.matchData.whiteUserName : this.matchData.blackUserName;
  }
}
