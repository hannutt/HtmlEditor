using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private bool createAttr = false;

        public bool OpenFileDialog
        {
            get { return openFileDialog; }
            set { openFileDialog = value; }
        }
        public int CaretIndex;

        public int attrCount = 1;




        FontSetups fontSetups = new FontSetups();
        Sizing sizing = new Sizing();
        DBconnection dbconnection = new DBconnection();
        DgDebug dgd = new DgDebug();
        List<DgDebug> debugList = new List<DgDebug>();
        public int count = 0;
        Testcases testCases = new Testcases();
        public MainWindow()
        {


            InitializeComponent();
            setDgFields();
            string path = Directory.GetCurrentDirectory();

            //luetaan tekstitiedoston sisältö htmltags listaan.
            List<string> htmlTags = System.IO.File.ReadLines("C:\\Users\\Omistaja\\source\\repos\\hannutt\\HtmlEditor\\HtmlEditor\\assets\\htmltags.txt").ToList();
            acbox.ItemsSource = htmlTags;
        }
        //mahdollistaa raahattavan elementin sisällön pudottamisen tekstikenttään.
        private void txtBox_Drop(object sender, DragEventArgs e)
        {

            txtBox.AppendText(e.Data.ToString());

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
        //lost focus eli kun kursori poistuu kentästä
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
            sizing.txtBoxBigger(txtBox, txtBoxHeight, txtBoxWidth, saveBtn, boxDecreaseBtn, boxIncreaseBtn, resetBtn, currentSize);


        }

        private void boxDecrease_Click(object sender, RoutedEventArgs e)
        {
            double txtBoxHeight = txtBox.Height;
            double txtBoxWidth = txtBox.Width;
            sizing.txtBoxSmaller(txtBox, txtBoxHeight, txtBoxWidth, saveBtn, boxDecreaseBtn, boxIncreaseBtn, resetBtn);

        }

        private void txtBox_KeyDown(object sender, KeyEventArgs e)
        {
            List<Key> tagsWithOneChar = new List<Key>();
            tagsWithOneChar.Add(Key.P);
            tagsWithOneChar.Add(Key.A);
            tagsWithOneChar.Add(Key.B);
            tagsWithOneChar.Add(Key.U);
            tagsWithOneChar.Add(Key.I);


            //listan läpikäynti, jokainen listan alkio on i muuttujassa, count on sama kuin length string listassa
            for (int i = 0; i < tagsWithOneChar.Count; i++)
            {
                //jos control + listalla oleva näppäin on painettu tulostetaa tekstilaatikko painettu
                //näppäin ilman controllia.
                if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == tagsWithOneChar[i] && createAttr)
                {
                    count += 1;
                    txtBox.Text = txtBox.Text.Insert(txtBox.CaretIndex, "<" + e.Key.ToString().ToLower() + " id='yourId" + count + "'" + " class='yourClass" + count + "'" + ">" + "</" + e.Key.ToString().ToLower() + ">");

                    //lisätään DgDebug luokan Tag ja Added propertyihin arvot
                    debugList.Add(new DgDebug() { Tag = "<" + e.Key.ToString().ToLower() + "> </" + e.Key.ToString().ToLower() + ">", Added = DateTime.Now.ToString(), Order = count });
                    addToDataGrid();

                }
                else if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == tagsWithOneChar[i])
                {
                    count += 1;
                    txtBox.Text = txtBox.Text.Insert(txtBox.CaretIndex, "<" + e.Key.ToString().ToLower() + ">" + "</" + e.Key.ToString().ToLower() + ">");

                    //lisätään DgDebug luokan Tag ja Added propertyihin arvot
                    debugList.Add(new DgDebug() { Tag = "<" + e.Key.ToString().ToLower() + "> </" + e.Key.ToString().ToLower() + ">", Added = DateTime.Now.ToString(), Order = count });
                    addToDataGrid();

                }


            }


        }
        private void txtBox_KeyUp(object sender, KeyEventArgs e)
        {

            Dictionary<Key, string> tagsWithAttributes = new Dictionary<Key, string>();
            tagsWithAttributes.Add(Key.L, "<li id='yourLIiD" + attrCount + "'" + "class='yourClass'></li>");
            tagsWithAttributes.Add(Key.O, "<ol id='yourOLiD" + attrCount + "'" + "class='yourClass'></ol>");
            tagsWithAttributes.Add(Key.D, "<div id='yourDivID" + attrCount + "'" + "class='yourClass'></div>");
            tagsWithAttributes.Add(Key.U, "<ul id='yourULiD" + attrCount + "'" + "class='yourClass'></ul>");
            tagsWithAttributes.Add(Key.B, "<button yourButtonID" + attrCount + "'" + "class='yourClass'></button>");

            Dictionary<Key, string> tagsWithoutAttributes = new Dictionary<Key, string>();
            tagsWithoutAttributes.Add(Key.L, "<li></li>");
            tagsWithoutAttributes.Add(Key.O, "<ol></ol>");
            tagsWithoutAttributes.Add(Key.D, "<div></div>");
            tagsWithoutAttributes.Add(Key.U, "<ul></ul>");
            tagsWithoutAttributes.Add(Key.B, "<button></button>");

            count += 1;
            if (createAttr && Keyboard.Modifiers == ModifierKeys.Shift && tagsWithAttributes.ContainsKey(e.Key))
            {
                attrCount += 1;
                txtBox.Text = txtBox.Text.Insert(txtBox.CaretIndex, tagsWithAttributes[e.Key]);
                debugList.Add(new DgDebug() { Tag = tagsWithAttributes[e.Key], Added = DateTime.Now.ToString(), Order = count });
                addToDataGrid();

            }
            else if (Keyboard.Modifiers == ModifierKeys.Shift && tagsWithoutAttributes.ContainsKey(e.Key))
            {
                txtBox.Text = txtBox.Text.Insert(txtBox.CaretIndex, tagsWithoutAttributes[e.Key]);
                debugList.Add(new DgDebug() { Tag = tagsWithoutAttributes[e.Key], Added = DateTime.Now.ToString(), Order = count });
                addToDataGrid();
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
            DataGridTextColumn TextColOrder = new DataGridTextColumn();

            //bindaus, eli määritellään missä sarakkeessa näytetään DgDebug luokan
            //Added ja Tag propertyihin talletetut arvot.
            TextColAdded.Binding = new Binding("Added");
            //sarakkeen otsikko
            TextColAdded.Header = "Added";
            TextColTag.Binding = new Binding("Tag");
            TextColTag.Header = "Tag";
            TextColOrder.Binding = new Binding("Order");
            TextColOrder.Header = "Order";

            debugdg.Columns.Add(TextColOrder);
            debugdg.Columns.Add(TextColAdded);
            debugdg.Columns.Add(TextColTag);
        }

        private void createAttributes_Checked(object sender, RoutedEventArgs e)
        {
            createAttr = true;

        }

        private void createAttributes_Unchecked(object sender, RoutedEventArgs e)
        {
            createAttr = false;
        }

        private void callFetch(object sender, RoutedEventArgs e)
        {
            //comboboxitemilla on XAML:ssa Tag-property, jota käytetään
            //tässä tapauksessa sql id:nä oikean skriptin hakemiseen. Ensimmäisellä CBitemilla tag on 1
            //toisella 2 jne.
            var sqlId = ((ComboBoxItem)sender).Tag;
            dbconnection.fetchScript(txtBox, sqlId, CaretIndex);
        }

        private void testbtn_Click(object sender, RoutedEventArgs e)
        {
            testCases.testBoilerPlateHtml(txtBox);
         
        }
    }
}
