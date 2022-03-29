import { Component, OnInit } from '@angular/core';
import { Chessground } from 'chessground';
import { Chess, ChessInstance, Square } from 'chess.js';
import { Color, Key, MoveMetadata } from 'chessground/types';
import { Api } from 'chessground/api';
import * as signalR from "@microsoft/signalr";

@Component({
  selector: 'app-chess-board',
  templateUrl: './chess-board.component.html',
  styleUrls: ['./chess-board.component.scss']
})
export class ChessBoardComponent implements OnInit {
  ChessReq:any = require('chess.js');
  private chess:ChessInstance = new this.ChessReq();
  private cg!: Api;

  connection = new signalR.HubConnectionBuilder().withUrl("https://localhost:5000/hubs/chesshub", {
    skipNegotiation: true,
    transport: signalR.HttpTransportType.WebSockets
  }).build();

  constructor() {

  }

  ngOnInit(): void {
    this.cg = Chessground(document.getElementById("board")!, {
      turnColor: 'white',
      movable: {
        color: 'white',
        free: false,
        dests: this.getDestinations(this.chess),
      },
      draggable: {
        showGhost: true
      },
    });
    this.cg.set({
      movable: {
        events: {
          after: this.switchToOtherSide(this.cg, this.chess)
        }
      }
    });

    this.connection.start().then(() =>
    {
      console.log("SignalR connected!");
      this.connection.send("JoinMatch", "68a3c6a1-e68a-4d1b-982f-045a157ae5e6");
    });

    this.connection.on("ReceiveMove", (orig, dest) => this.moveHappened(orig, dest));
  }

  getDestinations(chess: ChessInstance): Map<Key, Key[]>{
    const dests = new Map();
    chess.SQUARES.forEach(s => {
      const moves = chess.moves({square: s});
      if (moves.length) dests.set(s, moves.map(m => this.getDestinationFromMove(m)));
    });
    return dests;
  }

  getDestinationFromMove(move: string): string
  {
    let dest = move;
    if(move.charAt(-1) === '#' || move.charAt(-1) === '!')
    {
      dest = move.slice(0, -1);  // remove # or +
    }
    dest = move.slice(move.length - 2);  // Take the last 2 characters, should be the destination
    return dest;
  }

  toColor(chess: any): Color {
    return (chess.turn() === 'w') ? 'white' : 'black';
  }

  switchToOtherSide(cg: Api, chess: ChessInstance){
    return (orig: any, dest: any, metadata: MoveMetadata) => {
      chess.move({from: orig, to: dest});
      cg.set({
        turnColor: this.toColor(chess),
      });

      this.connection.send("SendMoveToServer", orig, dest);
    };
  }

  moveHappened(orig: any, dest: any){
    this.chess.move({from: orig, to: dest})
    this.cg.move(orig, dest)
    this.cg.set({
      turnColor: this.toColor(this.chess),
      movable: {
        dests: this.getDestinations(this.chess)
      }
    });
  }

  black(){
    this.cg.set({
      movable: {
        color: 'black',
        free: false,
        dests: this.getDestinations(this.chess),
      },
      orientation: 'black'
    });
  }

  movee2e4(){
    this.chess.move({from: 'e2', to: 'e4'})
    this.cg.move('e2', 'e4')
    this.cg.set({
      turnColor: this.toColor(this.chess),
      movable: {
        dests: this.getDestinations(this.chess)
      }
    });
  }
}
