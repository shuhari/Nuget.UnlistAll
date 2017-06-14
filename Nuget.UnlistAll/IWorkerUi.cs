using System.ComponentModel;

namespace Nuget.UnlistAll
{
    public interface IWorkerUi
    {
        BackgroundWorker Worker { get; }

        void NotifyRunning(bool running, object result = null);

        void NotifyProgress(int percentage, object userData);
    }
}
