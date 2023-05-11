using EstateView.Core.Model;

namespace EstateView.ViewModel.ClientLetter
{
    public class AssumptionsPageViewModel : ViewModel
    {
        public AssumptionsPageViewModel(EstateProjectionOptions options)
        {
            this.Options = options;
        }

        public EstateProjectionOptions Options { get; private set; }

        public string InstallmentSaleYearToToggleOffGrantorTrustStatusText
        {
            get
            {
                return
                    this.Options.InstallmentSaleYearToToggleOffGrantorTrustStatus == -1
                        ? "N/A"
                        : this.Options.InstallmentSaleYearToToggleOffGrantorTrustStatus.ToString();
            }
        }
    }
}
