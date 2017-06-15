using System;
using System.ComponentModel;
using Nuget.UnlistAll.Configuration;
using Nuget.UnlistAll.Models;
using Shuhari.Framework.Utils;

namespace Nuget.UnlistAll.Tasks
{
    /// <summary>
    /// Base implementation for background worker task
    /// </summary>
    public abstract class WorkerTask
    {
        protected WorkerTask(IWorkerUi ui, AppConfig config)
        {
            Expect.IsNotNull(ui, nameof(ui));
            Expect.IsNotNull(config, nameof(config));

            this.Ui = ui;
            this.Config = config;
        }

        protected IWorkerUi Ui { get; private set; }

        protected AppConfig Config { get; private set; }

        protected BackgroundWorker Worker => Ui.Worker;

        protected abstract object ExecuteCore();

        /// <summary>
        /// Start worker thread
        /// </summary>
        public void Execute()
        {
            Ui.NotifyTaskBegin();

            Worker.DoWork += OnWorkerDoWork;
            Worker.ProgressChanged += OnWorkerProgressChanged;
            Worker.RunWorkerCompleted += OnWorkerCompleted;
            Worker.RunWorkerAsync();
        }

        private void OnWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                e.Result = ExecuteCore();
            }
            catch (Exception exp)
            {
                Worker.ReportProgress(0, exp);
            }
        }

        private void OnWorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Ui.NotifyProgress(e.ProgressPercentage, e.UserState);
        }

        private void OnWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Worker.DoWork -= OnWorkerDoWork;
            Worker.ProgressChanged -= OnWorkerProgressChanged;
            Worker.RunWorkerCompleted -= OnWorkerCompleted;
            Ui.NotifyTaskFinished(e.Result);
        }

        /// <summary>
        /// Notify UI that a log should be added
        /// </summary>
        /// <param name="success"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        protected void NotifyLog(bool success, string format, params object[] args)
        {
            string msg = string.Format(format, args);
            var log = new LogItem(DateTime.Now, success, msg);
            Worker.ReportProgress(0, log);
        }
    }
}
