using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Chess;
using Chess.AI;
using Chess.Pieces;

namespace UCI
{
    public class UciController
    {
        private ChessBot chessBot;
        private Thread botThread;
        private readonly Dictionary<string, Action> commandActions;
        private ChessBoard chessBoard;

        private static readonly Dictionary<string, int> letterToXCache = new Dictionary<string, int>
        {
            {"a", 0},
            {"b", 1},
            {"c", 2},
            {"d", 3},
            {"e", 4},
            {"f", 5},
            {"g", 6},
            {"h", 7}
        };

        public UciController()
        {
            chessBoard = new ChessBoard();
            chessBot = new ChessBot(new OnlyPieceCountMatterEvaluator(), 3, Color.Nobody);

            commandActions = new Dictionary<string, Action>();
            commandActions[UciCommands.Uci] = HandleUCICommand;
            commandActions[UciCommands.IsReady] = HandleIsReadyCommand;
            commandActions[UciCommands.NewGame] = HandleNewGameCommand;
        }

        private void HandleGoCommand()
        {
            if (botThread == null)
                botThread = new Thread(RunBot);

            botThread.Start();
        }

        private void RunBot()
        {
            var move = chessBot.FindMove(chessBoard);

            Console.WriteLine(move.ToString());
            Console.WriteLine($"{UciCommands.BestMove} {move.GetUCIString()}");

            botThread = null;
        }

        private void HandleNewGameCommand()
        {
            chessBoard = new ChessBoard();
        }

        private void HandleSetOptionCommand(string input)
        {
            
        }

        private void HandleIsReadyCommand()
        {
            Console.WriteLine(UciCommands.Ready);
        }

        public void Start()
        {
            ReadCommands();
        }

        private void ReadCommands()
        {
            while (true)
            {
                var input = Console.ReadLine();

                if (input == null)
                    continue;

                if (commandActions.ContainsKey(input))
                    commandActions[input].Invoke();

                if (input.StartsWith(UciCommands.SetOption))
                    HandleSetOptionCommand(input);

                if (input.StartsWith(UciCommands.Position))
                    HandlePositionCommand(input);

                if (input.StartsWith(UciCommands.Go))
                    HandleGoCommand();
            }
        }

        private void HandlePositionCommand(string input)
        {
            var data = input.Split(' ').Skip(1).ToArray();

            if (data[0] == UciCommands.Startpos)
                chessBoard = new ChessBoard();
            else
                chessBoard = new ChessBoard(data[0]);

            for (int i = 1; i < data.Length; i++)
            {
                var moveString = data[i];

                if (moveString == "moves")
                    continue;

                var targetPos = new Position(8 - int.Parse(moveString[3].ToString()),letterToXCache[moveString[2].ToString()]);
                var piecePos = new Position(8 - int.Parse(moveString[1].ToString()),letterToXCache[moveString[0].ToString()]);
                var piece = chessBoard.GetPieceAtPosition(piecePos.X, piecePos.Y);

                var move = new Move()
                {
                    Piece = piece,
                    TargetPosition = targetPos
                };

                if (piece is King)
                {
                    if (targetPos.X == piecePos.X + 2)
                    {
                        move.IsCastleMove = true;
                        move.CastleRook = (Rook) chessBoard.GetPieceAtPosition(targetPos.X + 1, targetPos.Y);
                        move.RookTargetPosition = new Position(targetPos.Y, targetPos.X - 1);
                    }
                    else if (targetPos.X == piecePos.X - 2)
                    {
                        move.IsCastleMove = true;
                        move.CastleRook = (Rook)chessBoard.GetPieceAtPosition(targetPos.X - 2, targetPos.Y);
                        move.RookTargetPosition = new Position(targetPos.Y, targetPos.X - 1);
                    }
                }
                

                chessBoard.MakeMove(move);
            }

            Console.WriteLine(chessBoard.GetFenString());
        }

        private void HandleUCICommand()
        {
            Console.WriteLine($"{UciCommands.Id} {UciCommands.Name} SpongeBot");
            Console.WriteLine($"{UciCommands.Id} {UciCommands.Author} Philip Persson");
            Console.WriteLine($"{UciCommands.Options} {UciCommands.Name} depth, {UciCommands.OptionTypeSpin}");
            Console.WriteLine(UciCommands.Ok);
        }
    }
}
