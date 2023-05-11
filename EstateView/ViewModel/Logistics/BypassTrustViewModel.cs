namespace EstateView.ViewModel.Logistics
{
    public class BypassTrustViewModel
    {
        public string Title
        {
            get { return "Bypass Trust"; }
        }
        public decimal Value { get; set; }
        public float AnnualGrowthRate { get; set; }
        public float AnnualFeesRate { get; set; }
        public bool IsInTrust { get; set; }
    }
}