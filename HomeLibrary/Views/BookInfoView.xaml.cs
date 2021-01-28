using System.Windows;
using System.Windows.Controls;

namespace HomeLibrary.Views
{
    /// <summary>
    /// Interaction logic for BookInfoView.xaml
    /// </summary>
    public partial class BookInfoView : UserControl
    {
        public BookInfoView()
        {
            InitializeComponent();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            searchBox.Clear();
        }
    }
}
