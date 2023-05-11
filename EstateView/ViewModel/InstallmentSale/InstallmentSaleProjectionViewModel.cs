using EstateView.Core.Model;

namespace EstateView.ViewModel.InstallmentSale
{
    public class InstallmentSaleProjectionViewModel
    {
        public InstallmentSaleProjectionViewModel(InstallmentSaleProjection projection)
        {
            this.Projection = projection;
        }

        public InstallmentSaleProjection Projection { get; private set; }

        public double EstateTaxLiabilityColorWidth { get; set; }
        public double EstateTaxSavingsColorWidth { get; set; }
        public double EstateAssetsAmountColorWidth { get; set; }
        public double EstateAssetsAmountWithoutNoteColorWidth { get; set; }
    }
}
