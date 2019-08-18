using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Weather_Data
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            callDialog();
        }

        private void callDialog()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.DefaultExt = ".dat"; // Default file extension 


            // Show open file dialog box 
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {
                StringBuilder str = new StringBuilder("");
                var day = "";
                var min = 0.0;
                var tmp = 0;
                PreviewTextBlock.Text = "";
                // Open document 
                string filename = dlg.FileName;
                Console.WriteLine(filename);
                var lines = File.ReadAllLines(filename);
                var count = 1;
                
                foreach (var line in lines)
                {
                    var replacedLine = line.Trim();
                    RegexOptions options = RegexOptions.None;
                    Regex regex = new Regex("[ ]{2,}", options);
                    replacedLine = regex.Replace(replacedLine, " ");

                    if (!replacedLine.Contains("pre"))
                    {
                        str.Append("\n" + replacedLine);
                    }
                    var columns = replacedLine.Split(' ');
                    if (replacedLine.Trim().Length>0&&columns.Length > 3)
                    {
                        Double mxTmp = 0;
                        Double minTmp = 0;
                        Console.WriteLine(columns[1].Trim() + " - " + columns[2].Trim());
                        var isNumeric=Double.TryParse(RemoveExtraText(columns[1].Trim()), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out mxTmp);
                        var isNum=Double.TryParse(RemoveExtraText(columns[2].Trim()), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out minTmp);
                        if (isNumeric&&isNum)
                        {
                            var diff = mxTmp - minTmp;
                            if (tmp == 0)
                            {
                                min = diff;
                                tmp = 1;
                            }
                                
                            Console.WriteLine(mxTmp + " - " + minTmp + "=" + diff);
                            if (diff <= min)
                            {
                                day = columns[0];
                                min = diff;
                            }
                        }

                    }

                    count++;
                }
                PreviewTextBlock.Text = str.ToString();
                ResultLabel.Content = "Result:\n Day: " + day + "\n" + " Smallest Temperature Spread: " + min;
            }
        }

        private string RemoveExtraText(string value)
        {
            var allowedChars = "01234567890.";
            return new string(value.Where(c => allowedChars.Contains(c)).ToArray());
        }

        private void FileNameTextBox_Click(object sender, TextChangedEventArgs e)
        {
            callDialog();
        }
    }
}
