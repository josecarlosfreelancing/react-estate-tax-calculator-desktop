namespace EstateView.Core.Model.Scenarios
{
    public class InstallmentSaleScenario : LifeInsuranceScenario
    {
        public InstallmentSaleScenario(EstateProjectionOptions options, string name)
            : base(options, name)
        {
        }

        protected override EstateProjectionOptions GenerateOptions(EstateProjectionOptions options)
        {
            EstateProjectionOptions scenarioOptions = base.GenerateOptions(options);

            scenarioOptions.InstallmentSaleNoteAmount = options.InstallmentSaleNoteAmount;
            scenarioOptions.InstallmentSaleNoteDiscountRate = options.InstallmentSaleNoteDiscountRate;
            scenarioOptions.InstallmentSaleNoteInterestRate = options.InstallmentSaleNoteInterestRate;
            scenarioOptions.InstallmentSaleNoteTermInYears = options.InstallmentSaleNoteTermInYears;
            scenarioOptions.InstallmentSaleNoteType = options.InstallmentSaleNoteType;
            scenarioOptions.InstallmentSaleSeedMoneyAmount = options.InstallmentSaleSeedMoneyAmount;
            scenarioOptions.InstallmentSaleValueAfterDiscount = options.InstallmentSaleValueAfterDiscount;
            scenarioOptions.InstallmentSaleValueBeforeDiscount = options.InstallmentSaleValueBeforeDiscount;
            scenarioOptions.InstallmentSaleYearToToggleOffGrantorTrustStatus = options.InstallmentSaleYearToToggleOffGrantorTrustStatus;
            scenarioOptions.AnnualAdditionalIncomeForInstallmentSaleTrust = options.AnnualAdditionalIncomeForInstallmentSaleTrust;
            scenarioOptions.NumberOfYearsAdditionalIncomeForInstallmentSaleTrust = options.NumberOfYearsAdditionalIncomeForInstallmentSaleTrust;

            return scenarioOptions;
        }
    }
}
