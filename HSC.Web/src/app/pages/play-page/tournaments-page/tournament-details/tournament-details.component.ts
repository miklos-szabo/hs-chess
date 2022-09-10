import { Component, Input, OnInit } from '@angular/core';
import {
  TournamentDetailsDto,
  TournamentMessageDto,
  TournamentPlayerDto,
  TournamentService,
  TournamentStatus
} from 'src/app/api/app.generated';
import { Observable } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { NotificationService } from 'src/app/services/notification.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-tournament-details',
  templateUrl: './tournament-details.component.html',
  styleUrls: ['./tournament-details.component.scss']
})
export class TournamentDetailsComponent implements OnInit {
  tournamentId!: number;
  TournamentStatus = TournamentStatus;
  messages: TournamentMessageDto[] = [];
  standings: TournamentPlayerDto[] = [];
  writtenMessage = '';
  isSearching = false;

  details$: Observable<TournamentDetailsDto>;

  constructor(
    private tournamentService: TournamentService,
    private route: ActivatedRoute,
    private notificationService: NotificationService,
    private translateService: TranslateService
  ) {
    this.tournamentId = this.route.snapshot.params.matchId;
    this.details$ = this.tournamentService.getTournamentDetails(this.tournamentId);
  }

  ngOnInit(): void {
    this.refreshMessages();
    this.refreshUsers();
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

  joinTournament() {
    this.tournamentService.joinTournament(this.tournamentId).subscribe(() => {
      this.notificationService.success(this.translateService.instant('Tournaments.Joined'));
      window.location.reload();
    });
  }

  sendMessage() {
    this.tournamentService.sendMessage(this.tournamentId, this.writtenMessage).subscribe(() => {
      this.refreshMessages();
    });
  }

  findMatch() {
    this.tournamentService.searchForNextMatch(this.tournamentId).subscribe(() => {
      this.isSearching = true;
    });
  }
}
