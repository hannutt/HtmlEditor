using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Printing;
using System.Text;
using System.Threading.Channels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
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
        //bool openFileDialog = false;
        //kenttä
        private String currentFile;
        //property
        public String CurrentFile
        {
            get { return currentFile; }
            set { currentFile = value; }
        }
        private bool openFileDialog;

        public bool OpenFileDialog
        {
            get { return openFileDialog; }
            set { openFileDialog = value; }
        }
        public int CaretIndex;


        FontSetups fontSetups = new FontSetups();
        Sizing sizing = new Sizing();
        DBconnection dbconnection = new DBconnection();
        DgDebug dgd = new DgDebug();
        List<DgDebug> debugList = new List<DgDebug>();

        public MainWindow()
        {


            InitializeComponent();
            setDgFields();
            string path = Directory.GetCurrentDirectory();

            //luetaan tekstitiedoston sisältö htmltags listaan.
            List<string> htmlTags = System.IO.File.ReadLines("C:\\Users\\Omistaja\\source\\repos\\hannutt\\HtmlEditor\\HtmlEditor\\assets\\htmltags.txt").ToList();
            acbox.ItemsSource = htmlTags;
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
                CurrentFile = fullPath;
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
            try
            {
                if (openFileDialog)
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.ShowDialog();
                    //valittu tiedosto ja sen polku
                    var fullPath = openFileDialog.FileName;
                    //webbrowser avaa polussa olevan tiedoston*/
                    wbrow.Navigate(fullPath);

                }
                else
                {
                    wbrow.Navigate(currentFile);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("File open cancelled");
            }
        }

        private void htmlRadio_Checked(object sender, RoutedEventArgs e)
        {
            string boilerPlate = "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n<link rel=\"stylesheet\" href=\"styles.css\"> \r\n<title>Page Title</title>\r\n</head>\r\n\r\n<body>\r\n    <h2>Welcome</h2>\r\n</body>\r\n\r\n</html>";
            txtBox.AppendText(boilerPlate);
            //buttonit muutetaan näkyviksi
            tagBtn1.Visibility = Visibility.Visible;
            tagBtn2.Visibility = Visibility.Visible;

        }
        private void htmlBsJquery_Checked(object sender, RoutedEventArgs e)
        {

            /* \r\n + välilyönneillä saa tekstin sisennettyä*/

            string boilerPlate = "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n  <link rel=\"stylesheet\"href=\"https://\r\n    maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/\r\n    bootstrap.min.css/>\r\n  <script src=\"https://ajax.googleapis.com/ajax/libs/\r\n    jquery/3.7.1/jquery.min.js></script>\r\n    <script src=\"https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js>\r\n</script>\r\n<link rel=\"stylesheet\" href=\"styles.css\"> \r\n<title>Page Title</title>\r\n</head>\r\n\r\n<body>\r\n    <h2>Welcome</h2>\r\n</body>\r\n\r\n</html>";

            txtBox.AppendText(boilerPlate);

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

        private void autoCompCB_Checked(object sender, RoutedEventArgs e)
        {
            writeTag.Visibility = Visibility.Hidden;
            acbox.Visibility = Visibility.Visible;
        }

        private void autoCompCB_Unchecked(object sender, RoutedEventArgs e)
        {
            writeTag.Visibility = Visibility.Visible;
            acbox.Visibility = Visibility.Hidden;

        }
        private void fontItalic_Selected(object sender, RoutedEventArgs e)
        {
            fontSetups.fontItalic(txtBox);
        }


        //ctrl+r pikanäppäin resetoi fontin muotoilun
        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.R)
            {
                fontSetups.DoFontRestore(txtBox);
                // Call your method here
            }

        }

        private void boxIncrease_Click(object sender, RoutedEventArgs e)
        {
            double txtBoxHeight = txtBox.Height;
            double txtBoxWidth = txtBox.Width;
            sizing.txtBoxBigger(txtBox, txtBoxHeight, txtBoxWidth, saveBtn, boxDecreaseBtn, boxIncreaseBtn, resetBtn);


        }

        private void boxDecrease_Click(object sender, RoutedEventArgs e)
        {
            double txtBoxHeight = txtBox.Height;
            double txtBoxWidth = txtBox.Width;
            sizing.txtBoxSmaller(txtBox, txtBoxHeight, txtBoxWidth, saveBtn, boxDecreaseBtn, boxIncreaseBtn, resetBtn);

        }

        private void txtBox_KeyDown(object sender, KeyEventArgs e)
        {
            List<Key> keys = new List<Key>();
            keys.Add(Key.P);
            keys.Add(Key.A);
            keys.Add(Key.B);
            keys.Add(Key.U);
            keys.Add(Key.I);
            keys.Add(Key.L);

            //listan läpikäynti, jokainen listan alkio on i muuttujassa, count on sama kuin length string listassa
            for (int i = 0; i < keys.Count; i++)
            {
                //jos control + listalla oleva näppäin on painettu tulostetaa tekstilaatikko painettu
                //näppäin ilman controllia.
                if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == keys[i])
                {

                    txtBox.Text = txtBox.Text.Insert(txtBox.CaretIndex, "<" + e.Key.ToString().ToLower() + ">" + "</" + e.Key.ToString().ToLower() + ">");

                    //lisätään DgDebug luokan Tag ja Added propertyihin arvot
                    debugList.Add(new DgDebug() { Tag = "<" + e.Key.ToString().ToLower() + "> </" + e.Key.ToString().ToLower() + ">", Added = DateTime.Now.ToString() });
                    addToDataGrid();


                }

            }
        }
        private void addToDataGrid()
        {

            foreach (DgDebug d in debugList)
            {
                debugdg.Items.Add(d);
            }
            debugList.Clear();
        }

        private void ClrPcker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            txtBox.Text = txtBox.Text.Insert(txtBox.CaretIndex, "#" + ClrPcker.SelectedColor.ToString());
        }

        private void previewSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sizing.previewWindowSize(previewSlider.Value, wbrow);

        }

        private void openAnother_Checked(object sender, RoutedEventArgs e)
        {
            openFileDialog = true;

        }

        private void resetBtn_Click(object sender, RoutedEventArgs e)
        {
            sizing.restoreSizesAndMargins(txtBox, boxDecreaseBtn, boxIncreaseBtn);

        }

        private void saveValues_Selected(object sender, RoutedEventArgs e)
        {
            dbconnection.saveTxtBoxValues(txtBox.Width, txtBox.Height, boxDecreaseBtn.Margin, boxIncreaseBtn.Margin, resetBtn.Margin, saveBtn.Margin);
        }

        private void loadValues_Selected(object sender, RoutedEventArgs e)
        {
            dbconnection.fetchDbData(txtBox, boxDecreaseBtn, boxIncreaseBtn, resetBtn, saveBtn);
        }

        private void savePreviewValues_Selected(object sender, RoutedEventArgs e)
        {
            dbconnection.savePreViewValues(wbrow.Width, wbrow.Height);
        }

        private void setDgFields()
        {
            DataGridTextColumn TextColAdded = new DataGridTextColumn();
            DataGridTextColumn TextColTag = new DataGridTextColumn();

            //bindaus, eli määritellään missä sarakkeessa näytetään DgDebug luokan
            //Added ja Tag propertyihin talletetut arvot.
            TextColAdded.Binding = new Binding("Added");
            //sarakkeen otsikko
            TextColAdded.Header = "Added";
            TextColTag.Binding = new Binding("Tag");
            TextColTag.Header = "Tag";

            debugdg.Columns.Add(TextColAdded);
            debugdg.Columns.Add(TextColTag);
        }


    }
}
