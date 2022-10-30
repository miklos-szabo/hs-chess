import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { NotificationService } from '../notification.service';

@Injectable()
export class DefaultInterceptor implements HttpInterceptor {
  constructor(private notificationService: NotificationService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      tap(
        (response) => {},
        (exception) => {
          if (exception instanceof HttpErrorResponse) {
            const reader = new FileReader();

            reader.onload = () => {
              const errorText = reader.result as string;
              this.notificationService.error(errorText);
            };

            reader.readAsText(exception.error);
          }
        }
      )
    );
  }
}
