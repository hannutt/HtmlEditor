using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace HtmlEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void txtBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        //suoritetaan kun textbox on luotu
        private void txtBox_Initialized(object sender, EventArgs e)
        {
            string boilerPate = "<!DOCTYPE html>\r\n<html>\r\n\r\n<head>\r\n    <title>Page Title</title>\r\n</head>\r\n\r\n<body>\r\n    <h2>Welcome</h2>\r\n</body>\r\n\r\n</html>";
            txtBox.AppendText(boilerPate);
        }

        private void pTag_Selected(object sender, RoutedEventArgs e)
        {

            txtBox.AppendText("<p></p>");
           
          
        }

        private void hTag_Selected(object sender, RoutedEventArgs e)
        {
            txtBox.AppendText("<h1></h1>");

        }
    }
}