using System;
using System.Collections.Generic;

namespace EstateView.Core.Model
{
    public class ProjectionCalculator
    {
        public class Constants
        {
            public const decimal FirstYearAnnualGiftExclusionForNonCitizenSpouse = 169000M;
            public static decimal FirstYearAnnualGiftExclusionAmount = FirstYearAnnualGiftExclusionForNonCitizenSpouse / 10;
            public static readonly decimal FirstYearAnnualGiftExclusionAmountRounded = ProjectionCalculator.RoundDownToNearest(Constants.FirstYearAnnualGiftExclusionAmount, 1000);
            public const decimal FirstYearLifetimeGiftExclusionAmount = 12.06e6M;
            public const string Initial = "Initial Value";
            public const string AnnualGrowth = "Annual Growth";
            public const string AnnualInflationAdjustment = "Annual Adjustment for Inflation";
        }

        public CreateProjectionsResult CreateProjections(EstateProjectionOptions options)
        {
            EstateProjectionAccountBook book = this.CreateInitialAccounts(options);

            int startYear = book.GetMinYear();

            for (int i = 1; i <= options.NumberOfYears; i++)
            {
                int year = startYear + i;

                book.InflationIndex.Grow(year, options.ConsumerPriceIndexGrowthRate, Constants.AnnualInflationAdjustment);
                book.Residence.Grow(year, options.HomeValueGrowthRate, Constants.AnnualGrowth);

                if (year == options.Spouse1.ProjectedYearOfDeath)
                {
                    book.LifeInsuranceSpouse1.Credit(year, options.Spouse1.LifeInsurance.GetDeathBenefitForYear(i), "Death Benefit");
                }
                else if (year > options.Spouse1.ProjectedYearOfDeath)
                {
                    book.LifeInsuranceSpouse1.Grow(year, options.InvestmentsGrowthRate - options.InvestmentFeesRate - options.IncomeTaxRate, Constants.AnnualGrowth);
                }

                if (year == options.Spouse2.ProjectedYearOfDeath)
                {
                    book.LifeInsuranceSpouse2.Credit(year, options.Spouse2.LifeInsurance.GetDeathBenefitForYear(i), "Death Benefit");
                }
                else if (year > options.Spouse2.ProjectedYearOfDeath)
                {
                    book.LifeInsuranceSpouse2.Grow(year, options.InvestmentsGrowthRate - options.InvestmentFeesRate - options.IncomeTaxRate, Constants.AnnualGrowth);
                }

                if (year == options.SecondDyingSpouse.ProjectedYearOfDeath)
                {
                    book.LifeInsuranceSurvivorship.Credit(year, options.SecondToDieLifeInsurance.GetDeathBenefitForYear(i), "Death Benefit");
                }

                decimal annualInvestmentChange =
                    year <= options.FirstDyingSpouse.ProjectedYearOfDeath
                    ? book.AnnualInvestmentChangeBeforeFirstDeath.GetBalance(year)
                    : book.AnnualInvestmentChangeAfterFirstDeath.GetBalance(year);

                if (i <= options.NumberOfYearsAdditionalIncomeForInstallmentSaleTrust && options.AnnualAdditionalIncomeForInstallmentSaleTrust > 0)
                {
                    annualInvestmentChange -= options.AnnualAdditionalIncomeForInstallmentSaleTrust;
                }

                book.Investments.Grow(year, options.InvestmentsGrowthRate - options.InvestmentFeesRate - options.IncomeTaxRate, Constants.AnnualGrowth);
                book.Investments.Credit(year, annualInvestmentChange, "Annual Investment Additions/Withdrawals");
                book.Investments.Credit(year, book.InstallmentSaleNote.GetBalance(year) * options.InstallmentSaleNoteInterestRate, "Annual Interest from Grantor Trust");

                book.InstallmentSaleTrust.Grow(year, options.InvestmentsGrowthRate, Constants.AnnualGrowth);
                book.InstallmentSaleTrust.Debit(year, book.InstallmentSaleNote.GetBalance(year) * options.InstallmentSaleNoteInterestRate, "Annual Interest Payment to Grantor");

                book.InstallmentSaleTrust.Debit(year, book.InstallmentSaleTrust.GetBalance(year) * options.InvestmentFeesRate, "Annual Investment Cost for Grantor Trust");

                if (options.InstallmentSaleYearToToggleOffGrantorTrustStatus > -1 &&
                    i > options.InstallmentSaleYearToToggleOffGrantorTrustStatus)
                {
                    book.InstallmentSaleTrust.Debit(year, book.InstallmentSaleTrust.GetBalance(year) * options.IncomeTaxRate, "Annual Income Tax for Grantor Trust");
                }
                else
                {
                    book.Investments.Debit(year, book.InstallmentSaleTrust.GetBalance(year) * options.IncomeTaxRate, "Annual Income Tax for Grantor Trust"); 
                }
                
                this.ProcessAnnualGiftingAndLifeInsurance(book, options, year, i);

                if (year == startYear + options.InstallmentSaleNoteTermInYears)
                {
                    book.Investments.Credit(year, book.InstallmentSaleNote.GetBalance(year), "Balloon Payment from Grantor Trust");
                    book.InstallmentSaleTrust.Debit(year, book.InstallmentSaleNote.GetBalance(year), "Balloon Payment to Grantor");
                    book.InstallmentSaleNote.Debit(year, book.InstallmentSaleNote.GetBalance(year), "Balloon Payment to Grantor");
                }

                if (options.FirstDyingSpouse.ProjectedYearOfDeath == year)
                {
                    this.ProcessFirstDeath(book, options);
                }
                else
                {
                    book.BypassTrust.Grow(year, options.InvestmentsGrowthRate - options.InvestmentFeesRate - options.IncomeTaxRate, Constants.AnnualGrowth);
                }

                book.AnnualGiftTaxExclusion.Grow(year, options.ConsumerPriceIndexGrowthRate, Constants.AnnualInflationAdjustment);
                book.AnnualInvestmentChangeBeforeFirstDeath.Grow(year, options.ConsumerPriceIndexGrowthRate, Constants.AnnualInflationAdjustment);
                book.AnnualInvestmentChangeAfterFirstDeath.Grow(year, options.ConsumerPriceIndexGrowthRate, Constants.AnnualInflationAdjustment);

                if (year <= options.Spouse1.ProjectedYearOfDeath)
                {
                    book.LifetimeGiftTaxExclusionSpouse1.Grow(year, options.ConsumerPriceIndexGrowthRate, Constants.AnnualInflationAdjustment);

                    if (options.AssumeExemptionReductionIn2026 && year == 2026)
                    {
                        book.LifetimeGiftTaxExclusionSpouse1.Grow(year, -0.5m, "Lifetime Exemption reduced by 50%");
                    }
                }

                if (year <= options.Spouse2.ProjectedYearOfDeath)
                {
                    book.LifetimeGiftTaxExclusionSpouse2.Grow(year, options.ConsumerPriceIndexGrowthRate, Constants.AnnualInflationAdjustment);

                    if (options.AssumeExemptionReductionIn2026 && year == 2026)
                    {
                        book.LifetimeGiftTaxExclusionSpouse2.Grow(year, -0.5m, "Lifetime Exemption reduced by 50%");
                    }
                }
            }

            return new CreateProjectionsResult(book, this.CreateProjectionsFromAccounts(book, options));
        }

