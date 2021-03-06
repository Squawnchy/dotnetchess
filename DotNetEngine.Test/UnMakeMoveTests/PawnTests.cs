﻿using DotNetEngine.Engine.Helpers;
using DotNetEngine.Engine.Objects;
using NUnit.Framework;

namespace DotNetEngine.Test.UnMakeMoveTests
{
    public class PawnTests
    {
        private static readonly ZobristHash _zobristHash = new ZobristHash();

        #region White Pawns
        [Test]
        public void UnMakeMove_Sets_White_Pawn_Board_Correctly()
        {
            var gameState = new GameState("8/8/8/8/8/P7/8/8 w - - 0 1", _zobristHash);
            gameState.PreviousGameStateRecords[gameState.TotalMoveCount] = new GameStateRecord();

            var move = 0U;
            move = move.SetFromMove(8U);
            move = move.SetToMove(16U);
            move = move.SetMovingPiece(MoveUtility.WhitePawn);

            gameState.UnMakeMove(move);

            Assert.That(gameState.WhitePawns, Is.EqualTo(MoveUtility.BitStates[8]), "Piece Bitboard");
        }

        [Test]
        public void UnMakeMove_Sets_White_Pieces_Board_Correctly()
        {
            var gameState = new GameState("8/8/8/8/8/P7/8/8 w - - 0 1", _zobristHash);
            gameState.PreviousGameStateRecords[gameState.TotalMoveCount] = new GameStateRecord();

            var move = 0U;
            move = move.SetFromMove(8U);
            move = move.SetToMove(16U);
            move = move.SetMovingPiece(MoveUtility.WhitePawn);

            gameState.UnMakeMove(move);

            Assert.That(gameState.WhitePieces, Is.EqualTo(MoveUtility.BitStates[8]), "Color Bitboard");
        }

        [Test]
        public void UnMakeMove_Sets_All_Pieces_Board_Correctly_When_White()
        {
            var gameState = new GameState("8/8/8/8/8/P7/8/8 w - - 0 1", _zobristHash);
            gameState.PreviousGameStateRecords[gameState.TotalMoveCount] = new GameStateRecord();

            var move = 0U;
            move = move.SetFromMove(8U);
            move = move.SetToMove(16U);
            move = move.SetMovingPiece(MoveUtility.WhitePawn);

            gameState.UnMakeMove(move);

            Assert.That(gameState.AllPieces, Is.EqualTo(MoveUtility.BitStates[8]), "All Pieces Bitboard");
        }       

        [Test]
        public void UnMakeMove_Sets_EnPassant_Opposite_Pawn_Board_Correctly_When_White()
        {
            var gameState = new GameState("8/8/P7/8/8/8/8/8 w - - 0 1", _zobristHash);
            gameState.PreviousGameStateRecords[gameState.TotalMoveCount] = new GameStateRecord();

            var move = 0U;
            move = move.SetFromMove(33U);
            move = move.SetToMove(40U);
            move = move.SetMovingPiece(MoveUtility.WhitePawn);
            move = move.SetCapturedPiece(MoveUtility.BlackPawn);
            move = move.SetPromotionPiece(MoveUtility.WhitePawn);

            gameState.UnMakeMove(move);
            Assert.That(gameState.WhitePawns, Is.EqualTo(MoveUtility.BitStates[33]));
            Assert.That(gameState.WhitePieces, Is.EqualTo(MoveUtility.BitStates[33])); 
        }

        [Test]
        public void UnMakeMove_Sets_White_Queen_Promotion_Piece_Correctly()
        {
            var gameState = new GameState("Q7/8/8/8/8/8/8/8 b - - 0 1", _zobristHash);
            gameState.PreviousGameStateRecords[gameState.TotalMoveCount] = new GameStateRecord();

            var move = 0U;
            move = move.SetFromMove(48U);
            move = move.SetToMove(56U);
            move = move.SetMovingPiece(MoveUtility.WhitePawn);
            move = move.SetPromotionPiece(MoveUtility.WhiteQueen);

            gameState.UnMakeMove(move);
            Assert.That(gameState.WhitePawns, Is.EqualTo(MoveUtility.BitStates[48]), "PawnBoard");
            Assert.That(gameState.WhiteQueens, Is.EqualTo(0), "PromotionBoard");
        }

