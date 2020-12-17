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
        static string path;
        static string filename;

        private static ViewModel _vm;
        //private static List<Competitor> competitorsRawResults = new List<Competitor>();
        private static List<CompetitorViewModel> competitorsResults = new List<CompetitorViewModel>();

        public MainWindow()
        {

            InitializeComponent();
            _vm = new ViewModel();
            this.DataContext = _vm;

            DataGrid1.DataContext = competitorsResults;
        }

        private double singleTimeValidation(ref string timeString, string penalties)
        {
            if (timeString != "")
            {
                if (Convert.ToDouble(timeString) > 300.0)
                    timeString = "300.000";
                var time = Convert.ToDouble(timeString);
                
                if (penalties != "")
                    timeString += " (" + penalties + ")";

                return time;
            }
            return 0;
        }

        private List<CompetitorViewModel> generateViewModelResults(List<CompetitorAxware> competitorsRawResults)
        {
            int maxNumberOfTimes = 0;
            string[] timesStr = new string[20];
            double[] times = new double[20];
            double[] overallCompetitorTimes = new double[competitorsRawResults.Count()];
            List<double> n2CompetitorTimes = new List<double>();
            List<double> n4CompetitorTimes = new List<double>();
            List<double> e2CompetitorTimes = new List<double>();
            List<double> e4CompetitorTimes = new List<double>();
            List<double> proCompetitorTimes = new List<double>();
            List<double> truckCompetitorTimes = new List<double>();

            // Container to hold calculated results, but unordered
            List<CompetitorViewModel> competitorsProcessedResults = new List<CompetitorViewModel>();
            int competitorIndex = 0;

            // First find how many runs each competitor have completed and find the maximum value;
            foreach (CompetitorAxware competitor in competitorsRawResults)
            {
                var timeList = new List<string> { competitor.Run1, competitor.Run2, competitor.Run3,competitor.Run4,competitor.Run5,competitor.Run6,competitor.Run7,competitor.Run8, competitor.Run9,competitor.Run10,competitor.Run11,
                    competitor.Run12,competitor.Run13,competitor.Run14,competitor.Run15,competitor.Run16,competitor.Run17,competitor.Run18,competitor.Run19,competitor.Run20 };
                var numberOfTimes = timeList.Count(k => k != "");
                if (numberOfTimes > maxNumberOfTimes)
                    maxNumberOfTimes = numberOfTimes;
            }

            // Calculate total time and position for each competitor
            foreach (CompetitorAxware competitor in competitorsRawResults)
            {
                var competitorViewModel = new CompetitorViewModel
                {
                    // Assign the basic properties
                    Car = competitor.Car,
                    Name = competitor.Name,
                    Class = competitor.Class,
                    Number = competitor.Number
                };

                var timeList = new List<string> { competitor.Run1, competitor.Run2, competitor.Run3,competitor.Run4,competitor.Run5,competitor.Run6,competitor.Run7,competitor.Run8, competitor.Run9,competitor.Run10,competitor.Run11,
                    competitor.Run12,competitor.Run13,competitor.Run14,competitor.Run15,competitor.Run16,competitor.Run17,competitor.Run18,competitor.Run19,competitor.Run20 };
                var penaltyList = new List<string> { competitor.Pen1, competitor.Pen2, competitor.Pen3, competitor.Pen4, competitor.Pen5, competitor.Pen6, competitor.Pen7, competitor.Pen8, competitor.Pen9, competitor.Pen10,
                    competitor.Pen11, competitor.Pen12,competitor.Pen13, competitor.Pen14,competitor.Pen15, competitor.Pen16,competitor.Pen17, competitor.Pen18,competitor.Pen19, competitor.Pen20};

                // Add times to an array
                int i = 0;
                foreach (var time in timeList)
                {
                    timesStr[i] = time;
                    i++;
                }

                // Update time string with penalties (if applicable). Times over 300s or Off-course are capped
                i = 0;
                foreach (string penalty in penaltyList)
                {
                    times[i] = singleTimeValidation(ref timesStr[i], penalty);
                    i++;
                }

                // Update view model with times + penalty
                competitorViewModel.Time1 = timesStr[0];
                competitorViewModel.Time2 = timesStr[1];
                competitorViewModel.Time3 = timesStr[2];
                competitorViewModel.Time4 = timesStr[3];
                competitorViewModel.Time5 = timesStr[4];
                competitorViewModel.Time6 = timesStr[5];
                competitorViewModel.Time7 = timesStr[6];
                competitorViewModel.Time8 = timesStr[7];
                competitorViewModel.Time9 = timesStr[8];
                competitorViewModel.Time10 = timesStr[9];
                competitorViewModel.Time11 = timesStr[10];
                competitorViewModel.Time12 = timesStr[11];
                competitorViewModel.Time13 = timesStr[12];
                competitorViewModel.Time14 = timesStr[13];
                competitorViewModel.Time15 = timesStr[14];
                competitorViewModel.Time16 = timesStr[15];
                competitorViewModel.Time17 = timesStr[16];
                competitorViewModel.Time18 = timesStr[17];
                competitorViewModel.Time19 = timesStr[18];
                competitorViewModel.Time20 = timesStr[19];

                // If competitor has less timed runs than the maximum number of timed runs, add 300s to the missed runs
                if (times.Count(k => k > 0) < maxNumberOfTimes)
                {
                    for (int j = times.Count(k => k > 0); j < maxNumberOfTimes; j++)
                        times[j] = 300;
                }
                overallCompetitorTimes[competitorIndex] = times.Sum() - times.Max();

                // Raw time (all summed up)
                competitorViewModel.RawTime = times.Sum();
                //Discard the slowest time
                competitorViewModel.MinusSlowest = times.Sum() - times.Max();

                switch (competitor.Class)
                {
                    case CompetitionClass.n2:
                        n2CompetitorTimes.Add(competitorViewModel.MinusSlowest);
                        break;
                    case CompetitionClass.n4:
                        n4CompetitorTimes.Add(competitorViewModel.MinusSlowest);
                        break;
                    case CompetitionClass.e2:
                        e2CompetitorTimes.Add(competitorViewModel.MinusSlowest);
                        break;
                    case CompetitionClass.e4:
                        e4CompetitorTimes.Add(competitorViewModel.MinusSlowest);
                        break;
                    case CompetitionClass.pro:
                    case CompetitionClass.p2:
                    case CompetitionClass.p4:
                        proCompetitorTimes.Add(competitorViewModel.MinusSlowest);
                        break;
                    case CompetitionClass.truck:
                        truckCompetitorTimes.Add(competitorViewModel.MinusSlowest);
                        break;
                    default:
                        break;
                }
                competitorsProcessedResults.Add(competitorViewModel);
                competitorIndex++;
            }

            // Find out what is the overall position for each competitor
            double[] n2CompetitorTimesArray = n2CompetitorTimes.ToArray();
            double[] n4CompetitorTimesArray = n4CompetitorTimes.ToArray();
            double[] e2CompetitorTimesArray = e2CompetitorTimes.ToArray();
            double[] e4CompetitorTimesArray = e4CompetitorTimes.ToArray();
            double[] proCompetitorTimesArray = proCompetitorTimes.ToArray();
            double[] truckCompetitorTimesArray = truckCompetitorTimes.ToArray();

            Array.Sort(overallCompetitorTimes);
            Array.Sort(n2CompetitorTimesArray);
            Array.Sort(n4CompetitorTimesArray);
            Array.Sort(e2CompetitorTimesArray);
            Array.Sort(e4CompetitorTimesArray);
            Array.Sort(proCompetitorTimesArray);
            Array.Sort(truckCompetitorTimesArray);

            foreach (var competitor in competitorsProcessedResults)
            {
                competitor.Overall = Array.IndexOf(overallCompetitorTimes, competitor.MinusSlowest) + 1;
                switch (competitor.Class)
                {
                    case CompetitionClass.n2:
                        competitor.InClass = Array.IndexOf(n2CompetitorTimesArray, competitor.MinusSlowest) + 1;
                        break;
                    case CompetitionClass.n4:
                        competitor.InClass = Array.IndexOf(n4CompetitorTimesArray, competitor.MinusSlowest) + 1;
                        break;
                    case CompetitionClass.e2:
                        competitor.InClass = Array.IndexOf(e2CompetitorTimesArray, competitor.MinusSlowest) + 1;
                        break;
                    case CompetitionClass.e4:
                        competitor.InClass = Array.IndexOf(e4CompetitorTimesArray, competitor.MinusSlowest) + 1;
                        break;
                    case CompetitionClass.pro:
                    case CompetitionClass.p2:
                    case CompetitionClass.p4:
                        competitor.InClass = Array.IndexOf(proCompetitorTimesArray, competitor.MinusSlowest) + 1;
                        break;
                    case CompetitionClass.truck:
                        competitor.InClass = Array.IndexOf(truckCompetitorTimesArray, competitor.MinusSlowest) + 1;
                        break;
                    default:
                        break;
                }
            }
            return competitorsProcessedResults.OrderBy(o => o.Overall).ToList();
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
                var competitorsRawResults = CompetitorService.ReadFile(fullDir);
                competitorsResults = generateViewModelResults(competitorsRawResults);
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
            // Maximize application on second display (if any)
            ExtendedControls.WindowExt.MaximizeToSecondaryMonitor(this);
        }

        private void generateResults(object sender, RoutedEventArgs e)
        {
            // Generate ERC results (under development) 
            if (path != null)
                if (CompetitorService.WriteFile(Path.Combine(path, "ERCresult.csv"), competitorsResults))
                    MessageBox.Show("Resuls saved to:\n" + Path.Combine(path, "ERCresult.csv"), "Success");
                else
                MessageBox.Show("No results to calculate","Error");
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
            var competitorsRawResults = CompetitorService.ReadFile(Path.Combine(path, filename));

            this.Dispatcher.Invoke(() =>
            {
                DataGrid1.DataContext = generateViewModelResults(competitorsRawResults);
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