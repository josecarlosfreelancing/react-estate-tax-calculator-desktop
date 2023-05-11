namespace EstateView.Core.Model
{
    public class EstateProjectionAccountBook : AccountBook
    {
        public Account Residence { get; private set; }
        public Account Investments { get; private set; }
        public Account BypassTrust { get; private set; }
        public Account GiftingTrust { get; private set; }
        public Account LifeInsuranceSpouse1 { get; private set; }
        public Account LifeInsuranceSpouse2 { get; private set; }
        public Account LifeInsuranceSurvivorship { get; private set; }
        public Account InstallmentSaleTrust { get; private set; }
        public Account InstallmentSaleNote { get; private set; }

        public Account LifetimeTaxableGiftsSpouse1 { get; private set; }
        public Account LifetimeTaxableGiftsSpouse2 { get; private set; }

        public Account LifetimeGiftTaxExclusionSpouse1 { get; private set; }
        public Account LifetimeGiftTaxExclusionSpouse2 { get; private set; }
        
        public Account AnnualGiftTaxExclusion { get; private set; }

        public Account AnnualInvestmentChangeBeforeFirstDeath { get; private set; }
        public Account AnnualInvestmentChangeAfterFirstDeath { get; private set; }

        public Account TaxableLifeInsurancePremiums { get; private set; }

        public Account InflationIndex { get; private set; }

        public EstateProjectionAccountBook()
        {
            this.Residence = this.RegisterAccount("Residence and Personal Property");
            this.Investments = this.RegisterAccount("Business and Investment Assets");
            this.BypassTrust = this.RegisterAccount("Bypass Trust");
            this.GiftingTrust = this.RegisterAccount("Gifting Trust");

            this.LifeInsuranceSpouse1 = this.RegisterAccount("Life Insurance Trust (1st Spouse)");
            this.LifeInsuranceSpouse2 = this.RegisterAccount("Life Insurance Trust (2nd Spouse)");
            this.LifeInsuranceSurvivorship = this.RegisterAccount("Life Insurance Trust (Survivorship)");

            this.InstallmentSaleTrust = this.RegisterAccount("Intentionally Defective Grantor Trust");
            this.InstallmentSaleNote = this.RegisterAccount("Balance of Note from IDGT");

            this.LifetimeTaxableGiftsSpouse1 = this.RegisterAccount("Lifetime Taxable Gifts (1st Spouse)");
            this.LifetimeTaxableGiftsSpouse2 = this.RegisterAccount("Lifetime Taxable Gifts (2nd Spouse)");

            this.LifetimeGiftTaxExclusionSpouse1 = this.RegisterAccount("Lifetime Gift Tax Exclusion (1st Spouse)");
            this.LifetimeGiftTaxExclusionSpouse2 = this.RegisterAccount("Lifetime Gift Tax Exclusion (2nd Spouse)");
            
            this.AnnualGiftTaxExclusion = this.RegisterAccount("Annual Gift Tax Exclusion");

            this.AnnualInvestmentChangeBeforeFirstDeath = this.RegisterAccount("Annual Investment Change Before First Death");
            this.AnnualInvestmentChangeAfterFirstDeath = this.RegisterAccount("Annual Investment Change After First Death");

            this.TaxableLifeInsurancePremiums = this.RegisterAccount("Taxable Life Insurance Premiums");
            this.InflationIndex = this.RegisterAccount("Inflation Index");
        }
    }
}
