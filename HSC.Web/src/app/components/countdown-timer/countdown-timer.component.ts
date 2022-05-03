import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { Subscription, timer } from 'rxjs';

@Component({
  selector: 'app-countdown-timer',
  templateUrl: './countdown-timer.component.html',
  styleUrls: ['./countdown-timer.component.scss']
})
export class CountdownTimerComponent implements OnInit, OnDestroy {
  @Input()
  TimeMSec = 0;

  @Input()
  IncrementMSec = 0;

  @Output()
  timeRanOutEvent: EventEmitter<void> = new EventEmitter();

  timeRefreshInterval = 1000;
  timeSmallRefreshInterval = 100;

  subscription?: Subscription;
  isCounting = false;
  currentTime = 0;

  constructor() {}

  ngOnInit(): void {
    this.currentTime = this.TimeMSec;
  }

  ngOnDestroy(): void {
    this.stopTimer();
  }

  private startTimer() {
    this.subscription = timer(0, this.timeRefreshInterval).subscribe((val) => {
      this.currentTime -= this.timeRefreshInterval;
      // If the timer hits 10 seconds, change to a smaller interval
      if (this.currentTime <= 10000) {
        this.stopTimer();
        this.startPreciseTimer();
      }
    });
  }

  private startPreciseTimer() {
    this.subscription = timer(0, this.timeSmallRefreshInterval).subscribe((val) => {
      this.currentTime -= this.timeSmallRefreshInterval;
      if (this.currentTime <= 0) {
        this.stopTimer();
        this.timeRanOutEvent.emit();
        this.currentTime = 0;
      }
    });
  }

  private stopTimer() {
    this.subscription?.unsubscribe();
  }

  public start() {
    if (this.currentTime <= 10000) {
      this.startPreciseTimer();
    } else {
      this.startTimer();
    }

    this.isCounting = true;
  }

  public pause() {
    this.stopTimer();
    this.currentTime += this.IncrementMSec;
    this.isCounting = false;
  }

  public resume() {
    if (this.currentTime <= 10000) {
      this.startPreciseTimer();
    } else {
      this.startTimer();
    }
    this.isCounting = true;
  }

  public stop() {
    this.stopTimer();
    this.isCounting = false;
  }

  getReadableTime(): string {
    const hours = Math.floor(this.currentTime / 3600000);
    const minutes = Math.floor((this.currentTime - hours * 3600000) / 60000);
    const seconds = Math.floor((this.currentTime - hours * 3600000 - minutes * 60000) / 1000);
    const tenths = (this.currentTime - hours * 3600000 - minutes * 60000) % 1000;

    if (hours > 0) return `${hours}:${minutes}:${seconds.toString().padStart(2, '0')}`;
    if (minutes === 0 && seconds < 10)
      return `${minutes}:${seconds.toString().padStart(2, '0')}.${tenths.toString()[0]}`;
    return `${minutes}:${seconds.toString().padStart(2, '0')}`;
  }
}
