using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;
using Prism.Commands;
using Prism.Mvvm;
using StaffTimerRedditExample.Models;

namespace StaffTimerRedditExample.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        #region Private Members

        private string _title = "Staff Timer";
        private ObservableCollection<StaffMember> _activeStaffMembers = new ObservableCollection<StaffMember>();
        private StaffMember _selectedStaffMember;

        #endregion

        #region Constructor

        public MainWindowViewModel()
        {
            //Populate the combo box with some staff
            StaffMembers = GetStaffList();

            //Setup our delegate commands
            AddStaffMemberCommand = new DelegateCommand<StaffMember>(AddStaffMember, CanAddStaffMember);
            
            //Set up our main timer
            var timer = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(1)};
            timer.Tick += TimerTick;
            timer.Start();
        }               

        #endregion

        #region Private Methods
        private void TimerTick(object sender, EventArgs e)
        {
            foreach (var staffMember in ActiveStaffMembers.Where(staffMember => staffMember.TimerActive))
            {
                staffMember.UpdateTimer();
            }
        }

        private void AddStaffMember(StaffMember staffMember)
        {                        
            ActiveStaffMembers.Add(staffMember);
            staffMember.StartTimer();
            AddStaffMemberCommand.RaiseCanExecuteChanged();
        }

        private bool CanAddStaffMember(StaffMember staffMember)
        {
            return staffMember != null && !ActiveStaffMembers.Contains(staffMember);
        }

        public IEnumerable<StaffMember> GetStaffList()
        {
            return new List<StaffMember>
            {
                new StaffMember
                {
                    Name = "Geddy Lee"
                },
                new StaffMember
                {
                    Name = "Alex Lifeson"
                },
                new StaffMember
                {
                    Name = "Neil Peart"
                }
            };
        }

        #endregion

        #region Properties

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public IEnumerable<StaffMember> StaffMembers { get; set; }

        public StaffMember SelectedStaffMember
        {
            get { return _selectedStaffMember; }
            set
            {
                SetProperty(ref _selectedStaffMember, value);
                AddStaffMemberCommand.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<StaffMember> ActiveStaffMembers
        {
            get { return _activeStaffMembers; }
            set { SetProperty(ref _activeStaffMembers, value); }
        }

        public DelegateCommand<StaffMember> AddStaffMemberCommand { get; set; }

        #endregion
    }
}
