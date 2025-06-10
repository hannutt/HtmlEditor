using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HtmlEditor
{
    public class FontSetups
    {
        //public TextBox txtBox { get; set; }
        public void fontItalic(TextBox txtBox)
        {
            txtBox.FontStyle = FontStyles.Italic;

        }

        public void DoFontRestore(TextBox txtBox)
        {
            txtBox.FontStyle = FontStyles.Normal;
        }
    }
}
