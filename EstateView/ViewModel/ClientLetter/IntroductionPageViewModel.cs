using System;
using EstateView.Core.Model;

namespace EstateView.ViewModel.ClientLetter
{
    public class IntroductionPageViewModel : ViewModel
    {
        public IntroductionPageViewModel(EstateProjectionOptions options)
        {
            this.CurrentYear = DateTime.Now.Year;
            this.LifetimeGiftExclusionAmount = ProjectionCalculator.Constants.FirstYearLifetimeGiftExclusionAmount;
            this.LifetimeGiftExclusionAmountMinusOneMillion = this.LifetimeGiftExclusionAmount - 1000000;
            this.LifetimeGiftExclusionAmountDoubledMinusOneMillion = this.LifetimeGiftExclusionAmount * 2 - 1000000;
            this.AnnualGiftExclusionAmount = ProjectionCalculator.Constants.FirstYearAnnualGiftExclusionAmountRounded;
            this.AnnualGiftExclusionAmountDoubled = this.AnnualGiftExclusionAmount * 2;

            this.AssumeExemptionReductionIn2026Text =
                options.AssumeExemptionReductionIn2026
                ? "The illustration below assumes that the " + this.LifetimeGiftExclusionAmount.ToString("C0") + " plus CPI adjustments exemption will go down by one half in January of 2026. A person living who has not used their exemption will have one half of the exemption that they would have had, and a person who has used part of their exemption will have an exemption based on the adjusted one half amount, minus whatever exemption they have used during their lifetime, but not less than zero. If one spouse dies before 2025 and leaves a portability allowance, then any such portability allowance will not be reduced if the surviving spouse qualifies to use it.\n\n"
                : "The illustration below assumes that the " + this.LifetimeGiftExclusionAmount.ToString("C0") + " plus CPI adjustments exemption will continue after 2025, notwithstanding that the present law provides for the exemption amount to be reduced in half for those dying after 2025. Therefore, estate taxes may be much more than illustrated in this letter if one or both spouses will survive past 2025. EstateView had the ability to show the results of what would occur if the allowance goes down to one half of what it will otherwise be in 2026.\n\n";

            this.AssumeNoPortabilityText =
                options.AssumeNoPortability
                ? "This illustration assumes that the surviving spouse will not have the benefit of any unused estate tax exemption of the first dying spouse under the portability rules which will sometimes allow a surviving spouse to have a larger estate tax exemption on the second death.\n\n"
                : "This illustration also assumes that the surviving spouse will be able to use any portability allowance that may be passed by the first dying spouse, and therefore assumes that the surviving spouse will not remarry someone who dies before him or her.\n\n";
        }

        public int CurrentYear { get; set; }
        public decimal LifetimeGiftExclusionAmount { get; set; }
        public decimal LifetimeGiftExclusionAmountMinusOneMillion { get; set; }
        public decimal LifetimeGiftExclusionAmountDoubledMinusOneMillion { get; set; }
        public decimal AnnualGiftExclusionAmount { get; set; }
        public decimal AnnualGiftExclusionAmountDoubled { get; set; }
        public string AssumeExemptionReductionIn2026Text { get; set; }
        public string AssumeNoPortabilityText { get; set; }
    }
}
