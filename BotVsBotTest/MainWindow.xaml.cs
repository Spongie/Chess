using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Chess;
using Chess.Pieces;
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
        private Image selectedImage;
        private int startXIndex;
        private int startYIndex;

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

            viewModel.MakeBotMove();
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
                    Canvas.SetZIndex(rect, 0);

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
                    Canvas.SetZIndex(img, 1);

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
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
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

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var newDialog = new ValueByPieceCreatorWindow();

            newDialog.ShowDialog();
        }

        private void buttonClick_CommitBlack(object sender, RoutedEventArgs e)
        {
            viewModel.CommitBlackBot();
        }

        private void buttonClick_CommitWhite(object sender, RoutedEventArgs e)
        {
            viewModel.CommitWhiteBot();
        }

        private void MenuItem_WhiteMove_Click(object sender, RoutedEventArgs e)
        {
            viewModel.NextColor = Color.White;
        }

        private void MenuItem_BlackMove_Click(object sender, RoutedEventArgs e)
        {
            viewModel.NextColor = Color.Black;
        }

        private void MenuItem_SetBoard_Click(object sender, RoutedEventArgs e)
        {
            new BoardStateInput(viewModel).ShowDialog();
            DrawChessBoard(boardCanvas.RenderSize);
        }

        private void boardCanvas_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Image selected = null;

            foreach (var child in boardCanvas.Children)
            {
                var img = child as Image;

                if (img == null)
                    continue;

                if (img.IsMouseOver)
                {
                    selected = img;
                    break;
                }
            }

            if (selected == null)
                return;

            selectedImage = selected;

            int widthPerPiece = (int)(boardCanvas.RenderSize.Width / 8);
            int heightPerPiece = (int)(boardCanvas.RenderSize.Height / 8);

            startXIndex = (int)(e.GetPosition(boardCanvas).X / widthPerPiece);
            startYIndex = (int)(e.GetPosition(boardCanvas).Y / heightPerPiece);
        }

        private void boardCanvas_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            int widthPerPiece = (int)(boardCanvas.RenderSize.Width / 8);
            int heightPerPiece = (int)(boardCanvas.RenderSize.Height / 8);

            int dropXIndex = (int) (e.GetPosition(boardCanvas).X / widthPerPiece);
            int dropYIndex = (int)(e.GetPosition(boardCanvas).Y / heightPerPiece);

            viewModel.MakeDragMove(startXIndex, startYIndex, dropXIndex, dropYIndex);

            selectedImage = null;
        }

        private void boardCanvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (selectedImage != null)
            {
                int widthPerPiece = (int)(boardCanvas.RenderSize.Width / 8);
                int heightPerPiece = (int)(boardCanvas.RenderSize.Height / 8);

                Canvas.SetTop(selectedImage, e.GetPosition(boardCanvas).Y - (heightPerPiece / 2));
                Canvas.SetLeft(selectedImage, e.GetPosition(boardCanvas).X - (widthPerPiece / 2));
            }
        }

        private void ButtonClick_DisplayMoves(object sender, RoutedEventArgs e)
        {
            var moves = viewModel.GetBotMoves().Select(move => move.ToString());

            File.WriteAllLines("debug.txt", moves);

            Process.Start("debug.txt");
        }
    }
}
