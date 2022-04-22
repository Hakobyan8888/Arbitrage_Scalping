using ArbitrageDesktopApp.ArbitrageWrappers;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace ArbitrageDesktopApp.ViewModels.Explorers
{
    public class HomeExplorerViewModel : ExplorerViewModelBase
    {
        private bool _isFirstTimeBalance = true;
        private bool _isScalpingStopeed;

        public DelegateCommand StartArbitrageCommand { get; set; }
        public DelegateCommand StopArbitrageCommand { get; set; }

        private DispatcherTimer _dispatcherTimer;
        private Stopwatch _stopwatch;

        private decimal _startingBalance;
        private decimal _currentBalance;
        private decimal _sessionEarnings;
        private string _timer;

        public string Timer
        {
            get => _timer;
            set => SetProperty(ref _timer, value);
        }

        public decimal StartingBalance
        {
            get => _startingBalance;
            set => SetProperty(ref _startingBalance, value);
        }

        public decimal CurrentBalance
        {
            get => _currentBalance;
            set => SetProperty(ref _currentBalance, value);
        }

        public decimal SessionEarnings
        {
            get => _sessionEarnings;
            set => SetProperty(ref _sessionEarnings, value);
        }

        public bool IsScalpingStopeed
        {
            get => _isScalpingStopeed;
            set => SetProperty(ref _isScalpingStopeed, value);
        }

        public HomeExplorerViewModel()
        {
            StartArbitrageCommand = new DelegateCommand(StartArbitrage);
            StopArbitrageCommand = new DelegateCommand(StopArbitrage);
            IsScalpingStopeed = ArbitrageWrapper.FTXViewModelBaseInstance.IsStopped;


            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            _dispatcherTimer.Tick += timer_Tick;
            _stopwatch = new Stopwatch();
        }

        private void StartArbitrage()
        {
            ArbitrageWrapper.FTXViewModelBaseInstance.Start();
            IsScalpingStopeed = false;
            _dispatcherTimer.Start();
            _stopwatch.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Timer = string.Format($"{_stopwatch.Elapsed.Days}:{_stopwatch.Elapsed.Hours}:{_stopwatch.Elapsed.Minutes}:{_stopwatch.Elapsed.Seconds}");
        }

        private void StopArbitrage()
        {
            ArbitrageWrapper.FTXViewModelBaseInstance.Stop();
            IsScalpingStopeed = true;
            _dispatcherTimer.Stop();
            _stopwatch.Stop();
        }

        public override void Update()
        {
            if (_isFirstTimeBalance && ArbitrageWrapper.FTXViewModelBaseInstance.Wallet != null)
            {
                StartingBalance = ArbitrageWrapper.FTXViewModelBaseInstance.Wallet.Balances.Sum(x => x.UsdValue);
                _isFirstTimeBalance = false;
            }

            IsScalpingStopeed = ArbitrageWrapper.FTXViewModelBaseInstance.IsStopped;
            if (ArbitrageWrapper.FTXViewModelBaseInstance.Wallet != null)
            {
                CurrentBalance = ArbitrageWrapper.FTXViewModelBaseInstance.Wallet.Balances.Sum(x => x.UsdValue);
            }

            SessionEarnings = CurrentBalance - StartingBalance;
        }
    }
}
