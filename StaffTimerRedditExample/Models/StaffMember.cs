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
            //Set up our delegate commands for the Start and Stop timer buttons in the UI
            StartTimerCommand = new DelegateCommand(StartTimer, CanStartTimer);
            StopTimerCommand = new DelegateCommand(StopTimer, CanStopTimer);
        }

        #endregion

        #region Public Methods

        public void StartTimer()
        {
            DateStarted = DateTime.Now;
            TimerActive = true;

            //Call RaiseCanExecuteChanged to enable or disable the Start and Stop buttons appropriately
            StartTimerCommand.RaiseCanExecuteChanged();
            StopTimerCommand.RaiseCanExecuteChanged();
        }

        public void UpdateTimer()
        {            
            //This is the method called every millisecond from the ViewModel, but only if the current instance of staff member's TimerActive=true
            TimeElapsed = DateTime.Now - DateStarted;
        }

        #endregion

        #region Private Methods

        private bool CanStartTimer()
        {
            //Used to enable or disable the Start button - only enable if the timer is NOT currently running
            return TimerActive == false;
        }

        private void StopTimer()
        {
            TimerActive = false;

            //Get our start and stop buttons to update their enabled status appropriately
            StartTimerCommand.RaiseCanExecuteChanged();
            StopTimerCommand.RaiseCanExecuteChanged();
        }

        private bool CanStopTimer()
        {
            //Used to enable or disable the Stop button - only enable if the timer is currently running
            return TimerActive;
        }

        #endregion

        #region Properties              

        public string Name { get; set; }


        //Because we want our UI to update when the following properties are updated, we are using SetProperty method of PRISM
        //This will handle INotifyPropertyChanged for us, and allow the bindings in the UI to update accordingly.
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
