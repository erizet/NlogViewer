using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NlogViewer
{
    /// <summary>
    /// Interaction logic for NlogViewer.xaml
    /// </summary>
    public partial class NlogViewer : UserControl
    {
        public ListView LogView { get { return logView; } }
        public event EventHandler ItemAdded = delegate { };
        public ObservableCollection<LogEventViewModel> LogEntries { get; private set; }
        public bool IsTargetConfigured { get; private set; }

        private double _TimeWidth = 120;
        [Description("Width of time column in pixels"), Category("Data")]
        [TypeConverterAttribute(typeof(LengthConverter))]
        public double TimeWidth
        {
           get { return _TimeWidth; }
           set { _TimeWidth = value; }
        }

        private double _LoggerNameWidth = 50;
        [Description("Width of Logger column in pixels, or auto if not specified"), Category("Data")]
        [TypeConverterAttribute(typeof(LengthConverter))]
        public double LoggerNameWidth
        {
           get { return _LoggerNameWidth; }
           set { _LoggerNameWidth = value; }
        }

        private double _LevelWidth = 50;
        [Description("Width of Level column in pixels"), Category("Data")]
        [TypeConverterAttribute(typeof(LengthConverter))]
        public double LevelWidth
        {
           get { return _LevelWidth; }
           set { _LevelWidth = value; }
        }
        
        private double _MessageWidth = 200;
        [Description("Width of Message column in pixels"), Category("Data")]
        [TypeConverterAttribute(typeof(LengthConverter))]
        public double MessageWidth
        {
           get { return _MessageWidth; }
           set { _MessageWidth = value; }
        }
        
        private double _ExceptionWidth = 75;
        [Description("Width of Exception column in pixels"), Category("Data")]
        [TypeConverterAttribute(typeof(LengthConverter))]
        public double ExceptionWidth
        {
           get { return _ExceptionWidth; }
           set { _ExceptionWidth = value; }
        }

        private int _MaxRowCount = 50;
        [Description("The maximum number of row count. The oldest log gets deleted. Set to 0 for unlimited count."), Category("Data")]
        [TypeConverterAttribute(typeof(LengthConverter))]
        public int MaxRowCount
        {
           get { return _MaxRowCount; }
           set { _MaxRowCount = value; }
        }

        public NlogViewer()
        {
            IsTargetConfigured = false;
            LogEntries = new ObservableCollection<LogEventViewModel>();

            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                foreach (NlogViewerTarget target in NLog.LogManager.Configuration.AllTargets.Where(t => t is NlogViewerTarget).Cast<NlogViewerTarget>())
                {
                    IsTargetConfigured = true;
                    target.LogReceived += LogReceived;
                }
            }
        }

        protected void LogReceived(NLog.Common.AsyncLogEventInfo log)
        {
            LogEventViewModel vm = new LogEventViewModel(log.LogEvent);

            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (LogEntries.Count >= (MaxRowCount>0? MaxRowCount : Int32.MaxValue))
                    LogEntries.RemoveAt(0);
                
                LogEntries.Add(vm);
                ItemAdded(this, (NLog.NLogEvent)log.LogEvent);
            }));
        }
    }

}
