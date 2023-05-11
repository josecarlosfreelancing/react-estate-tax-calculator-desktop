using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using EstateView.Controls;
using EstateView.Core.Model;
using EstateView.Core.Model.Scenarios;
using EstateView.Utilities;
using EstateView.View;

namespace EstateView.ViewModel
{
    public class WorkspaceViewModel : ViewModel
    {
        private FrameworkElement lastPrintableActiveContent;

        public WorkspaceViewModel(EstateProjectionOptions options)
        {
            this.CreateScenarios(options);
        }

        public ObservableCollection<ScenarioViewModel> Scenarios
        {
            get { return this.GetValue(() => this.Scenarios); }
            set { this.SetValue(() => this.Scenarios, value); }
        }

        public ScenarioViewModel CurrentScenario
        {
            get { return this.GetValue(() => this.CurrentScenario); }
            set { this.SetValue(() => this.CurrentScenario, value); }
        }

        public object ActiveContent
        {
            get
            {
                return this.GetValue(() => this.ActiveContent);
            }
            set
            {
                this.UpdateLastPrintableActiveContent(value);
                this.SetValue(() => this.ActiveContent, value);
            }
        }

        private void CreateScenarios(EstateProjectionOptions options)
        {
            this.Scenarios = new ObservableCollection<ScenarioViewModel>();

            this.Scenarios.Add(new ScenarioViewModel(new BothDieThisYearNoPlanningScenario(options, "BOTH DIE THIS YEAR - NO PLANNING")));
            this.Scenarios.Add(new ScenarioViewModel(new NoPlanningScenario(options, "NO PLANNING")));
            this.Scenarios.Add(new ScenarioViewModel(new BypassTrustScenario(options, "BYPASS TRUST")));
            this.Scenarios.Add(new ScenarioViewModel(new AnnualGiftingScenario(options, "ANNUAL GIFTING")));
            this.Scenarios.Add(new ScenarioViewModel(new DiscountedGiftingScenario(options, "DISCOUNTED GIFTING")));
            this.Scenarios.Add(new ScenarioViewModel(new LifeInsuranceScenario(options, "LIFE INSURANCE - POST-PLANNING")));
            this.Scenarios.Add(new ScenarioViewModel(new InstallmentSaleScenario(options, "YEAR 1 GIFT/INSTALLMENT SALE")));

            this.CurrentScenario = this.Scenarios.First();
        }

        private void UpdateLastPrintableActiveContent(object value)
        {
            if (value is ZoomableContentControl || value is ChartView)
            {
                this.lastPrintableActiveContent = (FrameworkElement)value;
            }
        }

        public void SaveScreenshot()
        {
            if (this.lastPrintableActiveContent == null)
            {
                return;
            }

            ZoomableContentControl zoomableContentControl = this.lastPrintableActiveContent as ZoomableContentControl;
            if (zoomableContentControl != null)
            {
                ScreenshotHelper.SaveScreenshot((FrameworkElement)zoomableContentControl.Content);
            }
            else
            {
                ScreenshotHelper.SaveScreenshot(this.lastPrintableActiveContent);
            }
        }
    }
}