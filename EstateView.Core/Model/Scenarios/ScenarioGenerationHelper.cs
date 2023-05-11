namespace EstateView.Core.Model.Scenarios
{
    public static class ScenarioGenerationHelper
    {
        public static Person CopyPerson(Person source, Person target)
        {
            target.Age = source.Age;
            target.FirstName = source.FirstName;
            target.LastName = source.LastName;
            target.LifetimeGiftExclusionAmountUsed = source.LifetimeGiftExclusionAmountUsed;
            target.Sex = source.Sex;
            target.IsSmoker = source.IsSmoker;
            target.ProjectedYearOfDeath = source.ProjectedYearOfDeath;
            ScenarioGenerationHelper.CopyLifeInsuranceOptions(source.ExistingLifeInsurance, target.LifeInsurance);

            return target;
        }

        public static void CopyBasicOptions(EstateProjectionOptions source, EstateProjectionOptions target)
        {
            target.PlannerFirmName = source.PlannerFirmName;
            target.PlannerName = source.PlannerName;
            target.PreparationDate = source.PreparationDate;

            target.ConsumerPriceIndexGrowthRate = source.ConsumerPriceIndexGrowthRate;
            target.EstateTaxRate = source.EstateTaxRate;
            target.EstateTaxExclusionAmount = source.EstateTaxExclusionAmount;

            target.Spouse1 = ScenarioGenerationHelper.CopyPerson(source.Spouse1, target.Spouse1);
            target.Spouse2 = ScenarioGenerationHelper.CopyPerson(source.Spouse2, target.Spouse2);

            target.AmountCurrentlyInvested = source.AmountCurrentlyInvested;
            target.AnnualInvestmentsChangeAfterFirstDeath = source.AnnualInvestmentsChangeAfterFirstDeath;
            target.AnnualInvestmentsChangeBeforeFirstDeath = source.AnnualInvestmentsChangeBeforeFirstDeath;
            target.InvestmentsGrowthRate = source.InvestmentsGrowthRate;
            target.InvestmentFeesRate = source.InvestmentFeesRate;

            target.HomeValue = source.HomeValue;
            target.HomeValueGrowthRate = source.HomeValueGrowthRate;
            target.AssumeNoPortability = source.AssumeNoPortability;
            target.UseConstantDollars = source.UseConstantDollars;
            target.AssumeExemptionReductionIn2026 = source.AssumeExemptionReductionIn2026;

            ScenarioGenerationHelper.CopyLifeInsuranceOptions(source.ExistingSecondToDieLifeInsurance, target.SecondToDieLifeInsurance);
        }

        public static void CopyLifeInsuranceOptions(LifeInsurancePolicy source, LifeInsurancePolicy target)
        {
            target.IsInTrust = source.IsInTrust;
            target.PolicyType = source.PolicyType;
            target.AnnualPremium = source.AnnualPremium;
            target.DeathBenefit = source.DeathBenefit;
            target.NumberOfYears = source.NumberOfYears;
            target.AddtlYearsAnnualPremium = source.AddtlYearsAnnualPremium;
            target.NumberOfAddtlYears = source.NumberOfAddtlYears;
        }
    }
}