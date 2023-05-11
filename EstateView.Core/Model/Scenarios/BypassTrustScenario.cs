namespace EstateView.Core.Model.Scenarios
{
    public class BypassTrustScenario : NoPlanningScenario
    {
        public BypassTrustScenario(EstateProjectionOptions options, string name)
            : base(options, name)
        {
        }

        protected override EstateProjectionOptions GenerateOptions(EstateProjectionOptions options)
        {
            EstateProjectionOptions scenarioOptions = base.GenerateOptions(options);
            
            scenarioOptions.BypassTrustValue = options.BypassTrustValue;

            return scenarioOptions;
        }
    }
}