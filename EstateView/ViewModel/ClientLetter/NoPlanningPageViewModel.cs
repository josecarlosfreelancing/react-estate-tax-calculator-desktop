using System;
using System.Linq;
using EstateView.Core.Model;
using EstateView.Core.Model.Scenarios;

namespace EstateView.ViewModel.ClientLetter
{
    public class NoPlanningPageViewModel : ViewModel
    {
        public NoPlanningPageViewModel(NoPlanningScenario scenario)
        {
            this.Spouse1FirstName = scenario.Options.Spouse1.FirstName;
            this.Spouse2FirstName = scenario.Options.Spouse2.FirstName;
            this.CurrentYear = DateTime.Today.Year;
            this.EstateTaxRate = scenario.Options.EstateTaxRate;
            this.NumberOfYearsBetweenFirstAndSecondDeath = scenario.Options.SecondDyingSpouse.ProjectedYearOfDeath - scenario.Options.FirstDyingSpouse.ProjectedYearOfDeath;
            this.NumberOfYearsBetweenNowAndSecondDeath = scenario.Options.NumberOfYears;

            EstateProjection secondDeathProjection = scenario.Projections.Last();
            this.TotalExclusionAvailable = secondDeathProjection.TotalExclusionAvailable;
            this.DeceasedSpousesExclusionAvailable = secondDeathProjection.LifetimeGiftTaxExclusionAvailableSpouse1;
            this.SurvivingSpousesExclusionAvailable = secondDeathProjection.LifetimeGiftTaxExclusionAvailableSpouse2;
            this.SurvivingSpousesGrossEstate = secondDeathProjection.SurvivingSpousesGrossEstate;
            this.TaxableValueOfEstate = secondDeathProjection.TaxableValueOfEstate;
            this.EstateTaxDue = secondDeathProjection.EstateTaxDue;
        }

        public string Spouse1FirstName { get; set; }
        public string Spouse2FirstName { get; set; }
        public int CurrentYear { get; set; }
        public int NumberOfYearsBetweenFirstAndSecondDeath { get; set; }
        public int NumberOfYearsBetweenNowAndSecondDeath { get; set; }
        public decimal TotalExclusionAvailable { get; set; }
        public decimal DeceasedSpousesExclusionAvailable { get; set; }
        public decimal SurvivingSpousesExclusionAvailable { get; set; }
        public decimal SurvivingSpousesGrossEstate { get; set; }
        public decimal TaxableValueOfEstate { get; set; }
        public decimal EstateTaxRate { get; set; }
        public decimal EstateTaxDue { get; set; }
    }
}
