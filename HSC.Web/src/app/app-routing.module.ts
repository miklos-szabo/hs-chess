import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ChessBoardComponent } from './pages/chess-board/chess-board.component';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'chess'
  },
  {
    path: 'chess',
    component: ChessBoardComponent
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
