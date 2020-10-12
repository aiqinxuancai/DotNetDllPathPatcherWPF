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

namespace DotNetDllPathPatcherWPF
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


        private void mainGrid_Drop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                return;
            }
            string fileName = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();

            Debug.WriteLine(fileName);

            pathTextBox.Text = fileName;
        }

        private void mainGrid_DragEnter(object sender, DragEventArgs e)
        {
            //if (e.Data.GetDataPresent(DataFormats.FileDrop))
            //    e.Effects = DragDropEffects.Link;                            //WinForm中为e.Effect = DragDropEffects.Link
            //else e.Effects = DragDropEffects.None;                      //WinFrom中为e.Effect = DragDropEffects.None
        }

        private void runButton_Click(object sender, RoutedEventArgs e)
        {
            Conversion.ConversionDll(pathTextBox.Text, "bin", "");
        }
    }
}
