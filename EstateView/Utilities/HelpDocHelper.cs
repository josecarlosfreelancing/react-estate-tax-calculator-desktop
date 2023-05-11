using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;

namespace EstateView.Utilities
{
    internal class HelpDocHelper
    {
        public static void OpenHelpDoc()
        {
            try
            {
                var fileName = Path.Combine(Path.GetTempPath(), "EstateView_Help.docx");
                File.WriteAllBytes(fileName, Properties.Resources.EstateView_Help);

                var startInfo = new ProcessStartInfo(fileName);
                startInfo.UseShellExecute = true;
                Process.Start(startInfo);
            }
            catch(Exception e)
            {
                MessageBox.Show("Failed to open help document.\nError: " + e.Message);
            }
        }
    }
}
