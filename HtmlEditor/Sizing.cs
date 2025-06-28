using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HtmlEditor
{
    public class Sizing
    {
        private double saveBtnMarginLeft;
        public double SaveBtnMarginLeft
        {
            get { return saveBtnMarginLeft; }
            set { saveBtnMarginLeft = value; }

        }
        private double sizeBtnPlus;
        public double SizeBtnPlus
        {
            get { return sizeBtnPlus; }
            set { sizeBtnPlus = value; }
        }

        private double sizeBtnMinus;
        public double SizeBtnMinus
        {
            get { return sizeBtnMinus; }
            set { sizeBtnMinus = value; }
        }

        private double resetBtnMargin;
        public double ResetBtnMargin
        {
            get { return resetBtnMargin; }
            set { resetBtnMargin = value; }
        }
        public void txtBoxBigger(System.Windows.Controls.TextBox txtBox, double txtBoxHeight, double txtBoxWidth, System.Windows.Controls.Button saveBtn, System.Windows.Controls.Button boxDecreaseBtn, System.Windows.Controls.Button boxIncreaseBtn, System.Windows.Controls.Button resetBtn, System.Windows.Controls.Label currentSize)
        {
            txtBoxHeight += 5;
            txtBoxWidth += 5;
            txtBox.Height = txtBoxHeight;
            txtBox.Width = txtBoxWidth;
            currentSize.Content = txtBoxWidth + "x" + txtBoxHeight;

            //asetetaan savemarginleft propertyn avulla buttonin margin left arvo SaveBtnMarginLeft
            //kenttään.
            SaveBtnMarginLeft = saveBtn.Margin.Left;
            SizeBtnPlus = boxIncreaseBtn.Margin.Left;
            SizeBtnMinus = boxDecreaseBtn.Margin.Left;
            ResetBtnMargin = resetBtn.Margin.Left;
            //kun txtboxin leveys on yli 330 pikseliä, siirretään savebuttonia 5 pikseliä vasemmalle.
            if (txtBoxWidth > 330)
            {

                //kasvatetaan Margin left arvoja, eli tämäb avulla siirretään painikkeita vasemmall 5 px
                //klikkaus
                saveBtnMarginLeft += 5;
                sizeBtnPlus += 5;
                sizeBtnMinus += 5;
                resetBtnMargin += 5;
                saveBtn.Margin = new System.Windows.Thickness(saveBtnMarginLeft, saveBtn.Margin.Top, saveBtn.Margin.Right, saveBtn.Margin.Bottom);
                boxDecreaseBtn.Margin = new System.Windows.Thickness(sizeBtnMinus, boxDecreaseBtn.Margin.Top, boxDecreaseBtn.Margin.Right, boxDecreaseBtn.Margin.Bottom);
                boxIncreaseBtn.Margin = new System.Windows.Thickness(sizeBtnPlus, boxIncreaseBtn.Margin.Top, boxIncreaseBtn.Margin.Right, boxIncreaseBtn.Margin.Bottom);
                resetBtn.Margin = new System.Windows.Thickness(resetBtnMargin, resetBtn.Margin.Top, resetBtn.Margin.Right, resetBtn.Margin.Bottom);
            }
        }

        public void txtBoxSmaller(System.Windows.Controls.TextBox txtBox, double txtBoxHeight, double txtBoxWidth, System.Windows.Controls.Button saveBtn, System.Windows.Controls.Button boxDecreaseBtn, System.Windows.Controls.Button boxIncreaseBtn, System.Windows.Controls.Button resetBtn)
        {
            txtBoxHeight -= 5;
            txtBoxWidth -= 5;
            //txtboxsin minimiarvot, jos mennään näiden alapuolelle boksi ei enää pienene
            txtBox.MinHeight = 200;
            txtBox.MinWidth = 200;
            txtBox.Height = txtBoxHeight;
            txtBox.Width = txtBoxWidth;
            SaveBtnMarginLeft = saveBtn.Margin.Left;
            if (txtBoxWidth < 330)
            {
                saveBtnMarginLeft -= 5;
                sizeBtnPlus -= 5;
                sizeBtnMinus -= 5;
                resetBtnMargin -= 5;
                saveBtn.Margin = new System.Windows.Thickness(saveBtnMarginLeft, saveBtn.Margin.Top, saveBtn.Margin.Right, saveBtn.Margin.Bottom);
                boxDecreaseBtn.Margin = new System.Windows.Thickness(sizeBtnMinus, boxDecreaseBtn.Margin.Top, boxDecreaseBtn.Margin.Right, boxDecreaseBtn.Margin.Bottom);
                boxIncreaseBtn.Margin = new System.Windows.Thickness(sizeBtnPlus, boxIncreaseBtn.Margin.Top, boxIncreaseBtn.Margin.Right, boxIncreaseBtn.Margin.Bottom);
                resetBtn.Margin = new System.Windows.Thickness(resetBtnMargin, resetBtn.Margin.Top, resetBtn.Margin.Right, resetBtn.Margin.Bottom);
            }


        }

        public void previewWindowSize(double value, System.Windows.Controls.WebBrowser wbrow)
        {
            wbrow.Width = value;
            wbrow.Height = value;

        }

        public void restoreSizesAndMargins(System.Windows.Controls.TextBox txtBox, System.Windows.Controls.Button boxDecreaseBtn, System.Windows.Controls.Button boxIncreaseBtn)
        {
            txtBox.Width = 320;
            txtBox.Height = 200;
            boxIncreaseBtn.Margin = new System.Windows.Thickness(359, 188, 0, 0);
            boxDecreaseBtn.Margin = new System.Windows.Thickness(359, 231, 0, 0);

        }
    }
}
