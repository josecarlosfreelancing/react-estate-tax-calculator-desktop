using System;
using System.Collections.Generic;
using System.Linq;

namespace EstateView.Core.Model
{
    public class InstallmentSaleCalculator
    {
        public IEnumerable<InstallmentSaleProjection> Calculate(InstallmentSaleOptions options)
        {
            InstallmentSaleOptions noPlanningOptions = this.CreateNoPlanningOptions(options);
            List<InstallmentSaleProjection> projections = this.CalculateInternal(options).ToList();
            List<InstallmentSaleProjection> noPlanningProjections = this.CalculateInternal(noPlanningOptions).ToList();

            for (int i = 0; i < projections.Count; i++)
            {
                projections[i].EstateTaxSavingsOverNoPlanning =
                    noPlanningProjections[i].EstateTaxLiability -
                    projections[i].EstateTaxLiability;
            }

            return projections;
        }

        private InstallmentSaleOptions CreateNoPlanningOptions(InstallmentSaleOptions options)
        {
            return new InstallmentSaleOptions
            {
                PersonalAssetsAmount = options.PersonalAssetsAmount,
                EstateSpendingSavingAmount = options.EstateSpendingSavingAmount,
                AssetGrowthRate = options.AssetGrowthRate,
                LifetimeExclusionUsed = options.LifetimeExclusionUsed,
                IncomeTaxRate = options.IncomeTaxRate,
                EstateTaxRate = options.EstateTaxRate,
                ConsumerPriceIndexGrowthRate = options.ConsumerPriceIndexGrowthRate,
                NumberOfYearsToProject = options.NumberOfYearsToProject
            };
        }

        private IEnumerable<InstallmentSaleProjection> CalculateInternal(InstallmentSaleOptions options)
        {
            List<InstallmentSaleProjection> projections = new List<InstallmentSaleProjection>();
            projections.AddRange(this.CalculateInitialProjections(options));

            InstallmentSaleProjection last = projections.Last();

            for (int year = 1; year <= options.NumberOfYearsToProject; year++)
            {
                InstallmentSaleProjection projection = new InstallmentSaleProjection();

                projection.Year = DateTime.Today.Year + year;
                projection.YearNumber = year;

                if (year <= options.NoteNumberOfYears)
                {
                    projection.NotePayment = options.NoteAmount * options.NoteInterestRate;
                }
                else
                {
                    projection.NotePayment = 0;
                }

                projection.EstateStartingAssets = last.EstateBalance;
                projection.EstateStartingAssetsWithoutNote = projection.EstateStartingAssets - last.NoteBalance;

                if (year == options.NoteNumberOfYears)
                {
                    projection.NotePayment += options.NoteAmount;
                    projection.NoteBalance = 0;
                }
                else
                {
                    projection.NoteBalance = last.NoteBalance;
                }


                projection.EstateAssetsGrowth = projection.EstateStartingAssetsWithoutNote * (options.AssetGrowthRate - options.IncomeTaxRate);
                projection.EstateSpendingSavingAmount = options.EstateSpendingSavingAmount * (decimal)Math.Pow((double)(1 + options.ConsumerPriceIndexGrowthRate), projection.YearNumber - 1);

                projection.TrustStartingAssets = last.TrustBalance + last.NoteBalance;
                projection.TrustAssetsGrowth = projection.TrustStartingAssets * options.AssetGrowthRate;
                
                decimal trustIncomeTax = projection.TrustStartingAssets * options.IncomeTaxRate;
                if (options.YearToToggleOffGrantorTrustStatus > -1 &&
                    year > options.YearToToggleOffGrantorTrustStatus)
                {
                    projection.TrustIncomeTaxPaidByTrust = trustIncomeTax;
                }
                else
                {
                    projection.TrustIncomeTaxPaidByGrantor = trustIncomeTax;
                }

                projection.TrustBalance =
                    projection.TrustStartingAssets +
                    projection.TrustAssetsGrowth -
                    projection.TrustIncomeTaxPaidByTrust -
                    projection.NoteBalance -
                    projection.NotePayment;

                if (options.NoteType == InstallmentSaleNoteType.Conventional)
                {
                    projection.TrustBalanceUponDeath = projection.TrustBalance;
                }
                else if (options.NoteType == InstallmentSaleNoteType.SelfCancelling)
                {
                    projection.TrustBalanceUponDeath = projection.TrustBalance + projection.NoteBalance;
                }

                projection.LifetimeExclusionAvailable =
                    ProjectionCalculator.GetProjectedLifetimeGiftExclusionAmount(
                        projection.Year,
                        options.ConsumerPriceIndexGrowthRate,
                        assumeExemptionReductionIn2026: true) -
                    options.LifetimeExclusionUsed -
                    options.SeedCapitalAmount;

                projection.EstateBalance =
                    projection.EstateStartingAssets +
                    projection.EstateAssetsGrowth +
                    projection.EstateSpendingSavingAmount +
                    projection.NotePayment -
                    projection.TrustIncomeTaxPaidByGrantor;

                if (year == options.NoteNumberOfYears)
                {
                    projection.EstateBalance -= options.NoteAmount;
                }
                
                projection.TaxableEstate = projection.EstateBalance;

                if (options.NoteType == InstallmentSaleNoteType.SelfCancelling)
                {
                    projection.TaxableEstate -= projection.NoteBalance;
                }

                projection.EstateTaxLiability =
                    (projection.TaxableEstate - projection.LifetimeExclusionAvailable) *
                    options.EstateTaxRate;

                projection.EstateTaxLiability = Math.Max(0, projection.EstateTaxLiability);

                projections.Add(projection);
                last = projection;
            }

            return projections;
        }

