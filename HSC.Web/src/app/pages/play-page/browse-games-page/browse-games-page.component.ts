import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { CustomGameDto, MatchFinderService } from 'src/app/api/app.generated';
import { SignalrService } from 'src/app/services/signalr/signalr.service';

@Component({
  selector: 'app-browse-games-page',
  templateUrl: './browse-games-page.component.html',
  styleUrls: ['./browse-games-page.component.scss']
})
export class BrowseGamesPageComponent implements OnInit {
  challenges: CustomGameDto[] = [];
  joinGameSubscription!: Subscription;

  constructor(
    private matchFinderService: MatchFinderService,
    private singlarService: SignalrService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.matchFinderService.getCustomGames().subscribe((games) => {
      this.challenges = games;
    });
  }

  joinGame(challengeId: number) {
    this.joinGameSubscription = this.singlarService.matchFoundEvent.subscribe((matchId) => {
      this.joinGameSubscription.unsubscribe();
      this.router.navigateByUrl(`/chess/${matchId}`);
    });
    this.matchFinderService.joinCustomGame(challengeId).subscribe(() => {});
  }
}
