import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TranslateService } from '@ngx-translate/core';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  constructor(private snackBar: MatSnackBar, private translateService: TranslateService) {}

  error(message: string) {
    this.open(this.translateService.instant(message), 'error-style');
  }

  warning(message: string) {
    this.open(this.translateService.instant(message), 'warning-style');
  }

  success(message: string) {
    this.open(this.translateService.instant(message), 'success-style');
  }

  info(message: string) {
    this.open(this.translateService.instant(message), 'info-style');
  }

  private open(message: string, style: string) {
    this.snackBar.open(message, this.translateService.instant('Notification.Dismiss'), {
      duration: 3000,
      horizontalPosition: 'right',
      verticalPosition: 'top',
      panelClass: [style]
    });
  }
}
