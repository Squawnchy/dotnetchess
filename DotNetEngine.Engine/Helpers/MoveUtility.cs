﻿using System;
using System.Diagnostics;
using System.Text;

namespace DotNetEngine.Engine.Helpers
{
    /// <summary>
    /// Helper functions used when dealing with a representation of a single move.
    /// A single move is contained inside a single uint.
    /// 
    /// [MoveFrom] [MoveTo] [MovingPiece] [CapturedPiece] [PromotionPiece]
    /// 000000 000000 0000 0000 0000
    /// 
    /// MoveFrom - 6 bits 0-63 
    /// MoveTo - 6 bits 0-63
    /// MovingPiece - 4 bits 0-7 white 9=15 black
    /// CapturePiece - 4 bits 0-7 white 9=15 black
    /// PromotionPiece - 4 bits 0-7 white 9=15 black
    /// </summary>
    internal static class MoveUtility
    {
        /// <summary>
        /// Magic numbers used to get the first piece from the bitboard
        /// </summary>
        private static readonly uint[] _firstPieceArray =
        {
            0, 47, 1, 56, 48, 27, 2, 60,
            57, 49, 41, 37, 28, 16, 3, 61,
            54, 58, 35, 52, 50, 42, 21, 44,
            38, 32, 29, 23, 17, 11, 4, 62,
            46, 55, 26, 59, 40, 36, 15, 53,
            34, 51, 20, 43, 31, 22, 10, 45,
            25, 39, 14, 33, 19, 30, 9, 24,
            13, 18, 8, 12, 7, 6, 5, 63
        };

        #region Internal Fields

        internal const uint EmptyPiece = 0; // 0000
        internal const uint WhitePawn = 1; // 0001
        internal const uint WhiteKing = 2; // 0010
        internal const uint WhiteKnight = 3; // 0011
        internal const uint WhiteBishop = 5; // 0101
        internal const uint WhiteRook = 6; // 0110
        internal const uint WhiteQueen = 7; // 0111
        internal const uint BlackPawn = 9; // 1001
        internal const uint BlackKing = 10; // 1010
        internal const uint BlackKnight = 11; // 1011
        internal const uint BlackBishop = 13; // 1101
        internal const uint BlackRook = 14; // 1110
        internal const uint BlackQueen = 15; // 1111

        /// <summary>
        /// The representation of each bit in the ulong. 
        /// Bitboards use Little-Endian File-Rank Mapping a1 is position 0 and h8 is 63 
        /// </summary>
        internal static ulong[] BitStates =
        {
            1UL, 2UL, 4UL, 8UL, 16UL, 32UL, 64UL, 128UL, 256UL, 512UL, 1024UL, 2048UL, 4096UL, 8192UL, 16384UL, 32768UL,
            65536UL, 131072UL, 262144UL, 524288UL, 1048576UL, 2097152UL, 4194304UL, 8388608UL, 16777216UL, 33554432UL,
            67108864UL, 134217728UL, 268435456UL, 536870912UL, 1073741824UL, 2147483648UL, 4294967296UL, 8589934592UL,
            17179869184UL, 34359738368UL, 68719476736UL, 137438953472UL, 274877906944UL, 549755813888UL,
            1099511627776UL, 2199023255552UL, 4398046511104UL, 8796093022208UL, 17592186044416UL, 35184372088832UL,
            70368744177664UL, 140737488355328UL, 281474976710656UL, 562949953421312UL, 1125899906842624UL,
            2251799813685248UL, 4503599627370496UL, 9007199254740992UL, 18014398509481984UL, 36028797018963968UL,
            72057594037927936UL, 144115188075855872UL, 288230376151711744UL, 576460752303423488UL,
            1152921504606846976UL, 2305843009213693952UL, 4611686018427387904UL, 9223372036854775808UL
        };

        /// <summary>
        /// The rank of each position on the board
        /// </summary>
        internal static int[] Ranks =
        {
            1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 4, 4, 4, 4,
            4, 4, 4, 4, 5, 5, 5, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 6, 6, 6, 7, 7, 7, 7, 7, 7, 7, 7,
            8, 8, 8, 8, 8, 8, 8, 8
        };

        /// <summary>
        /// The file of each position on the board
        /// </summary>
        internal static int[] Files =
        {
            1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3,
            4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8
        };

