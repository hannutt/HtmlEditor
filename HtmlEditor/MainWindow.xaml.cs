using System.IO;
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
using Microsoft.Win32;
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
            string boilerPate = "<!DOCTYPE html>\r\n<html>\r\n\r\n<head>\r\n<title>Page Title</title>\r\n</head>\r\n\r\n<body>\r\n    <h2>Welcome</h2>\r\n</body>\r\n\r\n</html>";
            txtBox.AppendText(boilerPate);
        }

        private void pTag_Selected(object sender, RoutedEventArgs e)
        {
            /*skrollaa riville 8
            int lineIndex = 8;
            txtBox.ScrollToLine(lineIndex);*/
            txtBox.AppendText("<p></p>");
          
        }

        private void hTag_Selected(object sender, RoutedEventArgs e)
        {
            txtBox.AppendText("<h1></h1>");

        }

     
        //mahdollistaa raahattavan elementin Content propertyn pudottamisen tekstikenttään.
        private void txtBox_Drop(object sender, DragEventArgs e)
        {
            //Point dropPosition = e.GetPosition(txtBox);
            txtBox.AppendText((string)ptagBtn.Content);
            txtBox.AppendText((string)h1tagBtn.Content);
            txtBox.AppendText((string)writedTag.Content);

        }

        //aloittaa raahauksen
        private void ptagBtn_MouseMove(object sender, MouseEventArgs e)
        {
            //ptagBtn on elementti ja ptagBtn.content on elementin teksti, joka raahataan ja pudotetaan
            DragDrop.DoDragDrop(ptagBtn, ptagBtn.Content, DragDropEffects.Move);
        }

        //tiedostodialogin avaus
        private void fDialog_Selected(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            var fullPath = openFileDialog.FileName;
            Console.WriteLine("full path "+fullPath);
        }

        private void h1tagBtn_MouseMove(object sender, MouseEventArgs e)
        {
            DragDrop.DoDragDrop(h1tagBtn, h1tagBtn.Content, DragDropEffects.Move);
        }

        private void writeTag_LostFocus(object sender, RoutedEventArgs e)
        {
            string tag = writeTag.Text;
            writedTag.Content = tag;
        }

        private void writedTag_MouseMove(object sender, MouseEventArgs e)
        {
            DragDrop.DoDragDrop(writedTag, writedTag.Content, DragDropEffects.Move);
        }

 
        //tiedoston tallennus dialogin avulla
        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            string content = txtBox.Text;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.ShowDialog();
            var fullPath = saveFileDialog.FileName;
            File.WriteAllText(fullPath, content);

        }
        //tiedoston avaus dialogin avulla.
        private void viewBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            //valittu tiedosto ja sen polku
            var fullPath = openFileDialog.FileName;
            //webbrowser avaa polussa olevan tiedoston
            wbrow.Navigate(fullPath);
        }
    }
    
}