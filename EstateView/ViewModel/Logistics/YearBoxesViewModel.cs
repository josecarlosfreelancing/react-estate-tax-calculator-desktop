using System.Collections.Generic;

namespace EstateView.ViewModel.Logistics
{
    public class YearBoxesViewModel : ViewModel
    {
        public int DeltaYear { get; set; }
        public EstateBoxViewModel Estate { get; set; }
        public BypassTrustViewModel BypassTrust { get; set; }
        public IList<GiftingTrustViewModel> GiftingTrusts { get; set; }
        public IList<LifeInsuranceViewModel> Ilits { get; set; }
        public IList<InstallmentSaleTrustViewModel> InstallmentSaleTrusts { get; set; }
    }
}