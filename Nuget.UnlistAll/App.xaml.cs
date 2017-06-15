using System;
using System.Windows;
using System.Windows.Threading;
using System.Diagnostics;
using System.IO;
using System.Text;
using Shuhari.Framework.Utils;

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

        internal static void LogException(Exception exp)
        {
            Expect.IsNotNull(exp, nameof(exp));

            string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error.log");
            exp.LogToFile(logPath);
            File.AppendAllText(logPath, exp.GetFullTrace(), Encoding.UTF8);
        }
    }
}
