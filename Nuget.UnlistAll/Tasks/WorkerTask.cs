using System;
using System.ComponentModel;
using Nuget.UnlistAll.Models;

namespace Nuget.UnlistAll.Tasks
{
    public abstract class WorkerTask
    {
        public WorkerTask(IWorkerUi ui, NugetParams parameters)
        {
            this.Ui = ui;
            this.Params = parameters;
        }

        protected IWorkerUi Ui { get; private set; }

        protected NugetParams Params { get; private set; }

        protected BackgroundWorker Worker => Ui.Worker;

        protected abstract object ExecuteCore();

        public void Execute()
        {
            Ui.NotifyRunning(true);

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
            Ui.NotifyRunning(false, e.Result);
        }
    }
}
