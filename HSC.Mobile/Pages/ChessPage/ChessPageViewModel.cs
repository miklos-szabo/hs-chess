using HSC.Mobile.Enums;
using PonzianiComponents;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSC.Mobile.Services;
using HSCApi;
using PonzianiComponents.Chesslib;
using Result = HSCApi.Result;

namespace HSC.Mobile.Pages.ChessPage
{
    public class ChessPageViewModel: BaseViewModel
    {
        public Guid MatchId { get; set; }
        public MatchFullDataDto FullData { get; set; }
        public string OpponentUserName { get; set; }
        public string OwnUserName { get; set; }
        public bool AmIWhite { get; set; }
        public ObservableCollection<HistoryMove> Moves { get; set; } = new ObservableCollection<HistoryMove>();

        private readonly SignalrService _signalrService;
        private readonly CurrentGameService _currentgameService;
        private readonly MatchService _matchService;
        private readonly EventService _eventService;


        public ChessPageViewModel(SignalrService signalrService, CurrentGameService currentgameService, MatchService matchService, EventService eventService)
        {
            _signalrService = signalrService;
            _currentgameService = currentgameService;
            _matchService = matchService;
            _eventService = eventService;

            MatchId = _currentgameService.MatchId;
            FullData = _currentgameService.FullData;
            OpponentUserName = _currentgameService.OpponentUserName;
            AmIWhite = _currentgameService.AmIWhite;
            OwnUserName = _currentgameService.OwnUserName;

            Task.Run(async () => await _signalrService.JoinMatch(MatchId));

            _signalrService.MoveReceivedEvent += OpponentMoveReceived;
            _eventService.OpponentMoveProcessed += OpponentMoveProcessed;
            _eventService.OwnMoveEndedGame += OwnMoveEndedGame;
            _eventService.OwnMovePlayed += OwnMovePlayed;


            #region timers
            OwnTime = TimeSpan.FromMinutes(FullData.TimeLimitMinutes);
            OpponentTime = TimeSpan.FromMinutes(FullData.TimeLimitMinutes);

            _owntimer = Application.Current.Dispatcher.CreateTimer();
            _opponentTimer = Application.Current.Dispatcher.CreateTimer();
            _owntimer.Interval = TimeSpan.FromMilliseconds(1000);
            _opponentTimer.Interval = TimeSpan.FromMilliseconds(1000);
            _owntimer.Tick += (s, e) =>
            {
                if (OwnTime.Add(TimeSpan.FromSeconds(-1)) < TimeSpan.Zero)
                {
                    StopOwnTimer();
                    Task.Run(async () => await _matchService.MatchOverAsync(MatchId, AmIWhite ? Result.BlackWonByTimeOut : Result.WhiteWonByTimeout, OpponentUserName));
                    OwnTime = TimeSpan.Zero;
                    return; //TODO edn popup
                }

                OwnTime = OwnTime.Add(TimeSpan.FromSeconds(-1));
            };
            _opponentTimer.Tick += (s, e) =>
            {
                if (OpponentTime.Add(TimeSpan.FromSeconds(-1)) < TimeSpan.Zero)
                {
                    StopOpponentTimer();
                    OpponentTime = TimeSpan.Zero;
                }

                OpponentTime = OpponentTime.Add(TimeSpan.FromSeconds(-1));
            };
#endregion
        }

        private async void OwnMovePlayed(object sender, MovePlayedInfo e)
        {
            StopOwnTimer();
            StartOpponentTimer();
            await _signalrService.sendMoveToServer(
                new MoveDto
                {
                    Origin = e.Move.From.ToString().ToLower(),
                    Destination = e.Move.To.ToString().ToLower(),
                    Promotion = e.Move.PromoteTo == PieceType.NONE
                        ? string.Empty
                        : e.Move.PromoteTo.ToString().ToLower()[0].ToString(),
                    TimeLeft = (int)OwnTime.TotalMilliseconds,
                }, MatchId.ToString());
            Moves.Add(new HistoryMove{Fen = e.NewFen, San = e.San});
        }

        private async void OwnMoveEndedGame(object sender, Result result)
        {
            StopOwnTimerNoIcrement();
            StopOpponentTimerNoIncrement();
            await _matchService.MatchOverAsync(MatchId, result, OwnUserName);
            await _matchService.SaveMatchPgnAsync(MatchId, _currentgameService.Pgn);
        }

        private void OpponentMoveProcessed(object sender, HistoryMove e)
        {
            Moves.Add(e);
        }

        private void OpponentMoveReceived(object sender, MoveDto e)
        {
            StopOpponentTimerNoIncrement();
            OpponentTime = TimeSpan.FromMilliseconds(e.TimeLeft.Value);
            StartOwnTimer();
        }



        #region Timers
        private TimeSpan _ownTime;
        private TimeSpan _opponentTime;
        private bool _opponentClockIsActive;
        private bool _ownClockIsActive;

        IDispatcherTimer _owntimer;
        IDispatcherTimer _opponentTimer;

        public bool OpponentClockIsActive
        {
            get => _opponentClockIsActive;
            set => SetField(ref _opponentClockIsActive, value);
        }

        public bool OwnClockIsActive
        {
            get => _ownClockIsActive;
            set => SetField(ref _ownClockIsActive, value);
        }

        public TimeSpan OwnTime
        {
            get => _ownTime;
            set => SetField(ref _ownTime, value);
        }

        public TimeSpan OpponentTime
        {
            get => _opponentTime;
            set => SetField(ref _opponentTime, value);
        }

        public void StartOwnTimer()
        {
            OwnClockIsActive = true;
            _owntimer.Start();
        }

        public void StartOpponentTimer()
        {
            OpponentClockIsActive = true;
            _opponentTimer.Start();
        }

        public void StopOpponentTimer()
        {
            OpponentClockIsActive = false;
            OpponentTime = OpponentTime.Add(TimeSpan.FromSeconds(FullData.Increment));
            _opponentTimer.Stop();
        }

        public void StopOwnTimer()
        {
            OwnClockIsActive = false;
            OwnTime = OwnTime.Add(TimeSpan.FromSeconds(FullData.Increment));
            _owntimer.Stop();
        }

        public void StopOpponentTimerNoIncrement()
        {
            OpponentClockIsActive = false;
            _opponentTimer.Stop();
        }

        public void StopOwnTimerNoIcrement()
        {
            OwnClockIsActive = false;
            _owntimer.Stop();
        }
        #endregion
    }
}