        private void ProcessAnnualGiftingAndLifeInsurance(EstateProjectionAccountBook book, EstateProjectionOptions options, int year, int yearIndex)
        {
            decimal premiumFirstSpouse = year <= options.Spouse1.ProjectedYearOfDeath ? options.Spouse1.LifeInsurance.GetPremiumForYear(yearIndex) : 0;
            decimal premiumSecondSpouse = year <= options.Spouse2.ProjectedYearOfDeath ? options.Spouse2.LifeInsurance.GetPremiumForYear(yearIndex) : 0;
            decimal premiumSurvivorship = options.SecondToDieLifeInsurance.GetPremiumForYear(yearIndex);

            decimal totalTaxableLifeInsurancePremiums =
                (options.Spouse1.LifeInsurance.IsInTrust ? premiumFirstSpouse : 0) +
                (options.Spouse2.LifeInsurance.IsInTrust ? premiumSecondSpouse : 0) +
                (options.SecondToDieLifeInsurance.IsInTrust ? premiumSurvivorship : 0);

            decimal annualGiftAmount =
                options.NumberOfAnnualGiftsPerYear *
                ProjectionCalculator.RoundDownToNearest(book.AnnualGiftTaxExclusion.GetBalance(year), 1000);

            if (year <= options.FirstDyingSpouse.ProjectedYearOfDeath)
            {
                annualGiftAmount *= 2;
            }

            decimal giftAmountInvested = annualGiftAmount * options.PercentageOfAvailableGiftToPermissibleGift;
            decimal giftAmountSpent = annualGiftAmount - giftAmountInvested;

            decimal giftAmountTowardsLifeInsurancePremiums =
                giftAmountInvested > totalTaxableLifeInsurancePremiums
                ? totalTaxableLifeInsurancePremiums
                : giftAmountInvested;

            decimal remainingLifeInsurancePremiums = totalTaxableLifeInsurancePremiums - giftAmountTowardsLifeInsurancePremiums;
            decimal giftAmountToGiftingTrust = giftAmountInvested - giftAmountTowardsLifeInsurancePremiums;

            decimal discountedGiftAmount = giftAmountToGiftingTrust * options.PercentageOfInvestedGiftAmountDiscounted / (1 - options.DiscountPercentageForGifting);
            decimal nonDiscountedGiftAmount = giftAmountToGiftingTrust * (1 - options.PercentageOfInvestedGiftAmountDiscounted);

            book.Investments.Debit(year, book.GiftingTrust.GetBalance(year) * options.InvestmentFeesRate, "Annual Income Tax Cost for Gift Trust");
            book.GiftingTrust.Grow(year, options.InvestmentsGrowthRate, Constants.AnnualGrowth);

            if (yearIndex <= options.NumberOfYearsAdditionalIncomeForInstallmentSaleTrust && options.AnnualAdditionalIncomeForInstallmentSaleTrust > 0)
            {
                book.InstallmentSaleTrust.Credit(year, options.AnnualAdditionalIncomeForInstallmentSaleTrust, "Additional Annual Income for Installment Sale Trust");
            }

            book.Investments.Debit(year, premiumFirstSpouse, "Life Insurance Premiums (1st Spouse)");
            book.Investments.Debit(year, premiumSecondSpouse, "Life Insurance Premiums (2nd Spouse)");
            book.Investments.Debit(year, premiumSurvivorship, "Life Insurance Premiums (Second to Die)");

            book.Investments.Debit(year, giftAmountSpent, "Annual Gifting (Spent)");

            book.GiftingTrust.Credit(year, discountedGiftAmount, "Annual Gifting (Discounted)");
            book.Investments.Debit(year, discountedGiftAmount, "Annual Gifting (Discounted)");

            book.GiftingTrust.Credit(year, nonDiscountedGiftAmount, "Annual Gifting (Non-Discounted)");
            book.Investments.Debit(year, nonDiscountedGiftAmount, "Annual Gifting (Non-Discounted)");

            book.TaxableLifeInsurancePremiums.Credit(year, remainingLifeInsurancePremiums, "Premiums over Annual Gift Tax Exclusion");
        }