        [Test]
        public void UnMakeMove_Sets_White_Knight_Promotion_Piece_Correctly()
        {
            var gameState = new GameState("N7/8/8/8/8/8/8/8 b - - 0 1", _zobristHash);
            gameState.PreviousGameStateRecords[gameState.TotalMoveCount] = new GameStateRecord();

            var move = 0U;
            move = move.SetFromMove(48U);
            move = move.SetToMove(56U);
            move = move.SetMovingPiece(MoveUtility.WhitePawn);
            move = move.SetPromotionPiece(MoveUtility.WhiteKnight);

            gameState.UnMakeMove(move);
            Assert.That(gameState.WhitePawns, Is.EqualTo(MoveUtility.BitStates[48]), "PawnBoard");
            Assert.That(gameState.WhiteKnights, Is.EqualTo(0), "PromotionBoard");
        }

        [Test]
        public void UnMakeMove_Sets_White_Bishop_Promotion_Piece_Correctly()
        {
            var gameState = new GameState("B7/8/8/8/8/8/8/8 b - - 0 1", _zobristHash);
            gameState.PreviousGameStateRecords[gameState.TotalMoveCount] = new GameStateRecord();

            var move = 0U;
            move = move.SetFromMove(48U);
            move = move.SetToMove(56U);
            move = move.SetMovingPiece(MoveUtility.WhitePawn);
            move = move.SetPromotionPiece(MoveUtility.WhiteBishop);

            gameState.UnMakeMove(move);
            Assert.That(gameState.WhitePawns, Is.EqualTo(MoveUtility.BitStates[48]), "PawnBoard");
            Assert.That(gameState.WhiteBishops, Is.EqualTo(0), "PromotionBoard");
        }

        [Test]
        public void UnMakeMove_Sets_White_Rook_Promotion_Piece_Correctly()
        {
            var gameState = new GameState("R7/8/8/8/8/8/8/8 b - - 0 1", _zobristHash);
            gameState.PreviousGameStateRecords[gameState.TotalMoveCount] = new GameStateRecord();

            var move = 0U;
            move = move.SetFromMove(48U);
            move = move.SetToMove(56U);
            move = move.SetMovingPiece(MoveUtility.WhitePawn);
            move = move.SetPromotionPiece(MoveUtility.WhiteRook);

            gameState.UnMakeMove(move);
            Assert.That(gameState.WhitePawns, Is.EqualTo(MoveUtility.BitStates[48]), "PawnBoard");
            Assert.That(gameState.WhiteRooks, Is.EqualTo(0), "PromotionBoard");
        }
        #endregion

        #region Black Pawns
        [Test]
        public void UnMakeMove_Sets_Black_Pawn_Board_Correctly()
        {
            var gameState = new GameState("8/8/p7/8/8/8/8/8 w - - 0 1", _zobristHash);
            gameState.PreviousGameStateRecords[gameState.TotalMoveCount] = new GameStateRecord();

            var move = 0U;
            move = move.SetFromMove(48U);
            move = move.SetToMove(40U);
            move = move.SetMovingPiece(MoveUtility.BlackPawn);

            gameState.UnMakeMove(move);

            Assert.That(gameState.BlackPawns, Is.EqualTo(MoveUtility.BitStates[48]), "Piece Bitboard");
        }

        [Test]
        public void UnMakeMove_Sets_Black_Pieces_Board_Correctly()
        {
            var gameState = new GameState("8/8/p7/8/8/8/8/8 w - - 0 1", _zobristHash);
            gameState.PreviousGameStateRecords[gameState.TotalMoveCount] = new GameStateRecord();

            var move = 0U;
            move = move.SetFromMove(48U);
            move = move.SetToMove(40U);
            move = move.SetMovingPiece(MoveUtility.BlackPawn);

            gameState.UnMakeMove(move);

            Assert.That(gameState.BlackPieces, Is.EqualTo(MoveUtility.BitStates[48]), "Color Bitboard");
        }

