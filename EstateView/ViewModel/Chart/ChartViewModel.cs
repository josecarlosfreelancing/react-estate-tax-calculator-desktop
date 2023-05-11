using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using EstateView.Core.Model;
using EstateView.Utilities;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using AreaSeries = OxyPlot.Series.AreaSeries;
using LineAnnotation = OxyPlot.Annotations.LineAnnotation;
using LinearAxis = OxyPlot.Axes.LinearAxis;
using PlotCommands = OxyPlot.PlotCommands;

namespace EstateView.ViewModel.Chart
{
    using System;

    public class ChartViewModel : ViewModel
    {
        public ChartViewModel(ScenarioViewModel scenario)
        {
            if (scenario.Projections.Count() > 1)
            {
                this.SaveScreenshotCommand = new RelayCommand(this.SaveScreenshot);
                this.InitializePlotController();
                this.InitializePlotModel(scenario);
            }
            else
            {
                this.PlotController = new PlotController();
                this.PlotModel = new PlotModel { Title = "No timeline available for this scenario" };
            }
        }

        public PlotModel PlotModel { get; private set; }

        public PlotController PlotController { get; private set; }

        public bool IsPlotVisible { get; private set; }

        public ICommand SaveScreenshotCommand { get; private set; }

        private void InitializePlotModel(ScenarioViewModel scenario)
        {
            PlotModel plotModel = new PlotModel();

            plotModel.LegendPosition = LegendPosition.LeftMiddle;
            plotModel.DefaultColors = plotModel.DefaultColors.Except(new[] { OxyColors.Red, OxyColor.FromArgb(0xff, 0xcc, 0x00, 0x00) }).ToList();

            LinearAxis horizontalAxis = new LinearAxis();
            horizontalAxis.IsZoomEnabled = false;
            horizontalAxis.IsPanEnabled = false;
            horizontalAxis.Title = "Year";
            horizontalAxis.Position = AxisPosition.Bottom;
            horizontalAxis.MinorStep = 1;
            horizontalAxis.MajorStep = 5;
            horizontalAxis.MajorGridlineStyle = LineStyle.Solid;
            horizontalAxis.MinorGridlineStyle = LineStyle.Dot;
            horizontalAxis.MaximumPadding = 0.05;

            LinearAxis verticalAxis = new LinearAxis();
            verticalAxis.IsZoomEnabled = false;
            verticalAxis.IsPanEnabled = false;
            verticalAxis.StringFormat = "C0";
            verticalAxis.Position = AxisPosition.Left;
            verticalAxis.MajorGridlineStyle = LineStyle.Solid;
            verticalAxis.MinorGridlineStyle = LineStyle.Dot;

            plotModel.Axes.Add(horizontalAxis);
            plotModel.Axes.Add(verticalAxis);

            LineAnnotation firstDeathAnnotation = new LineAnnotation();
            firstDeathAnnotation.Type = LineAnnotationType.Vertical;
            firstDeathAnnotation.Color = OxyColors.Blue;
            plotModel.Annotations.Add(firstDeathAnnotation);

            LineAnnotation secondDeathAnnotation = new LineAnnotation();
            secondDeathAnnotation.Type = LineAnnotationType.Vertical;
            secondDeathAnnotation.Color = OxyColors.IndianRed;
            plotModel.Annotations.Add(secondDeathAnnotation);

            firstDeathAnnotation.MouseDown += (s, e) =>
            {
                if (e.ChangedButton != OxyMouseButton.Left)
                {
                    return;
                }

                firstDeathAnnotation.StrokeThickness *= 5;
                plotModel.InvalidatePlot(updateData: false);
                e.Handled = true;
            };

            // Handle mouse movements (note: this is only called when the mousedown event was handled)
            firstDeathAnnotation.MouseMove += (s, e) =>
            {
                int year = (int)Math.Round(firstDeathAnnotation.InverseTransform(e.Position).X);
                firstDeathAnnotation.X = year;
                plotModel.InvalidatePlot(updateData: false);
                e.Handled = true;
            };

            firstDeathAnnotation.MouseUp += (s, e) =>
            {
                scenario.Options.FirstDyingSpouse.ProjectedYearOfDeath = (int)firstDeathAnnotation.X;
                firstDeathAnnotation.StrokeThickness /= 5;
                plotModel.InvalidatePlot(updateData: false);
                e.Handled = true;
            };

            secondDeathAnnotation.MouseDown += (s, e) =>
            {
                if (e.ChangedButton != OxyMouseButton.Left)
                {
                    return;
                }

                secondDeathAnnotation.StrokeThickness *= 5;
                plotModel.InvalidatePlot(updateData: false);
                e.Handled = true;
            };

            // Handle mouse movements (note: this is only called when the mousedown event was handled)
            secondDeathAnnotation.MouseMove += (s, e) =>
            {
                int year = (int)Math.Round(secondDeathAnnotation.InverseTransform(e.Position).X);
                secondDeathAnnotation.X = year;
                plotModel.InvalidatePlot(updateData: false);
                e.Handled = true;
            };

            secondDeathAnnotation.MouseUp += (s, e) =>
            {
                scenario.Options.SecondDyingSpouse.ProjectedYearOfDeath = (int)secondDeathAnnotation.X;

                secondDeathAnnotation.StrokeThickness /= 5;
                plotModel.InvalidatePlot(updateData: false);
                e.Handled = true;
            };

            LineAnnotation exemptionCutInHalfIn2026Annotation = new LineAnnotation();
            exemptionCutInHalfIn2026Annotation.Type = LineAnnotationType.Vertical;
            exemptionCutInHalfIn2026Annotation.Color = OxyColors.LightSlateGray;
            exemptionCutInHalfIn2026Annotation.TextColor = OxyColors.LightSlateGray;
            exemptionCutInHalfIn2026Annotation.X = 2026;

            plotModel.Updating += (sender, args) =>
            {
                if (scenario.Options.AssumeExemptionReductionIn2026 && !plotModel.Annotations.Contains(exemptionCutInHalfIn2026Annotation))
                {
                    plotModel.Annotations.Add(exemptionCutInHalfIn2026Annotation);
                }
                else if (!scenario.Options.AssumeExemptionReductionIn2026 && plotModel.Annotations.Contains(exemptionCutInHalfIn2026Annotation))
                {
                    plotModel.Annotations.Remove(exemptionCutInHalfIn2026Annotation);
                }

                if (firstDeathAnnotation.X == 2026 || secondDeathAnnotation.X == 2026)
                {
                    exemptionCutInHalfIn2026Annotation.Text = string.Empty;
                }
                else
                {
                    exemptionCutInHalfIn2026Annotation.Text = string.Format("Lifetime exemption drops to {0:C0}", ProjectionCalculator.GetProjectedLifetimeGiftExclusionAmount(2026, scenario.Options.ConsumerPriceIndexGrowthRate, scenario.Options.AssumeExemptionReductionIn2026));
                }
            };

            EstateProjection scenarioProjection = scenario.Projections.SingleOrDefault(p => p.Year == scenario.Options.SecondDyingSpouse.ProjectedYearOfDeath);

            if (scenarioProjection != null)
            {
                plotModel.Title = scenario.Name;
                plotModel.Subtitle = string.Format(
                    "{0} {1} & {2} {3}\nEffective Estate Tax Rate: {4:P}",
                    scenario.Options.Spouse1.FirstName,
                    scenario.Options.Spouse1.LastName,
                    scenario.Options.Spouse2.FirstName,
                    scenario.Options.Spouse2.LastName,
                    scenarioProjection.EffectiveEstateTaxRate);

                if (scenario.Options.UseConstantDollars)
                {
                    plotModel.Subtitle += "\n(All values are in Constant " + DateTime.Today.Year + " Dollars)";
                }

                firstDeathAnnotation.X = scenario.Options.FirstDyingSpouse.ProjectedYearOfDeath;
                firstDeathAnnotation.Text = "1st Death - " + scenario.Options.FirstDyingSpouse.ProjectedYearOfDeath + " - " + scenario.Options.FirstDyingSpouse.FirstName;
                secondDeathAnnotation.X = scenario.Options.SecondDyingSpouse.ProjectedYearOfDeath;
                secondDeathAnnotation.Text = "2nd Death - " + scenario.Options.SecondDyingSpouse.ProjectedYearOfDeath + " - " + scenario.Options.SecondDyingSpouse.FirstName;

                List<CustomAreaDataPoint> bypassTrustDataPoints = new List<CustomAreaDataPoint>();
                List<CustomAreaDataPoint> giftTrustDataPoints = new List<CustomAreaDataPoint>();
                List<CustomAreaDataPoint> personalResidenceDataPoints = new List<CustomAreaDataPoint>();
                List<CustomAreaDataPoint> investmentAssetsDataPoints = new List<CustomAreaDataPoint>();
                List<CustomAreaDataPoint> estateTaxDataPoints = new List<CustomAreaDataPoint>();
                List<CustomAreaDataPoint> lifeInsuranceFirstSpouseDataPoints = new List<CustomAreaDataPoint>();
                List<CustomAreaDataPoint> lifeInsuranceSecondSpouseDataPoints = new List<CustomAreaDataPoint>();
                List<CustomAreaDataPoint> lifeInsuranceSurvivorshipDataPoints = new List<CustomAreaDataPoint>();
                List<CustomAreaDataPoint> installmentSaleTrustDataPoints = new List<CustomAreaDataPoint>();

                foreach (EstateProjection projection in scenario.Projections)
                {
                    int year = projection.Year;
                    double lastValue = 0;
                    double currentValue = 0;

                    currentValue = (double)projection.InstallmentSaleTrustValue - (double)projection.InstallmentSaleNoteValue;
                    installmentSaleTrustDataPoints.Add(new CustomAreaDataPoint(year, lastValue, currentValue));
                    lastValue += currentValue;

                    currentValue = (double)(projection.LifeInsuranceOnSecondToDieBenefit + projection.LifeInsuranceOnSecondToDieBenefitInTrust);
                    lifeInsuranceSurvivorshipDataPoints.Add(new CustomAreaDataPoint(year, lastValue, currentValue));
                    lastValue += currentValue;

                    currentValue = (double)(projection.LifeInsuranceOnSecondSpouseBenefit + projection.LifeInsuranceOnSecondSpouseBenefitInTrust);
                    lifeInsuranceSecondSpouseDataPoints.Add(new CustomAreaDataPoint(year, lastValue, currentValue));
                    lastValue += currentValue;

                    currentValue = (double)(projection.LifeInsuranceOnFirstSpouseBenefit + projection.LifeInsuranceOnFirstSpouseBenefitInTrust);
                    lifeInsuranceFirstSpouseDataPoints.Add(new CustomAreaDataPoint(year, lastValue, currentValue));
                    lastValue += currentValue;

                    currentValue = (double)projection.GiftingTrustValue;
                    giftTrustDataPoints.Add(new CustomAreaDataPoint(year, lastValue, currentValue));
                    lastValue += currentValue;

                    currentValue = (double)projection.BypassTrustValue;
                    bypassTrustDataPoints.Add(new CustomAreaDataPoint(year, lastValue, currentValue));
                    lastValue += currentValue;

                    currentValue = (double)projection.ResidenceValue;
                    personalResidenceDataPoints.Add(new CustomAreaDataPoint(year, lastValue, currentValue));
                    lastValue += currentValue;

                    currentValue = (double)projection.InvestmentsValue + (double)projection.InstallmentSaleNoteValue;
                    investmentAssetsDataPoints.Add(new CustomAreaDataPoint(year, lastValue, currentValue));
                    lastValue += currentValue;

                    if (year <= scenario.Options.SecondDyingSpouse.ProjectedYearOfDeath)
                    {
                        currentValue = (double)(-projection.EstateTaxDue);
                        estateTaxDataPoints.Add(new CustomAreaDataPoint(year, lastValue, currentValue));
                    }
                }

                AreaSeries personalResidenceSeries = this.CreateAreaSeries(personalResidenceDataPoints);
                personalResidenceSeries.Title = "Personal Residence and Property";

                AreaSeries investmentAssetsSeries = this.CreateAreaSeries(investmentAssetsDataPoints);
                investmentAssetsSeries.Title = "Business and Investment Assets";
                investmentAssetsSeries.Color = OxyColors.LimeGreen;

                AreaSeries bypassTrustSeries = this.CreateAreaSeries(bypassTrustDataPoints);
                bypassTrustSeries.Title = "Bypass Trust";

                AreaSeries giftTrustSeries = this.CreateAreaSeries(giftTrustDataPoints);
                giftTrustSeries.Title = "Gift Trust";

                AreaSeries lifeInsuranceFirstSpouseSeries = this.CreateAreaSeries(lifeInsuranceFirstSpouseDataPoints);
                lifeInsuranceFirstSpouseSeries.Title = "Life Insurance - 1st Spouse";

                AreaSeries lifeInsuranceSecondSpouseSeries = this.CreateAreaSeries(lifeInsuranceSecondSpouseDataPoints);
                lifeInsuranceSecondSpouseSeries.Title = "Life Insurance - 2nd Spouse";

                AreaSeries lifeInsuranceSurvivorshipSeries = this.CreateAreaSeries(lifeInsuranceSurvivorshipDataPoints);
                lifeInsuranceSurvivorshipSeries.Title = "Life Insurance - Survivorship";

                AreaSeries installmentSaleTrustSeries = this.CreateAreaSeries(installmentSaleTrustDataPoints);
                installmentSaleTrustSeries.Title = "Installment Sale Trust";

                AreaSeries estateTaxSeries = this.CreateAreaSeries(estateTaxDataPoints);
                estateTaxSeries.Title = "Estate Tax";
                estateTaxSeries.Color = OxyColors.Black;
                estateTaxSeries.Fill = OxyColor.FromArgb(Colors.Red.A, Colors.Red.R, Colors.Red.G, Colors.Red.B);

                plotModel.Series.Clear();
                plotModel.Series.Add(investmentAssetsSeries);
                plotModel.Series.Add(estateTaxSeries);

                this.AddSeriesIfNotZero(plotModel, personalResidenceSeries);
                this.AddSeriesIfNotZero(plotModel, bypassTrustSeries);
                this.AddSeriesIfNotZero(plotModel, giftTrustSeries);
                this.AddSeriesIfNotZero(plotModel, lifeInsuranceFirstSpouseSeries);
                this.AddSeriesIfNotZero(plotModel, lifeInsuranceSecondSpouseSeries);
                this.AddSeriesIfNotZero(plotModel, lifeInsuranceSurvivorshipSeries);
                this.AddSeriesIfNotZero(plotModel, installmentSaleTrustSeries);

                plotModel.InvalidatePlot(updateData: false);
            }

            this.PlotModel = plotModel;
        }

