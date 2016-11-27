using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;
using Chess;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace BotVsBotTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModel viewModel;
        private DispatcherTimer timer;

        public MainWindow()
        {
            viewModel = new ViewModel();
            InitializeComponent();

            DataContext = viewModel;

            viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(viewModel.SelectedFen))
                DrawChessBoard(boardCanvas.RenderSize);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.Board.Winner.HasWinner)
            {
                StopTimer();
                return;
            }

            viewModel.MakeMove();
            DrawChessBoard(boardCanvas.RenderSize);

            if (viewModel.Board.Winner.HasWinner)
            {
                MessageBox.Show(Enum.GetName(typeof(Chess.Pieces.Color), viewModel.Board.Winner.Color) + " Wins");
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
                Clipboard.SetText(e.AddedItems[0].ToString());
        }

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DrawChessBoard(e.NewSize);
        }

        private void DrawChessBoard(Size size)
        {
            boardCanvas.Children.Clear();

            int widthPerPiece = (int) (size.Width / 8);
            int heightPerPiece = (int) (size.Height / 8);

            var board = ChessBoard.CreateFromFenString(viewModel.SelectedFen);

            for (int y = 0; y < 8; y++)
            {
                Brush brush = Brushes.White;

                if (y % 2 != 0)
                    brush = Brushes.DarkGoldenrod;

                for (int x = 0; x < 8; x++)
                {
                    var rect = new Rectangle();
                    rect.Width = widthPerPiece;
                    rect.Height = heightPerPiece;
                    rect.Fill = brush;

                    Canvas.SetTop(rect, heightPerPiece * y);
                    Canvas.SetLeft(rect, widthPerPiece * x);

                    if (brush == Brushes.White)
                        brush = Brushes.DarkGoldenrod;
                    else
                        brush = Brushes.White;

                    boardCanvas.Children.Add(rect);

                    var piece = board.GetPieceAtPosition(x, y);

                    if (piece == null)
                        continue;

                    var bitmap =
                        new BitmapImage(
                            new Uri(
                                $"pack://application:,,,/Images/{Enum.GetName(typeof(Chess.Pieces.Color), piece.Color)}{piece.GetName()}.png"));

                    var img = new Image();
                    img.Source = bitmap;
                    img.Width = widthPerPiece;
                    img.Height = heightPerPiece;

                    Canvas.SetTop(img, heightPerPiece * y);
                    Canvas.SetLeft(img, widthPerPiece * x);

                    boardCanvas.Children.Add(img);
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (timer != null)
            {
                StopTimer();
                return;
            }

            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 1);
            timer.Start();
        }

        private void StopTimer()
        {
            timer?.Stop();
            timer = null;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Button_Click(this, new RoutedEventArgs());
        }
    }
}
