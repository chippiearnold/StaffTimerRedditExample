﻿using Microsoft.Practices.Unity;
using Prism.Unity;
using StaffTimerRedditExample.Views;
using System.Windows;

namespace StaffTimerRedditExample
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }
    }
}
