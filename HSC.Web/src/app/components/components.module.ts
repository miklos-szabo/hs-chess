import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './header/header.component';
import { TranslateModule } from '@ngx-translate/core';
import { LanguagePickerComponent } from './language-picker/language-picker.component';
import { MatOptionModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatMenuModule } from '@angular/material/menu';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatIconModule } from '@angular/material/icon';
import { MenuComponent } from './header/menu/menu.component';
import { MatButtonModule } from '@angular/material/button';
import { CountdownTimerComponent } from './countdown-timer/countdown-timer.component';

@NgModule({
  declarations: [HeaderComponent, LanguagePickerComponent, MenuComponent, CountdownTimerComponent],
  imports: [
    CommonModule,
    TranslateModule,
    MatOptionModule,
    MatSelectModule,
    RouterModule,
    FormsModule,
    MatInputModule,
    MatMenuModule,
    MatSlideToggleModule,
    MatIconModule,
    MatButtonModule
  ],
  exports: [HeaderComponent, TranslateModule, CountdownTimerComponent]
})
export class ComponentsModule {}
