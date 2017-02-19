using System;
using Prism.Commands;
using Prism.Mvvm;

namespace StaffTimerRedditExample.Models
{
    public class StaffMember : BindableBase
    {
        #region Private Members

        private bool _timerActive;
        private TimeSpan _timeElapsed;
        private DateTime _dateStarted;

        #endregion

        #region Constructor

        public StaffMember()
        {
            StartTimerCommand = new DelegateCommand(StartTimer, CanStartTimer);
            StopTimerCommand = new DelegateCommand(StopTimer, CanStopTimer);
        }

        #endregion

        #region Public Methods

        public void StartTimer()
        {
            DateStarted = DateTime.Now;
            TimerActive = true;
            StartTimerCommand.RaiseCanExecuteChanged();
            StopTimerCommand.RaiseCanExecuteChanged();
        }

        public void UpdateTimer()
        {            
            TimeElapsed = DateTime.Now - DateStarted;
        }

        #endregion

        #region Private Methods

        private bool CanStartTimer()
        {
            return TimerActive == false;
        }

        private void StopTimer()
        {
            TimerActive = false;
            StartTimerCommand.RaiseCanExecuteChanged();
            StopTimerCommand.RaiseCanExecuteChanged();
        }

        private bool CanStopTimer()
        {
            return TimerActive;
        }

        #endregion

        #region Properties              

        public string Name { get; set; }

        public DateTime DateStarted
        {
            get { return _dateStarted; }
            set { SetProperty(ref _dateStarted, value); }
        }

        public bool TimerActive
        {
            get { return _timerActive; }
            set { SetProperty(ref _timerActive, value); }
        }

        public TimeSpan TimeElapsed
        {
            get { return _timeElapsed; }
            set { SetProperty(ref _timeElapsed, value); }
        }

        public DelegateCommand StartTimerCommand { get; set; }
        public DelegateCommand StopTimerCommand { get; set; }

        #endregion
    }
}
