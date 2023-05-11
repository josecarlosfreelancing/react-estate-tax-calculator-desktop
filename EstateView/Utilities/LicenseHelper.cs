using System;
using System.Globalization;
using System.IO;

namespace EstateView.Utilities
{
    public static class LicenseHelper
    {
        public static bool IsTrialVersion()
        {
#if TRIAL
            return true;
#else
            return false;
#endif
        }

        public static int GetDaysLeftInTrial()
        {
            DateTime expirationDate = GetTrialExpirationDate();
            return (int)(expirationDate - DateTime.Now.Date).TotalDays;
        }

        private static DateTime GetTrialExpirationDate()
        {
            DateTime trialExpirationDate;
            DirectoryInfo directory = Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EstateView"));
            string filePath = Path.Combine(directory.FullName, "install.dat");
            if (File.Exists(filePath))
            {
                string contents = File.ReadAllText(filePath);
                if (DateTime.TryParse(contents, out trialExpirationDate))
                {
                    return trialExpirationDate;
                }
            }

            trialExpirationDate = DateTime.Now.Date.AddDays(7);
            LicenseHelper.SetTrialExpirationDate(trialExpirationDate);
            return trialExpirationDate;
        }

        private static void SetTrialExpirationDate(DateTime value)
        {
            DirectoryInfo directory = Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EstateView"));
            string filePath = Path.Combine(directory.FullName, "install.dat");
            File.WriteAllText(filePath, value.ToString(CultureInfo.InvariantCulture));
        }
    }
}
