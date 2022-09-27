﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PonzianiComponents.Chesslib
{
    /// <summary>
    /// This class provides constants and static functions to manage FEN strings (see https://chessprogramming.wikispaces.com/Forsyth-Edwards+Notation)
    /// </summary>
    public class Fen
    {
        /// <summary>
        /// FEN Representation of the starting position in standard chess
        /// </summary>
        public const string INITIAL_POSITION = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

        /// <summary>
        /// Character representing the white king in <see href="https://chessprogramming.wikispaces.com/Forsyth-Edwards+Notation">Forsyth-Edwards-Notation (FEN)</see> 
        /// </summary>
        public const char PIECE_CHAR_WKING = 'K';
        /// <summary>
        /// Character representing the white king in <see href="https://chessprogramming.wikispaces.com/Forsyth-Edwards+Notation">Forsyth-Edwards-Notation (FEN)</see> 
        /// </summary>
        public const char PIECE_CHAR_BKING = 'k';
        /// <summary>
        /// Character representing the white king in <see href="https://chessprogramming.wikispaces.com/Forsyth-Edwards+Notation">Forsyth-Edwards-Notation (FEN)</see> 
        /// </summary>
        public const char PIECE_CHAR_WQUEEN = 'Q';
        /// <summary>
        /// Character representing the black king in <see href="https://chessprogramming.wikispaces.com/Forsyth-Edwards+Notation">Forsyth-Edwards-Notation (FEN)</see> 
        /// </summary>
        public const char PIECE_CHAR_BQUEEN = 'q';
        /// <summary>
        /// Character representing the white queen in <see href="https://chessprogramming.wikispaces.com/Forsyth-Edwards+Notation">Forsyth-Edwards-Notation (FEN)</see> 
        /// </summary>
        public const char PIECE_CHAR_WROOK = 'R';
        /// <summary>
        /// Character representing the black queen in <see href="https://chessprogramming.wikispaces.com/Forsyth-Edwards+Notation">Forsyth-Edwards-Notation (FEN)</see> 
        /// </summary>
        public const char PIECE_CHAR_BROOK = 'r';
        /// <summary>
        /// Character representing the white bishop in <see href="https://chessprogramming.wikispaces.com/Forsyth-Edwards+Notation">Forsyth-Edwards-Notation (FEN)</see> 
        /// </summary>
        public const char PIECE_CHAR_WBISHOP = 'B';
        /// <summary>
        /// Character representing the black bishop in <see href="https://chessprogramming.wikispaces.com/Forsyth-Edwards+Notation">Forsyth-Edwards-Notation (FEN)</see> 
        /// </summary>
        public const char PIECE_CHAR_BBISHOP = 'b';
        /// <summary>
        /// Character representing the white knight in <see href="https://chessprogramming.wikispaces.com/Forsyth-Edwards+Notation">Forsyth-Edwards-Notation (FEN)</see> 
        /// </summary>
        public const char PIECE_CHAR_WKNIGHT = 'N';
        /// <summary>
        /// Character representing the black knight in <see href="https://chessprogramming.wikispaces.com/Forsyth-Edwards+Notation">Forsyth-Edwards-Notation (FEN)</see> 
        /// </summary>
        public const char PIECE_CHAR_BKNIGHT = 'n';
        /// <summary>
        /// Character representing the white pawn in <see href="https://chessprogramming.wikispaces.com/Forsyth-Edwards+Notation">Forsyth-Edwards-Notation (FEN)</see> 
        /// </summary>
        public const char PIECE_CHAR_WPAWN = 'P';
        /// <summary>
        /// Character representing the black pawn in <see href="https://chessprogramming.wikispaces.com/Forsyth-Edwards+Notation">Forsyth-Edwards-Notation (FEN)</see> 
        /// </summary>
        public const char PIECE_CHAR_BPAWN = 'p';
        /// <summary>
        /// Character representing no piece (empty square)
        /// </summary>
        public const char PIECE_CHAR_NONE = ' ';

        /// <summary>
        /// Get's the char used for the piece (Example 'q' for black queen or 'P' for White pawn)
        /// </summary>
        /// <param name="piece"></param>
        /// <returns></returns>
        public static char PieceChar(Piece piece) { return "QqRrBbNnPpKk "[(int)piece]; }
        /// <summary>
        /// Get's the piece for a piece characteristic (Example Black Queen for 'q' and White Pawn for 'p'
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static Piece ParsePieceChar(char c) { return (Piece)("QqRrBbNnPpKk ".IndexOf(c)); }

        /// <summary>
        /// Calulates an Array of Piece Characters represented by the provided string in FEN Notation. The array representation used
        /// is A1 = 0, H1 = 7, H8 = 63
        /// </summary>
        /// <param name="fen">The position in FEN representation</param>
        /// <returns></returns>
        public static char[] GetPieceArray(string fen)
        {
            char[] board = new char[64];
            for (int i = 0; i < 64; ++i) board[i] = PIECE_CHAR_NONE;
            int index = fen.IndexOf(' ');
            string position = index > 0 ? fen.Substring(0, index) : fen;
            string[] rows = position.Split(new char[] { '/' });
            for (int row = 7; row >= 0; --row)
            {
                int col = 0;
                foreach (char c in rows[7 - row])
                {
                    if ((int)c >= (int)'1' && (int)c <= (int)'8') col += (int)c - (int)'0';
                    else
                    {
                        board[8 * row + col] = c;
                        col++;
                    }
                }
            }
            return board;
        }
        /// <summary>
        /// Creates the first part of a FEN string from a board array
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public static string FenPartFromBoard(char[] board)
        {
            StringBuilder sb = new();
            for (int row = 7; row >= 0; --row)
            {
                int count_space = 0;
                for (int file = 0; file <= 7; ++file)
                {
                    int s = 8 * row + file;
                    if (board[s] == PIECE_CHAR_NONE) ++count_space;
                    else
                    {
                        if (count_space > 0)
                        {
                            sb.Append(count_space);
                            count_space = 0;
                        }
                        sb.Append(board[s]);
                    }
                    if (file == 7 && count_space > 0) sb.Append(count_space);
                    if (file == 7 && row > 0) sb.Append('/');
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// Checks if a given string is a valid Fen string. This means that the fen can be parsed and 
        /// a position can be determined, but not that the position is legal. Fens without 50 move counter and
        /// movenumber (as used in EPD) are accepted
        /// </summary>
        /// <param name="fen">Fen to be checked</param>
        /// <returns>treu, if valid</returns>
        public static bool CheckValid(string fen)
        {
            return RegexFen.IsMatch(fen);
        }

        /// <summary>
        /// Parses a square provided by it's field name (such as 'a1', 'e5' or 'h8') 
        /// </summary>
        /// <param name="squareString">The square as string</param>
        /// <returns>The square defined by <paramref name="squareString"/></returns>
        /// <remarks>This method doesn't make any error checking. If invalid input is provided
        /// this might result in a crash</remarks>
        public static Square ParseSquare(string squareString)
        {
            int file = (int)squareString[0] - (int)'a';
            int rank = Int32.Parse(squareString.Substring(1, 1)) - 1;
            return (Square)(8 * rank + file);
        }

        /// <summary>
        /// Creates a fen string representing an equivalent position with colors reversed
        /// </summary>
        /// <param name="fen">the positions which shall be swapped</param>
        /// <returns>fen string of the swapped position</returns>
        public static string Swap(string fen)
        {
            string[] token = fen.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string[] rows = token[0].Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sb = new();
            for (int i = 7; i >= 0; --i)
            {
                foreach (char c in rows[i])
                {
                    if (Char.IsLetter(c)) sb.Append(Char.IsUpper(c) ? Char.ToLower(c) : Char.ToUpper(c));
                    else sb.Append(c);
                }
                if (i > 0) sb.Append('/');
            }
            sb.Append(' ');
            sb.Append(token[1][0] == 'w' ? 'b' : 'w');
            sb.Append(' ');
            if (token[2][0] == '-') sb.Append("- ");
            else
            {
                string s1 = "kqKQ";
                string s2 = "KQkq";
                for (int i = 0; i < 4; ++i)
                {
                    if (token[2].Contains(s1[i])) sb.Append(s2[i]);
                }
                sb.Append(' ');
            }
            if (token[3][0] == '-') sb.Append('-');
            else
            {
                if (token[1][0] == 'w')
                    sb.Append(token[3].Replace('6', '3'));
                else
                    sb.Append(token[3].Replace('3', '6'));
            }
            for (int i = 4; i < token.Length; ++i) sb.Append(' ').Append(token[i]);
            return sb.ToString();
        }

        internal static Dictionary<char, CastleFlag> CastleFlagMapping = new() { { 'K', CastleFlag.W0_0 }, { 'Q', CastleFlag.W0_0_0 }, { 'k', CastleFlag.B0_0 }, { 'q', CastleFlag.B0_0_0 }, { '-', CastleFlag.NONE } };
        internal static Dictionary<char, Piece> PieceMapping = new() {
            { 'Q', Piece.WQUEEN }, { 'R', Piece.WROOK }, { 'B', Piece.WBISHOP }, { 'N', Piece.WKNIGHT }, { 'P', Piece.WPAWN }, { 'K', Piece.WKING },
            { 'q', Piece.BQUEEN }, { 'r', Piece.BROOK }, { 'b', Piece.BBISHOP }, { 'n', Piece.BKNIGHT }, { 'p', Piece.BPAWN }, { 'k', Piece.BKING }};

        private static Regex RegexFen = new Regex(@"^(?:[pnbrqkPNBRQK1-8]+/){7}[pnbrqkPNBRQK1-8]+\s(?:b|w)\s(?:-|K?Q?k?q)\s(?:-|[a-h][3-6])(?:\s+(?:\d+)){0,2}$", RegexOptions.Compiled);
    }
}
