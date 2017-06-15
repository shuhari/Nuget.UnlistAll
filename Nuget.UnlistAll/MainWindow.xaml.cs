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
using System.Threading;
using System.Windows.Interop;
using System.Windows.Media;
using Nuget.UnlistAll.Configuration;
using Nuget.UnlistAll.Dialogs;
using Nuget.UnlistAll.Resources;

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

            Title = Strings.Title;
            Loaded += OnLoaded;
        }

        private ObservableCollection<PackageVersion> _versions;

        private ObservableCollection<LogItem> _logs;

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // Initialize window
            Worker = new BackgroundWorker
            {
                WorkerReportsProgress = true
            };

            _versions = new ObservableCollection<PackageVersion>();
            versionList.ItemsSource = _versions;

            _logs = new ObservableCollection<LogItem>();
            logList.ItemsSource = _logs;

            var parameters = AppConfig.Load();
            txtPackageId.Text = parameters.PackageId;
            txtApiKey.Text = parameters.ApiKey;
        }

        private void OnSelectAll(object sender, RoutedEventArgs e)
        {
            // Select all versions
            foreach (var version in _versions)
                version.Selected = true;
        }

        private void OnSelectNone(object sender, RoutedEventArgs e)
        {
            // Select none version
            foreach (var version in _versions)
                version.Selected = false;
        }

        private AppConfig ValidateParams()
        {
            // Collect user input parameters and validate
            var parameters = new AppConfig(txtPackageId.Text.Trim(), txtApiKey.Text.Trim());
            parameters.Validate();
            parameters.Save();
            return parameters;
        }

        private void Alert(string message, MessageBoxImage icon = MessageBoxImage.Information)
        {
            MessageBox.Show(this, message, Strings.Title, 
                MessageBoxButton.OK, icon);
        }

        private bool Confirm(string message)
        {
            var reply = MessageBox.Show(this, message, Strings.Title,
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            return reply == MessageBoxResult.Yes;
        }

        private void OnListVersions(object sender, RoutedEventArgs e)
        {
            // List versions
            try
            {
                var parameters = ValidateParams();
                new ListVersionsTask(this, parameters).Execute();
            }
            catch (ValidationException exp)
            {
                Alert(exp.Message, MessageBoxImage.Warning);
            }
        }

        private void OnUnlist(object sender, RoutedEventArgs e)
        {
            // Unlist selected versions
            try
            {
                var parameters = ValidateParams();;
                var versions = _versions.Where(x => x.Selected).ToArray();
                if (versions.Length == 0)
                    throw new ValidationException(Strings.SelectVersions);
                string msg = string.Format(Strings.ConfirmUnlist, versions.Length, parameters.PackageId);
                if (Confirm(msg))
                    new UnlistTask(this, parameters, versions).Execute();
            }
            catch (ValidationException exp)
            {
                Alert(exp.Message, MessageBoxImage.Warning);
            }
        }

        public BackgroundWorker Worker { get; private set; }

        public void NotifyTaskBegin()
        {
            // Disable input when task begin
            EnableControls(false, txtPackageId, txtApiKey, 
                btnListVersions, btnSelect, btnUnlist);
            _logs.Clear();
        }

        public void NotifyTaskFinished(object result)
        {
            // Enable input when task finished, and show result
            EnableControls(true, txtPackageId, txtApiKey, 
                btnListVersions, btnSelect, btnUnlist);

            var versions = result as PackageVersion[];
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
            // Task notify callback
            if (userData is LogItem)
            {
                AddLog((LogItem)userData);
            }
            else if (userData is Exception)
            {
                var exp = (Exception)userData;
                AddLog(new LogItem(DateTime.Now, false, exp.Message));
                App.LogException(exp);
            }
        }

        private void AddLog(LogItem log)
        {
            _logs.Add(log);
            logList.SelectedItem = log;
            logList.ScrollIntoView(log);
        }

        private void OnAbout(object sender, RoutedEventArgs e)
        {
            var dlg = new AboutDialog();
            dlg.Owner = this;
            dlg.ShowDialog();
        }
    }

    /// <summary>
    /// Show success/error log in different colors
    /// </summary>
    public class LogForegroundConverter : IValueConverter
    {
        private readonly Brush _successBrush = new SolidColorBrush(Colors.DarkGreen);
        private readonly Brush _errorBrush = new SolidColorBrush(Colors.Red);

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
