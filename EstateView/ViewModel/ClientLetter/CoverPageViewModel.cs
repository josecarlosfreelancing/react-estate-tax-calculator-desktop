using EstateView.Core.Model;
using EstateView.Properties;

namespace EstateView.ViewModel.ClientLetter
{
    public class CoverPageViewModel : ViewModel
    {
        public CoverPageViewModel(EstateProjectionOptions options)
        {
            this.PlannerName = Settings.Default.PlannerName; // options.PlannerName
            this.PlannerFirmName = Settings.Default.PlannerFirmName; // options.PlannerFirmName
            this.AnnualGiftExclusionAmount = ProjectionCalculator.Constants.FirstYearAnnualGiftExclusionAmount;
            this.Spouse1FirstName = options.Spouse1.FirstName;
            this.Spouse1LastName = options.Spouse1.LastName;
            this.Spouse2FirstName = options.Spouse2.FirstName;
        }

        public string PlannerName { get; set; }
        public string PlannerFirmName { get; set; }
        public decimal AnnualGiftExclusionAmount { get; set; }
        public string Spouse1FirstName { get; set; }
        public string Spouse1LastName { get; set; }
        public string Spouse2FirstName { get; set; }
    }

    public class CoverPageDesignerViewModel : CoverPageViewModel
    {
        public CoverPageDesignerViewModel()
            : base(EstateProjectionOptions.CreateEmptyOptions())
        {
        }
    }
}
