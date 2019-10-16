using Serilog.Events;
using System;
using System.Globalization;
using System.Windows.Media;

namespace SerilogViewer
{
    public class LogEventViewModel
    {
        private LogEvent logEvent;
        private IFormatProvider formatProvider;

        public string Time { get; private set; }
        public string Context { get; private set; }
        public string Level { get; private set; }
        public string FormattedMessage { get; private set; }
        public Exception Exception { get; private set; }
        public string ToolTip { get; private set; }
        public SolidColorBrush Background { get; private set; }
        public SolidColorBrush Foreground { get; private set; }
        public SolidColorBrush BackgroundMouseOver { get; private set; }
        public SolidColorBrush ForegroundMouseOver { get; private set; }

        public LogEventViewModel(LogEvent logEvent, IFormatProvider formatProvider)
        {
            // TODO: Complete member initialization
            this.logEvent = logEvent;
            this.formatProvider = formatProvider;

            ToolTip = logEvent.RenderMessage();
            Level = logEvent.Level.ToString();
            FormattedMessage = logEvent.RenderMessage(formatProvider);
            Exception = logEvent.Exception;
            Context = logEvent.Properties["SourceContext"].ToString();
            Time = logEvent.Timestamp.ToString("G", CultureInfo.CurrentCulture);

            SetupColors(logEvent);
        }

        private void SetupColors(LogEvent logEvent)
        {
            if (logEvent.Level == LogEventLevel.Warning)
            {
                Background = Brushes.Yellow;
                BackgroundMouseOver = Brushes.GreenYellow;
            }
            else if (logEvent.Level == LogEventLevel.Error)
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