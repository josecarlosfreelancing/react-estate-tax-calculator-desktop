using System.Linq;
using EstateView.Core.Model;
using EstateView.Core.Model.Scenarios;

namespace EstateView.ViewModel.ClientLetter
{
    public class AnnualGiftingPageViewModel : ViewModel
    {
        public AnnualGiftingPageViewModel(AnnualGiftingScenario scenario)
        {
            this.Spouse1FirstName = scenario.Options.Spouse1.FirstName;
            this.Spouse2FirstName = scenario.Options.Spouse2.FirstName;

            EstateProjection initialProjection = scenario.Projections.First();
            this.AnnualGiftExclusionAmount = initialProjection.AnnualGiftExclusionAmount;
            this.AnnualGiftExclusionAmountDoubled = initialProjection.AnnualGiftExclusionAmount * 2;

            EstateProjection finalProjection = scenario.Projections.Last();
            this.EstateTaxSavingsFromAnnualGifting = finalProjection.GiftingTrustValue * scenario.Options.EstateTaxRate;
        }

        public string Spouse1FirstName { get; set; }
        public string Spouse2FirstName { get; set; }
        public decimal AnnualGiftExclusionAmount { get; set; }
        public decimal AnnualGiftExclusionAmountDoubled { get; set; }
        public decimal EstateTaxSavingsFromAnnualGifting { get; set; }
    }
}