        [Test]
        public void UnMakeMove_Sets_All_Pieces_Board_Correctly_When_Black()
        {
            var gameState = new GameState("8/8/p7/8/8/8/8/8 w - - 0 1", _zobristHash);
            gameState.PreviousGameStateRecords[gameState.TotalMoveCount] = new GameStateRecord();

            var move = 0U;
            move = move.SetFromMove(48U);
            move = move.SetToMove(40U);
            move = move.SetMovingPiece(MoveUtility.BlackPawn);

            gameState.UnMakeMove(move);

            Assert.That(gameState.AllPieces, Is.EqualTo(MoveUtility.BitStates[48]), "All Pieces Bitboard");
        }

        [Test]
        public void UnMakeMove_Sets_EnPassant_Opposite_Pawn_Board_Correctly_When_Black()
        {
            var gameState = new GameState("8/8/8/8/8/p7/8/8 w - - 0 1", _zobristHash);
            gameState.PreviousGameStateRecords[gameState.TotalMoveCount] = new GameStateRecord();

            var move = 0U;
            move = move.SetFromMove(25U);
            move = move.SetToMove(16U);
            move = move.SetMovingPiece(MoveUtility.BlackPawn);
            move = move.SetCapturedPiece(MoveUtility.WhitePawn);
            move = move.SetPromotionPiece(MoveUtility.BlackPawn);

            gameState.UnMakeMove(move);
            Assert.That(gameState.BlackPawns, Is.EqualTo(MoveUtility.BitStates[25]));
            Assert.That(gameState.BlackPieces, Is.EqualTo(MoveUtility.BitStates[25]));
        }

        [Test]
        public void UnMakeMove_Sets_Black_Queen_Promotion_Piece_Correctly()
        {
            var gameState = new GameState("8/8/8/8/8/8/8/q7 w - - 0 1", _zobristHash);
            gameState.PreviousGameStateRecords[gameState.TotalMoveCount] = new GameStateRecord();

            var move = 0U;
            move = move.SetFromMove(8U);
            move = move.SetToMove(0U);
            move = move.SetMovingPiece(MoveUtility.BlackPawn);
            move = move.SetPromotionPiece(MoveUtility.BlackQueen);

            gameState.UnMakeMove(move);
            Assert.That(gameState.BlackPawns, Is.EqualTo(MoveUtility.BitStates[8]), "PawnBoard");
            Assert.That(gameState.BlackQueens, Is.EqualTo(0), "PromotionBoard");
        }

        [Test]
        public void UnMakeMove_Sets_Black_Knight_Promotion_Piece_Correctly()
        {
            var gameState = new GameState("8/8/8/8/8/8/8/n7 w - - 0 1", _zobristHash);
            gameState.PreviousGameStateRecords[gameState.TotalMoveCount] = new GameStateRecord();

            var move = 0U;
            move = move.SetFromMove(8U);
            move = move.SetToMove(0U);
            move = move.SetMovingPiece(MoveUtility.BlackPawn);
            move = move.SetPromotionPiece(MoveUtility.BlackKnight);

            gameState.UnMakeMove(move);
            Assert.That(gameState.BlackPawns, Is.EqualTo(MoveUtility.BitStates[8]), "PawnBoard");
            Assert.That(gameState.BlackKnights, Is.EqualTo(0), "PromotionBoard");
        }

        [Test]
        public void UnMakeMove_Sets_Black_Bishop_Promotion_Piece_Correctly()
        {
            var gameState = new GameState("8/8/8/8/8/8/8/b7 w - - 0 1", _zobristHash);
            gameState.PreviousGameStateRecords[gameState.TotalMoveCount] = new GameStateRecord();

            var move = 0U;
            move = move.SetFromMove(8U);
            move = move.SetToMove(0U);
            move = move.SetMovingPiece(MoveUtility.BlackPawn);
            move = move.SetPromotionPiece(MoveUtility.BlackBishop);

            gameState.UnMakeMove(move);
            Assert.That(gameState.BlackPawns, Is.EqualTo(MoveUtility.BitStates[8]), "PawnBoard");
            Assert.That(gameState.BlackBishops, Is.EqualTo(0), "PromotionBoard");
        }

