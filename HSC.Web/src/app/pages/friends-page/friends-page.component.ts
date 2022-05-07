import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { FriendDto, FriendRequestDto, FriendService, MatchFinderService } from 'src/app/api/app.generated';
import { CreateCustomPopupComponent } from 'src/app/components/create-custom-popup/create-custom-popup.component';
import { EventService } from 'src/app/services/event.service';
import { NotificationService } from 'src/app/services/notification.service';
import { SignalrService } from 'src/app/services/signalr/signalr.service';

@Component({
  selector: 'app-friends-page',
  templateUrl: './friends-page.component.html',
  styleUrls: ['./friends-page.component.scss']
})
export class FriendsPageComponent implements OnInit {
  addFriendField = '';

  friendRequests: FriendRequestDto[] = [];
  friends: FriendDto[] = [];

  matchFoundSubscription!: Subscription;
  constructor(
    private friendService: FriendService,
    private signalrService: SignalrService,
    private notificationService: NotificationService,
    private eventService: EventService,
    private dialog: MatDialog,
    private matchFinderService: MatchFinderService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.getFriendRequests();
    this.getFriends();

    this.signalrService.friendRequestReceivedEvent.subscribe(() => this.getFriendRequests());
  }

  getFriendRequests() {
    this.friendService.getFriendRequests().subscribe((frequests) => {
      this.friendRequests = frequests.filter((r) => r.isIncoming);
    });
  }

  getFriends() {
    this.friendService.getFriends().subscribe((friends) => {
      this.friends = friends;
    });
  }

  acceptFriendRequest(requestId: number) {
    this.friendService.acceptFriendRequest(requestId).subscribe(() => {
      this.getFriendRequests();
      this.getFriends();
    });
  }

  declineFriendRequest(requestId: number) {
    this.friendService.declineFriendRequest(requestId).subscribe(() => {
      this.getFriendRequests();
    });
  }

  sendFriendRequest() {
    this.friendService.sendFriendRequest(this.addFriendField).subscribe(
      () => {
        this.notificationService.success('Friends.RequestSent.Success');
      },
      () => {
        this.notificationService.error('Friends.RequestSent.Error');
      }
    );
  }

  challenge(otherUserName: string | undefined) {
    const dialogRef = this.dialog.open(CreateCustomPopupComponent, {});
    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        const dto = result;
        dto.userName = otherUserName;
        this.matchFinderService.createCustomGame(dto).subscribe(() => {
          this.matchFoundSubscription = this.signalrService.matchFoundEvent.subscribe((dto) => {
            this.matchFoundSubscription.unsubscribe();
            this.router.navigateByUrl(`/chess/${dto}`);
          });
          this.notificationService.success('Friends.ChallangeSent');
        });
      }
    });
  }

  friendClicked(userName: string) {
    this.eventService.friendSelected(userName);
  }
}