        private IEnumerable<InstallmentSaleProjection> CalculateInitialProjections(InstallmentSaleOptions options)
        {
            InstallmentSaleProjection initialProjection = new InstallmentSaleProjection();
            initialProjection.Notes = "Initial Values";
            initialProjection.Year = DateTime.Today.Year;
            initialProjection.EstateStartingAssets = options.PersonalAssetsAmount;
            initialProjection.EstateBalance = initialProjection.EstateStartingAssets;
            initialProjection.TaxableEstate = initialProjection.EstateBalance;
            initialProjection.LifetimeExclusionAvailable = ProjectionCalculator.GetProjectedLifetimeGiftExclusionAmount(initialProjection.Year, options.ConsumerPriceIndexGrowthRate, assumeExemptionReductionIn2026: true) - options.LifetimeExclusionUsed;
            initialProjection.EstateTaxLiability = (initialProjection.TaxableEstate - initialProjection.LifetimeExclusionAvailable) * options.EstateTaxRate;
            initialProjection.EstateTaxLiability = Math.Max(0, initialProjection.EstateTaxLiability);

            InstallmentSaleProjection seedCapitalProjection = new InstallmentSaleProjection();
            seedCapitalProjection.Notes = "Gift of Seed Capital to Trust";
            seedCapitalProjection.Year = initialProjection.Year;
            seedCapitalProjection.EstateStartingAssets = initialProjection.EstateStartingAssets - options.SeedCapitalAmount;
            seedCapitalProjection.EstateBalance = seedCapitalProjection.EstateStartingAssets;
            seedCapitalProjection.TaxableEstate = seedCapitalProjection.EstateBalance;
            seedCapitalProjection.LifetimeExclusionAvailable = initialProjection.LifetimeExclusionAvailable - options.SeedCapitalAmount;
            seedCapitalProjection.EstateTaxLiability = (seedCapitalProjection.TaxableEstate - seedCapitalProjection.LifetimeExclusionAvailable) * options.EstateTaxRate;
            seedCapitalProjection.EstateTaxLiability = Math.Max(0, seedCapitalProjection.EstateTaxLiability);
            seedCapitalProjection.TrustStartingAssets = options.SeedCapitalAmount;

            InstallmentSaleProjection saleToTrustProjection = new InstallmentSaleProjection();
            saleToTrustProjection.Notes = "Sale of Assets to Trust";
            saleToTrustProjection.Year = seedCapitalProjection.Year;
            saleToTrustProjection.NoteBalance = options.NoteAmount;
            saleToTrustProjection.TrustStartingAssets = seedCapitalProjection.TrustStartingAssets + options.AssetValueBeforeDiscount;
            saleToTrustProjection.TrustBalance = saleToTrustProjection.TrustStartingAssets - saleToTrustProjection.NoteBalance;
            saleToTrustProjection.EstateStartingAssets = seedCapitalProjection.EstateStartingAssets - options.AssetValueBeforeDiscount + options.NoteAmount;
            saleToTrustProjection.EstateStartingAssetsWithoutNote = saleToTrustProjection.EstateStartingAssets - options.NoteAmount;
            saleToTrustProjection.EstateBalance = saleToTrustProjection.EstateStartingAssets;

            if (options.NoteType == InstallmentSaleNoteType.Conventional)
            {
                saleToTrustProjection.TrustBalanceUponDeath = saleToTrustProjection.TrustBalance;
                saleToTrustProjection.TaxableEstate = saleToTrustProjection.EstateStartingAssets;
            }
            else if (options.NoteType == InstallmentSaleNoteType.SelfCancelling)
            {
                saleToTrustProjection.TrustBalanceUponDeath = saleToTrustProjection.TrustBalance + saleToTrustProjection.NoteBalance;
                saleToTrustProjection.TaxableEstate = saleToTrustProjection.EstateStartingAssetsWithoutNote;
            }

            saleToTrustProjection.LifetimeExclusionAvailable = seedCapitalProjection.LifetimeExclusionAvailable;
            saleToTrustProjection.EstateTaxLiability = (saleToTrustProjection.TaxableEstate - saleToTrustProjection.LifetimeExclusionAvailable) * options.EstateTaxRate;
            saleToTrustProjection.EstateTaxLiability = Math.Max(0, saleToTrustProjection.EstateTaxLiability);

            return new[] { initialProjection, seedCapitalProjection, saleToTrustProjection };
        }
    }
}
