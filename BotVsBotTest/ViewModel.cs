﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Chess;
using Chess.AI;
using Chess.Pieces;

namespace BotVsBotTest
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<Move> moves;
        private ObservableCollection<string> fens;
        private ChessBoard board;
        private string selectedFen;
        private ObservableCollection<AvailableBot> availableBots;
        private AvailableBot selectedWhiteBot;
        private AvailableBot selectedBlackBot;
        private int whiteDepth;
        private int blackDepth;

        public ViewModel()
        {
            Moves = new ObservableCollection<Move>();
            Board = new ChessBoard();
            NextColor = Color.White;
            Fens = new ObservableCollection<string>();
            Fens.Add(Board.GetFenString());
            SelectedFen = Fens.Last();

            blackDepth = 3;
            whiteDepth = 3;

            availableBots = new ObservableCollection<AvailableBot>();

            availableBots.Add(new AvailableBot()
            {
                Name = "Player",
                BoardEvaluator = null
            });

            foreach (var boardEvaluator in ConfigManager.LoadAllBots())
            {
                var bot = new AvailableBot()
                {
                    Name = boardEvaluator.Name,
                    BoardEvaluator = boardEvaluator
                };

                availableBots.Add(bot);
            }

            SelectedWhiteBot = availableBots[0];
            SelectedBlackBot = availableBots[0];

            WhiteBot = new ChessBot(selectedWhiteBot.BoardEvaluator, whiteDepth);
            BlackBot = new ChessBot(selectedBlackBot.BoardEvaluator, blackDepth);
        }

        public ObservableCollection<Move> Moves
        {
            get { return moves; }
            set
            {
                moves = value; 
                OnPropertyChanged();
            }
        }

        public ChessBoard Board
        {
            get { return board; }
            set
            {
                board = value; 
                OnPropertyChanged();
            }
        }

        public Color NextColor { get; set; }

        public ObservableCollection<string> Fens
        {
            get { return fens; }
            set
            {
                fens = value;
                OnPropertyChanged();
            }
        }

        public string SelectedFen
        {
            get { return selectedFen; }
            set
            {
                selectedFen = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<AvailableBot> AvailableBots
        {
            get { return availableBots; }
            set
            {
                availableBots = value; 
                OnPropertyChanged();
            }
        }

        public AvailableBot SelectedWhiteBot
        {
            get { return selectedWhiteBot; }
            set
            {
                selectedWhiteBot = value; 
                OnPropertyChanged();
            }
        }

        public AvailableBot SelectedBlackBot
        {
            get { return selectedBlackBot; }
            set
            {
                selectedBlackBot = value;
                OnPropertyChanged();
            }
        }

        public int WhiteDepth
        {
            get { return whiteDepth; }
            set
            {
                whiteDepth = value;
                OnPropertyChanged();
            }
        }

        public int BlackDepth
        {
            get { return blackDepth; }
            set
            {
                blackDepth = value;
                OnPropertyChanged();
            }
        }

        public ChessBot WhiteBot { get; set; }
        public ChessBot BlackBot { get; set; }

        public void CommitWhiteBot()
        {
            WhiteBot = new ChessBot(SelectedWhiteBot.BoardEvaluator, whiteDepth);
        }

        public void CommitBlackBot()
        {
            BlackBot = new ChessBot(selectedBlackBot.BoardEvaluator, blackDepth);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void MakeBotMove()
        {
            var bot = NextColor == Color.White ? WhiteBot : BlackBot;

            var move = bot.FindMoveForColor(NextColor, Board);
            MakeMove(move);
        }

        private void MakeMove(Move move)
        {
            Board.MakeMove(move);
            Moves.Add(move);
            Fens.Add(Board.GetFenString());
            NextColor = ChessBoard.InvertColor(NextColor);
            SelectedFen = Fens.Last();
        }

        public void AddFen(string text)
        {
            Fens.Add(text);
            SelectedFen = text;
        }

        public void MakeDragMove(int startXIndex, int startYIndex, int dropXIndex, int dropYIndex)
        {
            var move = new Move
            {
                Piece = Board.GetPieceAtPosition(startXIndex, startYIndex),
                TargetPosition = new Position(dropYIndex, dropXIndex)
            };

            MakeMove(move);

            if (NextColor == Color.White && selectedBlackBot.Name == "Player")
            {
                MakeBotMove();
            }
            if (NextColor == Color.Black && selectedWhiteBot.Name == "Player")
            {
                MakeBotMove();
            }
        }
    }
}
