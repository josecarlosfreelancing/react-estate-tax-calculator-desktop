using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using EstateView.Core.Model;
using EstateView.Utilities;

namespace EstateView.ViewModel.InstallmentSale
{
    public class InstallmentSaleViewModel : ViewModel
    {
        private readonly InstallmentSaleCalculator calculator;

        public InstallmentSaleViewModel()
        {
            this.SaveScreenshotCommand = new RelayCommand(view => this.SaveScreenshot((FrameworkElement)view));
            this.SaveExcelSheetCommand = new RelayCommand(projections => this.SaveExcelSheet((IEnumerable<InstallmentSaleProjectionViewModel>)projections));

            this.calculator = new InstallmentSaleCalculator();

            InstallmentSaleOptions options = InstallmentSaleOptions.CreateSampleOptions();
            this.Options = new InstallmentSaleOptionsViewModel(options);
            this.Options.PropertyChanged += (senders, args) => this.CalculateProjections();

            this.CalculateProjections();
        }

        private void SaveExcelSheet(IEnumerable<InstallmentSaleProjectionViewModel> projections)
        {
            var excelHelper = new ExcelHelper();
            excelHelper.CreateExcelFromInstallmentSaleProjections(projections.ToList());
        }

        private void SaveScreenshot(FrameworkElement view)
        {
            ScreenshotHelper.SaveScreenshot(view);
        }

        public ICommand SaveScreenshotCommand { get; private set; }
        public ICommand SaveExcelSheetCommand { get; private set; }

        private void CalculateProjections()
        {
            IEnumerable<InstallmentSaleProjection> projections = this.calculator.Calculate(this.Options.Options);
            this.Projections = this.CreateProjectionViewModels(projections);
            this.NotifyPropertyChanged(() => this.Projections);
        }

        private IEnumerable<InstallmentSaleProjectionViewModel> CreateProjectionViewModels(IEnumerable<InstallmentSaleProjection> projections)
        {
            return
                projections
                .Select(p =>
                    new InstallmentSaleProjectionViewModel(p)
                    {
                        EstateTaxLiabilityColorWidth = this.CalculateColorWidth(p, projections, projection => projection.EstateTaxLiability),
                        EstateTaxSavingsColorWidth = this.CalculateColorWidth(p, projections, projection => projection.EstateTaxSavingsOverNoPlanning),
                        EstateAssetsAmountColorWidth = this.CalculateColorWidth(p, projections, projection => projection.EstateStartingAssets),
                        EstateAssetsAmountWithoutNoteColorWidth = this.CalculateColorWidth(p, projections, projection => projection.EstateStartingAssetsWithoutNote),
                    });
        }

        private double CalculateColorWidth(InstallmentSaleProjection projection, IEnumerable<InstallmentSaleProjection> projections, Func<InstallmentSaleProjection, decimal> getValue)
        {
            decimal value = getValue(projection);
            decimal maxValue = projections.Max(getValue);
            return value == 0 || maxValue == 0 ? 0 : (double)(value / maxValue) * 100;
        }

        public IEnumerable<InstallmentSaleProjectionViewModel> Projections { get; private set; }

        public InstallmentSaleOptionsViewModel Options { get; private set; }
    }
}
