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
            string res = "";
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
                        res = "Boilerplate code passed.";
                        testLinkReference(html, res);
                    }
                }

                else
                {
                    res = "Boilerplate code failed";
                    testLinkReference(html, res);
                }
            }
        }
        public void testLinkReference(string html, string res)
        {
            string linkres = "";
            string finalresult = "";
            string[] linkTags = { "href=>", " <a href=","<a href=>", "script src=" };
            for (int i = 0; i < linkTags.Length; i++)
            {
                if (html.Contains(linkTags[i]))
                {
                    linkres = "Missing link reference found.";
                    finalresult = res + " " + linkres;

                    MessageBox.Show(finalresult);
                    return;
                }
                else
                {
                    linkres = "No missing link references";
                    finalresult = res + " " + linkres;
                    MessageBox.Show(finalresult);
                    //päätetään metodin suoritus, muuten messagebox näytettäisiin useamman kerran
                    return;
                }
            }

        }

    }
}
