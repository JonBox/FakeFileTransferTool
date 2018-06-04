using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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


namespace FakeFileTransferTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> filenames = new List<string> { };
        private bool stop = false;

        ObservableCollection<string> consoleOutput = new ObservableCollection<string>() { "Console Emulation Sample..." };
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Start_Click(object sender, RoutedEventArgs e)
        {
            stop = false;
            foreach (var filename in filenames)
            {
                FileName.Content = filename;
                await MoveProgress();
                FakeConsole.WriteOutput(filename + Environment.NewLine, Color.FromRgb(0,255,0));
                if (stop)
                {
                    return;
                }
            }


        }

        private async Task MoveProgress()
        {
            for (int i = 0; i < 100; i++)
            {
                ProgressBar.Value = i;
                await Task.Run(() =>
                {
                    Thread.Sleep(200);
                    if (stop)
                    {
                        return;
                    }
                });
                if (stop)
                {
                    return;
                }
            }
        }


        public ObservableCollection<string> ConsoleOutput
        {
            get
            {
                return consoleOutput;
            }
            set
            {
                consoleOutput = value;
                OnPropertyChanged("ConsoleOutput");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string propertyName)
        {
            if (null != PropertyChanged)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void LoadFiles_Click(object sender, RoutedEventArgs e)
        {
           OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files (*.csv)|*.csv";
            if (openFileDialog.ShowDialog() == true)
            {
                filenames = File.ReadAllLines(openFileDialog.FileName).ToList();
                btnTransfer.IsEnabled = true;
            }
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            stop = true;
        }
    }
}
