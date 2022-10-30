import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
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

  infoWithAction(message: string, actionTitle: string, action: Function) {
    let snackbarRef = this.snackBar.open(message, actionTitle, {
      duration: 6000,
      horizontalPosition: 'right',
      verticalPosition: 'top',
      panelClass: ['info-style']
    });

    snackbarRef.onAction().subscribe(() => {
      action();
    });
  }

  private open(message: string, style: string) {
    let snackbarRef = this.snackBar.open(message, this.translateService.instant('Notification.Dismiss'), {
      duration: 3000,
      horizontalPosition: 'right',
      verticalPosition: 'top',
      panelClass: [style]
    });
  }
}