        [Test]
        public void UnMakeMove_Sets_Black_Rook_Promotion_Piece_Correctly()
        {
            var gameState = new GameState("8/8/8/8/8/8/8/r7 w - - 0 1", _zobristHash);
            gameState.PreviousGameStateRecords[gameState.TotalMoveCount] = new GameStateRecord();

            var move = 0U;
            move = move.SetFromMove(8U);
            move = move.SetToMove(0U);
            move = move.SetMovingPiece(MoveUtility.BlackPawn);
            move = move.SetPromotionPiece(MoveUtility.BlackRook);

            gameState.UnMakeMove(move);
            Assert.That(gameState.BlackPawns, Is.EqualTo(MoveUtility.BitStates[8]), "PawnBoard");
            Assert.That(gameState.BlackRooks, Is.EqualTo(0), "PromotionBoard");
        }
        #endregion

        #region Board Array
        [TestCase("8/8/8/8/8/P7/8/8 b - - 0 1", 8U, 16U, MoveUtility.WhitePawn)]
        [TestCase("8/8/p7/8/8/8/8/8 w - - 0 1", 48U, 40U, MoveUtility.BlackPawn)]
        public void UnMakeMove_Sets_Board_Array_From_Square(string initialFen, uint fromSquare, uint toSquare, uint movingPiece)
        {
            var gameState = new GameState(initialFen, _zobristHash);
            gameState.PreviousGameStateRecords[gameState.TotalMoveCount] = new GameStateRecord();

            var move = 0U;
            move = move.SetFromMove(fromSquare);
            move = move.SetToMove(toSquare);
            move = move.SetMovingPiece(movingPiece);

            gameState.UnMakeMove(move);

            Assert.That(gameState.BoardArray[fromSquare], Is.EqualTo(movingPiece));
        }

        [TestCase("8/8/8/8/8/P7/8/8 b - - 0 1", 8U, 16U, MoveUtility.WhitePawn)]
        [TestCase("8/8/p7/8/8/8/8/8 w - - 0 1", 48U, 40U, MoveUtility.BlackPawn)]
        public void UnMakeMove_Sets_Board_Array_To_Square(string initialFen, uint fromSquare, uint toSquare, uint movingPiece)
        {
            var gameState = new GameState(initialFen, _zobristHash); 
            gameState.PreviousGameStateRecords[gameState.TotalMoveCount] = new GameStateRecord();

            var move = 0U;
            move = move.SetFromMove(fromSquare);
            move = move.SetToMove(toSquare);
            move = move.SetMovingPiece(movingPiece);

            gameState.UnMakeMove(move);

            Assert.That(gameState.BoardArray[toSquare], Is.EqualTo(MoveUtility.EmptyPiece));
        }

        [TestCase("8/8/P7/8/8/8/8/8 w - - 0 1", 33U, 40U, MoveUtility.WhitePawn, MoveUtility.BlackPawn)]
        [TestCase("8/8/8/8/8/p7/8/8 b - - 0 1", 25U, 16U, MoveUtility.BlackPawn, MoveUtility.WhitePawn)]
        public void UnMakeMove_Sets_Board_Array_To_Square_After_Enpassant_Capture(string initialFen, uint fromSquare, uint toSquare, uint movingPiece, uint capturedPiece)
        {
            var gameState = new GameState(initialFen, _zobristHash);
            gameState.PreviousGameStateRecords[gameState.TotalMoveCount] = new GameStateRecord();

            var move = 0U;
            move = move.SetFromMove(fromSquare);
            move = move.SetToMove(toSquare);
            move = move.SetMovingPiece(movingPiece);
            move = move.SetPromotionPiece(movingPiece);
            move = move.SetCapturedPiece(capturedPiece);

            gameState.UnMakeMove(move);
            Assert.That(gameState.BoardArray[fromSquare], Is.EqualTo(movingPiece));
        }

