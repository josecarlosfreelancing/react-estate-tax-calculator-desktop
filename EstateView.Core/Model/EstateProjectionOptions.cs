using System;
using Newtonsoft.Json;

namespace EstateView.Core.Model
{
    public class EstateProjectionOptions
    {
        public static EstateProjectionOptions CreateEmptyOptions()
        {
            return new EstateProjectionOptions
            {
                PreparationDate = DateTime.Now,
                PlannerName = "",
                PlannerFirmName = "",
                Spouse1 =
                    new Person
                    {
                        FirstName = "Bob",
                        LastName = "Sample",
                        Age = 70,
                        IsSmoker = true,
                        Sex = Sex.Male,
                        ProjectedYearOfDeath = DateTime.Now.Year + 10,
                        ExistingLifeInsurance = new LifeInsurancePolicy
                        {
                            IsInTrust = false,
                            PolicyType = LifeInsurancePolicyType.Permanent,
                            AnnualPremium = 5000,
                            DeathBenefit = 5000000,
                            NumberOfYears = 20,
                            AddtlYearsAnnualPremium = 20000,
                            NumberOfAddtlYears = 10,
                        },
                        LifeInsurance = new LifeInsurancePolicy
                        {
                            IsInTrust = true,
                            PolicyType = LifeInsurancePolicyType.Permanent,
                            AnnualPremium = 5000,
                            DeathBenefit = 5000000,
                            NumberOfYears = 20,
                            AddtlYearsAnnualPremium = 20000,
                            NumberOfAddtlYears = 10,
                        },
                    },
                Spouse2 =
                    new Person
                    {
                        FirstName = "Mary",
                        LastName = "Sample",
                        Age = 64,
                        IsSmoker = false,
                        Sex = Sex.Female,
                        ProjectedYearOfDeath = DateTime.Now.Year + 30,
                        ExistingLifeInsurance = new LifeInsurancePolicy
                        {
                            IsInTrust = false,
                            PolicyType = LifeInsurancePolicyType.Permanent,
                            AnnualPremium = 5000,
                            DeathBenefit = 5000000,
                            NumberOfYears = 20,
                            AddtlYearsAnnualPremium = 20000,
                            NumberOfAddtlYears = 10,
                        },
                        LifeInsurance = new LifeInsurancePolicy
                        {
                            IsInTrust = true,
                            PolicyType = LifeInsurancePolicyType.Permanent,
                            AnnualPremium = 5000,
                            DeathBenefit = 5000000,
                            NumberOfYears = 20,
                            AddtlYearsAnnualPremium = 20000,
                            NumberOfAddtlYears = 10,
                        },
                    },
                InvestmentsGrowthRate = 0.0598M,
                InvestmentFeesRate = 0.004M,
                HomeValueGrowthRate = 0.0303M,
                ConsumerPriceIndexGrowthRate = 0.0296M,
                AnnualInvestmentsChangeBeforeFirstDeath = 500000M,
                AnnualInvestmentsChangeAfterFirstDeath = 300000M,
                NumberOfAnnualGiftsPerYear = 6,
                PercentageOfAvailableGiftToPermissibleGift = 1M,
                EstateTaxRate = .4M,
                PercentageOfInvestedGiftAmountDiscounted = 1M,
                DiscountPercentageForGifting = 0.25M,
                HomeValue = 3000000M,
                BypassTrustValue = 16140000M,
                AmountCurrentlyInvested = 25000000M,
                InstallmentSaleSeedMoneyAmount = 1500000M,
                InstallmentSaleNoteAmount = 10500000M,
                InstallmentSaleNoteTermInYears = 20,
                InstallmentSaleNoteInterestRate = 0.04M,
                InstallmentSaleValueBeforeDiscount = 15000000M,
                InstallmentSaleNoteDiscountRate = 0.3M,
                InstallmentSaleValueAfterDiscount = 10500000M,
                InstallmentSaleYearToToggleOffGrantorTrustStatus = 0,
                AnnualAdditionalIncomeForInstallmentSaleTrust = 200000,
                NumberOfYearsAdditionalIncomeForInstallmentSaleTrust = 30,
                InstallmentSaleNoteType = InstallmentSaleNoteType.SelfCancelling,
                ExistingSecondToDieLifeInsurance = new LifeInsurancePolicy
                {
                    IsInTrust = false,
                    PolicyType = LifeInsurancePolicyType.Permanent,
                    AnnualPremium = 5000,
                    DeathBenefit = 5000000,
                    NumberOfYears = 20,
                    AddtlYearsAnnualPremium = 20000,
                    NumberOfAddtlYears = 10,
                },
                SecondToDieLifeInsurance = new LifeInsurancePolicy
                {
                    IsInTrust = true,
                    PolicyType = LifeInsurancePolicyType.Permanent,
                    AnnualPremium = 5000,
                    DeathBenefit = 5000000,
                    NumberOfYears = 20,
                    AddtlYearsAnnualPremium = 20000,
                    NumberOfAddtlYears = 10,
                },
                UseConstantDollars = false,
                AssumeExemptionReductionIn2026 = false,
                IncomeTaxRate = 0.02M,
            };
        }

