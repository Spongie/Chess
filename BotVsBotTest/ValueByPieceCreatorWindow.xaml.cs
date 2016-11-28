using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BotVsBotTest
{
    /// <summary>
    /// Interaction logic for ValueByPieceCreatorWindow.xaml
    /// </summary>
    public partial class ValueByPieceCreatorWindow : Window
    {
        private ValueByPieceViewModel viewModel;

        public ValueByPieceCreatorWindow()
        {
            viewModel = new ValueByPieceViewModel();
            InitializeComponent();
            DataContext = viewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            viewModel.SaveBot();
            Close();
        }
    }
}