        [TestCase("8/8/P7/8/8/8/8/8 w - - 0 1", 33U, 40U, MoveUtility.WhitePawn, MoveUtility.BlackPawn)]
        [TestCase("8/8/8/8/8/p7/8/8 b - - 0 1", 25U, 16U, MoveUtility.BlackPawn, MoveUtility.WhitePawn)]
        public void UnMakeMove_Sets_Board_Array_From_Square_After_Enpassant_Capture(string initialFen, uint fromSquare, uint toSquare, uint movingPiece, uint capturedPiece)
        {
            var gameState = new GameState(initialFen, _zobristHash);
            gameState.PreviousGameStateRecords[gameState.TotalMoveCount] = new GameStateRecord();

            var move = 0U;
            move = move.SetFromMove(fromSquare);
            move = move.SetToMove(toSquare);
            move = move.SetMovingPiece(movingPiece);
            move = move.SetPromotionPiece(movingPiece);
            move = move.SetCapturedPiece(capturedPiece);

            gameState.UnMakeMove(move);
            Assert.That(gameState.BoardArray[toSquare], Is.EqualTo(MoveUtility.EmptyPiece));
        }

        [TestCase("pppppppp/pppppppp/Pppppppp/p1pppppp/pppppppp/pppppppp/pppppppp/pppppppp b - - 0 1", 33U, 40U, 32U, MoveUtility.WhitePawn, MoveUtility.BlackPawn)]
        [TestCase("PPPPPPPP/PPPPPPPP/PPPPPPPP/PPPPPPPP/1PPPPPPP/pPPPPPPP/PPPPPPPP/PPPPPPPP w - - 0 1", 24U, 17U, 25U, MoveUtility.BlackPawn, MoveUtility.WhitePawn)]
        public void UnMakeMove_Sets_Board_Array_Caputured_Square_After_Enpassant_Capture(string initialFen, uint fromSquare, uint toSquare, uint capturedSquare, uint movingPiece, uint capturedPiece)
        {
            var gameState = new GameState(initialFen, _zobristHash);
            gameState.PreviousGameStateRecords[gameState.TotalMoveCount] = new GameStateRecord();

            var move = 0U;
            move = move.SetFromMove(fromSquare);
            move = move.SetToMove(toSquare);
            move = move.SetMovingPiece(movingPiece);
            move = move.SetPromotionPiece(movingPiece);
            move = move.SetCapturedPiece(capturedPiece);

            gameState.UnMakeMove(move);
            Assert.That(gameState.BoardArray[capturedSquare], Is.EqualTo(capturedPiece));
        }

        [TestCase("Q7/8/8/8/8/8/8/8 b - - 0 1", 48U, 56U, MoveUtility.WhitePawn, MoveUtility.WhiteQueen)]
        [TestCase("R7/8/8/8/8/8/8/8 b - - 0 1", 48U, 56U, MoveUtility.WhitePawn, MoveUtility.WhiteRook)]
        [TestCase("N7/8/8/8/8/8/8/8 b - - 0 1", 48U, 56U, MoveUtility.WhitePawn, MoveUtility.WhiteKnight)]
        [TestCase("B7/8/8/8/8/8/8/8 b - - 0 1", 48U, 56U, MoveUtility.WhitePawn, MoveUtility.WhiteBishop)]
        [TestCase("8/8/8/8/8/8/8/q7 w - - 0 1", 48U, 56U, MoveUtility.BlackPawn, MoveUtility.BlackQueen)]
        [TestCase("8/8/8/8/8/8/8/r7 w - - 0 1", 48U, 56U, MoveUtility.BlackPawn, MoveUtility.BlackRook)]
        [TestCase("8/8/8/8/8/8/8/n7 w - - 0 1", 48U, 56U, MoveUtility.BlackPawn, MoveUtility.BlackKnight)]
        [TestCase("8/8/8/8/8/8/8/b7 w - - 0 1", 48U, 56U, MoveUtility.BlackPawn, MoveUtility.BlackBishop)]
        public void UnMakeMove_Sets_Board_Array_To_Square_On_Promotion(string fen, uint moveFrom, uint moveTo, uint movedPiece, uint promotedPiece)
        {
            var gameState = new GameState(fen, _zobristHash);
            gameState.PreviousGameStateRecords[gameState.TotalMoveCount] = new GameStateRecord();

            var move = 0U;
            move = move.SetFromMove(moveFrom);
            move = move.SetToMove(moveTo);
            move = move.SetMovingPiece(movedPiece);
            move = move.SetPromotionPiece(promotedPiece);

            gameState.UnMakeMove(move);

            Assert.That(gameState.BoardArray[moveFrom], Is.EqualTo(movedPiece));
        }

