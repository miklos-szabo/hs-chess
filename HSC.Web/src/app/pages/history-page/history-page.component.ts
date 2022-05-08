import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subject, Subscription } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { HistorySearchDto, HistoryService, PastGameDto, SearchSimpleResult } from 'src/app/api/app.generated';

@Component({
  selector: 'app-history-page',
  templateUrl: './history-page.component.html',
  styleUrls: ['./history-page.component.scss']
})
export class HistoryPageComponent implements OnInit, OnDestroy {
  matches: PastGameDto[] = [];
  searchDto = new HistorySearchDto();

  private searchInputChanged: Subject<string> = new Subject<string>();
  private searchFieldSubscription!: Subscription;
  debounceTime = 500;

  choices = [SearchSimpleResult.All, SearchSimpleResult.Victory, SearchSimpleResult.Defeat, SearchSimpleResult.Draw];

  constructor(private historyService: HistoryService, private router: Router) {}

  ngOnInit(): void {
    this.searchFieldSubscription = this.searchInputChanged.pipe(debounceTime(this.debounceTime)).subscribe(() => {
      this.getPastGames();
    });
    this.getPastGames();
  }

  ngOnDestroy(): void {
    this.searchFieldSubscription.unsubscribe();
  }

  getPastGames() {
    this.historyService.getPastGames(this.searchDto, 20, 0).subscribe((games) => {
      this.matches = games;
    });
  }

  searchFieldChanged() {
    this.searchInputChanged.next();
  }

  openGame(matchId: string) {
    this.router.navigateByUrl(`/chess/${matchId}`);
  }
}
