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

@NgModule({
  declarations: [HeaderComponent, LanguagePickerComponent],
  imports: [CommonModule, TranslateModule, MatOptionModule, MatSelectModule, RouterModule, FormsModule, MatInputModule],
  exports: [HeaderComponent, TranslateModule]
})
export class ComponentsModule {}
