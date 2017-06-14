using System;
using System.Windows;
using System.Windows.Threading;
using System.Diagnostics;

namespace Nuget.UnlistAll
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            if (e.Exception != null && !Debugger.IsAttached)
            {
                e.Dispatcher.Invoke(() => HandleException(e.Exception));
                e.Handled = true;
            }
        }

        private void HandleException(Exception exp)
        {
            MessageBox.Show(exp.Message);
        }
    }
}
