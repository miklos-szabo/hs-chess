import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { SearchTournamentDto, TournamentListDto, TournamentService } from 'src/app/api/app.generated';
import { NotificationService } from 'src/app/services/notification.service';
import { CreateTournamentPopupComponent } from './create-tournament-popup/create-tournament-popup.component';

@Component({
  selector: 'app-tournaments-page',
  templateUrl: './tournaments-page.component.html',
  styleUrls: ['./tournaments-page.component.scss']
})
export class TournamentsPageComponent implements OnInit {
  searchDto = new SearchTournamentDto();
  tournaments: TournamentListDto[] = [];

  constructor(
    private tournamentService: TournamentService,
    private dialog: MatDialog,
    private notificationService: NotificationService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.searchDto.pastTournaments = false;
    this.getTournaments();
  }

  getTournaments() {
    var newDto = new SearchTournamentDto();
    newDto.title = this.searchDto.title;
    newDto.buyInMin = this.searchDto.buyInMin;
    newDto.buyInMax = this.searchDto.buyInMax;
    newDto.pastTournaments = this.searchDto.pastTournaments;
    newDto.startDateIntervalStart = this.searchDto.startDateIntervalStart
      ? new Date(this.searchDto.startDateIntervalStart)
      : undefined;
    newDto.startDateIntervalEnd = this.searchDto.startDateIntervalEnd
      ? new Date(this.searchDto.startDateIntervalEnd)
      : undefined;

    this.tournamentService.getTournaments(newDto).subscribe((res) => {
      this.tournaments = res;
    });
  }

  createTournament() {
    const dialogRef = this.dialog.open(CreateTournamentPopupComponent, {
      width: '600px'
    });
    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.tournamentService.createTournament(result).subscribe(() => {
          this.notificationService.success('Tournaments.Created');
          this.getTournaments();
        });
      }
    });
  }

  openDetails(tournamentId: number) {
    this.router.navigateByUrl(`/tournaments/${tournamentId}`);
  }
}
