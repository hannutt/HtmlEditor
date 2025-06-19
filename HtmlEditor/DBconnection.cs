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
                command.Parameters.AddWithValue("@savebtntop",saveBtnMargin.Top);
                var rowInserted = command.ExecuteNonQuery();

            }
            catch (SqliteException ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message.ToString());
            }
            
        }

        public void savePreViewValues(double width,double height)
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

        public void fetchDbData(System.Windows.Controls.TextBox txtBox, System.Windows.Controls.Button boxDecreaseBtn, System.Windows.Controls.Button boxIncreaseBtn, System.Windows.Controls.Button resetBtn, System.Windows.Controls.Button saveBtn)
        {
            var boxwidth = 0.0;
            var boxheight = 0.0;
            var plusbtnleft = 0.0;
            var plusbtntop = 0.0;
            var minusbtnleft = 0.0;
            var minusbtntop = 0.0;
            var resetbtnleft= 0.0;
            var resetbtntop = 0.0;
            var savebtnleft= 0.0;
            var savebtntop= 0.0;

            var sql = "SELECT * FROM data";
            try
            {
                using var connection = new SqliteConnection(@"Data Source=C:\\Users\\Omistaja\\source\\repos\\hannutt\\HtmlEditor\\HtmlEditor\\assets\\editorDB.db");
                connection.Open();

                using var command = new SqliteCommand(sql, connection);

                using var reader = command.ExecuteReader();
                //int i = 1;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        //var id = reader.GetInt32(0);
                        boxwidth = reader.GetDouble(1);
                        boxheight = reader.GetDouble(2);
                        plusbtnleft = reader.GetDouble(3);
                        plusbtntop = reader.GetDouble(4);
                        minusbtnleft= reader.GetDouble(5);
                        minusbtntop = reader.GetDouble(6);
                        resetbtnleft = reader.GetDouble(7);
                        resetbtntop = reader.GetDouble(8);
                        savebtnleft= reader.GetDouble(9);
                        savebtntop= reader.GetDouble(10);

                    }
                    txtBox.Width = boxwidth;
                    txtBox.Height = boxheight;
                    boxIncreaseBtn.Margin = new System.Windows.Thickness(plusbtnleft, plusbtntop,0,0);
                    boxDecreaseBtn.Margin = new System.Windows.Thickness(minusbtnleft, minusbtntop, 0, 0);
                    resetBtn.Margin = new System.Windows.Thickness(resetbtnleft, resetbtntop, 0, 0);
                    saveBtn.Margin = new System.Windows.Thickness(savebtnleft,savebtntop, 0, 0);

                }

            }
            catch (SqliteException ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
