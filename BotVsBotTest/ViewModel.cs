using System.Collections.ObjectModel;
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

        public ViewModel()
        {
            Moves = new ObservableCollection<Move>();
            Board = new ChessBoard();
            Bot = new ChessBot(new OnlyPiecesMatterEvaluator(), 3);
            NextColor = Color.White;
            Fens = new ObservableCollection<string>();
            Fens.Add(Board.GetFenString());
            SelectedFen = Fens.Last();
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

        public ChessBot Bot { get; set; }

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

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void MakeMove()
        {
            var move = Bot.FindMoveForColor(NextColor, Board);
            Board.MakeMove(move);
            Moves.Add(move);
            Fens.Add(Board.GetFenString());
            NextColor = ChessBoard.InvertColor(NextColor);
            SelectedFen = Fens.Last();
        }
    }
}
