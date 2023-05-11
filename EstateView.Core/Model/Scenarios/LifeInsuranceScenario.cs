namespace EstateView.Core.Model.Scenarios
{
    public class LifeInsuranceScenario : DiscountedGiftingScenario
    {
        public LifeInsuranceScenario(EstateProjectionOptions options, string name)
            : base(options, name)
        {
        }

        protected override EstateProjectionOptions GenerateOptions(EstateProjectionOptions options)
        {
            EstateProjectionOptions scenarioOptions = base.GenerateOptions(options);

            ScenarioGenerationHelper.CopyLifeInsuranceOptions(options.Spouse1.LifeInsurance, scenarioOptions.Spouse1.LifeInsurance);
            ScenarioGenerationHelper.CopyLifeInsuranceOptions(options.Spouse2.LifeInsurance, scenarioOptions.Spouse2.LifeInsurance);
            ScenarioGenerationHelper.CopyLifeInsuranceOptions(options.SecondToDieLifeInsurance, scenarioOptions.SecondToDieLifeInsurance);

            return scenarioOptions;
        }
    }
}