import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ChessBoardComponent } from './pages/chess-board/chess-board.component';
import { FriendsPageComponent } from './pages/friends-page/friends-page.component';
import { GroupsPageComponent } from './pages/groups-page/groups-page.component';
import { HistoryPageComponent } from './pages/history-page/history-page.component';
import { PlayPageComponent } from './pages/play-page/play-page.component';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'play'
  },
  {
    path: 'play',
    component: PlayPageComponent
  },
  {
    path: 'history',
    component: HistoryPageComponent
  },
  {
    path: 'groups',
    component: GroupsPageComponent
  },
  {
    path: 'friends',
    component: FriendsPageComponent
  },
  {
    path: 'chess',
    component: ChessBoardComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
