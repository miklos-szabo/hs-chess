using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HSC.Common.Resources;
using HSC.Dal;
using HSC.Dal.Entities;
using HSC.Transfer.Analysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HSC.Bll.AnalysisService
{
    public class AnalysisService: IAnalysisService
    {
        private readonly HSCContext _dbContext;
        private readonly ILogger<AnalysisService> _logger;
        private readonly IStringLocalizer<LocalizedStrings> _localizer;

        private readonly AutoResetEvent _uciResetEvent = new AutoResetEvent(false);
        private readonly AutoResetEvent _readyResetEvent = new AutoResetEvent(false);
        private readonly AutoResetEvent _analysingResetEvent = new AutoResetEvent(false);

        public Process EngineProcess { get; set; } = new Process();
        public List<string> ReadLines { get; set; } = new List<string>();

        public AnalysisService(HSCContext dbContext, ILogger<AnalysisService> logger, IStringLocalizer<LocalizedStrings> localizer)
        {
            _dbContext = dbContext;
            _logger = logger;
            _localizer = localizer;

            EngineProcess.StartInfo.FileName = "stockfish_15_x64_avx2.exe";

            EngineProcess.StartInfo.RedirectStandardInput = true;
            EngineProcess.StartInfo.RedirectStandardOutput = true;
            EngineProcess.StartInfo.RedirectStandardError = true;
            EngineProcess.StartInfo.WorkingDirectory = AppContext.BaseDirectory;

            EngineProcess.OutputDataReceived += OutputReceived;
        }

        public async Task<AnalysedGameDto> GetAnalysis(GameToBeAnalysedDto dto)
        {
            var existingAnalysis = await _dbContext.Analyses.SingleOrDefaultAsync(a => a.MatchId == dto.MatchId);

            if (existingAnalysis != null)
            {
                _logger.LogInformation("{id} already analysed, returning existing", dto.MatchId);
                return JsonConvert.DeserializeObject<AnalysedGameDto>(existingAnalysis.AnalysedGame);
            }

            _logger.LogInformation("Starting engine to analyze {id}", dto.MatchId);

            EngineProcess.Start();
            EngineProcess.BeginOutputReadLine();

            try
            {
                // uci -> uciok
                await EngineProcess.StandardInput.WriteLineAsync("uci");

                var uciSuccess = _uciResetEvent.WaitOne(5000);
                if (!uciSuccess)
                {
                    _logger.LogError("Engine error: uciok wasn't received");
                    throw new Exception(_localizer["EngineError"]);
                }
                ReadLines.Clear();

                // Setting multipv to 3
                await EngineProcess.StandardInput.WriteLineAsync("setoption name multipv value 3");

                // isready -> readyok
                await EngineProcess.StandardInput.WriteLineAsync("isready");
                var readySuccess = _readyResetEvent.WaitOne(5000);
                if (!readySuccess)
                {
                    _logger.LogError("Engine error: readyok wasn't received");
                    throw new Exception(_localizer["EngineError"]);
                }
                ReadLines.Clear();

                var analysedGame = new AnalysedGameDto();

                for (int i = 0; i < dto.Fens.Count; i++)
                {
                    // Reset the inner board
                    await EngineProcess.StandardInput.WriteLineAsync("ucinewgame");

                    // Set the position to be analysed
                    await EngineProcess.StandardInput.WriteLineAsync($"position fen {dto.Fens[i]}");

                    // Analyse
                    await EngineProcess.StandardInput.WriteLineAsync("go depth 20");

                    var analyseSuccess = _analysingResetEvent.WaitOne(30000);
                    if (!analyseSuccess)
                    {
                        _logger.LogError("Position analysing has timed out!");
                        throw new Exception(_localizer["EngineError"]);
                    }


                    // Read results, take the last 3 which are the proper ones
                    var depth24 = ReadLines.Where(l => l.StartsWith("info depth 20 seldepth")).ToList();
                    var moveLines = depth24.Skip(Math.Max(0, depth24.Count() - 3)).ToList();

                    var isBlack = dto.Fens[i].Split(" ")[1] == "b";

                    analysedGame.BestMoves.Add(new BestMovesDto
                    {
                        MoveNumber = i,
                        FirstBest = GetEngineLineDtoFromLine(moveLines[0], isBlack),
                        SecondBest = GetEngineLineDtoFromLine(moveLines[1], isBlack),
                        ThirdBest = GetEngineLineDtoFromLine(moveLines[2], isBlack)
                    });

                    ReadLines.Clear();

                    _logger.LogDebug("Analysed move {move} out of {allmoves} on {id}", i + 1, dto.Fens.Count, dto.MatchId);
                }

                _logger.LogInformation("Analysis of {id} finished!", dto.MatchId);

                await EngineProcess.StandardInput.WriteLineAsync("quit");
                await EngineProcess.WaitForExitAsync();

                await _dbContext.Analyses.AddAsync(new Analysis
                {
                    MatchId = dto.MatchId,
                    AnalysedGame = JsonConvert.SerializeObject(analysedGame)
                });

                await _dbContext.SaveChangesAsync();

                return analysedGame;
            }
            catch (Exception e)
            {
                await EngineProcess.StandardInput.WriteLineAsync("quit");
                throw;
            }
        }

        private EngineLineDto GetEngineLineDtoFromLine(string line, bool isBlack)
        {
            var lineParts = line.Split(" ");
            var dto = new EngineLineDto();

            if (lineParts[8] == "cp")
            {
                var advantage = double.Parse(lineParts[9]) / 100;
                var sidedAdvantage = isBlack ? advantage * -1 : advantage;
                dto.Eval = sidedAdvantage.ToString("F2");
            }
            else
            {
                dto.Eval = $"M{Math.Abs(int.Parse(lineParts[9]))}";
            }

            dto.Move = lineParts[21];
            dto.Continuation = string.Join(" ", lineParts[22..]);

            return dto;
        }

        private void OutputReceived(object sender, DataReceivedEventArgs args)
        {
            if (args?.Data == null) return;
            ReadLines.Add(args.Data);

            if (args.Data == "uciok")
            {
                _logger.LogDebug("uciok received!");
                _uciResetEvent.Set();
            }
            else if (args.Data == "readyok")
            {
                _logger.LogDebug("readyok received!");
                _readyResetEvent.Set();
            }
            else if (args.Data.StartsWith("bestmove"))
            {
                _logger.LogDebug("Best move found!");
                _analysingResetEvent.Set();
            }
        }
    }
}
