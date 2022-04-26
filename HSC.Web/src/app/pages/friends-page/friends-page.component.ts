import { Component, OnInit } from '@angular/core';
import { FriendDto, FriendRequestDto, FriendService } from 'src/app/api/app.generated';
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
  constructor(
    private friendService: FriendService,
    private signalrService: SignalrService,
    private notificationService: NotificationService,
    private eventService: EventService
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

  challenge(otherUserName: string | undefined) {}

  friendClicked(userName: string) {
    this.eventService.friendSelected(userName);
  }
}
