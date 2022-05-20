import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ThemeService {
  private _darkTheme = new Subject<boolean>();
  isDarkTheme = this._darkTheme.asObservable();

  isCurrentlyDarkTheme = false;

  setDarkTheme(isDarkTheme: boolean): void {
    this.isCurrentlyDarkTheme = isDarkTheme;
    this._darkTheme.next(isDarkTheme);
  }
}
