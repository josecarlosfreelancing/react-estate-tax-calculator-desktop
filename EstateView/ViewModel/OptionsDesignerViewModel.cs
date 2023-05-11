using EstateView.Core.Model;

namespace EstateView.ViewModel
{
    public class OptionsDesignerViewModel : OptionsViewModel
    {
        public OptionsDesignerViewModel()
            : base(EstateProjectionOptions.CreateEmptyOptions())
        {
        }
    }
}
