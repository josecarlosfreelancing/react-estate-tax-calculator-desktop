using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using EstateView.Utilities;

namespace EstateView.Controls
{
    public class ZoomableContentControl : ContentControl
    {
        public static readonly DependencyProperty ZoomFactorProperty = DependencyProperty.Register("ZoomFactor", typeof(double), typeof(ZoomableContentControl), new PropertyMetadata(Constants.DefaultZoom, ZoomableContentControl.OnZoomFactorChanged));
        public static readonly DependencyProperty FormattedZoomFactorProperty = DependencyProperty.Register("FormattedZoomFactor", typeof(string), typeof(ZoomableContentControl), null);
        public static readonly DependencyProperty ContentWidthProperty = DependencyProperty.Register("ContentWidth", typeof(double), typeof(ZoomableContentControl), null);
        public static readonly DependencyProperty ContentHeightProperty = DependencyProperty.Register("ContentHeight", typeof(double), typeof(ZoomableContentControl), null);
        public static readonly DependencyProperty ScaledContentWidthProperty = DependencyProperty.Register("ScaledContentWidth", typeof(double), typeof(ZoomableContentControl), null);
        public static readonly DependencyProperty ViewportWidthProperty = DependencyProperty.Register("ViewportWidth", typeof(double), typeof(ZoomableContentControl), null);
        public static readonly DependencyProperty ViewportHeightProperty = DependencyProperty.Register("ViewportHeight", typeof(double), typeof(ZoomableContentControl), null);

        public static readonly DependencyProperty ZoomToWidthCommandProperty = DependencyProperty.Register("ZoomToWidthCommand", typeof(ICommand), typeof(ZoomableContentControl), null);
        public static readonly DependencyProperty ZoomToFullPageCommandProperty = DependencyProperty.Register("ZoomToFullPageCommand", typeof(ICommand), typeof(ZoomableContentControl), null);
        public static readonly DependencyProperty ResetZoomCommandProperty = DependencyProperty.Register("ResetZoomCommand", typeof(ICommand), typeof(ZoomableContentControl), null);
        public static readonly DependencyProperty DecrementZoomCommandProperty = DependencyProperty.Register("DecrementZoomCommand", typeof(ICommand), typeof(ZoomableContentControl), null);
        public static readonly DependencyProperty IncrementZoomCommandProperty = DependencyProperty.Register("IncrementZoomCommand", typeof(ICommand), typeof(ZoomableContentControl), null);

        public static readonly DependencyProperty SaveScreenshotCommandProperty = DependencyProperty.Register("SaveScreenshotCommand", typeof(ICommand), typeof(ZoomableContentControl), null);

        private static class Constants
        {
            public const double DefaultZoom = 1.0;
            public const double ZoomStep = 0.1;
            public const double ZoomMinimum = 0.1;
            public const double ZoomMaximum = 4.0;
        }

        private ScrollViewer scrollViewer;
        private Grid viewPort;
        private double lastVerticalOffset;
        private double lastHorizontalOffset;

        public ZoomableContentControl()
        {
            this.DefaultStyleKey = typeof(ZoomableContentControl);
            this.ResetZoomCommand = new RelayCommand(this.ResetZoom);
            this.ZoomToWidthCommand = new RelayCommand(this.ZoomToWidth);
            this.ZoomToFullPageCommand = new RelayCommand(this.ZoomToFullPage);
            this.IncrementZoomCommand = new RelayCommand(this.IncrementZoom);
            this.DecrementZoomCommand = new RelayCommand(this.DecrementZoom);
            this.SaveScreenshotCommand = new RelayCommand(this.SaveScreenshot);
        }

        public double ZoomFactor
        {
            get { return (double)this.GetValue(ZoomableContentControl.ZoomFactorProperty); }
            set { this.SetValue(ZoomableContentControl.ZoomFactorProperty, value); }
        }

        public string FormattedZoomFactor
        {
            get { return (string)this.GetValue(ZoomableContentControl.FormattedZoomFactorProperty); }
            set { this.SetValue(ZoomableContentControl.FormattedZoomFactorProperty, value); }
        }

