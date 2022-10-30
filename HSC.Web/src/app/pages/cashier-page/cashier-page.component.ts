import { Component, OnInit } from '@angular/core';
import { AccountService, UserMenuDto } from 'src/app/api/app.generated';

@Component({
  selector: 'app-cashier-page',
  templateUrl: './cashier-page.component.html',
  styleUrls: ['./cashier-page.component.scss']
})
export class CashierPageComponent implements OnInit {
  userData: UserMenuDto = new UserMenuDto();
  constructor(private accountService: AccountService) {}

  ngOnInit(): void {
    this.accountService.getUserMenuData().subscribe((data) => {
      this.userData = data;
    });
  }

  add100() {
    this.accountService.addMoney(100).subscribe(() => {
      this.accountService.getUserMenuData().subscribe((data) => {
        this.userData = data;
      });
    });
  }
}
