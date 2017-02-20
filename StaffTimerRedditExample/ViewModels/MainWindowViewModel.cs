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

            //Setup our delegate command for adding a staff member from the combo to our listbox of 'active' staff            
            //With prism delegate commands, you can have a second function which returns a bool to say whether or not 
            //the command can be triggered (CanAddStaffMember in this instance).
            //If this returns false, then the button will be disabled.
            AddStaffMemberCommand = new DelegateCommand<StaffMember>(AddStaffMember, CanAddStaffMember);
            
            //Set up our main timer - this is our "game loop" if you like - it will run continuously in the background
            //and call our TimerTick method every millisecond - you could reduce this if you want but I set it so you
            //can see how accurate we can have it but with a large list of active staff this might be too often.
            var timer = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(1)};
            timer.Tick += TimerTick;
            timer.Start();
        }               

        #endregion

        #region Private Methods

        private void TimerTick(object sender, EventArgs e)
        {
            //Loop through the staffMembers in our ActiveStaffMembers collection and call its UpdateTimer method if it's timer is active
            foreach (var staffMember in ActiveStaffMembers.Where(staffMember => staffMember.TimerActive))
            {
                staffMember.UpdateTimer();
            }
        }

        private void AddStaffMember(StaffMember staffMember)
        {                        
            ActiveStaffMembers.Add(staffMember);
            staffMember.StartTimer();

            //The reason for calling RaiseCanExecuteChanged is simply to update the UI so once a staff member is added, the Add button 
            //becomes disabled through the "CanAddStaffMember" method.
            AddStaffMemberCommand.RaiseCanExecuteChanged();
        }

        private bool CanAddStaffMember(StaffMember staffMember)
        {
            //The result of this method will enable or disable the Add button appropriately
            return staffMember != null && !ActiveStaffMembers.Contains(staffMember);
        }

        public IEnumerable<StaffMember> GetStaffList()
        {
            //Here you could connect to a database, or web service, or anything to get a list of staff to pick from
            //For now, we'll just create some test staff members from my favourite band!

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
                //This will be called when the combobox selection is changed.
                SetProperty(ref _selectedStaffMember, value);

                //Again, we call RaiseCanExecuteChanged to enable or disable the Add button appropriately
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
