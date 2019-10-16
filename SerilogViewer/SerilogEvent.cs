using Serilog.Events;
using System;

namespace Serilog
{
    public class SerilogEvent : EventArgs
   {
      public LogEvent EventInfo;

      public SerilogEvent(LogEvent logEvent)
      {
         // TODO: Complete member initialization
         this.EventInfo = logEvent;
      }

      public static implicit operator LogEvent(SerilogEvent e )
      {
         return e.EventInfo;
      }

      public static implicit operator SerilogEvent(LogEvent e)
      {
         return new SerilogEvent(e);
      }
   }
}