        private EstateProjectionAccountBook CreateInitialAccounts(EstateProjectionOptions options)
        {
            EstateProjectionAccountBook book = new EstateProjectionAccountBook();

            int year = DateTime.Now.Year;

            book.InflationIndex.Credit(year, 1, "Initial Index");

            book.AnnualInvestmentChangeBeforeFirstDeath.Credit(year, options.AnnualInvestmentsChangeBeforeFirstDeath, Constants.Initial);
            book.AnnualInvestmentChangeAfterFirstDeath.Credit(year, options.AnnualInvestmentsChangeAfterFirstDeath, Constants.Initial);

            book.LifetimeGiftTaxExclusionSpouse1.Credit(year, Constants.FirstYearLifetimeGiftExclusionAmount, Constants.Initial);
            book.LifetimeGiftTaxExclusionSpouse2.Credit(year, Constants.FirstYearLifetimeGiftExclusionAmount, Constants.Initial);
            book.AnnualGiftTaxExclusion.Credit(year, Constants.FirstYearAnnualGiftExclusionAmount, Constants.Initial);

            book.Residence.Credit(year, options.HomeValue, Constants.Initial);
            book.Investments.Credit(year, options.AmountCurrentlyInvested, Constants.Initial);
            book.LifetimeTaxableGiftsSpouse1.Credit(year, options.Spouse1.LifetimeGiftExclusionAmountUsed, Constants.Initial);
            book.LifetimeTaxableGiftsSpouse2.Credit(year, options.Spouse2.LifetimeGiftExclusionAmountUsed, Constants.Initial);

            book.GiftingTrust.Credit(year, options.InitialGiftingTrustValue, Constants.Initial);
            book.Investments.Debit(year, options.InitialGiftingTrustValue, "Initial funding of gifting trust");

            if (options.InstallmentSaleSeedMoneyAmount > 0 || options.InstallmentSaleValueBeforeDiscount > 0)
            {
                book.Investments.Debit(year, options.InstallmentSaleSeedMoneyAmount, "Seed Money Gift to Installment Sale Trust");
                book.LifetimeTaxableGiftsSpouse1.Credit(year, options.InstallmentSaleSeedMoneyAmount, "Seed Money Gift to Installment Sale Trust");
                book.InstallmentSaleTrust.Credit(year, options.InstallmentSaleSeedMoneyAmount, "Seed Money Gift from Grantor");
                
                book.Investments.Debit(year, options.InstallmentSaleValueBeforeDiscount, "Sale of Assets to Installment Sale Trust");
                book.InstallmentSaleTrust.Credit(year, options.InstallmentSaleValueBeforeDiscount, "Purchase of Assets from Grantor");
                book.InstallmentSaleNote.Credit(year, options.InstallmentSaleNoteAmount, Constants.Initial);
            }

            if (options.FirstDyingSpouse.ProjectedYearOfDeath == year)
            {
                this.ProcessFirstDeath(book, options);
            }

            return book;
        }

