using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlEditor
{
    public class Sizing
    {
        public void txtBoxBigger(System.Windows.Controls.TextBox txtBox, double txtBoxHeight, double txtBoxWidth)
        {
            txtBoxHeight += 5;
            txtBoxWidth += 5;
            txtBox.Height = txtBoxHeight;
            txtBox.Width = txtBoxWidth;
        }

        public void txtBoxSmaller(System.Windows.Controls.TextBox txtBox, double txtBoxHeight, double txtBoxWidth)
        {
            txtBoxHeight -= 5;
            txtBoxWidth -= 5;
            txtBox.Height = txtBoxHeight;
            txtBox.Width = txtBoxWidth;

        }

        public void previewWindowSize(double value, System.Windows.Controls.WebBrowser wbrow, System.Windows.Controls.Label sliderVal)
        {
            wbrow.Width = value;
            wbrow.Height = value;
            //sliderVal.Content = value.ToString();
        }
    }
}
