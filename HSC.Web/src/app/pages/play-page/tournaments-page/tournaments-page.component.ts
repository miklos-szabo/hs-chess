import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
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
    private notificationService: NotificationService
  ) {}

  ngOnInit(): void {
    this.searchDto.pastTournaments = false;
    this.getTournaments();
  }

  getTournaments() {
    this.tournamentService.getTournaments(this.searchDto).subscribe((res) => {
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
        });
      }
    });
  }
}
