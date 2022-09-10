import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import {
  TournamentDetailsDto,
  TournamentMessageDto,
  TournamentPlayerDto,
  TournamentService,
  TournamentStatus
} from 'src/app/api/app.generated';
import { Observable, Subscription } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { NotificationService } from 'src/app/services/notification.service';
import { TranslateService } from '@ngx-translate/core';
import { SignalrService } from 'src/app/services/signalr/signalr.service';
import { Dialog } from '@angular/cdk/dialog';
import { ConfirmJoinComponent } from './confirm-join/confirm-join.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-tournament-details',
  templateUrl: './tournament-details.component.html',
  styleUrls: ['./tournament-details.component.scss']
})
export class TournamentDetailsComponent implements OnInit, OnDestroy {
  tournamentId!: number;
  TournamentStatus = TournamentStatus;
  messages: TournamentMessageDto[] = [];
  standings: TournamentPlayerDto[] = [];
  writtenMessage = '';
  isSearching = false;

  details$: Observable<TournamentDetailsDto>;
  data = new TournamentDetailsDto();

  private matchFoundSubscription!: Subscription;
  private messageSubscription!: Subscription;
  private standingsSubscription!: Subscription;
  private tournamentStartedSubscription!: Subscription;
  private tournamentOverSubscription!: Subscription;

  constructor(
    private tournamentService: TournamentService,
    private route: ActivatedRoute,
    private notificationService: NotificationService,
    private translateService: TranslateService,
    private signalrService: SignalrService,
    private router: Router,
    private dialog: MatDialog
  ) {
    this.tournamentId = this.route.snapshot.params.tournamentId;
    this.details$ = this.tournamentService.getTournamentDetails(this.tournamentId);
  }

  ngOnInit(): void {
    this.refreshMessages();
    this.refreshUsers();
    this.reloadMainData();

    this.messageSubscription = this.signalrService.tournamentMessageReceivedEvent.subscribe(() => {
      this.refreshMessages();
    });

    this.standingsSubscription = this.signalrService.updateStandingsEvent.subscribe(() => {
      this.refreshUsers();
    });

    this.tournamentStartedSubscription = this.signalrService.tournamentStartedEvent.subscribe(() => {
      this.reloadMainData();
    });

    this.tournamentOverSubscription = this.signalrService.tournamentOverEvent.subscribe(() => {
      this.reloadMainData();
    });
  }

  ngOnDestroy(): void {
    this.messageSubscription.unsubscribe();
    this.standingsSubscription.unsubscribe();
    this.tournamentStartedSubscription.unsubscribe();
    this.tournamentOverSubscription.unsubscribe();
  }

  refreshMessages() {
    this.tournamentService.getMessages(this.tournamentId).subscribe((msgs) => {
      this.messages = msgs;
    });
  }

  refreshUsers() {
    this.tournamentService.getStandings(this.tournamentId).subscribe((st) => {
      this.standings = st;
    });
  }

  joinTournament(data: TournamentDetailsDto) {
    const dialogRef = this.dialog.open(ConfirmJoinComponent, {
      data: {
        buyIn: data.buyIn
      }
    });
    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.tournamentService.joinTournament(this.tournamentId).subscribe(() => {
          this.notificationService.success(this.translateService.instant('Tournaments.Joined'));
          this.reloadMainData();
        });
      }
    });
  }

  sendMessage() {
    this.tournamentService.sendMessage(this.tournamentId, this.writtenMessage).subscribe(() => {
      this.refreshMessages();
      this.writtenMessage = '';
    });
  }

  findMatch() {
    this.matchFoundSubscription = this.signalrService.matchFoundEvent.subscribe((dto) => {
      this.matchFound(dto);
    });
    this.tournamentService.searchForNextMatch(this.tournamentId).subscribe(() => {
      this.isSearching = true;
    });
  }

  matchFound(id: string) {
    this.matchFoundSubscription.unsubscribe();
    this.isSearching = false;
    this.router.navigateByUrl(`/chess/${id}`);
  }

  reloadMainData() {
    this.details$ = this.tournamentService.getTournamentDetails(this.tournamentId);
    this.refreshUsers();
  }
}
