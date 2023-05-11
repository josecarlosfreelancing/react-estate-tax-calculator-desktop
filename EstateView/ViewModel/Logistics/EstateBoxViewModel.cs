namespace EstateView.ViewModel.Logistics
{
    public class EstateBoxViewModel : ViewModel
    {
        public EstateBoxViewModel()
        {
            this.EstateTaxPercent = .40f;
        }

        public string Title { get; set; }
        public decimal Residence { get; set; }
        public decimal Investments { get; set; }
        public float ResidenceAnnualGrowthRate { get; set; }
        public decimal InvestmentsAnnualChange { get; set; }
        public float InvestmentsAnnualGrowthRate { get; set; }
        public float InvestmentsAnnualFeesRate { get; set; }
        public decimal Portability { get; set; }
        public decimal NetTaxableEstate { get; set; }
        public float EstateTaxPercent { get; set; }
        public decimal EstateTax { get; set; }
    }
}