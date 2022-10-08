using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using DotNetDllPathPatcherWPF;
using System.Diagnostics;
using System.Linq;

namespace DotNetDllPathPatcherAvalonia.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
       
            mainGrid.AddHandler(DragDrop.DropEvent, mainGrid_Drop, RoutingStrategies.Tunnel);

    
        }



        private void mainGrid_Drop(DragEventArgs e)
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
            e.DragEffects = DragDropEffects.Copy;
            e.Handled = true;
        }

        private void pathTextBox_PreviewDrop(object sender, DragEventArgs e)
        {
            OnDropFile(e);
        }

        private void OnDropFile(DragEventArgs e)
        {
            //DataFormats.Text

            var format = e.Data.GetDataFormats();

            if (!format.Any(a => a == DataFormats.FileNames))
            {
                return;
            }
            string fileName = e.Data.GetFileNames().First();

            Debug.WriteLine(fileName);

            pathTextBox.Text = fileName;
        }

    }
}
