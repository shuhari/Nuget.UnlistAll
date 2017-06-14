using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using Nuget.UnlistAll.Models;
using Nuget.UnlistAll.Tasks;

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

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Worker = new BackgroundWorker
            {
                WorkerReportsProgress = true
            };

            _versions = new ObservableCollection<PackageVersionInfo>();
            versionList.ItemsSource = _versions;

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
            throw new System.NotImplementedException();
        }

        public BackgroundWorker Worker { get; private set; }

        public void NotifyRunning(bool running, object result = null)
        {
            mainDock.IsEnabled = !running;

            if (result != null)
            {
                var versions = result as PackageVersionInfo[];
                if (versions != null)
                {
                    _versions.Clear();
                    foreach (var version in versions)
                        _versions.Add(version);
                }
            }
        }

        public void NotifyProgress(int percentage, object userData)
        {
        }
    }
}