        /// <summary>
        /// The rank and file of each position on the board in chess notation
        /// </summary>
        internal static string[] RankAndFile =
        {
            "A1", "B1", "C1", "D1", "E1", "F1", "G1", "H1",
            "A2", "B2", "C2", "D2", "E2", "F2", "G2", "H2",
            "A3", "B3", "C3", "D3", "E3", "F3", "G3", "H3",
            "A4", "B4", "C4", "D4", "E4", "F4", "G4", "H4",
            "A5", "B5", "C5", "D5", "E5", "F5", "G5", "H5",
            "A6", "B6", "C6", "D6", "E6", "F6", "G6", "H6",
            "A7", "B7", "C7", "D7", "E7", "F7", "G7", "H7",
            "A8", "B8", "C8", "D8", "E8", "F8", "G8", "H8"
        };

        /// <summary>
        /// A Translation of rank and file to its index on the board
        /// </summary>
        internal static int[][] BoardIndex =
        {
            new[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new[] {0, 0, 1, 2, 3, 4, 5, 6, 7},
            new[] {0, 8, 9, 10, 11, 12, 13, 14, 15},
            new[] {0, 16, 17, 18, 19, 20, 21, 22, 23},
            new[] {0, 24, 25, 26, 27, 28, 29, 30, 31},
            new[] {0, 32, 33, 34, 35, 36, 37, 38, 39},
            new[] {0, 40, 41, 42, 43, 44, 45, 46, 47},
            new[] {0, 48, 49, 50, 51, 52, 53, 54, 55},
            new[] {0, 56, 57, 58, 59, 60, 61, 62, 63}
        };

        /// <summary>
        /// The give rank of a row if it was shifted up by a single rank
        /// </summary>
        internal static int[] ShiftedRank =
        {
            1, 1, 1, 1, 1, 1, 1, 1,
            9, 9, 9, 9, 9, 9, 9, 9,
            17, 17, 17, 17, 17, 17, 17, 17,
            25, 25, 25, 25, 25, 25, 25, 25,
            33, 33, 33, 33, 33, 33, 33, 33,
            41, 41, 41, 41, 41, 41, 41, 41,
            49, 49, 49, 49, 49, 49, 49, 49,
            57, 57, 57, 57, 57, 57, 57, 57
        };

        /// <summary>
        /// Magic multiplication used when generating File moves
        /// </summary>
        internal static ulong[] FileMagicMultiplication =
        {
            9241421688590303744, 4620710844295151872, 2310355422147575936, 1155177711073787968, 577588855536893984,
            288794427768446992, 144397213884223496, 72198606942111748, 9241421688590303744, 4620710844295151872,
            2310355422147575936, 1155177711073787968, 577588855536893984, 288794427768446992, 144397213884223496,
            72198606942111748, 9241421688590303744, 4620710844295151872, 2310355422147575936, 1155177711073787968,
            577588855536893984, 288794427768446992, 144397213884223496, 72198606942111748, 9241421688590303744,
            4620710844295151872, 2310355422147575936, 1155177711073787968, 577588855536893984, 288794427768446992,
            144397213884223496, 72198606942111748, 9241421688590303744, 4620710844295151872, 2310355422147575936,
            1155177711073787968, 577588855536893984, 288794427768446992, 144397213884223496, 72198606942111748,
            9241421688590303744, 4620710844295151872, 2310355422147575936, 1155177711073787968, 577588855536893984,
            288794427768446992, 144397213884223496, 72198606942111748, 9241421688590303744, 4620710844295151872,
            2310355422147575936, 1155177711073787968, 577588855536893984, 288794427768446992, 144397213884223496,
            72198606942111748, 9241421688590303744, 4620710844295151872, 2310355422147575936, 1155177711073787968,
            577588855536893984, 288794427768446992, 144397213884223496, 72198606942111748
        };

        /// <summary>
        /// Magic multiplication used when generating diagonal A1H8 moves
        /// </summary>
        internal static ulong[] DiagonalA1H8MagicMultiplication =
        {
            72340172838076672, 36170086419038336, 18085043209519168, 9042521604759584, 4521260802379792,
            2260630401189896, 0, 0, 72340172838076672, 72340172838076672, 36170086419038336, 18085043209519168,
            9042521604759584, 4521260802379792, 2260630401189896, 0, 72340172838076672, 72340172838076672,
            72340172838076672, 36170086419038336, 18085043209519168, 9042521604759584, 4521260802379792,
            2260630401189896, 72340172838076672, 72340172838076672, 72340172838076672, 72340172838076672,
            36170086419038336, 18085043209519168, 9042521604759584, 4521260802379792, 72340172838076672,
            72340172838076672, 72340172838076672, 72340172838076672, 72340172838076672, 36170086419038336,
            18085043209519168, 9042521604759584, 72340172838076672, 72340172838076672, 72340172838076672,
            72340172838076672, 72340172838076672, 72340172838076672, 36170086419038336, 18085043209519168, 0,
            72340172838076672, 72340172838076672, 72340172838076672, 72340172838076672, 72340172838076672,
            72340172838076672, 36170086419038336, 0, 0, 72340172838076672, 72340172838076672, 72340172838076672,
            72340172838076672, 72340172838076672, 72340172838076672
        };

        /// <summary>
        /// Magic multiplication used when generating diagonal A8H1 moves
        /// </summary>
        internal static ulong[] DiagonalA8H1MagicMultiplication =
        {
            0, 0, 72340172838076672, 72340172838076672, 72340172838076672, 72340172838076672, 72340172838076672,
            72340172838076672, 0, 72340172838076672, 72340172838076672, 72340172838076672, 72340172838076672,
            72340172838076672, 72340172838076672, 36170086419038336, 72340172838076672, 72340172838076672,
            72340172838076672, 72340172838076672, 72340172838076672, 72340172838076672, 36170086419038336,
            18085043209519168, 72340172838076672, 72340172838076672, 72340172838076672, 72340172838076672,
            72340172838076672, 36170086419038336, 18085043209519168, 9042521604759584, 72340172838076672,
            72340172838076672, 72340172838076672, 72340172838076672, 36170086419038336, 18085043209519168,
            9042521604759584, 4521260802379792, 72340172838076672, 72340172838076672, 72340172838076672,
            36170086419038336, 18085043209519168, 9042521604759584, 4521260802379792, 2260630401189896,
            72340172838076672, 72340172838076672, 36170086419038336, 18085043209519168, 9042521604759584,
            4521260802379792, 2260630401189896, 0, 72340172838076672, 36170086419038336, 18085043209519168,
            9042521604759584, 4521260802379792, 2260630401189896, 0, 0
        };

        #endregion

        #region internal Methods

        /// <summary>
        /// Implementation of bitscanforward.
        /// Authored by Kim Walish 
        /// </summary>
        /// <param name="board"></param>
        /// <see cref="http://chessprogramming.wikispaces.com/BitScan"/>
        /// <returns>The lsb that is set on a board</returns>
        internal static uint GetFirstPieceFromBitBoard(this ulong board)
        {
            const ulong debruijn = 0x03f79d71b4cb0a89;
            Debug.Assert(board != 0);

            return _firstPieceArray[((board ^ (board - 1))*debruijn) >> 58];
        }

        /// <summary>
        /// Determines if a move was made by white or black
        /// </summary>
        /// <param name="move">The move being checked</param>
        /// <returns>A bool value indicating if a move was made by white or black</returns>
        internal static bool IsWhiteMove(this uint move)
        {
            return (~move & 0x00008000) == 0x00008000;
        }

        /// <summary>
        /// Determines if a move is an enpassant
        /// </summary>
        /// <param name="move">The move being checked</param>
        /// <returns>A bool value indicating if a move is an enpassant</returns>
        internal static bool IsEnPassant(this uint move)
        {
            return (move & 0x00700000) == 0x00100000;
        }

        /// <summary>
        /// Determines if during a move, a pawn moved
        /// </summary>
        /// <param name="move">The move being checked</param>
        /// <returns>A bool value indicating if a pawn moved</returns>
        internal static bool IsPawnMoved(this uint move)
        {
            return (move & 0x00007000) == 0x00001000;
        }

        /// <summary>
        /// Determines if during a move, a rook moved
        /// </summary>
        /// <param name="move">The move being checked</param>
        /// <returns>A bool value indicating if a rook moved</returns>
        internal static bool IsRookMoved(this uint move)
        {
            return (move & 0x00007000) == 0x00006000;
        }

        /// <summary>
        /// Determines if during a move, a king moved
        /// </summary>
        /// <param name="move">The move being checked</param>
        /// <returns>A bool value indicating if a king moved</returns>
        internal static bool IsKingMoved(this uint move)
        {
            return (move & 0x00007000) == 0x00002000;
        }

        /// <summary>
        /// Determines if during a move, a pawn moved two squares
        /// </summary>
        /// <param name="move">The move being checked</param>
        /// <returns>A bool value indicating if a pawn moved two squares</returns>
        internal static bool IsPawnDoubleMoved(this uint move)
        {
            return (((move & 0x00007000) == 0x00001000) &&
                    ((((move & 0x00000038) == 0x00000008) && (((move & 0x00000e00) == 0x00000600))) ||
                     (((move & 0x00000038) == 0x00000030) && (((move & 0x00000e00) == 0x00000800)))));
        }

        /// <summary>
        /// Determines if during a move, if any piece was captured
        /// </summary>
        /// <param name="move">The move being checked</param>
        /// <returns>A bool value indicating if any piece was captured</returns>
        internal static bool IsPieceCaptured(this uint move)
        {
            return (move & 0x000f0000) != 0x00000000;
        }

        /// <summary>
        /// Determines if during a move, if a king was captured
        /// </summary>
        /// <param name="move">The move being checked</param>
        /// <returns>A bool value indicating if a king was captured</returns>
        internal static bool IsKingCaptured(this uint move)
        {
            return (move & 0x00070000) == 0x00020000;
        }

        /// <summary>
        /// Determines if during a move, if a rook was captured
        /// </summary>
        /// <param name="move">The move being checked</param>
        /// <returns>A bool value indicating if a rook was captured</returns>
        internal static bool IsRookCaptured(this uint move)
        {
            return (move & 0x00070000) == 0x00060000;
        }

        /// <summary>
        /// Determines if during a move, either castle occurred
        /// </summary>
        /// <param name="move">The move being checked</param>
        /// <returns>A bool value indicating if either castle occurred</returns>
        internal static bool IsCastle(this uint move)
        {
            return (move & 0x00700000) == 0x00200000;
        }

        /// <summary>
        /// Determines if during a move, a O-O castle occurred
        /// </summary>
        /// <param name="move">The move being checked</param>
        /// <returns>A bool value indicating if a O-O castle occurred</returns>
// ReSharper disable InconsistentNaming
        internal static bool IsCastleOO(this uint move)
// ReSharper restore InconsistentNaming
        {
            return (move & 0x007001c0) == 0x00200180;
        }

        /// <summary>
        /// Determines if during a move, a O-O-O castle occurred
        /// </summary>
        /// <param name="move">The move being checked</param>
        /// <returns>A bool value indicating if a O-O-O castle occurred</returns>
// ReSharper disable InconsistentNaming
        internal static bool IsCastleOOO(this uint move)
// ReSharper restore InconsistentNaming
        {
            return (move & 0x007001c0) == 0x00200080;
        }

        /// <summary>
        /// Determines if during a move, a promotion of a piece occurred
        /// </summary>
        /// <param name="move">The move being checked</param>
        /// <returns>A bool value indicating if a promotion occurred</returns>
        internal static bool IsPromotion(this uint move)
        {
            return (move & 0x00700000) > 0x00200000;
        }

        /// <summary>
        /// Gets the bit state by rank and file
        /// </summary>
        /// <param name="rank">The rank</param>
        /// <param name="file">The file</param>
        /// <returns></returns>
        internal static ulong GetBitStatesByBoardIndex(int rank, int file)
        {
            return BitStates[BoardIndex[rank][file]];
        }

        /// <summary>
        /// Sets the promotion piece
        /// </summary>
        /// <param name="move">The move being modified</param>
        /// <param name="promotionPiece">The piece being promoted</param>
        /// <returns>a modified move</returns>
        internal static uint SetPromotionPiece(this uint move, uint promotionPiece)
        {
            move &= 0xff0fffff;
            move |= (promotionPiece & 0x0000000f) << 20;
            return move;
        }

        /// <summary>
        /// Sets the promotion piece
        /// </summary>
        /// <param name="move">The move being modified</param>
        /// <param name="capturedPiece">The piece being captured</param>
        /// <returns>a modified move</returns>
        internal static uint SetCapturedPiece(this uint move, uint capturedPiece)
        {
            move &= 0xfff0ffff;
            move |= (capturedPiece & 0x0000000f) << 16;
            return move;
        }

        /// <summary>
        /// Sets the from move
        /// </summary>
        /// <param name="move">The move being modified</param>
        /// <param name="moveFrom">The move from square</param>
        /// <returns>a modified move</returns>
        internal static uint SetFromMove(this uint move, uint moveFrom)
        {
            move &= 0xffffffc0;
            move |= (moveFrom & 0x0000003f);
            return move;
        }

        /// <summary>
        /// Sets the to move
        /// </summary>
        /// <param name="move">The move being modified</param>
        /// <param name="moveTo">The move from square</param>
        /// <returns>a modified move</returns>
        internal static uint SetToMove(this uint move, uint moveTo)
        {
            move &= 0xfffff03f;
            move |= (moveTo & 0x0000003f) << 6;
            return move;
        }

        /// <summary>
        /// Sets the piece that is moving
        /// </summary>
        /// <param name="move">The move being modified</param>
        /// <param name="movingpiece">The moving piece</param>
        /// <returns>a modified move</returns>
        internal static uint SetMovingPiece(this uint move, uint movingpiece)
        {
            move &= 0xffff0fff;
            move |= (movingpiece & 0x0000000f) << 12;
            return move;
        }

        /// <summary>
        /// Gets the from square from a move
        /// </summary>
        /// <param name="move">The move we are getting the from square from</param>
        /// <returns>The from square</returns>
        internal static uint GetFromMove(this uint move)
        {
            return (move & 0x0000003f);
        }

        /// <summary>
        /// Gets the to square from a move
        /// </summary>
        /// <param name="move">The move we are getting the to square from</param>
        /// <returns>The to square</returns>
        internal static uint GetToMove(this uint move)
        {
            return (move >> 6) & 0x0000003f;
        }

        /// <summary>
        /// Gets the moving piece square from a move
        /// </summary>
        /// <param name="move">The move we are getting the moving piece from</param>
        /// <returns>The moving piece</returns>
        internal static uint GetMovingPiece(this uint move)
        {
            return (move >> 12) & 0x0000000f;
        }

        /// <summary>
        /// Gets the captured piece square from a move
        /// </summary>
        /// <param name="move">The move we are getting the captured piece from</param>
        /// <returns>The captured piece</returns>
        internal static uint GetCapturedPiece(this uint move)
        {
            return (move >> 16) & 0x0000000f;
        }

        /// <summary>
        /// Gets the promoted piece square from a move
        /// </summary>
        /// <param name="move">The move we are getting the promoted piece from</param>
        /// <returns>The promoted piece</returns>
        internal static uint GetPromotedPiece(this uint move)
        {
            return (move >> 20) & 0x0000000f;
        }

        /// <summary>
        /// Converts a move index to its chess notation IE 0 = A1
        /// </summary>
        /// <param name="moveIndex">The index we are converting</param>
        /// <returns>A string representtion of a move index</returns>
        internal static string ToRankAndFile(this uint moveIndex)
        {
            return RankAndFile[moveIndex];
        }

        internal static string ToPromotionString(this uint move)
        {
            switch (move)
            {
                case BlackBishop:
                case WhiteBishop:
                    return "b";
                   
                case BlackKnight:
                case WhiteKnight:
                    return "n";
                case BlackQueen:
                case WhiteQueen:
                    return "q";
                case BlackRook:
                case WhiteRook:
                    return "r";
                default:
                throw new InvalidOperationException("Not a Valid Promotion");
            }
        }

        /// <summary>
        /// Trys to convers a chess notation move to a move
        /// </summary>
        /// <param name="moveText">The move text we are converting</param>
        /// <param name="whiteToMove">Is the move for white?</param>
        /// <param name="move">the move we are outputting</param>
        /// <returns>A boolean value that determines if the conversion was successful</returns>
        internal static bool TryConvertStringToMove(string moveText, bool whiteToMove, out uint move)
        {
            move = uint.MinValue;
            if (moveText.Length < 4 || moveText.Length > 5)
                return false;

            var fromMove = Array.IndexOf(RankAndFile, moveText.Substring(0, 2));
            var toMove = Array.IndexOf(RankAndFile, moveText.Substring(2, 2));
            var promotion = EmptyPiece;

            //Promotions
            if (moveText.Length == 5)
            {
                switch (moveText.Substring(4, 1).ToUpper())
                {
                    case "Q":
                    {
                        promotion = whiteToMove ? WhiteQueen : BlackQueen;
                        break;
                    }
                    case "R":
                    {
                        promotion = whiteToMove ? WhiteRook : BlackRook;
                        break;
                    }
                    case "B":
                    {
                        promotion = whiteToMove ? WhiteBishop : BlackBishop;
                        break;
                    }
                    case "N":
                    {
                        promotion = whiteToMove ? WhiteKnight : BlackKnight;
                        break;
                    }
                }

            }

            move = move.SetToMove((uint) toMove);
            move = move.SetFromMove((uint) fromMove);
            move = move.SetPromotionPiece(promotion);
            return true;
        }

        internal static string ToMoveString(this uint move)
        {
            return string.Format("{0}{1}{2}", move.GetFromMove().ToRankAndFile(), move.GetToMove().ToRankAndFile(),
                move.IsPromotion() ? move.GetPromotedPiece().ToPromotionString() : string.Empty).ToLower();
        }

        internal static string ToMoveListString(this uint[] moves, int depth)
        {
            var sb = new StringBuilder();
            
            for (var i = 0; i < depth; i++)          
            {
                sb.Append(moves[i] == 0 ? "*" : moves[i].ToMoveString());
                sb.Append(" ");
            }

            return sb.ToString();
        }
        #endregion
    }
}