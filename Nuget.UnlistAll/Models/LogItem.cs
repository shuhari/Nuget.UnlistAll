using System;
using System.Windows.Media;

namespace Nuget.UnlistAll.Models
{
    public class LogItem
    {
        public LogItem(DateTime time, bool success, string content)
        {
            this.Time = time;
            this.Success = success;
            this.Content = content;
        }

        private static Brush _successBrush = new SolidColorBrush(Colors.DarkGreen);
        private static Brush _errorBrush = new SolidColorBrush(Colors.Red);

        public DateTime Time { get; private set; }

        public bool Success { get; private set; }

        public string Content { get; private set; }

        public override string ToString()
        {
            return $"[{Time:yyyy-MM-dd HH:mm:ss}] {Content}";
        }

        public Brush Foreground => Success ? _successBrush : _errorBrush;
    }
}