        [TestCase("Q7/8/8/8/8/8/8/8 b - - 0 1", 48U, 56U, MoveUtility.WhitePawn, MoveUtility.WhiteQueen)]
        [TestCase("R7/8/8/8/8/8/8/8 b - - 0 1", 48U, 56U, MoveUtility.WhitePawn, MoveUtility.WhiteRook)]
        [TestCase("N7/8/8/8/8/8/8/8 b - - 0 1", 48U, 56U, MoveUtility.WhitePawn, MoveUtility.WhiteKnight)]
        [TestCase("B7/8/8/8/8/8/8/8 b - - 0 1", 48U, 56U, MoveUtility.WhitePawn, MoveUtility.WhiteBishop)]
        [TestCase("8/8/8/8/8/8/8/q7 w - - 0 1", 48U, 56U, MoveUtility.BlackPawn, MoveUtility.BlackQueen)]
        [TestCase("8/8/8/8/8/8/8/r7 w - - 0 1", 48U, 56U, MoveUtility.BlackPawn, MoveUtility.BlackRook)]
        [TestCase("8/8/8/8/8/8/8/n7 w - - 0 1", 48U, 56U, MoveUtility.BlackPawn, MoveUtility.BlackKnight)]
        [TestCase("8/8/8/8/8/8/8/b7 w - - 0 1", 48U, 56U, MoveUtility.BlackPawn, MoveUtility.BlackBishop)]
        public void UnMakeMove_Sets_Board_Array_From_Square_On_Promotion(string fen, uint moveFrom, uint moveTo, uint movedPiece, uint promotedPiece)
        {
            var gameState = new GameState(fen, _zobristHash);
            gameState.PreviousGameStateRecords[gameState.TotalMoveCount] = new GameStateRecord();
           
            var move = 0U;
            move = move.SetFromMove(moveFrom);
            move = move.SetToMove(moveTo);
            move = move.SetMovingPiece(movedPiece);
            move = move.SetPromotionPiece(promotedPiece);

            gameState.UnMakeMove(move);

            Assert.That(gameState.BoardArray[moveTo], Is.EqualTo(MoveUtility.EmptyPiece));
        }

        [TestCase("8/8/8/8/8/3P4/3q4/8 w - - 0 1", MoveUtility.BlackQueen, MoveUtility.WhitePawn)]
        [TestCase("8/8/8/8/8/3p4/3Q4/8 w - - 0 1", MoveUtility.WhiteQueen, MoveUtility.BlackPawn)]
        public void UnMakeMove_Sets_Board_Array_To_Square_When_Capture(string initialFen, uint movingPiece, uint capturedPiece)
        {
            var gameState = new GameState(initialFen, _zobristHash);
            gameState.PreviousGameStateRecords[gameState.TotalMoveCount] = new GameStateRecord();

            var move = 0U;
            move = move.SetFromMove(11U);
            move = move.SetToMove(19U);
            move = move.SetMovingPiece(movingPiece);
            move = move.SetCapturedPiece(capturedPiece);

            gameState.UnMakeMove(move);

            Assert.That(gameState.BoardArray[19U], Is.EqualTo(capturedPiece));
        }
        #endregion
    }
}
