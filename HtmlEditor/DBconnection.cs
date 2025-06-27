using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;
namespace HtmlEditor
{
    public class DBconnection
    {
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
        public void fetchScript(System.Windows.Controls.TextBox txtBox, object sqlId, int caretIndex)
        {
            var script = "";
            var sql = "Select script FROM bsscripts WHERE id="+sqlId.ToString();
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
