import { Component, OnInit } from '@angular/core';
import { MatchFinderService, SearchingForMatchDto } from 'src/app/api/app.generated';
import { UserService } from 'src/app/services/auth/user.service';

@Component({
  selector: 'app-quick-match-page',
  templateUrl: './quick-match-page.component.html',
  styleUrls: ['./quick-match-page.component.scss']
})
export class QuickMatchPageComponent implements OnInit {
  searchDto: SearchingForMatchDto = new SearchingForMatchDto();
  constructor(private matchFinderService: MatchFinderService, private userService: UserService) {}

  ngOnInit(): void {}

  searchForMatch() {
    this.searchDto.userName = this.userService.getUserName();
    this.matchFinderService.searchForMatch(this.searchDto).subscribe(() => {});
  }
}
