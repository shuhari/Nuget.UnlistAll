using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Navigation;
using Nuget.UnlistAll.Resources;

namespace Nuget.UnlistAll.Dialogs
{
    /// <summary>
    /// Interaction logic for AboutDialog.xaml
    /// </summary>
    public partial class AboutDialog : Window
    {
        public AboutDialog()
        {
            InitializeComponent();

            Title = Strings.AboutTitle;
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (Owner != null)
            {
                FontFamily = Owner.FontFamily;
                FontSize = Owner.FontSize;
            }
            txtAppTitle.Text = string.Format("{0} v{1}", Strings.Title, 
                Assembly.GetEntryAssembly().GetName().Version);
        }

        private void OnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void OnClose(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
