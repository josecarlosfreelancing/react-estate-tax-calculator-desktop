using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace EstateView.Utilities
{
    public static class ScreenshotHelper
    {
        private static class Constants
        {
            public const int ScreenDotsPerInch = 96;
            public const int PrintDotsPerInch = 384;
            public const string ScreenshotFileExtension = "png";
            public const string ScreenshotFileFilter = "Image Files|*.png";
        }

        public static void SaveScreenshot(FrameworkElement view)
        {
            ScreenshotHelper.SaveScreenshot(view, "Screenshot-" + DateTime.Now.ToString("yyyyMMdd-HHmmss"));
        }
        
        public static void SaveScreenshot(FrameworkElement view, string filename)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = Constants.ScreenshotFileExtension;
            saveFileDialog.FileName = filename;
            saveFileDialog.Filter = Constants.ScreenshotFileFilter;

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    using (Stream output = saveFileDialog.OpenFile())
                    {
                        ScreenshotHelper.RenderToImage(view, output);
                    }

                    MessageBoxResult result = MessageBox.Show(
                        "Screenshot saved successfully. Would you like to open the screenshot?",
                        "Screenshot Saved Successfully",
                        MessageBoxButton.YesNo);

                    if (result == MessageBoxResult.Yes)
                    {
                        Process.Start(saveFileDialog.FileName);
                    }

                }
                catch (IOException e)
                {
                    MessageBox.Show("Failed to save screenshot.\nError: " + e.Message);
                }
            }
        }
        
        private static void RenderToImage(FrameworkElement view, Stream output)
        {
            Size viewSize = new Size(view.ActualWidth, view.ActualHeight); 

            RenderTargetBitmap target = new RenderTargetBitmap(
                (int)(viewSize.Width * (Constants.PrintDotsPerInch / Constants.ScreenDotsPerInch)),
                (int)(viewSize.Height * (Constants.PrintDotsPerInch / Constants.ScreenDotsPerInch)),
                Constants.PrintDotsPerInch,
                Constants.PrintDotsPerInch,
                PixelFormats.Pbgra32);

            target.Render(view);

            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(target));

            encoder.Save(output);
        }
    }
}
