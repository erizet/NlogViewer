using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;

namespace NLog
{
   public class NLogEvent : EventArgs
   {
      public LogEventInfo EventInfo;

      public NLogEvent(LogEventInfo LogEventInfo)
      {
         // TODO: Complete member initialization
         this.EventInfo = LogEventInfo;
      }


      public static implicit operator LogEventInfo(NLogEvent e )
      {
         return e.EventInfo;
      }
      public static implicit operator NLogEvent(LogEventInfo e)
      {
         return new NLogEvent(e);
      }
   }
}
