import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { CustomGameDto } from 'src/app/api/app.generated';

@Component({
  selector: 'app-create-custom-popup',
  templateUrl: './create-custom-popup.component.html',
  styleUrls: ['./create-custom-popup.component.scss']
})
export class CreateCustomPopupComponent implements OnInit {
  customGame = new CustomGameDto();
  constructor(public dialogRef: MatDialogRef<CreateCustomPopupComponent>) {}

  ngOnInit(): void {}

  finish(event: NgForm) {
    if (event.valid) {
      this.closeWindow();
    }
  }

  closeWindow() {
    this.dialogRef.close(this.customGame);
  }
}
