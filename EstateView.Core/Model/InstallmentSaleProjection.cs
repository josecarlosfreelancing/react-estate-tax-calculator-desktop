namespace EstateView.Core.Model
{
    public class InstallmentSaleProjection
    {
        public int Year { get; set; }
        public int YearNumber { get; set; }

        public decimal EstateStartingAssets { get; set; }
        public decimal EstateSpendingSavingAmount { get; set; }
        public decimal EstateAssetsGrowth { get; set; }
        public decimal EstateStartingAssetsWithoutNote { get; set; }
        public decimal NotePayment { get; set; }
        public decimal TrustIncomeTaxPaidByGrantor { get; set; }
        public decimal EstateBalance { get; set; }

        public decimal NoteBalance { get; set; }
        
        public decimal TaxableEstate { get; set; }
        public decimal LifetimeExclusionAvailable { get; set; }
        public decimal EstateTaxLiability { get; set; }
        public decimal EstateTaxSavingsOverNoPlanning { get; set; }

        public decimal TrustStartingAssets { get; set; }
        public decimal TrustAssetsGrowth { get; set; }
        public decimal TrustIncomeTaxPaidByTrust { get; set; }
        public decimal TrustBalance { get; set; }
        public decimal TrustBalanceUponDeath { get; set; }

        public string Notes { get; set; }
    }
}