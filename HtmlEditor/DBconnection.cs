using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;
namespace HtmlEditor
{
    public class DBconnection
    {
        List<DgDebug> debugList = new List<DgDebug>();
        DgDebug dgd = new DgDebug();
        public int attrCount = 1;
        public void saveTxtBoxValues(double width, double height, System.Windows.Thickness plusMargin, Thickness minusMargin, Thickness resetMargin, Thickness saveBtnMargin)
        {

            var sql = "INSERT INTO data (tboxwidth, tboxheight, plusbtnleft, plusbtntop, minusbtnleft, minusbtntop, resetbtnleft, resetbtntop, savebtnleft, savebtntop)"
                + "VALUES (@tboxwidth, @tboxheight, @plusbtnleft, @plusbtntop, @minusbtnleft, @minusbtntop, @resetbtnleft, @resetbtntop, @savebtnleft, @savebtntop)";
            try
            {
                using var connection = new SqliteConnection(@"Data Source=C:\\Users\\Omistaja\\source\\repos\\hannutt\\HtmlEditor\\HtmlEditor\\assets\\editorDB.db");
                connection.Open();
                using var command = new SqliteCommand(sql, connection);

                command.Parameters.AddWithValue("@tboxwidth", width);
                command.Parameters.AddWithValue("@tboxheight", height);
                command.Parameters.AddWithValue("@plusbtnleft", plusMargin.Left);
                command.Parameters.AddWithValue("@plusbtntop", plusMargin.Top);
                command.Parameters.AddWithValue("@minusbtnleft", minusMargin.Left);
                command.Parameters.AddWithValue("@minusbtntop", minusMargin.Top);
                command.Parameters.AddWithValue("@resetbtntop", resetMargin.Top);
                command.Parameters.AddWithValue("@resetbtnleft", resetMargin.Left);
                command.Parameters.AddWithValue("@savebtnleft", saveBtnMargin.Left);
                command.Parameters.AddWithValue("@savebtntop", saveBtnMargin.Top);
                var rowInserted = command.ExecuteNonQuery();

            }
            catch (SqliteException ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message.ToString());
            }

        }

        public void savePreViewValues(double width, double height)
        {
            var sql = "INSERT INTO pdata (pboxwidth, pboxheight)" + "VALUES (@pboxwidth, @pboxheight)";
            try
            {
                using var connection = new SqliteConnection(@"Data Source=C:\\Users\\Omistaja\\source\\repos\\hannutt\\HtmlEditor\\HtmlEditor\\assets\\editorDB.db");
                connection.Open();
                using var command = new SqliteCommand(sql, connection);

                command.Parameters.AddWithValue("@pboxwidth", width);
                command.Parameters.AddWithValue("@pboxheight", height);
                var rowInserted = command.ExecuteNonQuery();

            }
            catch (SqliteException ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        public void fetchTags(AutoCompleteBox acbox, bool createAttr)
        {
            var sql = "";

            if (createAttr)
            {
                sql = "SELECT * FROM tagsattribute";
            }
            else
            {
                sql = "SELECT * FROM tags";
            }

            try
            {
                List<string> htmltags = new List<string>();
                using var connection = new SqliteConnection(@"Data Source=C:\\Users\\Omistaja\\source\\repos\\hannutt\\HtmlEditor\\HtmlEditor\\assets\\editorDB.db");
                connection.Open();

                using var command = new SqliteCommand(sql, connection);
                using var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        htmltags.Add(reader.GetString(1));

                    }

                    acbox.ItemsSource = htmltags;

                }


            }

            catch (SqliteException ex)
            {
                //mahdollinen sqlite-virhe näytetään viesti-ikkunassa.
                MessageBox.Show(ex.Message.ToString());
            }
        }

