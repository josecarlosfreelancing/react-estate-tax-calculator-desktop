namespace EstateView.Core.Model
{
    public class InstallmentSaleOptions
    {
        public decimal PersonalAssetsAmount { get; set; }
        public decimal EstateSpendingSavingAmount { get; set; }
        public decimal AssetGrowthRate { get; set; }

        public decimal AssetValueBeforeDiscount { get; set; }

        public decimal AssetValueAfterDiscount { get; set; }

        public decimal NoteAmount { get; set; }
        public decimal NoteInterestRate { get; set; }
        public int NoteNumberOfYears { get; set; }
        public InstallmentSaleNoteType NoteType { get; set; }

        public decimal SeedCapitalAmount { get; set; }
        public decimal LifetimeExclusionUsed { get; set; }
        public decimal DiscountRate { get; set; }
        public decimal IncomeTaxRate { get; set; }
        public decimal EstateTaxRate { get; set; }
        public decimal ConsumerPriceIndexGrowthRate { get; set; }

        public int NumberOfYearsToProject { get; set; }

        public int YearToToggleOffGrantorTrustStatus { get; set; }

        public static InstallmentSaleOptions CreateSampleOptions()
        {
            return
                new InstallmentSaleOptions
                    {
                        PersonalAssetsAmount = 25e6M,
                        EstateSpendingSavingAmount = -800000,
                        AssetGrowthRate = .08M,
                        LifetimeExclusionUsed = 0.25e6M,
                        DiscountRate = 0.35M,
                        AssetValueAfterDiscount = 4.4e6M,
                        AssetValueBeforeDiscount = (4.4e6M / (1 - 0.35M)),
                        NoteAmount = 4.4e6M,
                        NoteInterestRate = .0232M,
                        NoteNumberOfYears = 10,
                        NumberOfYearsToProject = 15,
                        SeedCapitalAmount = 0.6e6M,
                        IncomeTaxRate = 0.02M,
                        EstateTaxRate = 0.40M,
                        ConsumerPriceIndexGrowthRate = 0.0296M,
                        YearToToggleOffGrantorTrustStatus = -1,
                        NoteType = InstallmentSaleNoteType.SelfCancelling,
                    };
        }
    }
}