import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-time-bet-selector[value1][value2]',
  templateUrl: './time-bet-selector.component.html',
  styleUrls: ['./time-bet-selector.component.scss']
})
export class TimeBetSelectorComponent implements OnInit {
  @Input()
  value1: number = 0;

  @Input()
  value2: number = 0;

  @Output()
  selectedEvent = new EventEmitter<SelectorTwoValues>();
  constructor() {}

  ngOnInit(): void {}

  itemClicked() {
    this.selectedEvent.emit({ value1: this.value1, value2: this.value2 });
  }
}

export class SelectorTwoValues {
  value1: number = 0;
  value2: number = 0;
}
