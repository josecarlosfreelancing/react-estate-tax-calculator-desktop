using System;
using System.Collections.Generic;
using System.Linq;
using EstateView.Core.Model;
using EstateView.Core.Model.Scenarios;

namespace EstateView.ViewModel.Logistics
{
    public class BoxesViewModel : ViewModel
    {
        public BoxesViewModel(EstatePlanningScenario scenario)
        {
            this.Bind(scenario);
        }

        public EstatePlanningScenario CurrentScenario
        {
            get { return this.GetValue(() => this.CurrentScenario); }
            set { this.SetValue(() => this.CurrentScenario, value); }
        }

        public YearBoxesViewModel Today
        {
            get { return this.GetValue(() => this.Today); }
            set { this.SetValue(() => this.Today, value); }
        }

        public YearBoxesViewModel FirstDeath
        {
            get { return this.GetValue(() => this.FirstDeath); }
            set { this.SetValue(() => this.FirstDeath, value); }
        }

        public YearBoxesViewModel SecondDeath
        {
            get { return this.GetValue(() => this.SecondDeath); }
            set { this.SetValue(() => this.SecondDeath, value); }
        }

        public decimal TotalPassedToBeneficiaries
        {
            get { return this.GetValue(() => this.TotalPassedToBeneficiaries); }
            set { this.SetValue(() => this.TotalPassedToBeneficiaries, value); }
        }

        public void Bind(EstatePlanningScenario scenario)
        {
            this.CurrentScenario = scenario;

            EstateProjection currentScenarioProjection = this.CurrentScenario.Projections.SingleOrDefault(p => p.Year == this.CurrentScenario.Options.FirstDyingSpouse.ProjectedYearOfDeath);

            if (currentScenarioProjection != null)
            {
                this.Today = this.GenerateYearBoxes(
                    this.CurrentScenario.Projections.First(),
                    string.Format("{0} & {1} {2}", this.CurrentScenario.Options.Spouse1.FirstName, this.CurrentScenario.Options.Spouse2.FirstName, this.CurrentScenario.Options.Spouse1.LastName),
                    isLastYear: false);

                this.FirstDeath = this.GenerateYearBoxes(
                    currentScenarioProjection,
                    string.Format("{0} {1}", this.CurrentScenario.Options.SecondDyingSpouse.FirstName, this.CurrentScenario.Options.SecondDyingSpouse.LastName),
                    isLastYear: false);

                this.SecondDeath = this.GenerateYearBoxes(
                    this.CurrentScenario.Projections.Last(),
                    string.Format("{0}'s Estate", this.CurrentScenario.Options.SecondDyingSpouse.FirstName),
                    isLastYear: true);

                this.TotalPassedToBeneficiaries = this.CurrentScenario.Projections.Last().TotalAmountPassedToFamily;
            }
        }

