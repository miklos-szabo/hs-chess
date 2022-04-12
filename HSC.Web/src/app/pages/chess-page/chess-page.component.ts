import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Color } from 'chessground/types';
import { observable, Observable, of } from 'rxjs';
import { MatchService, MatchStartDto } from 'src/app/api/app.generated';
import { UserService } from 'src/app/services/auth/user.service';
import { SignalrService } from 'src/app/services/signalr/signalr.service';

@Component({
  selector: 'app-chess-page',
  templateUrl: './chess-page.component.html',
  styleUrls: ['./chess-page.component.scss']
})
export class ChessPageComponent implements OnInit {
  matchId = '';
  matchData$: Observable<MatchStartDto>;

  constructor(
    private route: ActivatedRoute,
    private matchService: MatchService,
    private userService: UserService,
    private signalrService: SignalrService
  ) {
    this.matchId = this.route.snapshot.params.matchId;
    this.matchData$ = this.matchService.getMatchStartingData(this.matchId);
  }

  ngOnInit(): void {
    this.signalrService.joinMatch(this.matchId);
  }

  getColorFromData(data: MatchStartDto): Color | undefined {
    let userName = this.userService.getUserName();
    if (userName === data.whiteUserName) {
      return 'white';
    }
    if (userName === data.blackUserName) {
      return 'black';
    } else return undefined;
  }
}
