using EstateView.Core.Model;
using EstateView.Core.Model.Scenarios;

namespace EstateView.ViewModel.Logistics
{
    public class BoxesDesignerViewModel : BoxesViewModel
    {
        public BoxesDesignerViewModel()
            : base(new InstallmentSaleScenario(EstateProjectionOptions.CreateEmptyOptions(), "NEW SCENARIO"))
        {
        }
    }
}