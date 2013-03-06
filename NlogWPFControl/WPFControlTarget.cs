using NLog.Common;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NlogWPFControl
{
    [Target("WPFControl")]
    public class WPFControlTarget : Target
    {
        public event Action<AsyncLogEventInfo> LogReceived;

        protected override void Write(NLog.Common.AsyncLogEventInfo logEvent)
        {
            base.Write(logEvent);

            if (LogReceived != null)
                LogReceived(logEvent);
        }
    }
}
