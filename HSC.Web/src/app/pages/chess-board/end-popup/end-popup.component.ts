import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Color } from 'chessground/types';
import { Result } from 'src/app/api/app.generated';

@Component({
  selector: 'app-end-popup',
  templateUrl: './end-popup.component.html',
  styleUrls: ['./end-popup.component.scss']
})
export class EndPopupComponent implements OnInit {
  constructor(
    public dialogRef: MatDialogRef<EndPopupComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { result: Result; color: Color | undefined }
  ) {}

  ngOnInit(): void {}

  getTitle() {
    if (
      this.data.result === Result.BlackWonByCheckmate ||
      this.data.result === Result.BlackWonByResignation ||
      this.data.result == Result.BlackWonByTimeOut
    ) {
      return this.data.color === 'white' ? 'EndPopup.Defeat' : 'EndPopup.Victory';
    } else if (
      this.data.result === Result.WhiteWonByCheckmate ||
      this.data.result === Result.WhiteWonByResignation ||
      this.data.result == Result.WhiteWonByTimeout
    ) {
      return this.data.color === 'white' ? 'EndPopup.Victory' : 'EndPopup.Defeat';
    } else return 'EndPopup.Stalemate';
  }
}