        private void AddSeriesIfNotZero(PlotModel plotModel, AreaSeries series)
        {
            if (((IEnumerable<CustomAreaDataPoint>)series.ItemsSource).Any(dp => Math.Abs(dp.Value) >= double.Epsilon))
            {
                plotModel.Series.Add(series);
            }
        }

        private void InitializePlotController()
        {
            PlotController plotController = new PlotController();
            plotController.UnbindAll();
            plotController.Bind(new OxyMouseEnterGesture(), PlotCommands.HoverSnapTrack);
            this.PlotController = plotController;
        }

        private AreaSeries CreateAreaSeries(IEnumerable<CustomAreaDataPoint> dataPoints)
        {
            AreaSeries areaSeries = new AreaSeries();
            areaSeries.DataFieldX = "Year";
            areaSeries.DataFieldX2 = "Year";
            areaSeries.DataFieldY = "StartValue";
            areaSeries.DataFieldY2 = "EndValue";
            areaSeries.CanTrackerInterpolatePoints = false;
            areaSeries.TrackerFormatString = "{0}\nYear: {Year:0}\nValue: {Value:C0}";
            areaSeries.ItemsSource = dataPoints;
            return areaSeries;
        }

        private void SaveScreenshot()
        {
            ScreenshotHelper.SaveScreenshot((FrameworkElement)this.PlotModel.PlotView);
        }

        private class CustomAreaDataPoint
        {
            public CustomAreaDataPoint(int year, double startValue, double value)
            {
                this.Year = year;
                this.StartValue = startValue;
                this.EndValue = startValue + value;
            }

            public int Year { get; private set; }

            public double Value
            {
                get { return this.EndValue - this.StartValue; }
            }

            public double StartValue { get; private set; }

            public double EndValue { get; private set; }
        }
    }
}
