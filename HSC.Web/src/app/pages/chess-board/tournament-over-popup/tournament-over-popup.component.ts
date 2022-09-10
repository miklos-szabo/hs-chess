import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { TournamentOverDto } from 'src/app/services/signalr/signalr-dtos';

@Component({
  selector: 'app-tournament-over-popup',
  templateUrl: './tournament-over-popup.component.html',
  styleUrls: ['./tournament-over-popup.component.scss']
})
export class TournamentOverPopupComponent implements OnInit {
  constructor(
    public dialogRef: MatDialogRef<TournamentOverPopupComponent>,
    @Inject(MAT_DIALOG_DATA)
    public data: { dto: TournamentOverDto; tournamentId: number },
    private router: Router
  ) {}

  ngOnInit(): void {}

  backToTournament() {
    this.router.navigateByUrl(`/tournaments/${this.data.tournamentId}`);
  }
}
