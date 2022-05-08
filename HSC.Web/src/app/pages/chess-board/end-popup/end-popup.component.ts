import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Color } from 'chessground/types';
import { MatchFullDataDto, MatchService, Result } from 'src/app/api/app.generated';

@Component({
  selector: 'app-end-popup',
  templateUrl: './end-popup.component.html',
  styleUrls: ['./end-popup.component.scss']
})
export class EndPopupComponent implements OnInit {
  matchData = new MatchFullDataDto();
  simpleResult!: SimpleResult;

  SimpleResult = SimpleResult;

  constructor(
    public dialogRef: MatDialogRef<EndPopupComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { result: Result; color: Color | undefined; matchId: string },
    private matchService: MatchService
  ) {}

  ngOnInit(): void {
    this.matchService.getMatchData(this.data.matchId).subscribe((data) => {
      this.matchData = data;
      this.setSimpleResult();
    });
  }

  setSimpleResult() {
    if (
      this.data.result === Result.BlackWonByCheckmate ||
      this.data.result === Result.BlackWonByResignation ||
      this.data.result == Result.BlackWonByTimeOut
    ) {
      this.simpleResult = this.data.color === 'black' ? SimpleResult.Victory : SimpleResult.Defeat;
    } else if (
      this.data.result === Result.WhiteWonByCheckmate ||
      this.data.result === Result.WhiteWonByResignation ||
      this.data.result == Result.WhiteWonByTimeout
    ) {
      this.simpleResult = this.data.color === 'white' ? SimpleResult.Victory : SimpleResult.Defeat;
    } else this.simpleResult = SimpleResult.Draw;
  }

  getTitle(): string {
    switch (this.simpleResult) {
      case SimpleResult.Victory:
        return 'EndPopup.Victory';
      case SimpleResult.Defeat:
        return 'EndPopup.Defeat';
      case SimpleResult.Draw:
        return 'EndPopup.Stalemate';
    }
  }
}

enum SimpleResult {
  Victory,
  Defeat,
  Draw
}
