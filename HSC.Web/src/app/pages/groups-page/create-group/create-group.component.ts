import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-create-group',
  templateUrl: './create-group.component.html',
  styleUrls: ['./create-group.component.scss']
})
export class CreateGroupComponent implements OnInit {
  nameField = '';
  descriptionField = '';
  constructor(public dialogRef: MatDialogRef<CreateGroupComponent>) {}

  ngOnInit(): void {}

  createGroup() {
    this.dialogRef.close({ name: this.nameField, description: this.descriptionField });
  }
}
