import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatInput } from '@angular/material/input';
import { KeycloakService } from 'keycloak-angular';
import { ChatMessageDto, ChatService } from 'src/app/api/app.generated';
import { EventService } from 'src/app/services/event.service';
import { SignalrService } from 'src/app/services/signalr/signalr.service';

@Component({
  selector: 'app-friends-chat',
  templateUrl: './friends-chat.component.html',
  styleUrls: ['./friends-chat.component.scss']
})
export class FriendsChatComponent implements OnInit {
  selectedUserName = '';
  message = '';
  messages: ChatMessageDto[] = [];
  currentUserName = '';

  constructor(
    private eventService: EventService,
    private chatService: ChatService,
    private signalrService: SignalrService,
    private keycloakService: KeycloakService
  ) {}

  ngOnInit(): void {
    this.eventService.friendSelectedEvent.subscribe((name) => {
      this.selectedUserName = name;
      this.chatService.messagesRead(name).subscribe(() => {
        this.getChatMessages();
      });
    });

    this.signalrService.chatMessageReceivedEvent.subscribe((msg) => {
      if (msg.senderUserName === this.selectedUserName) {
        var messageDto = new ChatMessageDto();
        messageDto.message = msg.message;
        messageDto.senderUserName = msg.senderUserName;
        messageDto.timeStamp = msg.timeStamp;
        this.messages.unshift(messageDto);
      }
    });

    this.currentUserName = this.keycloakService.getUsername();
  }

  sendMessage() {
    if (this.message === '') return;
    this.chatService.sendChatMessage(this.selectedUserName, this.message).subscribe(() => {
      this.getChatMessages();
    });
    this.message = '';
  }

  getChatMessages() {
    if (this.selectedUserName) {
      this.chatService.getChatMessages(this.selectedUserName, 50, 0).subscribe((messages) => {
        this.messages = messages;
      });
    }
  }
}
