import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { KeycloakService } from 'keycloak-angular';
import { Subject, Subscription } from 'rxjs';
import { MatchFinderService, SearchingForMatchDto } from 'src/app/api/app.generated';
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
  hasChosenTime = false;
  hasChosenBet = false;

  clearTimeSelectionSubject: Subject<void> = new Subject<void>();
  clearBetSelectionSubject: Subject<void> = new Subject<void>();

  private matchFoundSubscription!: Subscription;

  constructor(
    private matchFinderService: MatchFinderService,
    private router: Router,
    private signalrService: SignalrService,
    private keyCloak: KeycloakService
  ) {}

  ngOnInit(): void {}

  searchForMatch() {
    this.matchFoundSubscription = this.signalrService.matchFoundEvent.subscribe((dto) => {
      this.matchFound(dto);
    });
    this.searchDto.userName = this.keyCloak.getUsername();
    this.matchFinderService.searchForMatch(this.searchDto).subscribe(() => {
      this.isSearching = true;
    });
  }

  updateTimeControl(values: SelectorTwoValues) {
    this.searchDto.timeLimitMinutes = values.value1;
    this.searchDto.increment = values.value2;
    this.hasChosenTime = true;
    this.clearTimeSelectionSubject.next();
  }

  updateBetValue(values: SelectorTwoValues) {
    this.searchDto.minimumBet = values.value1;
    this.searchDto.maximumBet = values.value2;
    this.hasChosenBet = true;
    this.clearBetSelectionSubject.next();
  }

  matchFound(id: string) {
    this.matchFoundSubscription.unsubscribe();
    this.isSearching = false;
    this.router.navigateByUrl(`/chess/${id}`);
  }

  createCustomGame() {}
}
