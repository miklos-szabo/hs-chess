using HSC.Mobile.Enums;
using PonzianiComponents;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Maui.Views;
using HSC.Mobile.Pages.ChessPage.MatchEndPopup;
using HSC.Mobile.Resources.Translation;
using HSC.Mobile.Services;
using HSCApi;
using Microsoft.Extensions.Localization;
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
        private readonly IStringLocalizer<Translation> _localizer;
        private readonly BettingService _bettingService;

        public ICommand ResignCommand { get; set; }
        public ICommand DrawCommand { get; set; }
        public ICommand ChangeToStartCommand { get; set; }
        public ICommand ChangeToPreviousCommand { get; set; }
        public ICommand ChangeToNextCommand { get; set; }
        public ICommand ChangeToLastCommand { get; set; }
        public ICommand CheckCommand { get; set; }
        public ICommand CallCommand { get; set; }
        public ICommand RaiseCommand { get; set; }
        public ICommand FoldCommand { get; set; }

        public ChessPageViewModel(SignalrService signalrService, CurrentGameService currentgameService, MatchService matchService, EventService eventService, IStringLocalizer<Translation> localizer, BettingService bettingService)
        {
            _signalrService = signalrService;
            _currentgameService = currentgameService;
            _matchService = matchService;
            _eventService = eventService;
            _localizer = localizer;
            _bettingService = bettingService;

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
            _signalrService.MatchEndedReceivedEvent += MatchEnded;
            _signalrService.DrawOfferReceivedEvent += DrawOfferReceived;
            _signalrService.CheckReceivedEvent += OpponentChecked;
            _signalrService.CallReceivedEvent += OpponentCalled;
            _signalrService.BetReceivedEvent += OpponentRaised;
            _signalrService.FoldReceivedEvent += OpponentFolded;

            ResignCommand = new Command(async () => await Resign());
            DrawCommand = new Command(async () => await Draw());
            ChangeToStartCommand = new Command(ChangeToStart);
            ChangeToPreviousCommand = new Command(ChangeToPrevious);
            ChangeToNextCommand = new Command(ChangeToNext);
            ChangeToLastCommand = new Command(ChangeToLast);
            CheckCommand = new Command(async () => await Check());
            CallCommand = new Command(async () => await Call());
            RaiseCommand = new Command(async () => await Raise());
            FoldCommand = new Command(async () => await Fold());

            CurrentBet = FullData.MinimumBet;

            #region timers
            OwnTime = TimeSpan.FromMinutes(FullData.TimeLimitMinutes);
            OpponentTime = TimeSpan.FromMinutes(FullData.TimeLimitMinutes);

            _owntimer = Application.Current.Dispatcher.CreateTimer();
            _opponentTimer = Application.Current.Dispatcher.CreateTimer();
            _bettingHideTimer = Application.Current.Dispatcher.CreateTimer();
            _owntimer.Interval = TimeSpan.FromMilliseconds(1000);
            _opponentTimer.Interval = TimeSpan.FromMilliseconds(1000);
            _bettingHideTimer.Interval = TimeSpan.FromMilliseconds(3000);
            _owntimer.Tick += (s, e) =>
            {
                if (OwnTime.Add(TimeSpan.FromSeconds(-1)) < TimeSpan.Zero)
                {
                    StopOwnTimer();
                    Task.Run(async () => await _matchService.MatchOverAsync(MatchId, AmIWhite ? Result.BlackWonByTimeOut : Result.WhiteWonByTimeout, OpponentUserName));
                    OwnTime = TimeSpan.Zero;
                    Task.Run(async () => await GameOver(AmIWhite ? Result.BlackWonByTimeOut : Result.WhiteWonByTimeout));
                    return;
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
            _bettingHideTimer.Tick += (s, e) =>
            {
                _bettingHideTimer.Stop();
                BettingText = string.Empty;
                IsBettingOverReason = false;
                IsBettingActive = false;
            };

            #endregion
        }

        #region Resolution

        public bool HasDrawBeenOffered
        {
            get => _hasDrawBeenOffered;
            set => SetField(ref _hasDrawBeenOffered, value);
        }

        private void DrawOfferReceived(object sender, EventArgs e)
        {
            HasDrawBeenOffered = true;
        }

        private async void MatchEnded(object sender, Result result)
        {
            await GameOver(result);
        }

        public async Task GameOver(Result result)
        {
            StopOpponentTimerNoIncrement();
            StopOwnTimerNoIcrement();
            SearchSimpleResult searchSimpleResult;
            if (result == Result.DrawByAgreement || result == Result.DrawByInsufficientMaterial ||
                result == Result.DrawByStalemate || result == Result.DrawByThreefoldRepetition ||
                result == Result.DrawByTimeoutVsInsufficientMaterial)
            {
                searchSimpleResult = SearchSimpleResult.Draw;
            }
            else if (result == Result.WhiteWonByCheckmate || result == Result.WhiteWonByTimeout ||
                     result == Result.WhiteWonByResignation)
            {
                searchSimpleResult = OwnUserName == FullData.WhiteUserName
                    ? SearchSimpleResult.Victory
                    : SearchSimpleResult.Defeat;
            }
            else
            {
                searchSimpleResult = OwnUserName == FullData.WhiteUserName
                    ? SearchSimpleResult.Defeat
                    : SearchSimpleResult.Victory;
            }
            await MainThread.InvokeOnMainThreadAsync(() => Application.Current.MainPage.ShowPopup(new GameOverPopup(searchSimpleResult, result, CurrentBet, _localizer)));
        }

        public async Task Resign()
        {
            if (AmIWhite)
            {
                await _matchService.MatchOverAsync(MatchId, Result.BlackWonByResignation, FullData.BlackUserName);
            }
            else
            {
                await _matchService.MatchOverAsync(MatchId, Result.WhiteWonByResignation, FullData.WhiteUserName);
            }

            await GameOver(AmIWhite ? Result.BlackWonByResignation : Result.WhiteWonByResignation);
        }

        public async Task Draw()
        {
            if (HasDrawBeenOffered)
            {
                HasDrawBeenOffered = false;
                await _matchService.MatchOverAsync(MatchId, Result.DrawByAgreement, string.Empty);
                await GameOver(Result.DrawByAgreement);
            }
            else
            {
                await _signalrService.SendDrawOffer(AmIWhite ? FullData.BlackUserName : FullData.WhiteUserName);
            }
        }

        #endregion

        #region Moving

        public HistoryMove SelectedMove
        {
            get => _selectedMove;
            set => SetField(ref _selectedMove, value);
        }

        private async void OwnMovePlayed(object sender, MovePlayedInfo e)
        {
            StopOwnTimer();
            StartOpponentTimer();
            if (HasDrawBeenOffered) HasDrawBeenOffered = false;
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
            _eventService.OnScrollToMove(Moves.Count - 1);
            SelectedMove = Moves.Last();

            if (Moves.Count == 20)
            {
                StopOpponentTimerNoIncrement();
                StopOwnTimerNoIcrement();
                IsBettingActive = true;
            }
        }

        private async void OwnMoveEndedGame(object sender, Result result)
        {
            await _matchService.MatchOverAsync(MatchId, result, OwnUserName);
            await _matchService.SaveMatchPgnAsync(MatchId, _currentgameService.Pgn);
            await GameOver(result);
        }

        private void OpponentMoveProcessed(object sender, HistoryMove e)
        {
            Moves.Add(e);
            _eventService.OnScrollToMove(Moves.Count - 1);
            SelectedMove = Moves.Last();

            if (Moves.Count == 20)
            {
                StopOpponentTimerNoIncrement();
                StopOwnTimerNoIcrement();
                IsBettingActive = true;
            }
        }

        private void OpponentMoveReceived(object sender, MoveDto e)
        {
            StopOpponentTimerNoIncrement();
            OpponentTime = TimeSpan.FromMilliseconds(e.TimeLeft.Value);
            StartOwnTimer();
        }
#endregion

        #region MoveNavigation
                public void ChangeToStart()
                {
                    if (SelectedMove != null)
                    {
                        SelectedMove = null;
                        _eventService.OnScrollToMove(0);
                        _eventService.OnChangeToFen(Fen.INITIAL_POSITION);
                    }
                }

                public void ChangeToPrevious()
                {
                    if (SelectedMove != null && Moves.Count != 0)
                    {
                        var oldIndex = Moves.IndexOf(SelectedMove);
                        if (oldIndex == 0)
                        {
                            ChangeToStart();
                            return;
                        }
                        else
                        {
                            _eventService.OnChangeToFen(Moves[oldIndex - 1].Fen);
                            _eventService.OnScrollToMove(oldIndex - 1);
                            SelectedMove = Moves[oldIndex - 1];
                        }
                    }
                }

                public void ChangeToNext()
                {
                    if (Moves.Count > 0)
                    {
                        var oldIndex = Moves.IndexOf(SelectedMove);
                        if (oldIndex == Moves.Count - 1)
                        {
                            return;
                        }
                        else
                        {
                            _eventService.OnChangeToFen(Moves[oldIndex + 1].Fen);
                            _eventService.OnScrollToMove(oldIndex + 1);
                            SelectedMove = Moves[oldIndex + 1];
                        }
                    }
                }

                public void ChangeToLast()
                {
                    if (Moves.Count > 0)
                    {
                        var oldIndex = Moves.IndexOf(SelectedMove);
                        if (oldIndex == Moves.Count - 1)
                        {
                            return;
                        }
                        else
                        {
                            _eventService.OnChangeToFen(Moves[^1].Fen);
                            _eventService.OnScrollToMove(Moves.Count - 1);
                            SelectedMove = Moves[^1];
                        }
                    }
                }
        #endregion

        #region Betting

        public bool IsBettingOverReason
        {
            get => _isBettingOverReason1;
            set => SetField(ref _isBettingOverReason1, value);
        }

        public decimal CurrentBet
        {
            get => _currentBet;
            set => SetField(ref _currentBet, value);
        }

        public decimal CurrentBetSlider
        {
            get => _currentBetSlider;
            set => SetField(ref _currentBetSlider, value);
        }

        public bool HasOpponentRaised
        {
            get => _hasOpponentRaised;
            set => SetField(ref _hasOpponentRaised, value);
        }

        public bool MyTurn
        {
            get => _myTurn;
            set => SetField(ref _myTurn, value);
        }

        public bool IsBettingActive
        {
            get => _isBettingActive;
            set => SetField(ref _isBettingActive, value);
        }

        public string BettingText
        {
            get => _bettingText;
            set => SetField(ref _bettingText, value);
        }

        public async Task Check()
        {
            if (AmIWhite)
            {
                MyTurn = false;
            }
            else
            {
                IsBettingActive = false;
                StartOpponentTimer();
            }

            await _bettingService.CheckAsync(MatchId);
        }

        public async Task Call()
        {
            IsBettingActive = false;

            if (AmIWhite)
            {
                StartOwnTimer();
            }
            else
            {
                StartOpponentTimer();
            }

            await _bettingService.CallAsnycAsync(MatchId);
        }

        public async Task Raise()
        {
            MyTurn = false;
            CurrentBet = Math.Round(CurrentBetSlider, 2);
            await _bettingService.RaiseAsync(MatchId, CurrentBet);
        }

        public async Task Fold()
        {
            IsBettingActive = false;
            CurrentBet *= 1.25m;

            if (AmIWhite)
            {
                StartOwnTimer();
            }
            else
            {
                StartOpponentTimer();
            }

            await _bettingService.FoldAsync(MatchId);
        }

        private void OpponentFolded(object sender, decimal e)
        {
            HasOpponentRaised = true;
            BettingText = _localizer["Betting.OpponentFolded"];
            MyTurn = true;
            IsBettingOverReason = true;
            _bettingHideTimer.Start();

            if (AmIWhite)
            {
                StartOwnTimer();
            }
            else
            {
                StartOpponentTimer();
            }

        }

        private void OpponentRaised(object sender, decimal newBet)
        {
            HasOpponentRaised = true;
            CurrentBet = newBet;
            BettingText = $"{_localizer["Betting.OpponentRaised"]} {newBet:F2}$";
            MyTurn = true;
        }

        private void OpponentCalled(object sender, EventArgs e)
        {
            HasOpponentRaised = false;
            BettingText = _localizer["Betting.OpponentCalled"];
            MyTurn = true;
            IsBettingOverReason = true;
            _bettingHideTimer.Start();

            if (AmIWhite)
            {
                StartOwnTimer();
            }
            else
            {
                StartOpponentTimer();
            }
        }

        private void OpponentChecked(object sender, EventArgs e)
        {
            HasOpponentRaised = false;
            BettingText = _localizer["Betting.OpponentChecked"];
            MyTurn = true;
            if (AmIWhite)
            {
                IsBettingOverReason = true;
                _bettingHideTimer.Start();
                StartOwnTimer();
            }
        }

        #endregion

        #region Timers
        private TimeSpan _ownTime;
        private TimeSpan _opponentTime;
        private bool _opponentClockIsActive;
        private bool _ownClockIsActive;

        IDispatcherTimer _owntimer;
        IDispatcherTimer _opponentTimer;
        IDispatcherTimer _bettingHideTimer;
        private HistoryMove _selectedMove;
        private bool _hasDrawBeenOffered;
        private decimal _currentBet;
        private decimal _currentBetSlider;
        private bool _isBettingActive;
        private bool _myTurn;
        private bool _hasOpponentRaised;
        private string _bettingText;
        private bool _isBettingOverReason1;

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
