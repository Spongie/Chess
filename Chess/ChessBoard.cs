using System;
using System.Collections.Generic;
using System.Linq;
using Chess.Pieces;
using Newtonsoft.Json;

namespace Chess
{
    [Serializable]
    public class ChessBoard
    {
        private const int BoardSize = 8;
        private bool isWhiteInCheck = false;
        private bool isBlackInCheck = false;

        public ChessBoard()
        {
            Board = new Piece[BoardSize, BoardSize];
            Winner = new Winner { Color = Color.White, HasWinner = false };

            GenerateBlackSide();
            GenerateWhiteSide();
        }
        
        public Piece[,] Board { get; set; }

        private void GenerateWhiteSide()
        {
            Board[7, 0] = new Rook(Color.White);
            Board[7, 1] = new Knight(Color.White);
            Board[7, 2] = new Bishop(Color.White);
            Board[7, 3] = new Queen(Color.White);
            Board[7, 4] = new King(Color.White);
            Board[7, 5] = new Bishop(Color.White);
            Board[7, 6] = new Knight(Color.White);
            Board[7, 7] = new Rook(Color.White);

            for (int i = 0; i < BoardSize; i++)
            {
                Board[6, i] = new Pawn(Color.White);
            }
        }

        private void GenerateBlackSide()
        {
            Board[0, 0] = new Rook(Color.Black);
            Board[0, 1] = new Knight(Color.Black);
            Board[0, 2] = new Bishop(Color.Black);
            Board[0, 3] = new Queen(Color.Black);
            Board[0, 4] = new King(Color.Black);
            Board[0, 5] = new Bishop(Color.Black);
            Board[0, 6] = new Knight(Color.Black);
            Board[0, 7] = new Rook(Color.Black);

            for (int i = 0; i < BoardSize; i++)
            {
                Board[1, i] = new Pawn(Color.Black);
            }
        }

        public ChessBoard CopyWithMove(Move move)
        {
            var board = JsonCopy();

            board.MakeMove(move);

            return board;
        }

        public Piece[,] GetBoardAfterMove(Move move)
        {
            var newBoard = (Piece[,])Board.Clone();
            var position = GetPiecePosition(move.Piece);

            var oldId = move.Piece.Id;

            if (move.Piece is Pawn && move.Piece.Color == Color.White && move.TargetPosition.Y == 0)
                move.Piece = new Queen(move.Piece.Color, oldId);

            if (move.Piece is Pawn && move.Piece.Color == Color.Black && move.TargetPosition.Y == 7)
                move.Piece = new Queen(move.Piece.Color, oldId);

            newBoard[position.Y, position.X] = null;
            newBoard[move.TargetPosition.Y, move.TargetPosition.X] = move.Piece;

            return newBoard;
        }

        public Position GetKingPosition(Color color, Piece[,] board)
        {
            for (int x = 0; x < BoardSize; x++)
            {
                for (int y = 0; y < BoardSize; y++)
                {
                    if (board[y, x] != null && board[y, x] is King && board[y, x].Color == color)
                        return new Position(y, x);
                }
            }

            throw new InvalidOperationException("Error no king noob");
        }

        public string GetFenString()
        {
            return GetFenString(this);
        }

        public string GetFenString(ChessBoard board)
        {
            string fenString = string.Empty;

            for (int y = 0; y < BoardSize; y++)
            {
                int emptyCounter = 0;

                for (int x = 0; x < BoardSize; x++)
                {
                    var pieceAtPosition = board.GetPieceAtPosition(x, y);

                    if (pieceAtPosition == null)
                        emptyCounter++;
                    else
                    {
                        if (emptyCounter > 0)
                            fenString += emptyCounter;

                        fenString += pieceAtPosition.GetFenRepresentation();

                        emptyCounter = 0;
                    }
                }

                if (emptyCounter > 0)
                {
                    fenString += emptyCounter;
                }

                if (y < BoardSize - 1)
                    fenString += "/";
            }

            return fenString;
        }

        public void MakeMove(Move move)
        {
            if (Winner.HasWinner)
                return;

            Board = GetBoardAfterMove(move);
            move.Piece.OnMoved();

            if (move.IsCastleMove)
            {
                var castleMove = new Move
                {
                    Piece = move.CastleRook,
                    TargetPosition = move.RookTargetPosition
                };

                Board = GetBoardAfterMove(castleMove);
                move.Piece.OnMoved();
            }

            SetCheckValues(InvertColor(move.Piece.Color));

            var moves = GetAllAvailableMoves(InvertColor(move.Piece.Color));

            if (!moves.Any())
            {
                bool draw = !IsInCheck(InvertColor(move.Piece.Color));

                Winner = draw ? new Winner { Color = Color.Nobody, HasWinner = true } : new Winner { Color = move.Piece.Color, HasWinner = true };
            }
        }

        public IEnumerable<Move> GetAllAvailableMoves(Piece piece)
        {
            return piece.GetLegalMoves(this);
        }