        private void ProcessFirstDeath(EstateProjectionAccountBook book, EstateProjectionOptions options)
        {
            int year = options.FirstDyingSpouse.ProjectedYearOfDeath;

            Account lifetimeTaxableGifts =
                options.FirstDyingSpouse == options.Spouse1
                ? book.LifetimeTaxableGiftsSpouse1
                : book.LifetimeTaxableGiftsSpouse2;

            decimal bypassTrustValue = Math.Min(
                options.BypassTrustValue,
                ProjectionCalculator.GetMaximumBypassTrustValue(
                    options.FirstDyingSpouse.ProjectedYearOfDeath,
                    options.ConsumerPriceIndexGrowthRate,
                    lifetimeTaxableGifts.GetBalance(year),
                    options.AssumeExemptionReductionIn2026));

            bypassTrustValue = Math.Min(bypassTrustValue, book.Investments.GetBalance(year));

            if (bypassTrustValue < 0) bypassTrustValue = 0;

            book.Investments.Debit(year, bypassTrustValue, "Funding of Bypass Trust on First Death");
            book.BypassTrust.Credit(year, bypassTrustValue, "Funding of Bypass Trust on First Death");
            lifetimeTaxableGifts.Credit(year, bypassTrustValue, "Funding of Bypass Trust on First Death");

            if (options.AssumeNoPortability)
            {
                Account lifetimeGiftExclusion =
                    options.FirstDyingSpouse == options.Spouse1
                    ? book.LifetimeGiftTaxExclusionSpouse1
                    : book.LifetimeGiftTaxExclusionSpouse2;

                lifetimeGiftExclusion.Debit(year, lifetimeGiftExclusion.GetBalance(year), "Zero out exclusion for deceased spouse (assume no portability available)");
            }

            if (options.InstallmentSaleNoteType == InstallmentSaleNoteType.SelfCancelling &&
                book.InstallmentSaleNote.GetBalance(year) > 0)
            {
                book.InstallmentSaleNote.Debit(year, book.InstallmentSaleNote.GetBalance(year), "Note cancelled due to death before balloon payment");
            }
        }

