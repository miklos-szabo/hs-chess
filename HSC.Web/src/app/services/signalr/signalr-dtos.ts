export class MoveDto {
  origin = '';
  destination = '';
  promotion = '';
  timeLeft? = 0;
}

export class ChatMessageDto {
  timeStamp = '';
  senderUserName = '';
  message = '';
}
