import { Injectable } from '@angular/core';
import { LocalStorageService } from '../local-storage.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private userName = '';
  constructor(private localStorageService: LocalStorageService) {}

  public getUserName(): string {
    if (this.userName === '') this.userName = this.localStorageService.getUserName() ?? '';
    return this.userName;
  }

  public setUserName(name: string) {
    this.localStorageService.setUserName(name);
  }
}
