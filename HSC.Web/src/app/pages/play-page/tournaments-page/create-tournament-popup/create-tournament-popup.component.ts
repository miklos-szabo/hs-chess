import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { CreateTournamentDto } from 'src/app/api/app.generated';

@Component({
  selector: 'app-create-tournament-popup',
  templateUrl: './create-tournament-popup.component.html',
  styleUrls: ['./create-tournament-popup.component.scss']
})
export class CreateTournamentPopupComponent implements OnInit {
  createTournamentDto = new CreateTournamentDto();

  constructor(public dialogRef: MatDialogRef<CreateTournamentDto>) {}

  ngOnInit(): void {}

  finish(event: NgForm) {
    if (event.valid) {
      this.closeWindow();
    }
  }

  closeWindow() {
    this.dialogRef.close(this.createTournamentDto);
  }
}
