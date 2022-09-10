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

export class ChallengeDto {
  id!: number;
  userName = '';
}

export class TournamentOverDto {
  winner = '';
  winnings = 0;
}
