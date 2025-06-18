using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
namespace HtmlEditor
{
    public class DBconnection
    {
        public void connectToDbAndSave(double width, double height)
        {

            var sql = "INSERT INTO data2 (tboxwidth, tboxheight)" + "VALUES (@tboxwidth, @tboxheight)";
            try
            {
                using var connection = new SqliteConnection(@"Data Source=C:\\Users\\Omistaja\\source\\repos\\hannutt\\HtmlEditor\\HtmlEditor\\assets\\editorDB.db");
                connection.Open();
                using var command = new SqliteCommand(sql, connection);

                command.Parameters.AddWithValue("@tboxwidth", width);
                command.Parameters.AddWithValue("@tboxheight", height);
                var rowInserted = command.ExecuteNonQuery();

            }
            catch (SqliteException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void fetchDbData(System.Windows.Controls.TextBox txtBox)
        {
            var boxwidth = 0.0;
            var boxheight = 0.0;

            var sql = "SELECT * FROM data2";
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
                        //var id = reader.GetInt32(0);
                        boxwidth = reader.GetDouble(1);
                        boxheight = reader.GetDouble(2);
                    }
                    txtBox.Width = boxwidth;
                    txtBox.Height = boxheight;
                }
                else
                {
                    Console.WriteLine("No data found.");
                }

            }
            catch (SqliteException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
