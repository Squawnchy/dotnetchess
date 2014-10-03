﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Logging;
using DotNetEngine.Engine.Enums;
using DotNetEngine.Engine.Helpers;
using DotNetEngine.Engine.Objects;

namespace DotNetEngine.Engine
{
	/// <summary>
	/// The Chess Engine, This class handles actually playing the game.
	/// </summary>
	public class Engine
	{
// ReSharper disable once FieldCanBeMadeReadOnly.Local
		private ILog _logger = LogManager.GetCurrentClassLogger();
        private static readonly MoveData _moveData = new MoveData();
        private GameState _gameState;
        private const string DefaultFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        private static readonly Random _random = new Random();
	   

        public event EventHandler<BestMoveFoundEventArgs> BestMoveFound;

	    protected virtual void OnBestMoveFound(BestMoveFoundEventArgs e)
	    {
	        var handler = BestMoveFound;

	        if (handler != null)
	        {
	            handler(this, e);
	        }
	    }

       	public void SetBoard(string initialFen)
        {
            _gameState = GameStateUtility.LoadGameStateFromFen(initialFen);
        }

        public void Perft(int depth)
        {
	        var perftData = new PerftData();
			var stopwatch = new Stopwatch();
			stopwatch.Start();
			var count = _gameState.RunPerftRecursively(_moveData, perftData, 1, depth);
			stopwatch.Stop();

			_logger.InfoFormat("Total Nodes: {0} {1} mS Elapsed: {2}", count, Environment.NewLine, stopwatch.ElapsedMilliseconds);
			_logger.DebugFormat("Total Nodes: {0} Total Captures {1} Total Checks {2} Total EnPassants {3} Total OO Castles {4} Total OOO Castles {5} Total Promotions {6}", count, perftData.TotalCaptures, perftData.TotalChecks, perftData.TotalEnpassants, perftData.TotalOOCastles, perftData.TotalOOOCastles, perftData.TotalPromotions);
        }

        public void Divide(int depth)
        {
            _gameState.CalculateDivide(_moveData, new PerftData(), 1, depth);
        }

        public void NewGame(string fen)
        {
            SetBoard(fen);
        }

	    public void NewGame() 
	    {
          NewGame(DefaultFen);
	    }

        public void TryMakeMove(string moveText)
        {   
            _logger.InfoFormat("Making Other Side Move");
            _gameState.GenerateMoves(MoveGenerationMode.All, 1, _moveData);

            uint move;
            var canParse = MoveUtility.TryConvertStringToMove(moveText.ToUpper(), _gameState.WhiteToMove, out move);

            if (!canParse)
            {
                _logger.ErrorFormat("Unable to Parse Move Text");
            }

            var foundMove = _gameState.Moves[1].FirstOrDefault(
                x =>
                    x.GetFromMove() == move.GetFromMove() && x.GetToMove() == move.GetToMove() &&
                    x.GetPromotedPiece() == move.GetPromotedPiece());

            if (foundMove == uint.MinValue)
            {
                _logger.ErrorFormat("Unable to Locate Move in Move Generator");
                return;
            }

            _gameState.MakeMove(foundMove);
            _gameState.TotalMoveCount++;
            _logger.InfoFormat("Other Side Move Made");
        }

        public void Calculate()
        {
            var move = GetBestMove(_gameState.TotalMoveCount, 4);
            _gameState.MakeMove(move);

            OnBestMoveFound(new BestMoveFoundEventArgs
            {
                BestMove = string.Format("{0}{1}", move.GetFromMove().ToRankAndFile(), move.GetToMove().ToRankAndFile()).ToLower()
            });
        }

        public void Stop()
        {
            //Do nothing for now.
        }

	    public uint GetBestMove(int ply, int depth)
	    {
            _gameState.GenerateMoves(MoveGenerationMode.All, ply, _moveData);
            var bestValue = int.MinValue;
	        var bestMove = 0U;

            foreach (var move in _gameState.Moves[ply])
            {
                _gameState.MakeMove(move);

                if (!_gameState.IsOppositeSideKingAttacked(_moveData))
                {
                    var value = Think(ply + 1, depth - 1, true) * -1;


                    if (value > bestValue)
                    {
                        bestValue = value;
                        bestMove = move;
                    }
                }
                _gameState.UnMakeMove(move);
            }
	        return bestMove;
	    }

        private int Think(int ply, int depth, bool side)
	    {
            if (depth == 0)
            {
                return _gameState.Evaluate() * (side ? 1 : -1);
            }

            var bestValue = int.MinValue;

            _gameState.GenerateMoves(MoveGenerationMode.All, ply, _moveData);

            foreach (var move in _gameState.Moves[ply])
            {
                _gameState.MakeMove(move);

                if (!_gameState.IsOppositeSideKingAttacked(_moveData))
                {

                    var value = Think(ply + 1, depth - 1, !side) * -1;

                    if (value > bestValue)
                        bestValue = value;
                }

               _gameState.UnMakeMove(move);
            }
            return bestValue;
	    }
    }
}
