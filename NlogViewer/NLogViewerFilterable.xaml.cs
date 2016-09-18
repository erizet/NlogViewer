using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
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
    /// Interaction logic for NLogViewerFilterable.xaml
    /// </summary>
    public partial class NLogViewerFilterable : UserControl, INotifyPropertyChanged
    {
        public ObservableCollection<LogEventViewModel> LogEntries { get; private set; }

        public bool IsTargetConfigured { get; private set; }

        [Description("Width of time column in pixels"), Category("Data")]
        [TypeConverterAttribute(typeof(LengthConverter))]
        public double TimeWidth { get; set; }

        [Description("Width of Logger column in pixels, or auto if not specified"), Category("Data")]
        [TypeConverterAttribute(typeof(LengthConverter))]
        public double LoggerNameWidth { set; get; }

        [Description("Width of Level column in pixels"), Category("Data")]
        [TypeConverterAttribute(typeof(LengthConverter))]
        public double LevelWidth { get; set; }
        
        [Description("Width of Message column in pixels"), Category("Data")]
        [TypeConverterAttribute(typeof(LengthConverter))]
        public double MessageWidth { get; set; }
        
        [Description("Width of Exception column in pixels"), Category("Data")]
        [TypeConverterAttribute(typeof(LengthConverter))]
        public double ExceptionWidth { get; set; }

        [Description("Maximum number of log entries to show"), Category("Data")]
        public int MaximumLogEntries { get; set; }

        #region "Flags to turn show/hide levels"
        bool showTrace = true, showInfo = true, showDebug = true, showWarn = true, showError = true, showFatal = true;
        
        public bool ShowTrace 
        {
            get { return showTrace; }
            set { showTrace = value; OnPropertyChanged("ShowTrace"); }
        }

        public bool ShowInfo
        {
            get { return showInfo; }
            set { showInfo = value; OnPropertyChanged("ShowInfo"); }
        }

        public bool ShowDebug
        {
            get { return showDebug; }
            set { showDebug = value; OnPropertyChanged("ShowDebug"); }
        }

        public bool ShowWarn
        {
            get { return showWarn; }
            set { showWarn = value; OnPropertyChanged("ShowWarn"); }
        }

        public bool ShowError
        {
            get { return showError; }
            set { showError = value; OnPropertyChanged("ShowError"); }
        }

        public bool ShowFatal
        {
            get { return showFatal; }
            set { showFatal = value; OnPropertyChanged("ShowFatal"); }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public NLogViewerFilterable()
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
                bool addLogEvent = false;
                switch(vm.Level.ToLower())
                {
                    case "debug": addLogEvent = ShowDebug; break;
                    case "info": addLogEvent = ShowInfo; break;
                    case "trace": addLogEvent = ShowTrace; break;
                    case "warn": addLogEvent = ShowWarn; break;
                    case "error": addLogEvent = ShowError; break;
                    case "fatal": addLogEvent = ShowFatal; break;
                }
                if (!addLogEvent) return;
                if (LogEntries.Count >= MaximumLogEntries) LogEntries.RemoveAt(0);

                LogEntries.Add(vm);
                grid.ScrollIntoView(vm);
            }));
        }
    }
}
