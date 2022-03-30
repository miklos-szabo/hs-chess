import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Color } from 'chessground/types';

@Component({
  selector: 'app-promotion-picker',
  templateUrl: './promotion-picker.component.html',
  styleUrls: ['./promotion-picker.component.scss']
})
export class PromotionPickerComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<PromotionPickerComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { color: Color },
  ) {}

  ngOnInit(): void {
  }

  qClicked() {
    this.dialogRef.close({piece: 'q'});
  }

  rClicked() {
    this.dialogRef.close({piece: 'r'});
  }

  bClicked() {
    this.dialogRef.close({piece: 'b'});
  }

  nClicked() {
    this.dialogRef.close({piece: 'n'});
  }

}
