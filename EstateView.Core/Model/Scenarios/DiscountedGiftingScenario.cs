namespace EstateView.Core.Model.Scenarios
{
    public class DiscountedGiftingScenario : AnnualGiftingScenario
    {
        public DiscountedGiftingScenario(EstateProjectionOptions options, string name)
            : base(options, name)
        {
        }

        protected override EstateProjectionOptions GenerateOptions(EstateProjectionOptions options)
        {
            EstateProjectionOptions scenarioOptions = base.GenerateOptions(options);

            scenarioOptions.PercentageOfInvestedGiftAmountDiscounted = options.PercentageOfInvestedGiftAmountDiscounted;
            scenarioOptions.DiscountPercentageForGifting = options.DiscountPercentageForGifting;

            return scenarioOptions;
        }
    }
}