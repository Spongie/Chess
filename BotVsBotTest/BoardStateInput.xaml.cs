using System.Linq;
using System.Windows;
using Chess;

namespace BotVsBotTest
{
    /// <summary>
    /// Interaction logic for BoardStateInput.xaml
    /// </summary>
    public partial class BoardStateInput : Window
    {
        private ViewModel viewModel;

        public BoardStateInput(ViewModel model)
        {
            InitializeComponent();
            viewModel = model;
            fenInputBox.Focus();
        }

        private void ButtonSaveClick(object sender, RoutedEventArgs e)
        {
            viewModel.Board = ChessBoard.CreateFromFenString(fenInputBox.Text);
            viewModel.AddFen(fenInputBox.Text);
            Close();
        }
    }
}
