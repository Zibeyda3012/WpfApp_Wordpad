using Microsoft.Win32;
using System.Data.SqlTypes;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace WpfApp_Wordpad
{
    public partial class MainWindow : Window
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        string copyTxt { get; set; } = "";
        bool autoSaveEnabled = false;
        public MainWindow()
        {
            InitializeComponent();
            chkBox.Checked += chkBox_Checked;

        }

        private void open_btn_Click(object sender, RoutedEventArgs e)
        {

            if (openFileDialog.ShowDialog() == true)
            {
                txtBox.Text = openFileDialog.FileName;
                string text = File.ReadAllText(openFileDialog.FileName);
                richTxt_Box.Document.Blocks.Clear();
                richTxt_Box.Document.Blocks.Add(new Paragraph(new Run(text)));
            }
        }

        private void chkBox_Checked(object sender, RoutedEventArgs e)
        {
            if(chkBox.IsChecked == true)
                autoSaveEnabled = true;
            else
                autoSaveEnabled = false;
        }

        private void save_btn_Click(object sender, RoutedEventArgs e)
        {
            SaveTextToFile();   
        }

        private void cut_btn_Click(object sender, RoutedEventArgs e)
        {
            TextRange textRange = new TextRange(richTxt_Box.Selection.Start, richTxt_Box.Selection.End);
            copyTxt = textRange.Text;
            textRange.Text = "";

        }

        private void copy_btn_Click(object sender, RoutedEventArgs e)
        {
            string selectedText = new TextRange(richTxt_Box.Selection.Start, richTxt_Box.Selection.End).Text;
            copyTxt = selectedText;
            selectedText = "";
        }

        private void paste_btn_Click(object sender, RoutedEventArgs e)
        {
            TextPointer insertPosition = richTxt_Box.CaretPosition;
            insertPosition.InsertTextInRun(copyTxt);
        }

        private void selectAll_btn_Click(object sender, RoutedEventArgs e)
        {
            richTxt_Box.SelectAll();
            string selectedText = new TextRange(richTxt_Box.Selection.Start, richTxt_Box.Selection.End).Text;
        }

        private void richTxt_Box_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (autoSaveEnabled)
                SaveTextToFile();
        }

        private void saveAs_btn_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();   
            if(saveFileDialog.ShowDialog()==true)
            {
                TextRange textRange = new TextRange(richTxt_Box.Document.ContentStart, richTxt_Box.Document.ContentEnd);
                string text = textRange.Text;
                File.WriteAllText (saveFileDialog.FileName,text);
            }
        }

        private void SaveTextToFile()
        {
            string text = new TextRange(richTxt_Box.Document.ContentStart, richTxt_Box.Document.ContentEnd).Text;
            File.WriteAllText(txtBox.Text, text);
        }
    }
}