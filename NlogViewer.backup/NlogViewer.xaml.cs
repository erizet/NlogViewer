using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using NLog;
using NLog.Common;

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
        [TypeConverter(typeof(LengthConverter))]
        public double TimeWidth
        {
            get { return _TimeWidth; }
            set { _TimeWidth = value; }
        }

        private double _LoggerNameWidth = 50;
        [Description("Width of Logger column in pixels, or auto if not specified"), Category("Data")]
        [TypeConverter(typeof(LengthConverter))]
        public double LoggerNameWidth
        {
            get { return _LoggerNameWidth; }
            set { _LoggerNameWidth = value; }
        }

        private double _LevelWidth = 50;
        [Description("Width of Level column in pixels"), Category("Data")]
        [TypeConverter(typeof(LengthConverter))]
        public double LevelWidth
        {
            get { return _LevelWidth; }
            set { _LevelWidth = value; }
        }

        private double _MessageWidth = 200;
        [Description("Width of Message column in pixels"), Category("Data")]
        [TypeConverter(typeof(LengthConverter))]
        public double MessageWidth
        {
            get { return _MessageWidth; }
            set { _MessageWidth = value; }
        }

        private double _ExceptionWidth = 75;
        [Description("Width of Exception column in pixels"), Category("Data")]
        [TypeConverter(typeof(LengthConverter))]
        public double ExceptionWidth
        {
            get { return _ExceptionWidth; }
            set { _ExceptionWidth = value; }
        }

        private int _MaxRowCount = 50;
        [Description("The maximum number of row count. The oldest log gets deleted. Set to 0 for unlimited count."), Category("Data")]
        [TypeConverter(typeof(Int32Converter))]
        public int MaxRowCount
        {
            get { return _MaxRowCount; }
            set { _MaxRowCount = value; }
        }

        private bool _autoScrollToLast = true;
        [Description("Automatically scrolls to the last log item in the viewer. Default is true."), Category("Data")]
        [TypeConverter(typeof(BooleanConverter))]
        public bool AutoScrollToLast
        {
            get { return _autoScrollToLast; }
            set { _autoScrollToLast = value; }
        }

        public NlogViewer()
        {
            IsTargetConfigured = false;
            LogEntries = new ObservableCollection<LogEventViewModel>();

            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                foreach (NlogViewerTarget target in LogManager.Configuration.AllTargets.Where(t => t is NlogViewerTarget).Cast<NlogViewerTarget>())
                {
                    IsTargetConfigured = true;
                    target.LogReceived += LogReceived;
                }
            }
        }

        protected void LogReceived(AsyncLogEventInfo log)
        {
            LogEventViewModel vm = new LogEventViewModel(log.LogEvent);

            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (MaxRowCount > 0 && LogEntries.Count >= MaxRowCount)
                    LogEntries.RemoveAt(0);
                LogEntries.Add(vm);
                if (AutoScrollToLast) ScrollToLast();
                ItemAdded(this, (NLogEvent)log.LogEvent);
            }));
        }
        public void Clear()
        {
            LogEntries.Clear();
        }

        public void ScrollToFirst()
        {
            if (LogView.Items.Count <= 0) return;
            LogView.SelectedIndex = 0;
            ScrollToItem(LogView.SelectedItem);
        }
        public void ScrollToLast()
        {
            if (LogView.Items.Count <= 0) return;
            LogView.SelectedIndex = LogView.Items.Count - 1;
            ScrollToItem(LogView.SelectedItem);
        }

        private void ScrollToItem(object item)
        {
            LogView.ScrollIntoView(item);
        }

    }

}
