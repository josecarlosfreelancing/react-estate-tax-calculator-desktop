using System.Collections.Generic;

namespace EstateView.Core.Model.Scenarios
{
    public class EstatePlanningScenario
    {
        private readonly ProjectionCalculator calculator;

        public EstatePlanningScenario(EstateProjectionOptions options, string name)
        {
            this.calculator = new ProjectionCalculator();
            
            this.Name = name;
            this.OriginalOptions = options;
            this.UpdateProjections();
        }

        public string Name { get; set; }
        
        public IEnumerable<EstateProjection> Projections { get; protected set; }

        public EstateProjectionAccountBook Accounts { get; protected set; }

        public EstateProjectionOptions OriginalOptions { get; protected set; }

        public EstateProjectionOptions Options
        {
            get { return this.GenerateOptions(this.OriginalOptions); }
        }

        protected virtual EstateProjectionOptions GenerateOptions(EstateProjectionOptions options)
        {
            return options;
        }

        public void UpdateProjections()
        {
            CreateProjectionsResult result = calculator.CreateProjections(this.Options);
            this.Projections = result.Projections;
            this.Accounts = result.Accounts;
        }
    }
}