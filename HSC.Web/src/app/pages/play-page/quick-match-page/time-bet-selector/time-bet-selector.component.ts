import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Observable, Subscription } from 'rxjs';

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

  @Input()
  unit = '';

  @Input()
  separator = '-';

  private clearEventSubscription!: Subscription;
  @Input() clearEvent!: Observable<void>;

  @Output()
  selectedEvent = new EventEmitter<SelectorTwoValues>();

  selected = false;
  justSelected = false;
  constructor() {}

  ngOnInit(): void {
    this.clearEvent.subscribe(() => {
      this.clearSelected();
    });
  }

  itemClicked() {
    this.selected = true;
    this.justSelected = true;
    this.selectedEvent.emit({ value1: this.value1, value2: this.value2 });
  }

  clearSelected() {
    if (this.justSelected) {
      this.justSelected = false;
    } else {
      this.selected = false;
    }
  }
}

export class SelectorTwoValues {
  value1: number = 0;
  value2: number = 0;
}
