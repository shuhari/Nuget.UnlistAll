using System;

namespace Nuget.UnlistAll.Models
{
    /// <summary>
    /// Log item
    /// </summary>
    public class LogItem
    {
        public LogItem(DateTime time, bool success, string content)
        {
            this.Time = time;
            this.Success = success;
            this.Content = content;
        }


        public DateTime Time { get; private set; }

        public bool Success { get; private set; }

        public string Content { get; private set; }

        public override string ToString()
        {
            return $"[{Time:yyyy-MM-dd HH:mm:ss}] {Content}";
        }
    }
}
