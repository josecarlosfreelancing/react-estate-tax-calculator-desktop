using System;
using System.Linq;
using EstateView.Core.Model;
using EstateView.Core.Model.Scenarios;

namespace EstateView.ViewModel.ClientLetter
{
    public class DiscountedGiftingPageViewModel : ViewModel
    {
        public DiscountedGiftingPageViewModel(EstatePlanningScenario previousScenario, DiscountedGiftingScenario discountedGiftingScenario)
        {
            this.Spouse1FirstName = discountedGiftingScenario.Options.Spouse1.FirstName;
            this.Spouse2FirstName = discountedGiftingScenario.Options.Spouse2.FirstName;

            EstateProjection initialProjection = discountedGiftingScenario.Projections.First();

            this.DiscountPercentageForGifting = discountedGiftingScenario.Options.DiscountPercentageForGifting;
            this.TotalAnnualGiftsBeforeFirstDeath = discountedGiftingScenario.Options.NumberOfAnnualGiftsPerYear * initialProjection.AnnualGiftExclusionAmount * 2;
            this.TotalAnnualGiftsAfterFirstDeath = this.TotalAnnualGiftsBeforeFirstDeath / 2;
            this.OneHundredThousandDiscounted = 100000 * (1 - this.DiscountPercentageForGifting);
            this.EstateTaxSavingsFromDiscountedGifting =
                previousScenario.Projections.Last().EstateTaxDue -
                discountedGiftingScenario.Projections.Last().EstateTaxDue;
        }

        public string Spouse1FirstName { get; set; }
        public string Spouse2FirstName { get; set; }
        public decimal DiscountPercentageForGifting { get; set; }
        public decimal OneHundredThousandDiscounted { get; set; }
        public decimal TotalAnnualGiftsBeforeFirstDeath { get; set; }
        public decimal TotalAnnualGiftsAfterFirstDeath { get; set; }
        public decimal EstateTaxSavingsFromDiscountedGifting { get; set; }
    }
}
