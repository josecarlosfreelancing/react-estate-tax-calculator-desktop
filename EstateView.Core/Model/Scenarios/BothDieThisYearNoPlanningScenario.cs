using System;

namespace EstateView.Core.Model.Scenarios
{
    public class BothDieThisYearNoPlanningScenario : NoPlanningScenario
    {
        public BothDieThisYearNoPlanningScenario(EstateProjectionOptions options, string name)
            : base(options, name)
        {
        }

        protected override EstateProjectionOptions GenerateOptions(EstateProjectionOptions options)
        {
            EstateProjectionOptions scenarioOptions = base.GenerateOptions(options);

            scenarioOptions.AssumeNoPortability = false;
            scenarioOptions.Spouse1.ProjectedYearOfDeath = DateTime.Now.Year + 1;
            scenarioOptions.Spouse2.ProjectedYearOfDeath = DateTime.Now.Year + 1;

            return scenarioOptions;
        }
    }
}
