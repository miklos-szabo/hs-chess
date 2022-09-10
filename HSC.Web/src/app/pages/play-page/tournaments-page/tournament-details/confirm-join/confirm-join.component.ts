import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-confirm-join',
  templateUrl: './confirm-join.component.html',
  styleUrls: ['./confirm-join.component.scss']
})
export class ConfirmJoinComponent implements OnInit {
  constructor(
    public dialogRef: MatDialogRef<ConfirmJoinComponent>,
    @Inject(MAT_DIALOG_DATA)
    public data: { buyIn: number }
  ) {}

  ngOnInit(): void {}

  yes() {
    this.dialogRef.close(true);
  }

  cancel() {
    this.dialogRef.close();
  }
}
