namespace EstateView.Core.Model.Scenarios
{
    public class NoPlanningScenario : EstatePlanningScenario
    {
        public NoPlanningScenario(EstateProjectionOptions options, string name)
            : base(options, name)
        {
        }

        protected override EstateProjectionOptions GenerateOptions(EstateProjectionOptions options)
        {
            EstateProjectionOptions scenarioOptions = new EstateProjectionOptions();

            ScenarioGenerationHelper.CopyBasicOptions(options, scenarioOptions);
            ScenarioGenerationHelper.CopyLifeInsuranceOptions(options.Spouse1.ExistingLifeInsurance, scenarioOptions.Spouse1.ExistingLifeInsurance);
            ScenarioGenerationHelper.CopyLifeInsuranceOptions(options.Spouse2.ExistingLifeInsurance, scenarioOptions.Spouse2.ExistingLifeInsurance);
            ScenarioGenerationHelper.CopyLifeInsuranceOptions(options.ExistingSecondToDieLifeInsurance, scenarioOptions.ExistingSecondToDieLifeInsurance);


            return scenarioOptions;
        }
    }
}
