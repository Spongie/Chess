using System.ComponentModel;
using System.Runtime.CompilerServices;
using Chess.AI;
using Chess.Pieces;

namespace BotVsBotTest
{
    public class ValueByPieceViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ValueByPieceViewModel()
        {
            Name = "NewValueByPiece";
        }

        private float pawnValue;
        private float rookValue;
        private float bishopValue;
        private float knightValue;
        private float queenValue;
        private float kingValue;
        private string name;

        public float RookValue
        {
            get { return rookValue; }
            set
            {
                rookValue = value;
                OnPropertyChanged();
            }
        }

        public float PawnValue
        {
            get { return pawnValue; }
            set
            {
                pawnValue = value;
                OnPropertyChanged();
            }
        }

        public float BishopValue
        {
            get { return bishopValue; }
            set
            {
                bishopValue = value;
                OnPropertyChanged();
            }
        }

        public float KnightValue
        {
            get { return knightValue; }
            set
            {
                knightValue = value;
                OnPropertyChanged();
            }
        }

        public float QueenValue
        {
            get { return queenValue; }
            set
            {
                queenValue = value;
                OnPropertyChanged();
            }
        }

        public float KingValue
        {
            get { return kingValue; }
            set
            {
                kingValue = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value; 
                OnPropertyChanged();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SaveBot()
        {
            var bot = new ValueByPieceEvalutator();
            bot.PieceValues[typeof(Pawn)] = PawnValue;
            bot.PieceValues[typeof(Rook)] = RookValue;
            bot.PieceValues[typeof(Bishop)] = BishopValue;
            bot.PieceValues[typeof(Knight)] = KnightValue;
            bot.PieceValues[typeof(Queen)] = QueenValue;
            bot.PieceValues[typeof(King)] = KingValue;

            ConfigManager.SaveBot(bot, Name);
        }
    }
}