        public double ContentWidth
        {
            get { return (double)this.GetValue(ZoomableContentControl.ContentWidthProperty); }
            set { this.SetValue(ZoomableContentControl.ContentWidthProperty, value); }
        }

        public double ContentHeight
        {
            get { return (double)this.GetValue(ZoomableContentControl.ContentHeightProperty); }
            set { this.SetValue(ZoomableContentControl.ContentHeightProperty, value); }
        }

        public double ViewportWidth
        {
            get { return (double)this.GetValue(ZoomableContentControl.ViewportWidthProperty); }
            set { this.SetValue(ZoomableContentControl.ViewportWidthProperty, value); }
        }

        public double ViewportHeight
        {
            get { return (double)this.GetValue(ZoomableContentControl.ViewportHeightProperty); }
            set { this.SetValue(ZoomableContentControl.ViewportHeightProperty, value); }
        }

        public double ScaledContentWidth
        {
            get { return (double)this.GetValue(ZoomableContentControl.ScaledContentWidthProperty); }
            set { this.SetValue(ZoomableContentControl.ScaledContentWidthProperty, value); }
        }

        public ICommand ResetZoomCommand
        {
            get { return (ICommand)this.GetValue(ZoomableContentControl.ResetZoomCommandProperty); }
            set { this.SetValue(ZoomableContentControl.ResetZoomCommandProperty, value); }
        }

        public ICommand ZoomToWidthCommand
        {
            get { return (ICommand)this.GetValue(ZoomableContentControl.ZoomToWidthCommandProperty); }
            set { this.SetValue(ZoomableContentControl.ZoomToWidthCommandProperty, value); }
        }

        public ICommand ZoomToFullPageCommand
        {
            get { return (ICommand)this.GetValue(ZoomableContentControl.ZoomToFullPageCommandProperty); }
            set { this.SetValue(ZoomableContentControl.ZoomToFullPageCommandProperty, value); }
        }

        public ICommand IncrementZoomCommand
        {
            get { return (ICommand)this.GetValue(ZoomableContentControl.IncrementZoomCommandProperty); }
            set { this.SetValue(ZoomableContentControl.IncrementZoomCommandProperty, value); }
        }

        public ICommand DecrementZoomCommand
        {
            get { return (ICommand)this.GetValue(ZoomableContentControl.DecrementZoomCommandProperty); }
            set { this.SetValue(ZoomableContentControl.DecrementZoomCommandProperty, value); }
        }

        public ICommand SaveScreenshotCommand
        {
            get { return (ICommand)this.GetValue(ZoomableContentControl.SaveScreenshotCommandProperty); }
            set { this.SetValue(ZoomableContentControl.SaveScreenshotCommandProperty, value); }
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            FrameworkElement oldContentElement = oldContent as FrameworkElement;
            if (oldContentElement != null)
            {
                oldContentElement.SizeChanged -= this.OnContentSizeChanged;
            }

            FrameworkElement newContentElement = newContent as FrameworkElement;
            if (newContentElement != null)
            {
                newContentElement.SizeChanged += this.OnContentSizeChanged;
            }
        }

