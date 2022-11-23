using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSC.Mobile.Handlers;
using HSCApi;
using Microsoft.AspNetCore.SignalR.Client;

namespace HSC.Mobile.Services
{
    public class SignalrService
    {
        private HubConnection _hubConnection;
        private readonly AuthService _authService;

        public event EventHandler<MoveDto> MoveReceivedEvent;
        public event EventHandler<Guid> MatchFoundEvent;
        public event EventHandler<ChallengeDto> ChallengeFoundEvent;
        public event EventHandler<decimal> FoldReceivedEvent;
        public event EventHandler CheckReceivedEvent;
        public event EventHandler CallReceivedEvent;
        public event EventHandler<decimal> BetReceivedEvent;
        public event EventHandler<string> FriendRequestReceivedEvent;
        public event EventHandler<ChatMessageDto> ChatMessageReceivedEvent;
        public event EventHandler DrawOfferReceivedEvent;
        public event EventHandler<Result> MatchEndedReceivedEvent;
        public event EventHandler<TournamentOverDto> TournamentOverEvent;
        public event EventHandler TournamentMessageReceivedEvent;
        public event EventHandler UpdateStandingsEvent;
        public event EventHandler TournamentStartedEvent;

        public SignalrService(AuthService authService)
        {
            _authService = authService;
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("http://hschess.azurewebsites.net/hubs/chesshub", options =>
                {
                    options.AccessTokenProvider = async () => await _authService.GetStoredToken();
                })
                .WithAutomaticReconnect()
                .Build();


            _hubConnection.On<MoveDto>("ReceiveMove", move =>
            {
                MoveReceivedEvent?.Invoke(this, move);
            });

            _hubConnection.On<Guid>("ReceiveMatchFound", m =>
            {
                MatchFoundEvent?.Invoke(this, m);
            });

            _hubConnection.On<ChallengeDto>("ReceiveChallenge", m =>
            {
                ChallengeFoundEvent?.Invoke(this, m);
            });

            _hubConnection.On<decimal>("ReceiveFold", m =>
            {
                FoldReceivedEvent?.Invoke(this, m);
            });

            _hubConnection.On("ReceiveCheck", () =>
            {
                CheckReceivedEvent?.Invoke(this, EventArgs.Empty);
            });

            _hubConnection.On("ReceiveCall", () =>
            {
                CallReceivedEvent?.Invoke(this, EventArgs.Empty);
            });

            _hubConnection.On<decimal>("ReceiveBet", m =>
            {
                BetReceivedEvent?.Invoke(this, m);
            });

            _hubConnection.On<string>("ReceiveFriendRequest", m =>
            {
                FriendRequestReceivedEvent?.Invoke(this, m);
            });

            _hubConnection.On<ChatMessageDto>("ReceiveMessage", m =>
            {
                ChatMessageReceivedEvent?.Invoke(this, m);
            });

            _hubConnection.On("ReceiveDrawOffer", () =>
            {
                DrawOfferReceivedEvent?.Invoke(this, EventArgs.Empty);
            });

            _hubConnection.On<Result>("ReceiveGameEnded", move =>
            {
                MatchEndedReceivedEvent?.Invoke(this, move);
            });

            _hubConnection.On<TournamentOverDto>("ReceiveTournamentOver", m =>
            {
                TournamentOverEvent?.Invoke(this, m);
            });

            _hubConnection.On("ReceiveTournamentMessage", () =>
            {
                TournamentMessageReceivedEvent?.Invoke(this, EventArgs.Empty);
            });

            _hubConnection.On("ReceiveUpdateStandings", () =>
            {
                UpdateStandingsEvent?.Invoke(this, EventArgs.Empty);
            });

            _hubConnection.On("ReceiveTournamentStarted", () =>
            {
                TournamentStartedEvent?.Invoke(this, EventArgs.Empty);
            });
        }

        public async Task Connect()
        {
            if(_hubConnection.State == HubConnectionState.Connected)
                return;

            await _hubConnection.StartAsync();
        }

        public async Task JoinMatch(Guid matchId)
        {
            await _hubConnection.InvokeAsync("JoinMatch", matchId);
        }

        public async Task LeaveMatch(Guid matchId)
        {
            await _hubConnection.InvokeAsync("LeaveMatch", matchId);
        }

        public async Task sendMoveToServer(MoveDto move, string matchId)
        {
            await _hubConnection.InvokeAsync("SendMoveToServer", move, matchId);
        }

        public async Task SendDrawOffer(string toUserName)
        {
            _hubConnection.InvokeAsync("SendDrawOfferToUser", toUserName);
        }
    }

    public class ChallengeDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
    }

    public class ChatMessageDto
    {
        public string TimeStamp { get; set; }
        public string SenderUserName { get; set; }
        public string Message { get; set; }
    }

    public class MoveDto
    {
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string Promotion { get; set; }
        public int? TimeLeft { get; set; }
    }

    public class TournamentOverDto
    {
        public string Winner { get; set; }
        public decimal Winnings { get; set; }
    }
}
