import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ChessPageComponent } from './pages/chess-page/chess-page.component';
import { FriendsPageComponent } from './pages/friends-page/friends-page.component';
import { GroupDetailsPageComponent } from './pages/groups-page/group-details-page/group-details-page.component';
import { GroupsPageComponent } from './pages/groups-page/groups-page.component';
import { HistoryPageComponent } from './pages/history-page/history-page.component';
import { PlayPageComponent } from './pages/play-page/play-page.component';
import { TournamentDetailsComponent } from './pages/play-page/tournaments-page/tournament-details/tournament-details.component';
import { WelcomeComponent } from './pages/welcome/welcome.component';
import { AuthGuard } from './services/auth/auth-guard';

const routes: Routes = [
  {
    path: 'play',
    component: PlayPageComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'history',
    component: HistoryPageComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'groups',
    component: GroupsPageComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'groups/:groupId',
    component: GroupDetailsPageComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'friends',
    component: FriendsPageComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'chess/:matchId',
    component: ChessPageComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'tournaments/:tournamentId',
    component: TournamentDetailsComponent,
    canActivate: [AuthGuard]
  },
  {
    path: '',
    pathMatch: 'full',
    component: WelcomeComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