        private YearBoxesViewModel GenerateYearBoxes(EstateProjection projection, string title, bool isLastYear)
        {
            YearBoxesViewModel viewModel = new YearBoxesViewModel();
            viewModel.DeltaYear = projection.Year - DateTime.Today.Year;
            bool isFirstYear = viewModel.DeltaYear == 0;

            viewModel.Estate = new EstateBoxViewModel();
            viewModel.Estate.Title = title;
            viewModel.Estate.Residence = projection.ResidenceValue;
            viewModel.Estate.Investments = projection.InvestmentsValue;
            viewModel.Estate.EstateTax = projection.EstateTaxDue;

            if (isFirstYear)
            {
                viewModel.Estate.InvestmentsAnnualChange = projection.AnnualInvestmentsChangeBeforeFirstDeath;
            }
            else
            {
                viewModel.Estate.InvestmentsAnnualChange = projection.AnnualInvestmentsChangeAfterFirstDeath;
            }

            if (!isLastYear)
            {
                viewModel.Estate.ResidenceAnnualGrowthRate = (float)this.CurrentScenario.Options.HomeValueGrowthRate;
                viewModel.Estate.InvestmentsAnnualGrowthRate = (float)this.CurrentScenario.Options.InvestmentsGrowthRate;
                viewModel.Estate.InvestmentsAnnualFeesRate = (float)this.CurrentScenario.Options.InvestmentFeesRate;
            }
            else
            {
                viewModel.Estate.Portability = -projection.TotalExclusionAvailable;
                viewModel.Estate.NetTaxableEstate = projection.TaxableValueOfEstate;
            }

            if (projection.BypassTrustValue > 0)
            {
                viewModel.BypassTrust = new BypassTrustViewModel();
                viewModel.BypassTrust.Value = projection.BypassTrustValue;
                if (!isLastYear)
                {
                    viewModel.BypassTrust.AnnualGrowthRate = (float)this.CurrentScenario.Options.InvestmentsGrowthRate;
                    viewModel.BypassTrust.AnnualFeesRate = (float)this.CurrentScenario.Options.InvestmentFeesRate;
                }
            }

            viewModel.GiftingTrusts = new List<GiftingTrustViewModel>();
            if (this.CurrentScenario.Projections.Any(p => p.GiftingTrustValue > 0))
            {
                viewModel.GiftingTrusts.Add(this.GenerateGiftingTrustBox(projection, isFirstYear, isLastYear));
            }

            viewModel.InstallmentSaleTrusts = new List<InstallmentSaleTrustViewModel>();
            if (this.CurrentScenario.Projections.Any(p => p.InstallmentSaleTrustValue > 0))
            {
                viewModel.InstallmentSaleTrusts.Add(this.GenerateInstallmentSaleTrustBox(projection));
            }
            
            viewModel.Ilits = new List<LifeInsuranceViewModel>();
            if (!this.CurrentScenario.Options.Spouse1.LifeInsurance.IsEmpty())
            {
                viewModel.Ilits.Add(
                    this.GenerateIlitBox(
                        this.CurrentScenario.Options.Spouse1.FirstName,
                        projection.LifeInsuranceOnFirstSpouseBenefit + projection.LifeInsuranceOnFirstSpouseBenefitInTrust > 0 ? projection.LifeInsuranceOnFirstSpouseBenefit + projection.LifeInsuranceOnFirstSpouseBenefitInTrust : this.CurrentScenario.Options.Spouse1.LifeInsurance.GetDeathBenefitForYear(projection.YearNumber),
                        isFirstYear || this.CurrentScenario.Options.Spouse1.Name == this.CurrentScenario.Options.SecondDyingSpouse.Name ? this.CurrentScenario.Options.Spouse1.LifeInsurance.GetPremiumForYear(projection.YearNumber) : 0,
                        isLastYear,
                        this.CurrentScenario.Options.Spouse1.LifeInsurance.IsInTrust));
            }

            if (!this.CurrentScenario.Options.Spouse2.LifeInsurance.IsEmpty())
            {
                viewModel.Ilits.Add(
                    this.GenerateIlitBox(
                        this.CurrentScenario.Options.Spouse2.FirstName,
                        projection.LifeInsuranceOnSecondSpouseBenefit + projection.LifeInsuranceOnSecondSpouseBenefitInTrust > 0 ? projection.LifeInsuranceOnSecondSpouseBenefit + projection.LifeInsuranceOnSecondSpouseBenefitInTrust : this.CurrentScenario.Options.Spouse2.LifeInsurance.GetDeathBenefitForYear(projection.YearNumber),
                        isFirstYear || this.CurrentScenario.Options.Spouse2.Name == this.CurrentScenario.Options.SecondDyingSpouse.Name ? this.CurrentScenario.Options.Spouse2.LifeInsurance.GetPremiumForYear(projection.YearNumber) : 0,
                        isLastYear,
                        this.CurrentScenario.Options.Spouse2.LifeInsurance.IsInTrust)); 
            }

            if (!this.CurrentScenario.Options.SecondToDieLifeInsurance.IsEmpty())
            {
                viewModel.Ilits.Add(
                    this.GenerateIlitBox(
                        "Survivorship",
                        this.CurrentScenario.Options.SecondToDieLifeInsurance.GetDeathBenefitForYear(projection.YearNumber),
                        this.CurrentScenario.Options.SecondToDieLifeInsurance.GetPremiumForYear(projection.YearNumber),
                        isLastYear,
                        this.CurrentScenario.Options.SecondToDieLifeInsurance.IsInTrust));
            }

            return viewModel;
        }

        private InstallmentSaleTrustViewModel GenerateInstallmentSaleTrustBox(EstateProjection projection)
        {
            InstallmentSaleTrustViewModel viewModel = new InstallmentSaleTrustViewModel();
            viewModel.Title = "Year 1 Gift / Installment Sale Trust";
            viewModel.GrossValue = projection.InstallmentSaleTrustValue;
            viewModel.NoteValue = projection.InstallmentSaleNoteValue;
            viewModel.Value = viewModel.GrossValue - viewModel.NoteValue;

            return viewModel;
        }

        private GiftingTrustViewModel GenerateGiftingTrustBox(EstateProjection projection, bool isFirstYear, bool isLastYear)
        {
            GiftingTrustViewModel giftingTrustViewModel = new GiftingTrustViewModel();
            giftingTrustViewModel.Title = "Gifting Trust(s)";
            giftingTrustViewModel.Value = projection.GiftingTrustValue;

            if (!isLastYear)
            {
                int numberOfAliveSpouses = isFirstYear ? 2 : 1;
                giftingTrustViewModel.AnnualGifts = projection.AnnualGiftExclusionAmount * this.CurrentScenario.Options.NumberOfAnnualGiftsPerYear * numberOfAliveSpouses;
                giftingTrustViewModel.AnnualFeesRate = (float)this.CurrentScenario.Options.InvestmentFeesRate;
                giftingTrustViewModel.AnnualGrowthRate = (float)this.CurrentScenario.Options.InvestmentsGrowthRate;
            }

            return giftingTrustViewModel;
        }

        private LifeInsuranceViewModel GenerateIlitBox(string name, decimal value, decimal annualPremium, bool isLastYear, bool isInTrust)
        {
            LifeInsuranceViewModel lifeInsuranceViewModel = new LifeInsuranceViewModel();
            lifeInsuranceViewModel.Title = string.Format("Life Ins. - {0}", name);
            lifeInsuranceViewModel.Value = value;

            if (!isLastYear)
            {
                if (annualPremium > 0)
                {
                    lifeInsuranceViewModel.AnnualPremium = annualPremium;
                }
                else
                {
                    lifeInsuranceViewModel.AnnualFeesRate = (float)this.CurrentScenario.Options.InvestmentFeesRate;
                    lifeInsuranceViewModel.AnnualGrowthRate = (float)this.CurrentScenario.Options.InvestmentsGrowthRate;
                }
            }

            lifeInsuranceViewModel.IsInTrust = isInTrust;

            return lifeInsuranceViewModel;
        }
    }

    public class InstallmentSaleTrustViewModel
    {
        public string Title { get; set; }
        public decimal Value { get; set; }
        public decimal GrossValue { get; set; }
        public decimal NoteValue { get; set; }
    }
}
