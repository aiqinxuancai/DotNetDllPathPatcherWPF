using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace DotNetDllPathPatcherWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            //nowDirTextBlock.Text = Directory.GetCurrentDirectory();
           
        }


        private void mainGrid_Drop(object sender, DragEventArgs e)
        {
            OnDropFile(e);
        }

        private void mainGrid_DragEnter(object sender, DragEventArgs e)
        {

        }

        private void runButton_Click(object sender, RoutedEventArgs e)
        {
            Conversion.ConversionDll(pathTextBox.Text, newPathTextBox.Text, "");
        }

        private void pathTextBox_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
            e.Handled = true;
        }

        private void pathTextBox_PreviewDrop(object sender, DragEventArgs e)
        {
            OnDropFile(e);
        }

        private void OnDropFile(DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                return;
            }
            string fileName = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();

            Debug.WriteLine(fileName);

            pathTextBox.Text = fileName;
        }


    }
}
