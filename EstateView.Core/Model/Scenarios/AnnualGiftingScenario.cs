namespace EstateView.Core.Model.Scenarios
{
    public class AnnualGiftingScenario : BypassTrustScenario
    {
        public AnnualGiftingScenario(EstateProjectionOptions options, string name)
            : base(options, name)
        {
        }

        protected override EstateProjectionOptions GenerateOptions(EstateProjectionOptions options)
        {
            EstateProjectionOptions scenarioOptions = base.GenerateOptions(options);

            scenarioOptions.InitialGiftingTrustValue = options.InitialGiftingTrustValue;
            scenarioOptions.NumberOfAnnualGiftsPerYear = options.NumberOfAnnualGiftsPerYear;
            scenarioOptions.PercentageOfAvailableGiftToPermissibleGift = options.PercentageOfAvailableGiftToPermissibleGift;

            return scenarioOptions;
        }
    }
}