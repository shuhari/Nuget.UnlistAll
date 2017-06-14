using System;
using System.Linq;
using System.Reflection;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Controls;
using Nuget.UnlistAll.Models;
using Nuget.UnlistAll.Tasks;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Media;

namespace Nuget.UnlistAll
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IWorkerUi
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += OnLoaded;

        }

        private ObservableCollection<PackageVersionInfo> _versions;

        private ObservableCollection<LogItem> _logs;

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Worker = new BackgroundWorker
            {
                WorkerReportsProgress = true
            };

            _versions = new ObservableCollection<PackageVersionInfo>();
            versionList.ItemsSource = _versions;

            _logs = new ObservableCollection<LogItem>();
            logList.ItemsSource = _logs;

            var parameters = NugetParams.Load();
            txtPackageId.Text = parameters.PackageId;
            txtApiKey.Text = parameters.ApiKey;
        }

        private void OnSelectAll(object sender, RoutedEventArgs e)
        {
            foreach (var version in _versions)
                version.Selected = true;
        }

        private void OnSelectNone(object sender, RoutedEventArgs e)
        {
            foreach (var version in _versions)
                version.Selected = false;
        }

        private void OnGetVersions(object sender, RoutedEventArgs e)
        {
            try
            {
                var parameters = new NugetParams(txtPackageId.Text.Trim(), txtApiKey.Text.Trim());
                parameters.Validate();
                parameters.Save();
                new GetVersionsTask(this, parameters).Execute();
            }
            catch (ValidationException exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void OnUnlist(object sender, RoutedEventArgs e)
        {
            try
            {
                var parameters = new NugetParams(txtPackageId.Text.Trim(), txtApiKey.Text.Trim());
                parameters.Validate();
                parameters.Save();
                var versions = _versions.Where(x => x.Selected).ToArray();
                if (versions.Length == 0)
                {
                    MessageBox.Show("Select version(s) to unlist");
                    return;
                }
                new UnlistTask(this, parameters, versions).Execute();
            }
            catch (ValidationException exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        public BackgroundWorker Worker { get; private set; }

        public void NotifyTaskBegin()
        {
            EnableControls(false, txtPackageId, txtApiKey, 
                btnGetVersions, btnSelect, btnUnlist);
        }

        public void NotifyTaskFinished(object result)
        {
            EnableControls(true, txtPackageId, txtApiKey, 
                btnGetVersions, btnSelect, btnUnlist);

            var versions = result as PackageVersionInfo[];
            if (versions != null)
            {
                _versions.Clear();
                foreach (var version in versions)
                    _versions.Add(version);
            }
        }

        private void EnableControls(bool enable, params Control[] controls)
        {
            foreach (var control in controls)
            {
                control.IsEnabled = enable;
            }
        }

        public void NotifyProgress(int percentage, object userData)
        {
            if (userData is LogItem)
            {
                AddLog((LogItem)userData);
            }
            else if (userData is Exception)
            {
                var exp = (Exception)userData;
                AddLog(new LogItem(DateTime.Now, false, exp.Message));
            }
        }

        private void AddLog(LogItem log)
        {
            _logs.Add(log);
            logList.ScrollIntoView(log);
        }

        private void OnAbout(object sender, RoutedEventArgs e)
        {
            var msg = string.Format("Nuget Unlist Tool version {0} by shuhari (https://github.com/shuhari)",
                Assembly.GetEntryAssembly().GetName().Version);
            MessageBox.Show(msg, "About Nuget Unlist Tool");
        }
    }

    class LogForegroundConverter : IValueConverter
    {
        private Brush _successBrush = new SolidColorBrush(Colors.DarkGreen);
        private Brush _errorBrush = new SolidColorBrush(Colors.Red);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var log = value as LogItem;
            if (log != null)
                return log.Success ? _successBrush : _errorBrush;
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
