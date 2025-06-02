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
            txtBox.AppendText((string)tagBtn1.Content);
            txtBox.AppendText((string)tagBtn2.Content);
            txtBox.AppendText((string)writedTag.Content);

        }

        //aloittaa raahauksen
        private void tagBtn1_MouseMove(object sender, MouseEventArgs e)
        {
            //ptagBtn on elementti ja ptagBtn.content on elementin teksti, joka raahataan ja pudotetaan
            DragDrop.DoDragDrop(tagBtn1, tagBtn1.Content, DragDropEffects.Move);
        }

        //tiedostodialogin avaus
        private void fDialog_Selected(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            var fullPath = openFileDialog.FileName;
            //tiedoston sisälön luku ja tallennus muuttujaan
            var content = File.ReadAllText(fullPath);
            //näytetään textboxissa
            txtBox.AppendText(content);
        }

        private void tagBtn2_MouseMove(object sender, MouseEventArgs e)
        {
            DragDrop.DoDragDrop(tagBtn2, tagBtn2.Content, DragDropEffects.Move);
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
            //try/catch lohko, että ohjelma ei kaadu virheseen, josa
            //tiedostoa ei talleteta.
            try
            {   //dialogissa näytettävät tiedostopäätteet
                if (content.StartsWith("<!"))
                {
                    saveFileDialog.Filter = "HTML files (*.html)|*.html|All files (*.*)|*.*";

                }
                else
                {
                    saveFileDialog.Filter = "CSS files (*.css)|*.css|All files (*.*)|*.*";

                }

                saveFileDialog.ShowDialog();
                var fullPath = saveFileDialog.FileName;
                File.WriteAllText(fullPath, content);

            }
            catch (Exception)
            {
                MessageBox.Show("File saving cancelled");

            }


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

        private void htmlRadio_Checked(object sender, RoutedEventArgs e)
        {
            string boilerPlate = "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n<link rel=\"stylesheet\r\nhref => \r\n<title>Page Title</title>\r\n</head>\r\n\r\n<body>\r\n    <h2>Welcome</h2>\r\n</body>\r\n\r\n</html>";
            txtBox.AppendText(boilerPlate);
            //buttonit muutetaan näkyviksi
            tagBtn1.Visibility = Visibility.Visible;
            tagBtn2.Visibility = Visibility.Visible;

        }

        private void cssRadio_Click(object sender, RoutedEventArgs e)
        {
            tagBtn1.Content = "color:";
            tagBtn2.Content = "text-align:";
            tagBtn1.Visibility = Visibility.Visible;
            tagBtn2.Visibility = Visibility.Visible;
            string cssBoilerPlate = "html {\r\n}\r\nbody{\r\n}";
            txtBox.AppendText(cssBoilerPlate);
        }
    }

}