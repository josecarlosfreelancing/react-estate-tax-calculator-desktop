using System.Linq;
using EstateView.Core.Model;
using EstateView.Core.Model.Scenarios;

namespace EstateView.ViewModel.ClientLetter
{
    public class BypassTrustPageViewModel : ViewModel
    {
        public BypassTrustPageViewModel(BypassTrustScenario scenario)
        {
            this.Spouse1FirstName = scenario.Options.Spouse1.FirstName;
            this.Spouse2FirstName = scenario.Options.Spouse2.FirstName;
            this.InitialBypassTrustValue = scenario.Options.BypassTrustValue;
            this.YearOfFirstSpousesDeath = scenario.Options.Spouse1.ProjectedYearOfDeath;
            this.InvestmentsNetGrowthRate = scenario.Options.InvestmentsGrowthRate - scenario.Options.InvestmentFeesRate - scenario.Options.IncomeTaxRate;

            EstateProjection secondDeathProjection = scenario.Projections.Last();
            this.DeceasedSpousesExclusionAvailable = secondDeathProjection.LifetimeGiftTaxExclusionAvailableSpouse1;
            this.FinalBypassTrustValue = secondDeathProjection.BypassTrustValue;
            this.EstateTaxSavingsFromBypassTrust = (this.FinalBypassTrustValue - this.InitialBypassTrustValue) * scenario.Options.EstateTaxRate;
            this.EstateTaxCostFromLosingDeceasedSpousesExclusion = this.DeceasedSpousesExclusionAvailable * scenario.Options.EstateTaxRate;
        }

        public string Spouse1FirstName { get; set; }
        public string Spouse2FirstName { get; set; }
        public decimal InitialBypassTrustValue { get; set; }
        public int YearOfFirstSpousesDeath { get; set; }
        public decimal DeceasedSpousesExclusionAvailable { get; set; }
        public decimal FinalBypassTrustValue { get; set; }
        public decimal InvestmentsNetGrowthRate { get; set; }
        public decimal EstateTaxSavingsFromBypassTrust { get; set; }
        public decimal EstateTaxCostFromLosingDeceasedSpousesExclusion { get; set; }
    }
}
