import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { Subject, Subscription } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { GroupDto, GroupService } from 'src/app/api/app.generated';
import { NotificationService } from 'src/app/services/notification.service';
import { CreateGroupComponent } from './create-group/create-group.component';

@Component({
  selector: 'app-groups-page',
  templateUrl: './groups-page.component.html',
  styleUrls: ['./groups-page.component.scss']
})
export class GroupsPageComponent implements OnInit, OnDestroy {
  ownGroups: GroupDto[] = [];
  otherGroups: GroupDto[] = [];
  searchField = '';

  private searchInputChanged: Subject<string> = new Subject<string>();
  private subscription!: Subscription;
  debounceTime = 500;
  constructor(
    private groupService: GroupService,
    private dialog: MatDialog,
    private notificationService: NotificationService,
    private translateService: TranslateService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.subscription = this.searchInputChanged.pipe(debounceTime(this.debounceTime)).subscribe(() => {
      this.getGroups(this.searchField);
    });

    this.getGroups('');
  }

  getGroups(field: string) {
    this.groupService.getGroups(field).subscribe((data) => {
      this.ownGroups = data.filter((g) => g.isInGroup);
      this.otherGroups = data.filter((g) => !g.isInGroup);
    });
  }

  searchFieldChanged() {
    this.searchInputChanged.next();
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  createGroupClicked() {
    const dialogRef = this.dialog.open(CreateGroupComponent, {
      width: '40%'
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.groupService.createGroup(result.name, result.description).subscribe(() => {
          this.notificationService.success(this.translateService.instant('Groups.Created'));
          this.getGroups('');
        });
      }
    });
  }

  joinGroupClicked(groupId: number) {
    this.groupService.joinGroup(groupId).subscribe(() => {
      this.notificationService.success(this.translateService.instant('Groups.Joined'));
      this.getGroups('');
    });
  }

  groupDetailsClicked(groupId: number) {
    this.router.navigateByUrl(`groups/${groupId}`);
  }
}
