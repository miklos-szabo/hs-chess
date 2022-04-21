import { APP_INITIALIZER, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ChessBoardComponent } from './pages/chess-board/chess-board.component';
import { PromotionPickerComponent } from './pages/chess-board/promotion-picker/promotion-picker.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatDialogModule } from '@angular/material/dialog';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { ComponentsModule } from './components/components.module';
import { PlayPageComponent } from './pages/play-page/play-page.component';
import { QuickMatchPageComponent } from './pages/play-page/quick-match-page/quick-match-page.component';
import { TournamentsPageComponent } from './pages/play-page/tournaments-page/tournaments-page.component';
import { BrowseGamesPageComponent } from './pages/play-page/browse-games-page/browse-games-page.component';
import { HistoryPageComponent } from './pages/history-page/history-page.component';
import { GroupsPageComponent } from './pages/groups-page/groups-page.component';
import { FriendsPageComponent } from './pages/friends-page/friends-page.component';
import { ChessPageComponent } from './pages/chess-page/chess-page.component';
import { MatTabsModule } from '@angular/material/tabs';
import { MatInputModule } from '@angular/material/input';
import { FormsModule } from '@angular/forms';
import { DefaultInterceptor } from './services/interceptors/default-interceptor';
import { TimeBetSelectorComponent } from './pages/play-page/quick-match-page/time-bet-selector/time-bet-selector.component';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { EndPopupComponent } from './pages/chess-board/end-popup/end-popup.component';
import { KeycloakAngularModule, KeycloakService } from 'keycloak-angular';
import { WelcomeComponent } from './pages/welcome/welcome.component';

export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http);
}

function initializeKeycloak(keycloak: KeycloakService) {
  return () =>
    keycloak.init({
      config: {
        url: 'http://localhost:8080/auth',
        realm: 'chess',
        clientId: 'hsc-dev'
      },
      initOptions: {
        onLoad: 'check-sso',
        silentCheckSsoRedirectUri: window.location.origin + '/assets/silent-check-sso.html'
      }
    });
}

@NgModule({
  declarations: [
    AppComponent,
    ChessBoardComponent,
    PromotionPickerComponent,
    PlayPageComponent,
    QuickMatchPageComponent,
    TournamentsPageComponent,
    BrowseGamesPageComponent,
    HistoryPageComponent,
    GroupsPageComponent,
    FriendsPageComponent,
    ChessPageComponent,
    TimeBetSelectorComponent,
    EndPopupComponent,
    WelcomeComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatDialogModule,
    HttpClientModule,
    MatInputModule,
    MatTabsModule,
    ComponentsModule,
    FormsModule,
    MatProgressSpinnerModule,
    KeycloakAngularModule,

    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      },
      defaultLanguage: 'en'
    })
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: DefaultInterceptor, multi: true },
    {
      provide: APP_INITIALIZER,
      useFactory: initializeKeycloak,
      multi: true,
      deps: [KeycloakService]
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
