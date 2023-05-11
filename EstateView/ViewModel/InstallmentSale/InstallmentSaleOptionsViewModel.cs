using EstateView.Core.Model;

namespace EstateView.ViewModel.InstallmentSale
{
    public class InstallmentSaleOptionsViewModel : ViewModel
    {
        private readonly InstallmentSaleOptions options;
        private bool isAssetValueChanging;

        public InstallmentSaleOptionsViewModel(InstallmentSaleOptions options)
        {
            this.options = options;
            this.NoteTypes = new[]
            {
                new ValueObject(InstallmentSaleNoteType.Conventional, "Conventional"),
                new ValueObject(InstallmentSaleNoteType.SelfCancelling, "Self-Cancelling"),
            };
        }

        public InstallmentSaleOptions Options
        {
            get { return this.options; }
        }

        public ValueObject[] NoteTypes { get; private set; }

        public decimal PersonalAssetsAmount
        {
            get
            {
                return this.options.PersonalAssetsAmount;
            }
            set
            {
                if (this.options.PersonalAssetsAmount == value) return;
                this.options.PersonalAssetsAmount = value;
                this.NotifyPropertyChanged(() => this.PersonalAssetsAmount);
            }
        }

        public decimal EstateSpendingSavingAmount
        {
            get
            {
                return this.options.EstateSpendingSavingAmount;
            }
            set
            {
                if (this.options.EstateSpendingSavingAmount == value) return;
                this.options.EstateSpendingSavingAmount = value;
                this.NotifyPropertyChanged(() => this.EstateSpendingSavingAmount);
            }
        }

        public decimal AssetGrowthRate
        {
            get
            {
                return this.options.AssetGrowthRate;
            }
            set
            {
                if (this.options.AssetGrowthRate == value) return;
                this.options.AssetGrowthRate = value;

                if (this.IncomeTaxRate > value)
                {
                    this.IncomeTaxRate = value;
                }

                this.NotifyPropertyChanged(() => this.AssetGrowthRate);
            }
        }

        public decimal AssetValueAfterDiscount
        {
            get
            {
                return this.options.AssetValueAfterDiscount;
            }
            set
            {
                decimal previousValue = this.options.AssetValueAfterDiscount;
                this.options.AssetValueAfterDiscount = value;

                if (!this.isAssetValueChanging)
                {
                    this.isAssetValueChanging = true;
                    this.AssetValueBeforeDiscount = value / (1 - this.DiscountRate);
                    this.isAssetValueChanging = false;
                }

                this.NotifyPropertyChanged(() => this.AssetValueAfterDiscount);

                if (this.NoteAmount == previousValue || this.NoteAmount < value)
                {
                    this.NoteAmount = value;
                }
            }
        }

        public decimal AssetValueBeforeDiscount
        {
            get
            {
                return this.options.AssetValueBeforeDiscount;
            }
            set
            {
                if (this.options.AssetValueBeforeDiscount == value) return;
                this.options.AssetValueBeforeDiscount = value;

                if (!this.isAssetValueChanging)
                {
                    this.isAssetValueChanging = true;
                    this.AssetValueAfterDiscount = value * (1 - this.DiscountRate);
                    this.isAssetValueChanging = false;
                }

                this.NotifyPropertyChanged(() => this.AssetValueBeforeDiscount);
            }
        }

        public decimal NoteAmount
        {
            get
            {
                return this.options.NoteAmount;
            }
            set
            {
                if (this.options.NoteAmount == value) return;
                this.options.NoteAmount = value;
                this.NotifyPropertyChanged(() => this.NoteAmount);
            }
        }

        public decimal NoteInterestRate
        {
            get
            {
                return this.options.NoteInterestRate;
            }
            set
            {
                if (this.options.NoteInterestRate == value) return;
                this.options.NoteInterestRate = value;
                this.NotifyPropertyChanged(() => this.NoteInterestRate);
            }
        }

        public int NoteNumberOfYears
        {
            get
            {
                return this.options.NoteNumberOfYears;
            }
            set
            {
                if (this.options.NoteNumberOfYears == value) return;
                this.options.NoteNumberOfYears = value;
                this.NotifyPropertyChanged(() => this.NoteNumberOfYears);
            }
        }

        public decimal SeedCapitalAmount
        {
            get
            {
                return this.options.SeedCapitalAmount;
            }
            set
            {
                if (this.options.SeedCapitalAmount == value) return;
                this.options.SeedCapitalAmount = value;
                this.NotifyPropertyChanged(() => this.SeedCapitalAmount);
            }
        }

        public decimal LifetimeExclusionUsed
        {
            get
            {
                return this.options.LifetimeExclusionUsed;
            }
            set
            {
                if (this.options.LifetimeExclusionUsed == value) return;
                this.options.LifetimeExclusionUsed = value;
                this.NotifyPropertyChanged(() => this.LifetimeExclusionUsed);
            }
        }

        public decimal DiscountRate
        {
            get
            {
                return this.options.DiscountRate;
            }
            set
            {
                if (this.options.DiscountRate == value) return;
                this.options.DiscountRate = value;

                this.AssetValueAfterDiscount = this.AssetValueAfterDiscount;

                this.NotifyPropertyChanged(() => this.DiscountRate);
            }
        }

        public decimal IncomeTaxRate
        {
            get
            {
                return this.options.IncomeTaxRate;
            }
            set
            {
                if (this.options.IncomeTaxRate == value) return;
                this.options.IncomeTaxRate = value;
                this.NotifyPropertyChanged(() => this.IncomeTaxRate);
            }
        }

        public decimal EstateTaxRate
        {
            get
            {
                return this.options.EstateTaxRate;
            }
            set
            {
                if (this.options.EstateTaxRate == value) return;
                this.options.EstateTaxRate = value;
                this.NotifyPropertyChanged(() => this.EstateTaxRate);
            }
        }

        public decimal ConsumerPriceIndexGrowthRate
        {
            get
            {
                return this.options.ConsumerPriceIndexGrowthRate;
            }
            set
            {
                if (this.options.ConsumerPriceIndexGrowthRate == value) return;
                this.options.ConsumerPriceIndexGrowthRate = value;
                this.NotifyPropertyChanged(() => this.ConsumerPriceIndexGrowthRate);
            }
        }

        public InstallmentSaleNoteType? NoteType
        {
            get
            {
                return this.options.NoteType;
            }
            set
            {
                if (value.HasValue)
                {
                    this.options.NoteType = value.Value;
                    this.NotifyPropertyChanged(() => this.NoteType);
                }
            }
        }

        public int NumberOfYearsToProject
        {
            get
            {
                return this.options.NumberOfYearsToProject;
            }
            set
            {
                if (this.options.NumberOfYearsToProject == value) return;
                this.options.NumberOfYearsToProject = value;
                this.NotifyPropertyChanged(() => this.NumberOfYearsToProject);
            }
        }

        public int YearToToggleOffGrantorTrustStatus
        {
            get
            {
                return this.options.YearToToggleOffGrantorTrustStatus;
            }
            set
            {
                if (this.options.YearToToggleOffGrantorTrustStatus == value) return;
                this.options.YearToToggleOffGrantorTrustStatus = value;
                this.NotifyPropertyChanged(() => this.YearToToggleOffGrantorTrustStatus);
            }
        }
    }
}