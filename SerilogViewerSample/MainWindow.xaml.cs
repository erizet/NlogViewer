using Serilog;
using Serilog.Events;
using SerilogViewer;
using System;
using System.Media;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SerilogViewerSample
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Task _logTask;
        CancellationTokenSource _cancelLogTask;
        ILogger log;

        public MainWindow()
        {
            InitializeComponent();
            cbAutoScroll.IsChecked = true;

            log = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.SerilogViewerSink(logCtrl)
                .CreateLogger();

            logCtrl.ItemAdded += OnLogMessageItemAdded;

        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            LogEventLevel level = LogEventLevel.Verbose;
            if (sender.Equals(btnDebug)) level = LogEventLevel.Debug;
            if (sender.Equals(btnWarning)) level = LogEventLevel.Warning;
            if (sender.Equals(btnError)) level = LogEventLevel.Error;

            if ((bool)cbWithContext.IsChecked)
            {
                log.ForContext<MainWindow>().Write(level, tbLogText.Text);
            }
            else 
            {
                log.Write(level, tbLogText.Text);
            }
        }

        private void SendWithContext_Click(object sender, RoutedEventArgs e)
        {
            LogEventLevel level = LogEventLevel.Verbose;
            if (sender.Equals(btnDebug)) level = LogEventLevel.Debug;
            if (sender.Equals(btnWarning)) level = LogEventLevel.Warning;
            if (sender.Equals(btnError)) level = LogEventLevel.Error;

            log.ForContext<MainWindow>().Write(level, tbLogText.Text);
        }

        private void OnLogMessageItemAdded(object o, EventArgs Args )
       {
          // Do what you want :)
          LogEvent logEvent = (SerilogEvent)Args;
          if( logEvent.Level >= LogEventLevel.Error)
            SystemSounds.Beep.Play();
       }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
           logCtrl.Clear();
        }
        private void TopScroll_Click(object sender, RoutedEventArgs e)
        {
           logCtrl.ScrollToFirst();
        }
        private void BottomScroll_Click(object sender, RoutedEventArgs e)
        {
            logCtrl.ScrollToLast();
        }
        private void AutoScroll_Checked(object sender, RoutedEventArgs e)
        {
            logCtrl.AutoScrollToLast = true;
        }
        private void AutoScroll_Unchecked(object sender, RoutedEventArgs e)
        {
            logCtrl.AutoScrollToLast = false;
        }


        private void BackgroundSending_Checked(object sender, RoutedEventArgs e)
        {
            _cancelLogTask = new CancellationTokenSource();
            var token = _cancelLogTask.Token;
            _logTask = new Task(SendLogs, token, token);
            _logTask.Start();
        }

        private void BackgroundSending_Unchecked(object sender, RoutedEventArgs e)
        {
            if (_cancelLogTask != null)
                _cancelLogTask.Cancel();
        }

        private void SendLogs(object obj)
        {
            CancellationToken ct = (CancellationToken)obj;

            int counter = 0;

            var backgroundLogger = log.ForContext("Type", "Backgroundtask");

            backgroundLogger.Debug("Backgroundtask started.");

            while (!ct.WaitHandle.WaitOne(2000))
            {
                backgroundLogger.ForContext("Type", "Backgroundtask").Verbose(string.Format("Message number: {0} from backgroudtask.", counter++));
            }

            backgroundLogger.Debug("Backgroundtask stopped.");
        }


    }
}
