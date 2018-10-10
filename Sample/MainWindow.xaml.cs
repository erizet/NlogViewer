using NLog;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Media;

namespace Sample
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Task _logTask;
        CancellationTokenSource _cancelLogTask;


        public MainWindow()
        {
            InitializeComponent();
            cbAutoScroll.IsChecked = true;

            logCtrl.ItemAdded += OnLogMessageItemAdded;

        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            Logger log = LogManager.GetLogger("button");

            LogLevel level = LogLevel.Trace;
            if (sender.Equals(btnDebug)) level = LogLevel.Debug;
            if (sender.Equals(btnWarning)) level = LogLevel.Warn;
            if (sender.Equals(btnError)) level = LogLevel.Error;

            log.Log(level, tbLogText.Text);
        }

       private void OnLogMessageItemAdded(object o, EventArgs Args )
       {
          // Do what you want :)
          LogEventInfo logInfo = (NLogEvent)Args;
          if( logInfo.Level >= NLog.LogLevel.Error)
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
            Logger log = NLog.LogManager.GetLogger("task");

            log.Debug("Backgroundtask started.");

            while (!ct.WaitHandle.WaitOne(2000))
            {
                log.Trace(string.Format("Messageno {0} from backgroudtask.", counter++));
            }

            log.Debug("Backgroundtask stopped.");
        }


    }
}
