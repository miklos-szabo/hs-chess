import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { Color } from 'chessground/types';
import { KeycloakService } from 'keycloak-angular';
import { Observable } from 'rxjs';
import { MatchFullDataDto, MatchService, MatchStartDto } from 'src/app/api/app.generated';
import { EventService } from 'src/app/services/event.service';
import { SignalrService } from 'src/app/services/signalr/signalr.service';
import { BettingPopupComponent } from '../chess-board/betting-popup/betting-popup.component';

@Component({
  selector: 'app-chess-page',
  templateUrl: './chess-page.component.html',
  styleUrls: ['./chess-page.component.scss']
})
export class ChessPageComponent implements OnInit {
  matchId = '';
  matchData$: Observable<MatchStartDto>;
  orientation: Color | undefined = 'white';
  userName: string;
  matchFullData = new MatchFullDataDto();

  constructor(
    private route: ActivatedRoute,
    private matchService: MatchService,
    private signalrService: SignalrService,
    private keycloak: KeycloakService,
    private dialog: MatDialog,
    private eventService: EventService
  ) {
    this.matchId = this.route.snapshot.params.matchId;
    this.matchData$ = this.matchService.getMatchStartingData(this.matchId);
    this.userName = this.keycloak.getUsername();
  }

  ngOnInit(): void {
    this.signalrService.joinMatch(this.matchId);
    this.getMatchFullData();
  }

  getColorFromData(data: MatchStartDto): Color | undefined {
    if (this.userName === data.whiteUserName) {
      this.orientation = 'white';
      return 'white';
    }
    if (this.userName === data.blackUserName) {
      this.orientation = 'black';
      return 'black';
    } else return undefined;
  }

  startBetting(data: MatchStartDto) {
    let color = this.getColorFromData(data);
    if (color === undefined) return;
    const dialogRef = this.dialog.open(BettingPopupComponent, {
      data: {
        isCurrentPlayerStarting: color === 'white',
        startingBet: this.matchFullData.minimumBet,
        maximumBet: this.matchFullData.maximumBet,
        otherUserName: this.userName === data.whiteUserName ? data.blackUserName : data.whiteUserName,
        matchId: this.matchId
      },
      hasBackdrop: false,
      width: '350px',
      minHeight: '150px',
      position: { top: '200px', left: '50px' }
    });
    dialogRef.afterClosed().subscribe((result) => {
      this.eventService.resumeGame();
      this.getMatchFullData();
    });
  }

  getMatchFullData() {
    this.matchService.getMatchData(this.matchId).subscribe((data) => {
      this.matchFullData = data;
    });
  }
}
