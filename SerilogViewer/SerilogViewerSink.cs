using Serilog.Core;
using Serilog.Events;
using System;

namespace SerilogViewer
{
    public class SerilogViewerSink : ILogEventSink
    {
        public event Action<LogEvent, IFormatProvider> LogReceived;
        private readonly IFormatProvider _formatProvider;

        public SerilogViewerSink(IFormatProvider formatProvider, SerilogViewer serilogViewer)
        {
            _formatProvider = formatProvider;
            this.LogReceived += serilogViewer.LogReceived;
        }

        public void Emit(LogEvent logEvent)
        {
            LogReceived?.Invoke(logEvent, _formatProvider);
        }
    }
}