        private IEnumerable<EstateProjection> CreateProjectionsFromAccounts(EstateProjectionAccountBook book, EstateProjectionOptions options)
        {
            List<EstateProjection> projections = new List<EstateProjection>();

            int startYear = book.GetMinYear();
            int endYear = book.GetMaxYear();

            for (int year = startYear; year <= endYear; year++)
            {
                EstateProjection projection = new EstateProjection();

                projection.Year = year;
                projection.YearNumber = year - startYear;

                projection.InflationIndex = book.InflationIndex.GetBalance(year);
                projection.AnnualGiftExclusionAmount = ProjectionCalculator.RoundDownToNearest(book.AnnualGiftTaxExclusion.GetBalance(year), 1000);
                projection.LifetimeGiftTaxExclusionSpouse1 = ProjectionCalculator.RoundDownToNearest(book.LifetimeGiftTaxExclusionSpouse1.GetBalance(year), 10000);
                projection.LifetimeGiftTaxExclusionSpouse2 = ProjectionCalculator.RoundDownToNearest(book.LifetimeGiftTaxExclusionSpouse2.GetBalance(year), 10000);

                projection.ResidenceValue = book.Residence.GetBalance(year);
                projection.InvestmentsValue = book.Investments.GetBalance(year);
                projection.BypassTrustValue = book.BypassTrust.GetBalance(year);
                projection.GiftingTrustValue = book.GiftingTrust.GetBalance(year);
                projection.InstallmentSaleTrustValue = book.InstallmentSaleTrust.GetBalance(year);
                projection.InstallmentSaleNoteValue = book.InstallmentSaleNote.GetBalance(year);

                if (options.Spouse1.LifeInsurance.IsInTrust)
                {
                    projection.LifeInsuranceOnFirstSpouseBenefitInTrust = book.LifeInsuranceSpouse1.GetBalance(year);
                }
                else
                {
                    projection.LifeInsuranceOnFirstSpouseBenefit = book.LifeInsuranceSpouse1.GetBalance(year);
                }

                if (options.Spouse2.LifeInsurance.IsInTrust)
                {
                    projection.LifeInsuranceOnSecondSpouseBenefitInTrust = book.LifeInsuranceSpouse2.GetBalance(year);
                }
                else
                {
                    projection.LifeInsuranceOnSecondSpouseBenefit = book.LifeInsuranceSpouse2.GetBalance(year);
                }

                if (options.SecondToDieLifeInsurance.IsInTrust)
                {
                    projection.LifeInsuranceOnSecondToDieBenefitInTrust = book.LifeInsuranceSurvivorship.GetBalance(year);
                }
                else
                {
                    projection.LifeInsuranceOnSecondToDieBenefit = book.LifeInsuranceSurvivorship.GetBalance(year);
                }

                projection.AnnualInvestmentsChangeBeforeFirstDeath = book.AnnualInvestmentChangeBeforeFirstDeath.GetBalance(year);
                projection.AnnualInvestmentsChangeAfterFirstDeath = book.AnnualInvestmentChangeAfterFirstDeath.GetBalance(year);

                projection.LifetimeTaxableGiftsSpouse1 = book.LifetimeTaxableGiftsSpouse1.GetBalance(year);
                projection.LifetimeTaxableGiftsSpouse2 = book.LifetimeTaxableGiftsSpouse2.GetBalance(year);

                if (projection.Year <= options.SecondDyingSpouse.ProjectedYearOfDeath)
                {
                    projection.EstateTaxDue = projection.TaxableValueOfEstate * options.EstateTaxRate;
                }

                if (options.UseConstantDollars)
                {
                    this.AdjustForInflation(projection);
                }

                projections.Add(projection);
            }

            return projections;
        }

