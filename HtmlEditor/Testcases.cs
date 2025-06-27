using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HtmlEditor
{
    public class Testcases
    {
        public void testBoilerPlateHtml(TextBox txtBox)
        {
            int j = 0;

            string[] tags = { "<!DOCTYPE html>", "<html>", "</html>", "<body>", "</body>", "<head>", "</head>" };
            var html = txtBox.Text;
            for (int i = 0; i < tags.Length; i++)
            {
                if (html.StartsWith("<") && html.EndsWith(">") && html.Contains(tags[i]))
                {
                    j += 1;
                    //j kasvaa aina, kun tekstistä löydetään haluttu tagi, joita on yhteensä 7 tags listassa
                    //jos kaikki 7 tagia löydetään näytetään test ok ilmoitus. muuten messagebox näytettäisiin
                    //jokaisen löydetyn tagin jälkeen.
                    if (j == 7)
                    {
                        string res = "Boilerplate code passed.";
                        testLinkReference(html,res);
                    }
                }
               
                else
                {
                    MessageBox.Show("test failed");
                    //päätetään metodin suoritus, muuten messagebox näytettäisiin useamman kerran
                    return;
                }

            }

        }
        public void testLinkReference(string html, string res)
        {
            if (html.Contains("href=>") || html.Contains(" <a href="));
            {
                string linkres = "Missing link reference found.";
                string finalresult = res + " " + linkres;

                MessageBox.Show(finalresult);
            }
        }
    }
}
