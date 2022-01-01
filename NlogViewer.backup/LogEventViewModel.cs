using System;
using System.Globalization;
using System.Windows.Media;
using NLog;

namespace NlogViewer
{
    public class LogEventViewModel
    {
        private LogEventInfo logEventInfo;

        public LogEventViewModel(LogEventInfo logEventInfo)
        {
            // TODO: Complete member initialization
            this.logEventInfo = logEventInfo;

            ToolTip = logEventInfo.FormattedMessage;
            Level = logEventInfo.Level.ToString();
            FormattedMessage = logEventInfo.FormattedMessage;
            Exception = logEventInfo.Exception;
            LoggerName = logEventInfo.LoggerName;
            Time = logEventInfo.TimeStamp.ToString(CultureInfo.InvariantCulture);

            SetupColors(logEventInfo);
        }


        public string Time { get; private set; }
        public string LoggerName { get; private set; }
        public string Level { get; private set; }
        public string FormattedMessage { get; private set; }
        public Exception Exception { get; private set; }
        public string ToolTip { get; private set; }
        public SolidColorBrush Background { get; private set; }
        public SolidColorBrush Foreground { get; private set; }
        public SolidColorBrush BackgroundMouseOver { get; private set; }
        public SolidColorBrush ForegroundMouseOver { get; private set; }

        private void SetupColors(LogEventInfo logEventInfo)
        {
            if (logEventInfo.Level == LogLevel.Warn)
            {
                Background = Brushes.Yellow;
                BackgroundMouseOver = Brushes.GreenYellow;
            }
            else if (logEventInfo.Level == LogLevel.Error)
            {
                Background = Brushes.Tomato;
                BackgroundMouseOver = Brushes.IndianRed;
            }
            else
            {
                Background = Brushes.White;
                BackgroundMouseOver = Brushes.LightGray;
            }
            Foreground = Brushes.Black;
            ForegroundMouseOver = Brushes.Black;
        }
    }
}