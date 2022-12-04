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
using Microsoft.AspNetCore.Components;
using PonzianiComponents;
using PonzianiComponents.Chesslib;

namespace HSC.Mobile.Pages.HistoryPage.HistoryChessPage
{
    public class HistoryChessPageViewModel : BaseViewModel
    {
        private readonly EventService _eventService;
        private readonly MatchService _matchService;
        private readonly CurrentGameService _currentGameService;
        private readonly AnalysisService _analysisService;

        public Guid MatchId { get; set; }
        public MatchFullDataDto FullData { get; set; }
        public string OpponentUserName { get; set; }
        public string OwnUserName { get; set; }
        public bool AmIWhite { get; set; }

        private HistoryMove _selectedMove;
        private Game _game = new Game();
        private Engine _engine = new Engine();
        private bool _serverAnalysisSelected;
        private bool _localAnalysisSelected;
        private BestMovesDto _currentAnalysis;
        private AnalysedGameDto _fullServerAnalysis;
        private bool _isServerAnalysisLoading;

        public ICommand ChangeToStartCommand { get; set; }
        public ICommand ChangeToPreviousCommand { get; set; }
        public ICommand ChangeToNextCommand { get; set; }
        public ICommand ChangeToLastCommand { get; set; }
        public ICommand ServerAnalysisCommand { get; set; }
        public ICommand LocalAnalysisCommand { get; set; }

        public ObservableCollection<HistoryMove> Moves { get; set; } = new ObservableCollection<HistoryMove>();

        public HistoryChessPageViewModel(EventService eventService, CurrentGameService currentGameService, MatchService matchService, AnalysisService analysisService)
        {
            _eventService = eventService;
            _currentGameService = currentGameService;
            _matchService = matchService;
            _analysisService = analysisService;

            MatchId = _currentGameService.MatchId;
            FullData = _currentGameService.FullData;
            OpponentUserName = _currentGameService.OpponentUserName;
            AmIWhite = _currentGameService.AmIWhite;
            OwnUserName = _currentGameService.OwnUserName;

            ChangeToStartCommand = new Command(async () => await ChangeToStart());
            ChangeToPreviousCommand = new Command(async () => await ChangeToPrevious());
            ChangeToNextCommand = new Command(async () => await ChangeToNext());
            ChangeToLastCommand = new Command(async () => await ChangeToLast());
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

        public async Task ChangeToStart()
        {
            if (SelectedMove != null)
            {
                SelectedMove = null;
                _eventService.OnScrollToMove(0);
                _eventService.OnChangeToFen(Fen.INITIAL_POSITION);
            }
        }

        public async Task ChangeToPrevious()
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
                    if (ServerAnalysisSelected && !IsServerAnalysisLoading)
                    {
                        CurrentAnalysis = FullServerAnalysis.BestMoves.ToList()[oldIndex - 1];
                    }
                }
            }
        }

        public async Task ChangeToNext()
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
                    if (ServerAnalysisSelected && !IsServerAnalysisLoading)
                    {
                        CurrentAnalysis = FullServerAnalysis.BestMoves.ToList()[oldIndex + 1];
                    }
                }
            }
        }

        public async Task ChangeToLast()
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
                    if (ServerAnalysisSelected && !IsServerAnalysisLoading)
                    {
                        CurrentAnalysis = FullServerAnalysis.BestMoves.ToList()[Moves.Count - 1];
                    }
                }
            }
        }
        #endregion

        public async Task ServerAnalysis()
        {
            ServerAnalysisSelected = true;
            IsServerAnalysisLoading = true;

            FullServerAnalysis = await _analysisService.GetAnalysisAsync(new GameToBeAnalysedDto
            {
                Fens = Moves.Select(m => m.Fen).ToList(),
                MatchId = MatchId,
            });

            for (int i = 0; i < FullServerAnalysis.BestMoves.Count; i++)
            {
                var bestMove = FullServerAnalysis.BestMoves.ToList()[i];

                _engine.position = new Position(Moves[i].Fen);
                var san1 = _engine.PVToSAN(bestMove.FirstBest.Move + " " + bestMove.FirstBest.Continuation);
                var sansplit1 = san1.Split();
                bestMove.FirstBest.Move = sansplit1[1];
                bestMove.FirstBest.Continuation = string.Join(" ", sansplit1[1..^2]);

                _engine.position = new Position(Moves[i].Fen);
                var san2 = _engine.PVToSAN(bestMove.SecondBest.Move + " " + bestMove.SecondBest.Continuation);
                var sansplit2 = san2.Split();
                bestMove.SecondBest.Move = sansplit2[1];
                bestMove.SecondBest.Continuation = string.Join(" ", sansplit2[1..^2]);

                _engine.position = new Position(Moves[i].Fen);
                var san3 = _engine.PVToSAN(bestMove.ThirdBest.Move + " " + bestMove.ThirdBest.Continuation);
                var sansplit3 = san3.Split();
                bestMove.ThirdBest.Move = sansplit3[1];
                bestMove.ThirdBest.Continuation = string.Join(" ", sansplit3[1..^2]);
            }

            IsServerAnalysisLoading = false;
            CurrentAnalysis = FullServerAnalysis.BestMoves.ToList()[Moves.IndexOf(SelectedMove)];
        }

        public async Task LocalAnalysis()
        {
            LocalAnalysisSelected = true;
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

        public bool IsServerAnalysisLoading
        {
            get => _isServerAnalysisLoading;
            set => SetField(ref _isServerAnalysisLoading, value);
        }
    }
}
