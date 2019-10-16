using Serilog;
using Serilog.Configuration;
using System;

namespace SerilogViewer
{
    public static class SerilogViewerSinkExtensions
    {
        public static LoggerConfiguration SerilogViewerSink(
          this LoggerSinkConfiguration loggerConfiguration,
          SerilogViewer serilogViewer,
          IFormatProvider formatProvider = null)
        {
            return loggerConfiguration.Sink(new SerilogViewerSink(formatProvider, serilogViewer));
        }
    }
}
