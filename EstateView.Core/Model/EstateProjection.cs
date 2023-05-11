using System;

namespace EstateView.Core.Model
{
    public class EstateProjection
    {
        public int Year { get; set; }
        public int YearNumber { get; set; }

        public decimal ResidenceValue { get; set; }
        public decimal InvestmentsValue { get; set; }
        public decimal BypassTrustValue { get; set; }
        public decimal InstallmentSaleTrustValue { get; set; }
        public decimal InstallmentSaleNoteValue { get; set; }

        public decimal TotalCombinedAssets
        {
            get
            {
                return
                    this.ResidenceValue +
                    this.InvestmentsValue +
                    this.BypassTrustValue +
                    this.GiftingTrustValue +
                    this.InstallmentSaleTrustValue;
            }
        }

        public decimal EffectiveEstateTaxRate
        {
            get
            {
                if (this.TotalCombinedAssets != 0)
                {
                    return this.EstateTaxDue / this.TotalCombinedAssets;
                }
                else
                {
                    return 0;
                }
            }
        }

        public decimal AnnualInvestmentsChangeBeforeFirstDeath { get; set; }
        public decimal AnnualInvestmentsChangeAfterFirstDeath { get; set; }

        public decimal LifetimeGiftTaxExclusionSpouse1 { get; set; }
        public decimal LifetimeTaxableGiftsSpouse1 { get; set; }
        public decimal LifetimeGiftTaxExclusionAvailableSpouse1
        {
            get { return Math.Max(0, this.LifetimeGiftTaxExclusionSpouse1 - this.LifetimeTaxableGiftsSpouse1); }
        }

        public decimal LifetimeGiftTaxExclusionSpouse2 { get; set; }
        public decimal LifetimeTaxableGiftsSpouse2 { get; set; }
        public decimal LifetimeGiftTaxExclusionAvailableSpouse2
        {
            get { return Math.Max(0, this.LifetimeGiftTaxExclusionSpouse2 - this.LifetimeTaxableGiftsSpouse2); }
        }

        public decimal TotalExclusionAvailable
        {
            get { return this.LifetimeGiftTaxExclusionAvailableSpouse1 + this.LifetimeGiftTaxExclusionAvailableSpouse2; }
        }

        public decimal LifeInsurancePremiumsOverGiftAmount { get; set; }
        public decimal AnnualGiftExclusionAmount { get; set; }
        public decimal GiftingTrustValue { get; set; }

        public decimal AmountPassingOutsideOfSurvivingSpousesEstate
        {
            get
            {
                return
                    this.BypassTrustValue +
                    this.GiftingTrustValue +
                    this.LifeInsuranceOnFirstSpouseBenefitInTrust +
                    this.LifeInsuranceOnSecondSpouseBenefitInTrust +
                    this.LifeInsuranceOnSecondToDieBenefitInTrust +
                    (this.InstallmentSaleTrustValue - this.InstallmentSaleNoteValue);
            }
        }

        public decimal SurvivingSpousesGrossEstate
        {
            get
            {
                return
                    this.ResidenceValue +
                    this.InvestmentsValue +
                    this.InstallmentSaleNoteValue +
                    this.LifeInsuranceOnFirstSpouseBenefit +
                    this.LifeInsuranceOnSecondSpouseBenefit +
                    this.LifeInsuranceOnSecondToDieBenefit;
            }
        }

        public decimal TaxableValueOfEstate
        {
            get { return Math.Max(0, this.SurvivingSpousesGrossEstate - this.TotalExclusionAvailable); }
        }

        public decimal EstateTaxDue { get; set; }

        public decimal LifeInsuranceOnFirstSpouseBenefit { get; set; }
        public decimal LifeInsuranceOnFirstSpouseBenefitInTrust { get; set; }

        public decimal LifeInsuranceOnSecondSpouseBenefit { get; set; }
        public decimal LifeInsuranceOnSecondSpouseBenefitInTrust { get; set; }

        public decimal LifeInsuranceOnSecondToDieBenefit { get; set; }
        public decimal LifeInsuranceOnSecondToDieBenefitInTrust { get; set; }

        public decimal TotalAmountPassedToFamily
        {
            get
            {
                return
                    this.AmountPassingOutsideOfSurvivingSpousesEstate +
                    this.SurvivingSpousesGrossEstate -
                    this.EstateTaxDue;
            }
        }

        public decimal InflationIndex { get; set; }
    }
}