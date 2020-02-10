using AxwareERC.Objects;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;

namespace AxwareERC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string path = @"C:\Users\Fernando\Documents\ERC\RCC #11";
        static string filename = "rc11.rgg";

        private static ViewModel _vm;
        private static List<Competitor> competitorsResults = new List<Competitor>();

        public MainWindow()
        {

            InitializeComponent();
            _vm = new ViewModel();
            this.DataContext = _vm;

            //competitorsResults = CompetitorService.ReadFile(Path.Combine(path,filename));
            //_vm.CompetitorsResultList = competitorsResults;
            DataGrid1.DataContext = competitorsResults;
            //var lastUpdated = File.GetLastWriteTime(Path.Combine(path, filename));
            //_vm.AppTitle = string.Concat("ERC Axware - last updated: ", lastUpdated.ToString());
            //CreateFileWatcher(path);
        }

        private void openFileClick (object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Axware RGG files (*.rgg)|*.rgg";
            if (openFileDialog.ShowDialog() == true)
            {
                var fullDir = openFileDialog.FileName;
                filename = Path.GetFileName(fullDir);
                path = Path.GetDirectoryName(fullDir);
                competitorsResults = CompetitorService.ReadFile(fullDir);
                DataGrid1.DataContext = competitorsResults;
                var lastUpdated = File.GetLastWriteTime(fullDir);
                _vm.AppTitle = string.Concat("ERC Axware - last updated: ", lastUpdated.ToString());
                CreateFileWatcher(path, filename);
            }

        }

        private void exitClick(object sender, RoutedEventArgs e)
        {
            // Exit application
            Application.Current.Shutdown();
        }

        private void maximizeSecondDisplay(object sender, RoutedEventArgs e)
        {
            // Exit application
            ExtendedControls.WindowExt.MaximizeToSecondaryMonitor(this);
        }


        public void CreateFileWatcher(string path, string filename)
        {
            // Create a new FileSystemWatcher and set its properties.
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = path;
            /* Watch for changes in LastAccess and LastWrite times, and 
               the renaming of files or directories. */
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            // Only watch text files.
            watcher.Filter = filename;

            // Add event handlers.
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.Deleted += new FileSystemEventHandler(OnChanged);

            // Begin watching.
            watcher.EnableRaisingEvents = true;
        }

        // Define the event handlers.
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            Thread.Sleep(500);
            // Specify what is done when a file is changed, created, or deleted.
            competitorsResults = CompetitorService.ReadFile(Path.Combine(path, filename));
            this.Dispatcher.Invoke(() =>
            {
                DataGrid1.DataContext = competitorsResults;
            });
            
            // Update window title to reflect changes
            var lastUpdated = File.GetLastWriteTime(Path.Combine(path, filename));
            _vm.AppTitle = string.Concat("ERC Axware - last updated: ", lastUpdated.ToString());
        }
    }


}

namespace ExtendedControls
{
    static public class WindowExt
    {
        public static void MaximizeToSecondaryMonitor(this Window window)
        {
            var secondaryScreen = System.Windows.Forms.Screen.AllScreens.Where(s => !s.Primary).FirstOrDefault();

            if (secondaryScreen != null)
            {
                if (!window.IsLoaded)
                    window.WindowStartupLocation = WindowStartupLocation.Manual;

                var workingArea = secondaryScreen.WorkingArea;
                window.Left = workingArea.Left;
                window.Top = workingArea.Top;
                window.Width = workingArea.Width;
                window.Height = workingArea.Height;
                // If window isn't loaded then maxmizing will result in the window displaying on the primary monitor
                if (window.IsLoaded)
                    window.WindowState = WindowState.Maximized;
            }
        }
    }
}