        public EstateProjectionOptions()
        {
            this.Spouse1 = new Person();
            this.Spouse2 = new Person();
            this.SecondToDieLifeInsurance = new LifeInsurancePolicy();
            this.ExistingSecondToDieLifeInsurance = new LifeInsurancePolicy();
            this.AssumeExemptionReductionIn2026 = false;
        }

        public DateTime PreparationDate { get; set; }

        public Person Spouse1 { get; set; }
        public Person Spouse2 { get; set; }

        [JsonIgnore]
        public Person FirstDyingSpouse
        {
            get { return this.Spouse1.ProjectedYearOfDeath < this.Spouse2.ProjectedYearOfDeath ? this.Spouse1 : this.Spouse2; }
        }

        [JsonIgnore]
        public Person SecondDyingSpouse
        {
            get { return this.Spouse1.ProjectedYearOfDeath < this.Spouse2.ProjectedYearOfDeath ? this.Spouse2 : this.Spouse1; }
        }

        public decimal HomeValue { get; set; }
        public decimal AmountCurrentlyInvested { get; set; }
        public decimal BypassTrustValue { get; set; }

        public int NumberOfYears
        {
            get { return Math.Max(this.Spouse1.ProjectedYearOfDeath, this.Spouse2.ProjectedYearOfDeath) - DateTime.Today.Year; }
        }

        public decimal AnnualInvestmentsChangeBeforeFirstDeath { get; set; }
        public decimal AnnualInvestmentsChangeAfterFirstDeath { get; set; }

        public int NumberOfAnnualGiftsPerYear { get; set; }
        public decimal PercentageOfAvailableGiftToPermissibleGift { get; set; }
        public decimal DiscountPercentageForGifting { get; set; }
        public decimal PercentageOfInvestedGiftAmountDiscounted { get; set; }

        public decimal HomeValueGrowthRate { get; set; }
        public decimal InvestmentsGrowthRate { get; set; }
        public decimal InvestmentFeesRate { get; set; }
        public decimal IncomeTaxRate { get; set; }
        public decimal ConsumerPriceIndexGrowthRate { get; set; }
        public decimal EstateTaxRate { get; set; }
        public decimal EstateTaxExclusionAmount { get; set; }

        public LifeInsurancePolicy SecondToDieLifeInsurance { get; set; }
        public LifeInsurancePolicy ExistingSecondToDieLifeInsurance { get; set; }

        public string PlannerName { get; set; }
        public string PlannerFirmName { get; set; }

        public bool AssumeNoPortability { get; set; }
        public bool UseConstantDollars { get; set; }
        public bool AssumeExemptionReductionIn2026 { get; set; }

        public decimal InstallmentSaleSeedMoneyAmount { get; set; }
        public decimal InstallmentSaleNoteAmount { get; set; }
        public decimal InstallmentSaleNoteInterestRate { get; set; }
        public decimal InstallmentSaleValueBeforeDiscount { get; set; }
        public decimal InstallmentSaleNoteDiscountRate { get; set; }
        public decimal InstallmentSaleValueAfterDiscount { get; set; }
        public InstallmentSaleNoteType InstallmentSaleNoteType { get; set; }
        public int InstallmentSaleNoteTermInYears { get; set; }
        public int InstallmentSaleYearToToggleOffGrantorTrustStatus { get; set; }
        public decimal AnnualAdditionalIncomeForInstallmentSaleTrust { get; set; }
        public int NumberOfYearsAdditionalIncomeForInstallmentSaleTrust { get; set; }

        public decimal InitialGiftingTrustValue { get; set; }
    }
}