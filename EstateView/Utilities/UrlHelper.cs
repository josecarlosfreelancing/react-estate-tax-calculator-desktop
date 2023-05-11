using System;
using System.Collections.Generic;
using System.Text;

namespace EstateView.Utilities
{
    internal class UrlHelper
    {
        const string YOU_TUBE_PlAYLIST = "https://youtube.com/playlist?list=PL4T6LFdbkPFkVsDEF0wCBjAVCvq1N-f7t";
        const string IRS_AFR = "https://www.irs.gov/applicable-federal-rates";

        static UrlHelper()
        {

        }

        public static void GoToYouTubePlayList()
        {
            OpenUrl(YOU_TUBE_PlAYLIST);
        }

        public static void LookupAFR()
        {
            OpenUrl(IRS_AFR);
        }

        private static void OpenUrl(string url)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
    }
}
