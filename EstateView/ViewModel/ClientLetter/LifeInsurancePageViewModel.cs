using System.Linq;
using EstateView.Core.Model;
using EstateView.Core.Model.Scenarios;

namespace EstateView.ViewModel.ClientLetter
{
    public class LifeInsurancePageViewModel : ViewModel
    {
        public LifeInsurancePageViewModel(EstatePlanningScenario previousScenario, LifeInsuranceScenario scenario)
        {

            this.EstateTaxSavingsFromLifeInsuranceTrust = previousScenario.Projections.Last().EstateTaxDue - scenario.Projections.Last().EstateTaxDue;

        }

        public decimal EstateTaxSavingsFromLifeInsuranceTrust { get; set; }
    }
}
