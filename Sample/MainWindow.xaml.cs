using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            Logger log = NLog.LogManager.GetLogger("button");

            LogLevel level = LogLevel.Trace;
            if (sender.Equals(btnDebug)) level = LogLevel.Debug;
            if (sender.Equals(btnWarning)) level = LogLevel.Warn;
            if (sender.Equals(btnError)) level = LogLevel.Error;

            log.Log(level, tbLogText.Text);
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
                log.Trace(String.Format("Messageno {0} from backgroudtask.", counter++));
            }

            log.Debug("Backgroundtask stopped.");
        }


    }
}
