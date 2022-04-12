import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Color } from 'chessground/types';
import { Observable } from 'rxjs';
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
  orientation: Color | undefined = 'white';

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
      this.orientation = 'white';
      return 'white';
    }
    if (userName === data.blackUserName) {
      this.orientation = 'black';
      return 'black';
    } else return undefined;
  }
}