        private void AdjustForInflation(EstateProjection projection)
        {
            projection.AnnualGiftExclusionAmount /= projection.InflationIndex;
            projection.AnnualInvestmentsChangeAfterFirstDeath /= projection.InflationIndex;
            projection.AnnualInvestmentsChangeBeforeFirstDeath /= projection.InflationIndex;
            projection.BypassTrustValue /= projection.InflationIndex;
            projection.InstallmentSaleNoteValue /= projection.InflationIndex;
            projection.InstallmentSaleTrustValue /= projection.InflationIndex;
            projection.EstateTaxDue /= projection.InflationIndex;
            projection.GiftingTrustValue /= projection.InflationIndex;
            projection.InvestmentsValue /= projection.InflationIndex;
            projection.LifeInsuranceOnFirstSpouseBenefit /= projection.InflationIndex;
            projection.LifeInsuranceOnFirstSpouseBenefitInTrust /= projection.InflationIndex;
            projection.LifeInsuranceOnSecondSpouseBenefit /= projection.InflationIndex;
            projection.LifeInsuranceOnSecondSpouseBenefitInTrust /= projection.InflationIndex;
            projection.LifeInsuranceOnSecondToDieBenefit /= projection.InflationIndex;
            projection.LifeInsuranceOnSecondToDieBenefitInTrust /= projection.InflationIndex;
            projection.LifeInsurancePremiumsOverGiftAmount /= projection.InflationIndex;
            projection.LifetimeGiftTaxExclusionSpouse1 /= projection.InflationIndex;
            projection.LifetimeGiftTaxExclusionSpouse2 /= projection.InflationIndex;
            projection.LifetimeTaxableGiftsSpouse1 /= projection.InflationIndex;
            projection.LifetimeTaxableGiftsSpouse2 /= projection.InflationIndex;
            projection.ResidenceValue /= projection.InflationIndex;
        }

        private static decimal RoundDownToNearest(decimal value, int roundDownTo)
        {
            return decimal.Floor(value / roundDownTo) * roundDownTo;
        }

        public static decimal GetProjectedLifetimeGiftExclusionAmount(int year, decimal consumerPriceIndex, bool assumeExemptionReductionIn2026)
        {
            if (year < DateTime.Now.Year)
            {
                year = DateTime.Now.Year;
            }

            decimal projectedLifetimeGiftExclusionAmount = Constants.FirstYearLifetimeGiftExclusionAmount;

            for (int i = DateTime.Now.Year; i < year; i++)
            {
                projectedLifetimeGiftExclusionAmount *= (1 + consumerPriceIndex);

                if (assumeExemptionReductionIn2026 && i == 2026)
                {
                    projectedLifetimeGiftExclusionAmount *= 0.5m;
                }
            }

            if (assumeExemptionReductionIn2026 && year == 2026)
            {
                projectedLifetimeGiftExclusionAmount *= 0.5m;
            }

            return ProjectionCalculator.RoundDownToNearest(projectedLifetimeGiftExclusionAmount, 10000);
        }

        public static decimal GetMaximumBypassTrustValue(int projectedYearOfDeath, decimal cpiGrowthRate, decimal lifetimeExclusionUsed, bool assumeExemptionReductionIn2026)
        {
            return Math.Max(0, ProjectionCalculator.GetProjectedLifetimeGiftExclusionAmount(projectedYearOfDeath, cpiGrowthRate, assumeExemptionReductionIn2026) - lifetimeExclusionUsed);
        }
    }
}