        private void OnContentSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.ContentWidth = e.NewSize.Width;
            this.ContentHeight = e.NewSize.Height;
            this.ScaledContentWidth = this.ContentWidth * this.ZoomFactor;
            this.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)this.ZoomToFullPage);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.scrollViewer = this.GetTemplateChild("scrollViewer") as ScrollViewer;
            if (this.scrollViewer != null)
            {
                this.scrollViewer.LayoutUpdated -= this.OnScrollViewerLayoutUpdated;
                this.scrollViewer.ScrollChanged -= this.OnScrollViewerScrollChanged;
                this.scrollViewer.LayoutUpdated += this.OnScrollViewerLayoutUpdated;
                this.scrollViewer.ScrollChanged += this.OnScrollViewerScrollChanged;
            }

            this.viewPort = this.GetTemplateChild("viewPort") as Grid;
            if (this.viewPort != null)
            {
                this.viewPort.MouseWheel -= this.OnViewportMouseWheel;
                this.viewPort.MouseWheel += this.OnViewportMouseWheel;
            }

            Application.Current.MainWindow.SizeChanged -= this.MainWindow_SizeChanged;
            Application.Current.MainWindow.SizeChanged += this.MainWindow_SizeChanged;
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)this.ZoomToFullPage);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            bool isCtrlPressed = (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
            if (isCtrlPressed)
            {
                if (e.Key == Key.Add)
                {
                    this.IncrementZoom();
                    e.Handled = true;
                }

                if (e.Key == Key.Subtract)
                {
                    this.DecrementZoom();
                    e.Handled = true;
                }
            }

            base.OnKeyDown(e);
        }

        private void OnViewportMouseWheel(object sender, MouseWheelEventArgs e)
        {
            bool isCtrlPressed = (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
            if (isCtrlPressed)
            {
                if (e.Delta > 0)
                {
                    this.IncrementZoom();
                }
                else if (e.Delta < 0)
                {
                    this.DecrementZoom();
                }

                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        private void OnScrollViewerScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            this.lastHorizontalOffset = this.scrollViewer.HorizontalOffset;
            this.lastVerticalOffset = this.scrollViewer.VerticalOffset;
        }

        private void OnScrollViewerLayoutUpdated(object sender, EventArgs eventArgs)
        {
            this.ViewportHeight = this.scrollViewer.ViewportHeight;
            this.ViewportWidth = this.scrollViewer.ViewportWidth;
        }

        private static void OnZoomFactorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ZoomableContentControl control = (ZoomableContentControl)d;

            // Adjust scroll position to avoid that zooming changes visible viewport. Left-top corner is fixed point.
            double transformFactor = (double)e.NewValue / (double)e.OldValue;

            // Don't trust on scrollViewer.VerticalOffset because ScrollToVerticalOffset is async,
            // so if 2 MouseWheel events are so fast, scroll operations is not done yet.
            // So I need to trust in lastVerticalOffset and lastHorizontalOffset
            control.lastVerticalOffset = control.lastVerticalOffset * transformFactor;
            control.lastHorizontalOffset = control.lastHorizontalOffset * transformFactor;

            control.scrollViewer.ScrollToVerticalOffset(control.lastVerticalOffset);
            control.scrollViewer.ScrollToHorizontalOffset(control.lastHorizontalOffset);

            control.ScaledContentWidth = control.ContentWidth * control.ZoomFactor;
            control.FormattedZoomFactor = control.ZoomFactor.ToString("P0");
        }

        /// <summary>
        /// Reset the zoom to 100%
        /// </summary>
        public void ResetZoom()
        {
            this.ZoomFactor = 1.0;
        }

        /// <summary>
        /// Changes to a new zoom level which exactly fits the width of the content.
        /// </summary>
        public void ZoomToWidth()
        {
            if (this.ContentWidth > 0)
            {
                this.ZoomFactor = this.GetZoomFactorToFit(this.ViewportWidth, this.ContentWidth);
            }
        }

        /// <summary>
        /// Changes to a new zoom level which fits both the width and height of the content.
        /// </summary>
        public void ZoomToFullPage()
        {
            if (this.ContentWidth > 0)
            {
                this.ZoomFactor = Math.Min(
                    this.GetZoomFactorToFit(this.ViewportWidth, this.ContentWidth),
                    this.GetZoomFactorToFit(this.ViewportHeight, this.ContentHeight));
            }
        }

        private void IncrementZoom()
        {
            this.ZoomFactor = Math.Min(Constants.ZoomMaximum, this.ZoomFactor + Constants.ZoomStep);
        }

        private void DecrementZoom()
        {
            this.ZoomFactor = Math.Max(Constants.ZoomMinimum, this.ZoomFactor - Constants.ZoomStep);
        }

        private void SaveScreenshot()
        {
            ScreenshotHelper.SaveScreenshot((FrameworkElement)this.Content);
        }

        private double GetZoomFactorToFit(double viewportSize, double contentSize)
        {
            return Math.Floor(100 * viewportSize / contentSize) / 100;
        }
    }
}