        public bool IsInCheckAfterMove(Color color, Move move, ChessBoard board)
        {
            var boardAfterMove = board.GetBoardAfterMove(move);
            var kingPosition = board.GetKingPosition(color, boardAfterMove);

            var moves = board.GetAllAvailableMovesWithBoard(InvertColor(color), new ChessBoard {Board = boardAfterMove}, false);

            return moves.Any(opponentMove => opponentMove.TargetPosition.Equals(kingPosition));
        }

        public  bool IsInCheck(Color color)
        {
            return color == Color.Black ? isBlackInCheck : isWhiteInCheck;
        }

        public void SetCheckValues(Color color)
        {
            var kingPosition = GetKingPosition(color);

            var moves = GetAllAvailableMovesWithBoard(InvertColor(color), this, false, true);

            var check = moves.Any(opponentMove => opponentMove.TargetPosition.Equals(kingPosition));

            if (color == Color.Black)
                isBlackInCheck = check;
            else
                isWhiteInCheck = check;
        }

        public IEnumerable<Move> GetAllAvailableMovesWithBoard(Color color, ChessBoard board, bool checkIfCheck = true, bool ignoreCastleMove = false)
        {
            var legalMoves = new List<Move>();

            foreach (Piece piece in board.Board)
            {
                if (piece != null && piece.Color == color)
                {
                    if (piece is King)
                        ((King) piece).IgnoreCastle = ignoreCastleMove;

                    legalMoves.AddRange(piece.GetLegalMoves(board));

                    if (piece is King)
                        ((King)piece).IgnoreCastle = !ignoreCastleMove;
                }
            }

            if (checkIfCheck)
                return legalMoves.Where(move => !IsInCheckAfterMove(color, move, board));

            return legalMoves;
        }

        public Position GetKingPosition(Color color)
        {
            return GetKingPosition(color, Board);
        }

        private IEnumerable<Move> GetAllAvailableMoves(Color color, bool ignoreKing)
        {
            return GetAllAvailableMovesWithBoard(color, this);
        }

        public IEnumerable<Move> GetAllAvailableMoves(Color color)
        {
            return GetAllAvailableMovesWithBoard(color, this);
        }

        public static Color InvertColor(Color color)
        {
            return (Color)((int)color * -1);
        }

        public Position GetPiecePosition(Piece piece)
        {
            for (int x = 0; x < BoardSize; x++)
            {
                for (int y = 0; y < BoardSize; y++)
                {
                    if (Board[y,x] != null && Board[y, x].Equals(piece))
                        return new Position(y, x);
                }
            }

            throw new ArgumentOutOfRangeException(nameof(piece), piece, "Piece not found on board");
        }

        public ChessBoard JsonCopy()
        {
            var deserializeSettings = new JsonSerializerSettings
            {
                ObjectCreationHandling = ObjectCreationHandling.Replace,
                TypeNameHandling = TypeNameHandling.All
            };

            return JsonConvert.DeserializeObject<ChessBoard>(JsonConvert.SerializeObject(this, deserializeSettings), deserializeSettings);
        }

        public bool IsPositionInsideBoard(int x, int y)
        {
            return x >= 0 && x < BoardSize && y >= 0 && y < BoardSize;
        }

        public Piece GetPieceAtPosition(int x, int y)
        {
            if (!IsPositionInsideBoard(x, y))
                return null;

            return Board[y, x];
        }

        public IEnumerable<Piece> GetPiecesForColor(Color color)
        {
            return Board.Cast<Piece>().Where(piece => piece != null && piece.Color == color).ToList();
        }

        public static ChessBoard CreateFromFenString(string selectedFen)
        {
            var board = new ChessBoard();

            var rows = selectedFen.Split('/');

            int y = 0;
            bool finished = false;

            foreach (var row in rows)
            {
                int x = 0;

                foreach (var character in row.ToCharArray())
                {
                    if (character == ' ')
                    {
                        finished = true;
                        break;
                    }

                    int value;
                    if (!int.TryParse(character.ToString(), out value))
                    {
                        Color color = char.IsUpper(character) ? Color.White : Color.Black;

                        var pieceString = character.ToString().ToUpper();

                        if (pieceString == "B")
                            board.Board[y, x] = new Bishop(color);
                        if (pieceString == "K")
                            board.Board[y, x] = new King(color);
                        if (pieceString == "N")
                            board.Board[y, x] = new Knight(color);
                        if (pieceString == "P")
                            board.Board[y, x] = new Pawn(color);
                        if (pieceString == "Q")
                            board.Board[y, x] = new Queen(color);
                        if (pieceString == "R")
                            board.Board[y, x] = new Rook(color);

                        x++;
                    }
                    else
                    {
                        for (int val = 0; val < value; val++)
                        {
                            board.Board[y, x] = null;
                            x++;
                        }
                        
                    }
                }

                if (finished)
                    break;

                y++;
            }

            return board;
        }

        public Winner Winner { get; set; }
    }
}
