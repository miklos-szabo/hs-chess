import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LocalStorageService {
  constructor() {}
  userNameKey = 'USERNAME';
  boardThemeKey = 'BOARDTHEME';

  public setUserName(name: string) {
    localStorage.setItem(this.userNameKey, name);
  }

  public getUserName(): string | null {
    return localStorage.getItem(this.userNameKey);
  }

  public removeUserName() {
    localStorage.removeItem(this.userNameKey);
  }

  public setBoardTheme(theme: string) {
    localStorage.setItem(this.boardThemeKey, theme);
  }

  public getBoardTheme(): string | null {
    return localStorage.getItem(this.boardThemeKey);
  }

  public removeBoardTheme() {
    localStorage.removeItem(this.boardThemeKey);
  }

  public setItem(key: string, value: string) {
    localStorage.setItem(key, value);
  }
  public getItem(key: string) {
    return localStorage.getItem(key);
  }
  public removeItem(key: string) {
    localStorage.removeItem(key);
  }
  public clear() {
    localStorage.clear();
  }
}
