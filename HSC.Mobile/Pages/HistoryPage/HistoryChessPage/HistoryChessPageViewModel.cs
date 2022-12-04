using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using HSC.Mobile.Pages.ChessPage;
using HSC.Mobile.Services;
using HSCApi;
using PonzianiComponents.Chesslib;

namespace HSC.Mobile.Pages.HistoryPage.HistoryChessPage
{
    public class HistoryChessPageViewModel : BaseViewModel
    {
        private readonly EventService _eventService;
        private readonly MatchService _matchService;
        private readonly CurrentGameService _currentGameService;

        public Guid MatchId { get; set; }
        public MatchFullDataDto FullData { get; set; }
        public string OpponentUserName { get; set; }
        public string OwnUserName { get; set; }
        public bool AmIWhite { get; set; }

        private HistoryMove _selectedMove;
        private Game _game = new Game();
        private bool _serverAnalysisSelected;
        private bool _localAnalysisSelected;
        private BestMovesDto _currentAnalysis;
        private AnalysedGameDto _fullServerAnalysis;

        public ICommand ChangeToStartCommand { get; set; }
        public ICommand ChangeToPreviousCommand { get; set; }
        public ICommand ChangeToNextCommand { get; set; }
        public ICommand ChangeToLastCommand { get; set; }
        public ICommand ServerAnalysisCommand { get; set; }
        public ICommand LocalAnalysisCommand { get; set; }

        public ObservableCollection<HistoryMove> Moves { get; set; } = new ObservableCollection<HistoryMove>();

        public HistoryChessPageViewModel(EventService eventService, CurrentGameService currentGameService, MatchService matchService)
        {
            _eventService = eventService;
            _currentGameService = currentGameService;
            _matchService = matchService;

            MatchId = _currentGameService.MatchId;
            FullData = _currentGameService.FullData;
            OpponentUserName = _currentGameService.OpponentUserName;
            AmIWhite = _currentGameService.AmIWhite;
            OwnUserName = _currentGameService.OwnUserName;

            ChangeToStartCommand = new Command(ChangeToStart);
            ChangeToPreviousCommand = new Command(ChangeToPrevious);
            ChangeToNextCommand = new Command(ChangeToNext);
            ChangeToLastCommand = new Command(ChangeToLast);
            ServerAnalysisCommand = new Command(async () => await ServerAnalysis());
            LocalAnalysisCommand = new Command(async () => await LocalAnalysis());

            Task.Run(async () =>
            {
                var pgn = await _matchService.GetMatchPgnAsync(_currentGameService.MatchId);
                var games = PGN.Parse(pgn);
                _game = games.First();

                var positionGame = new Game();

                _game.Moves.ForEach(m =>
                {
                    var san = positionGame.Position.ToSAN(m);
                    positionGame.Add(m);
                    var fen = positionGame.Position.FEN;

                    Moves.Add(new HistoryMove
                    {
                        San = san,
                        Fen = fen
                    });
                });

                SelectedMove = Moves.Last();
                _eventService.OnScrollToMove(Moves.IndexOf(Moves.Last()));
                _eventService.OnChangeToFen(Moves.Last().Fen);
            });
        }

        #region MoveNavigation
        public HistoryMove SelectedMove
        {
            get => _selectedMove;
            set => SetField(ref _selectedMove, value);
        }

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

        public async Task ServerAnalysis()
        {

        }

        public async Task LocalAnalysis()
        {

        }

        public BestMovesDto CurrentAnalysis
        {
            get => _currentAnalysis;
            set => SetField(ref _currentAnalysis, value);
        }

        public AnalysedGameDto FullServerAnalysis
        {
            get => _fullServerAnalysis;
            set => SetField(ref _fullServerAnalysis, value);
        }

        public bool ServerAnalysisSelected
        {
            get => _serverAnalysisSelected;
            set => SetField(ref _serverAnalysisSelected, value);
        }

        public bool LocalAnalysisSelected
        {
            get => _localAnalysisSelected;
            set => SetField(ref _localAnalysisSelected, value);
        }
    }
}
