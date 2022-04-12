import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatchFinderService, SearchingForMatchDto } from 'src/app/api/app.generated';
import { UserService } from 'src/app/services/auth/user.service';
import { SignalrService } from 'src/app/services/signalr/signalr.service';
import { SelectorTwoValues } from './time-bet-selector/time-bet-selector.component';

@Component({
  selector: 'app-quick-match-page',
  templateUrl: './quick-match-page.component.html',
  styleUrls: ['./quick-match-page.component.scss']
})
export class QuickMatchPageComponent implements OnInit {
  searchDto: SearchingForMatchDto = new SearchingForMatchDto();
  isSearching = false;

  constructor(
    private matchFinderService: MatchFinderService,
    private userService: UserService,
    private router: Router,
    private signalrService: SignalrService
  ) {}

  ngOnInit(): void {}

  searchForMatch() {
    this.signalrService.matchFoundEvent.subscribe((matchId) => {
      this.matchFound(matchId);
    });
    this.searchDto.userName = this.userService.getUserName();
    this.matchFinderService.searchForMatch(this.searchDto).subscribe(() => {
      // todo noti?
      this.isSearching = true;
    });
  }

  updateTimeControl(values: SelectorTwoValues) {
    this.searchDto.timeLimitMinutes = values.value1;
    this.searchDto.increment = values.value2;
    this.signalrService.joinMatch('1');
  }

  updateBetValue(values: SelectorTwoValues) {
    this.searchDto.minimumBet = values.value1;
    this.searchDto.maximumBet = values.value2;
  }

  matchFound(id: string) {
    this.signalrService.matchFoundEvent.unsubscribe();
    this.isSearching = false;
    this.router.navigateByUrl(`/chess/${id}`);
  }
}
