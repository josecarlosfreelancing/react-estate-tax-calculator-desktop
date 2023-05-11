using System;
using System.IO;
using System.Windows;
using EstateView.Utilities;

using EstateView.Properties;
using System.Configuration;

[assembly: System.Windows.ThemeInfo(
    System.Windows.ResourceDictionaryLocation.None,
    System.Windows.ResourceDictionaryLocation.SourceAssembly
)]
namespace EstateView
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            File.WriteAllText("EstateView-CrashLog.txt", e.Exception.ToString());
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (LicenseHelper.IsTrialVersion())
            {
                int daysleftInTrial = LicenseHelper.GetDaysLeftInTrial();
                if (daysleftInTrial <= 0)
                {
                    string message = "This trial of EstateView has expired. Please visit www.gassmanlaw.com to purchase a license for the full version.";
                    string caption = "Trial Expired";
                    MessageBox.Show(message, caption);
                    this.Shutdown(exitCode: 1);
                }
                else
                {
                    string message = string.Format("We hope you are enjoying using this trial version of EstateView. You have {0} days left in the trial period. Please visit www.gassmanlaw.com to purchase a license for the full version.", daysleftInTrial);
                    string caption = string.Format("Trial Notification - {0} days left", daysleftInTrial);
                    MessageBox.Show(message, caption);
                }
            }

            this.StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
        }
        protected override void OnExit(ExitEventArgs e)
        {
                Settings.Default.Save();
        }
    }
}