        public void fetchHtmlBoilerPlate(TextBox txtBox)
        {
            var script = "";
            var sql = "SELECT script FROM bsscripts WHERE id=3";
            try
            {
                using var connection = new SqliteConnection(@"Data Source=C:\\Users\\Omistaja\\source\\repos\\hannutt\\HtmlEditor\\HtmlEditor\\assets\\editorDB.db");
                connection.Open();

                using var command = new SqliteCommand(sql, connection);
                using var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        script = reader.GetString(0);
                        txtBox.AppendText(script);

                    }

                }
            }

            catch (SqliteException ex)
            {
                //mahdollinen sqlite-virhe näytetään viesti-ikkunassa.
                MessageBox.Show(ex.Message.ToString());
            }
        }
        public void GetHotkeys(TextBox txtBox, KeyEventArgs e, bool createAttr)
        {   //lista, johon tallettevat arvot ovat key-tyyppiä eli näppäimistön näppäimiä
            List<Key> tagsWithOneChar = new List<Key>();
            var kbShort = "";
            var sql = "Select hotkey from kbshorts";
            try
            {
                using var connection = new SqliteConnection(@"Data Source=C:\\Users\\Omistaja\\source\\repos\\hannutt\\HtmlEditor\\HtmlEditor\\assets\\editorDB.db");
                connection.Open();

                using var command = new SqliteCommand(sql, connection);
                using var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        //pikanäppäimet ovat tietokannassa merkkijonoja, eli ne haetaa ensin string muuttujaan
                        kbShort = reader.GetString(0);
                        //parseroidaan string muuttujat key-tyypiksi eli näppäimistön näppäimiksi
                        Enum.TryParse(kbShort, out Key hotkey);

                        //lisätään jokainen näppäin listaan
                        tagsWithOneChar.Add(hotkey);
                    }

                }
            }


            catch (SqliteException ex)
            {
                //mahdollinen sqlite-virhe näytetään viesti-ikkunassa.
                MessageBox.Show(ex.Message.ToString());
            }
            //listan läpikäynti, jokainen listan alkio on i muuttujassa, count on sama kuin length string listassa
            for (int i = 0; i < tagsWithOneChar.Count; i++)
            {
                if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == tagsWithOneChar[i] && createAttr)
                {
                    //count += 1;
                    txtBox.Text = txtBox.Text.Insert(txtBox.CaretIndex, "<" + e.Key.ToString().ToLower() + " id='yourId" + attrCount + " class='yourClass" + attrCount + ">" + "</" + e.Key.ToString().ToLower() + ">");
                    //lisätään DgDebug luokan Tag ja Added propertyihin arvot
                    //debugList.Add(new DgDebug() { Tag = "<" + e.Key.ToString().ToLower() + "> </" + e.Key.ToString().ToLower() + ">", Added = DateTime.Now.ToString(), Order = count });
                    //addToDataGrid();
                    attrCount += 1;
                }

                else if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == tagsWithOneChar[i])
                {

                    txtBox.Text = txtBox.Text.Insert(txtBox.CaretIndex, "<" + e.Key.ToString().ToLower() + ">" + "</" + e.Key.ToString().ToLower() + ">");
                    //count += 1;
                    //lisätään DgDebug luokan Tag ja Added propertyihin arvot
                    //debugList.Add(new DgDebug() { Tag = "<" + e.Key.ToString().ToLower() + "> </" + e.Key.ToString().ToLower() + ">", Added = DateTime.Now.ToString(), Order = count });
                    //addToDataGrid();

                }

            }

        }
        public void getLongerHotkeys(TextBox txtBox, KeyEventArgs e, int count, List<DgDebug> debugList, DataGrid debugdg, bool createAttr)
        {
            Dictionary<Key, string> tagsWithoutAttributes = new Dictionary<Key, string>();
            var tag = "";
            var sql = "Select tag from longerTags";
            try
            {
                using var connection = new SqliteConnection(@"Data Source=C:\\Users\\Omistaja\\source\\repos\\hannutt\\HtmlEditor\\HtmlEditor\\assets\\editorDB.db");
                connection.Open();

                using var command = new SqliteCommand(sql, connection);
                using var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        //pikanäppäimet ovat tietokannassa merkkijonoja, eli ne haetaa ensin string muuttujaan
                        tag = reader.GetString(0);
                        //jaetaan merkkijono , merkin kohdalta 2 osaan, ensimmäinen on pikanäppäin
                        //toinen on tagi, el esim. D + <div></div>
                        string[] splitTag = tag.Split(',');
                        //muunnetaan ensimmäinen osa tagista merkkijonosta Key-tyypiksi
                        Enum.TryParse(splitTag[0], out Key hotkey);

                        //lisätään dictionaryyn avain-arvo parina Key-tyyppi ja merkkijono.
                        tagsWithoutAttributes.Add(hotkey, splitTag[1]);
                    }

                }
                //alustetaan muuttuja jo tässä, että sitä voidaan käyttää if/elsen ulkopuolella
                string htmltagWithAttrs = "";
                if (Keyboard.Modifiers == ModifierKeys.Shift && tagsWithoutAttributes.ContainsKey(e.Key) && createAttr)
                {
                    
                    //talletetaan merkkijonomuuttujaan dictionarysta avainta (eli näppäintä) vastaava arvo
                    string htmltag = tagsWithoutAttributes[e.Key];
                    //lasketaan html tagin pituus merkkeinä.
                    int htmlTagLng = htmltag.Length;
                    //span tagin pituus on 13 merkkiä, jolloin id ja class asetetaan insertillä kohtaan4
                    //että tagi muotoutuu oikein
                    if (htmlTagLng == 13)
                    {
                        htmltagWithAttrs = htmltag.Insert(5, " id=yourId" + attrCount + " class=yourClass" + attrCount);
                    }
                    else if (htmlTagLng == 11)

                    {
                        htmltagWithAttrs = htmltag.Insert(4, " id=yourId" + attrCount + " class=yourClass" + attrCount);
                      
                    }
                    //kaksimerkkiset tagit on 9 merkkiä eli attribuutin sijoitetaan kohtaa 3
                    else if (htmlTagLng == 9)
                    {
                        htmltagWithAttrs = htmltag.Insert(3, " id=yourId" + attrCount + " class=yourClass" + attrCount);
                    }
                    // htmltagWithAttrs on alustettu if/elsen ulkopuolella, joten se voidaan asettaa
                    //txtboksiin lohkon ulkopuolella, muuten allaoleva pitäisi tehdä jokaisessa if-lohkossa
                    txtBox.Text = txtBox.Text.Insert(txtBox.CaretIndex, htmltagWithAttrs);
                    attrCount += 1;


                }
                else if (Keyboard.Modifiers == ModifierKeys.Shift && tagsWithoutAttributes.ContainsKey(e.Key))
                {
                    count += 1;
                    txtBox.Text = txtBox.Text.Insert(txtBox.CaretIndex, tagsWithoutAttributes[e.Key]);
                    debugList.Add(new DgDebug() { Tag = tagsWithoutAttributes[e.Key], Added = DateTime.Now.ToString(), Order = count });
                    addToDataGrid(debugdg);
                }
            }


            catch (SqliteException ex)
            {
                //mahdollinen sqlite-virhe näytetään viesti-ikkunassa.
                MessageBox.Show(ex.Message.ToString());
            }
        }
        private void addToDataGrid(DataGrid debugdg)
        {

            foreach (DgDebug d in debugList)
            {
                debugdg.Items.Add(d);
            }
            debugList.Clear();
        }
        public void fetchScript(System.Windows.Controls.TextBox txtBox, object sqlId, int caretIndex)
        {
            var script = "";
            var sql = "Select script FROM bsscripts WHERE id=" + sqlId.ToString();
            try
            {
                using var connection = new SqliteConnection(@"Data Source=C:\\Users\\Omistaja\\source\\repos\\hannutt\\HtmlEditor\\HtmlEditor\\assets\\editorDB.db");
                connection.Open();

                using var command = new SqliteCommand(sql, connection);
                using var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        script = reader.GetString(0);

                    }
                    //caretIndeksin avulla boilerpate koodi lisätään siihen kohtaan, missä kursori on.
                    txtBox.Text = txtBox.Text.Insert(txtBox.CaretIndex, script);
                }
            }

            catch (SqliteException e)
            {
                //mahdollinen sqlite-virhe näytetään viesti-ikkunassa.
                MessageBox.Show(e.Message.ToString());
            }
        }

        public void fetchDbData(System.Windows.Controls.TextBox txtBox, System.Windows.Controls.Button boxDecreaseBtn, System.Windows.Controls.Button boxIncreaseBtn, System.Windows.Controls.Button resetBtn, System.Windows.Controls.Button saveBtn)
        {

            var sql = "SELECT * FROM data";
            try
            {
                using var connection = new SqliteConnection(@"Data Source=C:\\Users\\Omistaja\\source\\repos\\hannutt\\HtmlEditor\\HtmlEditor\\assets\\editorDB.db");
                connection.Open();

                using var command = new SqliteCommand(sql, connection);
                using var reader = command.ExecuteReader();

                //lista desimaaliluvuille
                List<Double> values = new List<Double>();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        //getdouble hakee kannasta desimaaliluvut suluissa olevan arvon perusteella
                        //arvot alkaa luvusta 0, joka on tässä tapuksessa id-numero, joten annetaan
                        //kierrosmuuttujalle aloitusarvoksi 1. haettavia arvoja on yhteensä 10
                        //joten niin kauan kuin j on pienempi kuin 11, talletetaan values listaan
                        //kierrosmuuttujan avulla kulloinenkin arvo.
                        for (int j = 1; j < 11; j++)
                        {
                            values.Add(reader.GetDouble(j));

                        }
                    }
                    txtBox.Width = values[0];
                    txtBox.Height = values[1];
                    boxIncreaseBtn.Margin = new System.Windows.Thickness(values[2], values[3], 0, 0);
                    boxDecreaseBtn.Margin = new System.Windows.Thickness(values[4], values[5], 0, 0);
                    resetBtn.Margin = new System.Windows.Thickness(values[6], values[7], 0, 0);
                    saveBtn.Margin = new System.Windows.Thickness(values[8], values[9], 0, 0);

                }

            }
            catch (SqliteException ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
