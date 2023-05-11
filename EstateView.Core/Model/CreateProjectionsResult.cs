using System.Collections.Generic;

namespace EstateView.Core.Model
{
    public class CreateProjectionsResult
    {
        public CreateProjectionsResult(EstateProjectionAccountBook accounts, IEnumerable<EstateProjection> projections)
        {
            this.Accounts = accounts;
            this.Projections = new List<EstateProjection>(projections);
        }
        
        public List<EstateProjection> Projections { get; private set; }
        public EstateProjectionAccountBook Accounts { get; private set; }
    }
}