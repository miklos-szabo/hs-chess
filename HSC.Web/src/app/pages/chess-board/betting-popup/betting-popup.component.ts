import { Component, Inject, Input, OnDestroy, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { TranslateService } from '@ngx-translate/core';
import { BettingService } from 'src/app/api/app.generated';
import { EventService } from 'src/app/services/event.service';
import { SignalrService } from 'src/app/services/signalr/signalr.service';

@Component({
  selector: 'app-betting-popup',
  templateUrl: './betting-popup.component.html',
  styleUrls: ['./betting-popup.component.scss']
})
export class BettingPopupComponent implements OnInit, OnDestroy {
  currentBet: number;
  ownLastBet: number;
  isCurrentPlayersTurn: boolean;
  sliderStep: number;
  sliderValue: number;
  otherPlayersAction = '';
  isBettingOver = false;
  constructor(
    public dialogRef: MatDialogRef<BettingPopupComponent>,
    @Inject(MAT_DIALOG_DATA)
    public data: {
      isCurrentPlayerStarting: boolean;
      startingBet: number;
      maximumBet: number;
      otherUserName: string;
      matchId: string;
    },
    private signalrService: SignalrService,
    private bettingService: BettingService,
    private translateService: TranslateService,
    private eventService: EventService
  ) {
    this.currentBet = data.startingBet;
    this.ownLastBet = data.startingBet;
    this.sliderValue = data.startingBet;
    this.isCurrentPlayersTurn = data.isCurrentPlayerStarting;
    if (data.maximumBet > 1000) this.sliderStep = 10;
    else if (data.maximumBet > 100) this.sliderStep = 1;
    else if (data.maximumBet > 10) this.sliderStep = 0.1;
    else this.sliderStep = 0.01;
  }

  ngOnInit(): void {
    this.signalrService.checkReceivedEvent.subscribe(() => {
      this.receiveCheck();
    });
    this.signalrService.callReceivedEvent.subscribe(() => {
      this.receiveCall();
    });
    this.signalrService.foldReceivedEvent.subscribe((amount) => {
      this.receiveFold(amount);
    });
    this.signalrService.betReceivedEvent.subscribe((amount) => {
      this.receiveRaise(amount);
    });
  }

  ngOnDestroy(): void {
    this.signalrService.checkReceivedEvent.unsubscribe();
    this.signalrService.callReceivedEvent.unsubscribe();
    this.signalrService.foldReceivedEvent.unsubscribe();
    this.signalrService.betReceivedEvent.unsubscribe();
  }

  receiveCheck() {
    this.otherPlayersAction = this.translateService.instant('Betting.OtherPlayerChecked', {
      userName: this.data.otherUserName
    });
    this.isCurrentPlayersTurn = true;
    if (this.data.isCurrentPlayerStarting) {
      this.isBettingOver = true;
      this.eventService.resumeGame();
    }
  }

  receiveCall() {
    this.otherPlayersAction = this.translateService.instant('Betting.OtherPlayerCalled', {
      userName: this.data.otherUserName,
      amount: this.currentBet
    });
    this.isCurrentPlayersTurn = true;
    this.isBettingOver = true;
    this.eventService.resumeGame();
  }

  receiveRaise(amount: number) {
    this.otherPlayersAction = this.translateService.instant('Betting.OtherPlayerRaised', {
      userName: this.data.otherUserName,
      amount: amount
    });
    this.currentBet = amount;
    this.isCurrentPlayersTurn = true;
  }

  receiveFold(amount: number) {
    this.otherPlayersAction = this.translateService.instant('Betting.OtherPlayerFolded', {
      userName: this.data.otherUserName,
      amount: amount
    });
    this.currentBet = amount;
    this.isCurrentPlayersTurn = true;
    this.isBettingOver = true;
    this.eventService.resumeGame();
  }

  check() {
    this.otherPlayersAction = '';
    this.isCurrentPlayersTurn = false;
    this.bettingService.check(this.data.matchId).subscribe(() => {});
    if (!this.data.isCurrentPlayerStarting) {
      // This is the second check, betting is over
      this.eventService.resumeGame();
      this.closeWindow();
    }
  }

  call() {
    this.otherPlayersAction = '';
    this.isCurrentPlayersTurn = false;
    this.ownLastBet = this.currentBet;
    this.bettingService.callAsnyc(this.data.matchId).subscribe(() => {});
    this.eventService.resumeGame();
    this.closeWindow();
  }

  raise() {
    this.otherPlayersAction = '';
    this.isCurrentPlayersTurn = false;
    this.currentBet = this.sliderValue;
    this.ownLastBet = this.sliderValue;
    this.bettingService.raise(this.data.matchId, this.sliderValue).subscribe(() => {});
  }

  fold() {
    this.bettingService.fold(this.data.matchId).subscribe(() => {});
    this.eventService.resumeGame();
    this.closeWindow();
  }

  closeWindow() {
    this.dialogRef.close(this.currentBet);
  }
}
