import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { KeycloakService } from 'keycloak-angular';
import { FriendService, GroupDetailsDto, GroupDto, GroupService } from 'src/app/api/app.generated';
import { NotificationService } from 'src/app/services/notification.service';

@Component({
  selector: 'app-group-details-page',
  templateUrl: './group-details-page.component.html',
  styleUrls: ['./group-details-page.component.scss']
})
export class GroupDetailsPageComponent implements OnInit {
  groupId: number;
  currentUserName = '';
  data: GroupDetailsDto = new GroupDetailsDto();
  constructor(
    private route: ActivatedRoute,
    private groupService: GroupService,
    private friendService: FriendService,
    private notificationService: NotificationService,
    private router: Router,
    private translateService: TranslateService,
    private keyCloak: KeycloakService
  ) {
    this.groupId = this.route.snapshot.params.groupId;
  }

  ngOnInit(): void {
    this.groupService.getGroupDetails(this.groupId).subscribe((data) => {
      this.data = data;
    });
    this.currentUserName = this.keyCloak.getUsername();
  }

  messageButtonClicked(userName: string) {
    this.router.navigateByUrl('friends'); //TODO egyből menjen rá a beszélgetésre
  }

  addFriendButtonClicked(userName: string) {
    this.friendService.sendFriendRequest(userName).subscribe(
      () => {
        this.notificationService.success('Friends.RequestSent.Success');
      },
      () => {
        this.notificationService.error('Friends.RequestSent.Error');
      }
    );
  }

  joinGroupClicked() {
    this.groupService.joinGroup(this.groupId).subscribe(() => {
      this.notificationService.success(this.translateService.instant('Groups.Joined'));
      this.router.navigateByUrl('groups');
    });
  }

  back() {
    this.router.navigateByUrl('/groups');
  }
}
