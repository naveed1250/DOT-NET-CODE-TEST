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
                var team = "";
                var min = -1;
             
                PreviewTextBlock.Text = "";
                // Open document 
                string filename = dlg.FileName;
                Console.WriteLine(filename);
                var lines = File.ReadAllLines(filename);
                var count = 1;
                
                foreach (var line in lines)
                {
                    var replacedLine = line.Trim();
                   

                    if (!replacedLine.Contains("pre"))
                    {
                        str.Append("\n" + line);
                    }
                    RegexOptions options = RegexOptions.None;
                    Regex regex = new Regex("[ ]{2,}", options);
                    replacedLine = regex.Replace(replacedLine, " ");
                    var columns = replacedLine.Split(' ');
                    if (replacedLine.Trim().Length>0&&columns.Length > 8)
                    {
                        int forGoals = 0;
                        int againstGoals = 0;
                        
                        var isNumeric = int.TryParse(RemoveExtraText(columns[6].Trim()), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out forGoals);
                        var isNum = int.TryParse(RemoveExtraText(columns[8].Trim()), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out againstGoals);
                        if (isNumeric && isNum)
                        {
                            var diff = Math.Abs(forGoals - againstGoals);
                            if (min < 0)
                            {
                                min = diff;

                            }

                            Console.WriteLine(forGoals + " - " + againstGoals + "=" + diff);
                            if (diff <= min)
                            {
                                team = columns[1].Trim();
                                min = diff;
                            }
                        }


                    }

                    count++;
                }
                PreviewTextBlock.Text = str.ToString();
                ResultLabel.Content = "Result:\n Team: " + team + "\n" + " Goals Difference: " + min;
            }
        }

        private string RemoveExtraText(string value)
        {
            var allowedChars = "01234567890";
            return new string(value.Where(c => allowedChars.Contains(c)).ToArray());
        }

        private void FileNameTextBox_Click(object sender, TextChangedEventArgs e)
        {
            callDialog();
        }
    }
}
