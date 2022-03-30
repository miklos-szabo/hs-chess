import { Component, OnInit } from '@angular/core';
import { Chessground } from 'chessground';
import { Chess, ChessInstance, Move, Piece, Square } from 'chess.js';
import { Color, Key, MoveMetadata, PiecesDiff, Role } from 'chessground/types';
import { Api } from 'chessground/api';
import * as signalR from '@microsoft/signalr';
import { MatDialog } from '@angular/material/dialog';
import { PromotionPickerComponent } from './promotion-picker/promotion-picker.component';

@Component({
  selector: 'app-chess-board',
  templateUrl: './chess-board.component.html',
  styleUrls: ['./chess-board.component.scss']
})
export class ChessBoardComponent implements OnInit {
  ChessReq: any = require('chess.js');
  private chess: ChessInstance = new this.ChessReq();
  private cg!: Api;

  connection = new signalR.HubConnectionBuilder()
    .withUrl('https://localhost:5000/hubs/chesshub', {
      skipNegotiation: true,
      transport: signalR.HttpTransportType.WebSockets
    })
    .build();

  constructor(private dialog: MatDialog) {}

  ngOnInit(): void {
    this.cg = Chessground(document.getElementById('board')!, {
      turnColor: 'white',
      movable: {
        color: 'white',
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
          after: this.thisPlayerMoved(this.cg, this.chess)
        }
      }
    });

    this.connection.start().then(() => {
      console.log('SignalR connected!');
      this.connection.send('JoinMatch', '68a3c6a1-e68a-4d1b-982f-045a157ae5e6');
    });

    this.connection.on('ReceiveMove', (orig, dest, promotion) => this.otherPlayerMoved(orig, dest, promotion));
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

  thisPlayerMoved(cg: Api, chess: ChessInstance) {
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

  black() {
    this.cg.set({
      movable: {
        color: 'black',
        free: false,
        dests: this.getDestinations(this.chess)
      },
      orientation: 'black'
    });
  }

  holyHell(m: Move) {
    let pawnToTake: any = m?.to.charAt(0)! + m?.from.charAt(1);
    this.cg.setPieces(new Map([[pawnToTake, undefined]]));
  }

  checkPromotionAndSendMove(orig: any, dest: any) {
    if (
      ((origin.charAt(1) === '2' && dest.charAt(1) === '1') || (orig.charAt(1) === '7' && dest.charAt(1) === '8')) &&
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

    this.connection.send('SendMoveToServer', orig, dest, null);
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

    this.connection.send('SendMoveToServer', orig, dest, promotion);
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
}
