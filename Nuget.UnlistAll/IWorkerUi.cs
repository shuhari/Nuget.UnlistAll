using System.ComponentModel;

namespace Nuget.UnlistAll
{
    public interface IWorkerUi
    {
        BackgroundWorker Worker { get; }

        void NotifyTaskBegin();

        void NotifyTaskFinished(object result);

        void NotifyProgress(int percentage, object userData);
    }
}
