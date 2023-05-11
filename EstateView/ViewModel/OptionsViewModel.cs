using System.Collections.Generic;
using System.ComponentModel;
using EstateView.Attributes;
using EstateView.Core.Model;
using EstateView.View.PropertyEditors;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using EstateView.Properties;

namespace EstateView.ViewModel
{
    [CategoryOrder(Categories.Planner, 1)]
    [CategoryOrder(Categories.Client, 2)]
    [CategoryOrder(Categories.PersonalResidence, 3)]
    [CategoryOrder(Categories.BusinessInvestments, 4)]
    [CategoryOrder(Categories.Adjustment2026, 5)]
    [CategoryOrder(Categories.Portability, 6)]
    [CategoryOrder(Categories.BypassTrust, 7)]
    [CategoryOrder(Categories.Gifting, 8)]
    [CategoryOrder(Categories.LifeInsurance, 9)]
    [CategoryOrder(Categories.InstallmentSale, 10)]
    [CategoryOrder(Categories.Misc, 11)]
    public class OptionsViewModel : ViewModel
    {
        private static class Categories
        {
            public const string Planner = "Planner";
            public const string Client = "Client";
            public const string PersonalResidence = "Personal Residence and Property";
            public const string BusinessInvestments = "Business and Investments";
            public const string Portability = "Portability Option";
            public const string Adjustment2026 = "2026 Exemption Adjustment";
            public const string BypassTrust = "Bypass Trust";
            public const string Gifting = "Gifting";
            public const string LifeInsurance = "Life Insurance";
            public const string InstallmentSale = "Installment Sale / Bulk Gift to Irrevocable Trust";
            public const string Misc = "Other information";
        }

        private EstateProjectionOptions options;
        private bool isInstallmentSaleValueChanging;

        public OptionsViewModel(EstateProjectionOptions options)
        {
            this.Bind(options);
        }

        [Browsable(false)]
        public EstateProjectionOptions Options
        {
            get { return this.options; }
        }

        [Category(Categories.Planner)]
        [DisplayName("Planner Name")]
        public string PlannerName
        {
            get { return Settings.Default.PlannerName; }
            set { Settings.Default.PlannerName = value; }
        }

        [Category(Categories.Planner)]
        [DisplayName("Firm Name")]
        public string PlannerFirmName
        {
            get { return Settings.Default.PlannerFirmName; }
            set { Settings.Default.PlannerFirmName = value; }
        }

        [Category(Categories.Client)]
        [ExpandableObject]
        public PersonViewModel Spouse1 { get; private set; }

        [Category(Categories.Client)]
        [ExpandableObject]
        public PersonViewModel Spouse2 { get; private set; }

        [Category(Categories.PersonalResidence)]
        [DisplayName("Current Value")]
        [MinimumValue(0), MaximumValue(5e12), IncrementValue(250000), FormatString("C0")]
        [Editor(typeof(DecimalUpDownEditor), typeof(DecimalUpDownEditor))]
        public decimal HomeValue
        {
            get
            {
                return this.options.HomeValue;
            }
            set
            {
                if (this.options.HomeValue == value) return;
                this.options.HomeValue = value;
                this.NotifyPropertyChanged(() => HomeValue);
            }
        }

        [Category(Categories.PersonalResidence)]
        [DisplayName("Annual Growth Rate")]
        [MinimumValue(-1), MaximumValue(1), IncrementValue(0.0025), FormatString("P2")]
        [Editor(typeof(DecimalUpDownEditor), typeof(DecimalUpDownEditor))]
        public decimal HomeValueGrowthRate
        {
            get
            {
                return this.options.HomeValueGrowthRate;
            }
            set
            {
                if (this.options.HomeValueGrowthRate == value) return;
                this.options.HomeValueGrowthRate = value;
                this.NotifyPropertyChanged(() => HomeValueGrowthRate);
            }
        }

        [Category(Categories.BusinessInvestments)]
        [PropertyOrder(1)]
        [DisplayName("Current Value")]
        [MinimumValue(0), MaximumValue(5e12), IncrementValue(250000), FormatString("C0")]
        [Editor(typeof(DecimalUpDownEditor), typeof(DecimalUpDownEditor))]
        public decimal AmountCurrentlyInvested
        {
            get
            {
                return this.options.AmountCurrentlyInvested;
            }
            set
            {
                if (this.options.AmountCurrentlyInvested == value) return;
                this.options.AmountCurrentlyInvested = value;
                this.NotifyPropertyChanged(() => AmountCurrentlyInvested);
                this.NotifyPropertyChanged(() => this.MaxInstallmentSaleValueBeforeDiscount);
                this.NotifyPropertyChanged(() => this.MaxInstallmentSaleValueAfterDiscount);
                this.NotifyPropertyChanged(() => this.MaxInstallmentSaleSeedMoneyAmount);
            }
        }

        [Category(Categories.BusinessInvestments)]
        [PropertyOrder(2)]
        [DisplayName("Annual Growth Rate")]
        [MinimumValue(-1), MaximumValue(1), IncrementValue(0.0025), FormatString("P2")]
        [Editor(typeof(DecimalUpDownEditor), typeof(DecimalUpDownEditor))]
        public decimal InvestmentsGrowthRate
        {
            get
            {
                return this.options.InvestmentsGrowthRate;
            }
            set
            {
                if (this.options.InvestmentsGrowthRate == value) return;
                this.options.InvestmentsGrowthRate = value;
                this.NotifyPropertyChanged(() => InvestmentsGrowthRate);
            }
        }

        [Category(Categories.BusinessInvestments)]
        [PropertyOrder(3)]
        [DisplayName("Annual Investment Costs Rate")]
        [MinimumValue(0), MaximumValue(0.05), IncrementValue(0.001), FormatString("P2")]
        [Editor(typeof(DecimalUpDownEditor), typeof(DecimalUpDownEditor))]
        public decimal InvestmentFeesRate
        {
            get
            {
                return this.options.InvestmentFeesRate;
            }
            set
            {
                if (this.options.InvestmentFeesRate == value) return;
                this.options.InvestmentFeesRate = value;
                this.NotifyPropertyChanged(() => InvestmentFeesRate);
            }
        }

        [Category(Categories.BusinessInvestments)]
        [PropertyOrder(3)]
        [DisplayName("Annual Investment Tax Rate (as % of assets)")]
        [MinimumValue(0), MaximumValue(0.05), IncrementValue(0.001), FormatString("P2")]
        [Editor(typeof(DecimalUpDownEditor), typeof(DecimalUpDownEditor))]
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
                this.NotifyPropertyChanged(() => IncomeTaxRate);
            }
        }

        [Category(Categories.BusinessInvestments)]
        [PropertyOrder(4)]
        [DisplayName("Net yearly savings/outgo (while both alive)")]
        [Description("This is the annual amount, as will be adjusted for CPI, that non investment earnings exceeds spending and taxes on such other earnings. This is what is added to net worth, in addition to growth in investments, or subtracted if spending exceeds such other earnings.")]
        [MinimumValue(-5e12), MaximumValue(5e12), IncrementValue(50000), FormatString("C0")]
        [Editor(typeof(DecimalUpDownEditor), typeof(DecimalUpDownEditor))]
        public decimal AnnualInvestmentsChangeBeforeFirstDeath
        {
            get
            {
                return this.options.AnnualInvestmentsChangeBeforeFirstDeath;
            }
            set
            {
                if (this.options.AnnualInvestmentsChangeBeforeFirstDeath == value) return;
                this.options.AnnualInvestmentsChangeBeforeFirstDeath = value;
                this.NotifyPropertyChanged(() => AnnualInvestmentsChangeBeforeFirstDeath);
            }
        }

        [Category(Categories.BusinessInvestments)]
        [PropertyOrder(5)]
        [DisplayName("Net yearly savings/outgo (after first death)")]
        [Description("This is the annual amount, as will be adjusted for CPI, that non investment earnings exceeds spending and taxes on such other earnings. This is what is added to net worth, in addition to growth in investments, or subtracted if spending exceeds such other earnings.")]
        [MinimumValue(-5e12), MaximumValue(5e12), IncrementValue(50000), FormatString("C0")]
        [Editor(typeof(DecimalUpDownEditor), typeof(DecimalUpDownEditor))]
        public decimal AnnualInvestmentsChangeAfterFirstDeath
        {
            get
            {
                return this.options.AnnualInvestmentsChangeAfterFirstDeath;
            }
            set
            {
                if (this.options.AnnualInvestmentsChangeAfterFirstDeath == value) return;
                this.options.AnnualInvestmentsChangeAfterFirstDeath = value;
                this.NotifyPropertyChanged(() => AnnualInvestmentsChangeAfterFirstDeath);
            }
        }

        [DisplayName("Assume no portability available?")]
        [Category(Categories.Portability)]
        [Description(
            "Checking this box will assume that the first spouse's unused lifetime gift exclusion " +
            "portability will not be available for use by the surviving spouse. This could happen " +
            "if the surviving spouse remarries someone who has already used their lifetime exclusion, " +
            "or dies and leaves more than the exclusion amount to their own descendants.")]
        public bool AssumeNoPortability
        {
            get
            {
                return this.options.AssumeNoPortability;
            }
            set
            {
                this.options.AssumeNoPortability = value;
                this.NotifyPropertyChanged(() => this.AssumeNoPortability);
            }
        }

        [Category(Categories.BypassTrust)]
        [Description(
            "A bypass trust is a trust funded on the death of a spouse that can benefit the surviving " +
            "spouse without being included in the surviving spouse's estate on the 2nd death.  We assume " +
            "that all other assets pass to the surviving spouse on the first death and qualify for the " +
            "marital deduction.")]
        [DisplayName("Max Bypass Trust Value")]
        [MinimumValue(0), MaximumValue("MaxBypassTrustValue"), IncrementValue(250000), FormatString("C0")]
        [Editor(typeof(DecimalUpDownEditor), typeof(DecimalUpDownEditor))]
        public decimal BypassTrustValue
        {
            get
            {
                return this.options.BypassTrustValue;
            }
            set
            {
                if (this.options.BypassTrustValue == value) return;
                this.options.BypassTrustValue = value;
                this.NotifyPropertyChanged(() => BypassTrustValue);
            }
        }

        [Browsable(false)]
        public decimal MaxBypassTrustValue
        {
            get
            {
                decimal max =
                    ProjectionCalculator.GetMaximumBypassTrustValue(
                        this.FirstDyingSpouse.ProjectedYearOfDeath,
                        this.ConsumerPriceIndexGrowthRate,
                        this.FirstDyingSpouse.LifetimeGiftExclusionAmountUsed,
                        this.AssumeExemptionReductionIn2026);

                if (this.BypassTrustValue > max)
                {
                    this.BypassTrustValue = max;
                }

                return max;
            }
        }

        [Category(Categories.Gifting)]
        [PropertyOrder(1)]
        [DisplayName("Initial Gifting Trust Value")]
        [MinimumValue(0), MaximumValue(5e12), IncrementValue(250000), FormatString("C0")]
        [Editor(typeof(DecimalUpDownEditor), typeof(DecimalUpDownEditor))]
        public decimal InitialGiftingTrustValue
        {
            get
            {
                return this.options.InitialGiftingTrustValue;
            }
            set
            {
                this.options.InitialGiftingTrustValue = value;
                this.NotifyPropertyChanged(() => this.InitialGiftingTrustValue);
            }
        }

        [Category(Categories.Gifting)]
        [PropertyOrder(2)]
        [DisplayName("Number of Exempt Gifts per year")]
        [Description(
            "The first $16,000 (2022 amount) of lifetime gifts made to any one " +
            "donee during a calendar year is exempt from gift taxes. Transferring " +
            "property into a Gifting Trust for the benefit of family members allows " +
            "the grantor to take advantage of the annual gift tax exclusion without " +
            "yet giving control over the property to the recipient." +
            "\n" +
            "\n" +
            "The actual gift amount is automatically adjusted for inflation each year " +
            "using the CPI growth rate.")]
        [MinimumValue(0)]
        [Editor(typeof(IntegerUpDownEditor), typeof(IntegerUpDownEditor))]
        public int NumberOfAnnualGiftsPerYear
        {
            get
            {
                return this.options.NumberOfAnnualGiftsPerYear;
            }
            set
            {
                if (this.options.NumberOfAnnualGiftsPerYear == value) return;
                this.options.NumberOfAnnualGiftsPerYear = value;
                this.NotifyPropertyChanged(() => NumberOfAnnualGiftsPerYear);
            }
        }

        [Category(Categories.Gifting)]
        [PropertyOrder(3)]
        [DisplayName("Percentage of Exempt Gifts to Gift Trust")]
        [Description(
            "Percentage of the exempt gifts going to Gifting Trust and saved there. The remaining " +
            "percentage is assumed to be spent by or on the descendants in the year of the gift.")]
        [MinimumValue(0), MaximumValue(1), IncrementValue(0.05), FormatString("P2")]
        [Editor(typeof(DecimalUpDownEditor), typeof(DecimalUpDownEditor))]
        public decimal PercentageOfAvailableGiftToPermissibleGift
        {
            get
            {
                return this.options.PercentageOfAvailableGiftToPermissibleGift;
            }
            set
            {
                if (this.options.PercentageOfAvailableGiftToPermissibleGift == value) return;
                this.options.PercentageOfAvailableGiftToPermissibleGift = value;
                this.NotifyPropertyChanged(() => PercentageOfAvailableGiftToPermissibleGift);
            }
        }

        [Category(Categories.Gifting)]
        [PropertyOrder(4)]
        [DisplayName("Percentage of Exempt Gifts using Discounting")]
        [MinimumValue(0), MaximumValue(1), IncrementValue(0.05), FormatString("P2")]
        [Editor(typeof(DecimalUpDownEditor), typeof(DecimalUpDownEditor))]
        [Description(
            "What portion of value gifted is via discounted LLC or similar interests. " +
            "Life insurance premium amounts for policies in trust reduce remaining " +
            "amounts available for discounted gifting in this program.")]
        public decimal PercentageOfInvestedGiftAmountDiscounted
        {
            get
            {
                return this.options.PercentageOfInvestedGiftAmountDiscounted;
            }
            set
            {
                if (this.options.PercentageOfInvestedGiftAmountDiscounted == value) return;
                this.options.PercentageOfInvestedGiftAmountDiscounted = value;
                this.NotifyPropertyChanged(() => PercentageOfInvestedGiftAmountDiscounted);
            }
        }

        [Category(Categories.Gifting)]
        [PropertyOrder(5)]
        [DisplayName("Discount Percentage for Gifting")]
        [MinimumValue(0), MaximumValue(1), IncrementValue(0.05), FormatString("P2")]
        [Editor(typeof(DecimalUpDownEditor), typeof(DecimalUpDownEditor))]
        public decimal DiscountPercentageForGifting
        {
            get
            {
                return this.options.DiscountPercentageForGifting;
            }
            set
            {
                if (this.options.DiscountPercentageForGifting == value) return;
                this.options.DiscountPercentageForGifting = value;
                this.NotifyPropertyChanged(() => DiscountPercentageForGifting);
            }
        }

        [Category(Categories.Misc)]
        [DisplayName("Consumer Price Index Growth Rate")]
        [MinimumValue(0), MaximumValue(1), IncrementValue(0.0025), FormatString("P2")]
        [Editor(typeof(DecimalUpDownEditor), typeof(DecimalUpDownEditor))]
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
                this.NotifyPropertyChanged(() => ConsumerPriceIndexGrowthRate);
                this.NotifyPropertyChanged(() => MaxBypassTrustValue);
            }
        }

        [Category(Categories.Misc)]
        [DisplayName("Use Constant Dollars")]
        [Description(
           "When selected, all values shown for future years are automatically adjusted " +
           "down to constant 2016 dollars using the provided CPI growth rate. This can " +
           "demonstrate the impact of inflation on taxes and assets remaining.")]
        public bool UseConstantDollars
        {
            get
            {
                return this.options.UseConstantDollars;
            }
            set
            {
                if (this.options.UseConstantDollars == value) return;
                this.options.UseConstantDollars = value;
                this.NotifyPropertyChanged(() => this.UseConstantDollars);
            }
        }

        [Category(Categories.Adjustment2026)]
        [DisplayName("Assume Lifetime Exemption drops 50% in 2026")]
        [Description(
            "As part of the tax bill passed in December 2017, the lifetime exemption " +
            "was temporarily doubled to $11.2M for ten years. In 2026, barring any new legislation, " +
            "the exemption will drop to 50% of its inflation adjusted value.  " +
            "Uncheck this box to assume the temporary doubling of the lifetime exemption is made permanent."
        )]
        public bool AssumeExemptionReductionIn2026
        {
            get
            {
                return this.options.AssumeExemptionReductionIn2026;
            }
            set
            {
                if (this.options.AssumeExemptionReductionIn2026 == value) return;
                this.options.AssumeExemptionReductionIn2026 = value;
                this.NotifyPropertyChanged(() => this.AssumeExemptionReductionIn2026);
                this.NotifyPropertyChanged(() => this.MaxBypassTrustValue);
            }
        }

        [Category(Categories.Misc)]
        [DisplayName("Estate Tax Rate")]
        [MinimumValue(0), MaximumValue(1), IncrementValue(0.0025), FormatString("P2")]
        [Editor(typeof(DecimalUpDownEditor), typeof(DecimalUpDownEditor))]
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
                this.NotifyPropertyChanged(() => EstateTaxRate);
            }
        }

        [Category(Categories.LifeInsurance), PropertyOrder(1)]
        [DisplayName("Spouse 1 - Pre-planning")]
        [ExpandableObject]
        public LifeInsuranceOptionsViewModel ExistingSpouse1LifeInsurance { get; private set; }

        [Category(Categories.LifeInsurance), PropertyOrder(2)]
        [DisplayName("Spouse 1 - Post-planning")]
        [ExpandableObject]
        public LifeInsuranceOptionsViewModel Spouse1LifeInsurance { get; private set; }

        [Category(Categories.LifeInsurance), PropertyOrder(3)]
        [DisplayName("Spouse 2 - Pre-planning")]
        [ExpandableObject]
        public LifeInsuranceOptionsViewModel ExistingSpouse2LifeInsurance { get; private set; }

        [Category(Categories.LifeInsurance), PropertyOrder(4)]
        [DisplayName("Spouse 2 - Post-planning")]
        [ExpandableObject]
        public LifeInsuranceOptionsViewModel Spouse2LifeInsurance { get; private set; }

        [Category(Categories.LifeInsurance), PropertyOrder(5)]
        [DisplayName("Second to Die - Pre-planning")]
        [ExpandableObject]
        public LifeInsuranceOptionsViewModel ExistingSecondToDieLifeInsurance { get; private set; }

        [Category(Categories.LifeInsurance), PropertyOrder(6)]
        [DisplayName("Second to Die - Post-planning")]
        [ExpandableObject]
        public LifeInsuranceOptionsViewModel SecondToDieLifeInsurance { get; private set; }

        [Category(Categories.InstallmentSale), PropertyOrder(1)]
        [DisplayName("Seed / First-year Gift Amount")]
        [Description(
            "When an irrevocable trust is created for the purpose of selling property to the trust with an " +
            "installment note, the IRS requires that the trust have some assets other than the property sold to " +
            "it. The concept is the trust must have some seed capital that will be secured by the installment " +
            "note in addition to the property sold to the trust. The seed capital will typically be " +
            "10% of the value of the property being sold to the trust. The transfer of the seed capital to " +
            "the trust will reduce the exemption of the donor spouse.")]
        [MinimumValue(0), MaximumValue("MaxInstallmentSaleSeedMoneyAmount"), IncrementValue(250000), FormatString("C0")]
        [Editor(typeof(DecimalUpDownEditor), typeof(DecimalUpDownEditor))]
        public decimal InstallmentSaleSeedMoneyAmount
        {
            get
            {
                return this.options.InstallmentSaleSeedMoneyAmount;
            }
            set
            {
                if (this.options.InstallmentSaleSeedMoneyAmount == value) return;
                this.options.InstallmentSaleSeedMoneyAmount = value;

                decimal max = this.options.AmountCurrentlyInvested - this.options.InstallmentSaleSeedMoneyAmount;

                if (this.InstallmentSaleValueBeforeDiscount > max)
                {
                    this.InstallmentSaleValueBeforeDiscount = max;
                }

                this.NotifyPropertyChanged(() => this.InstallmentSaleSeedMoneyAmount);
            }
        }

        [Browsable(false)]
        public decimal MaxInstallmentSaleSeedMoneyAmount
        {
            get
            {
                decimal max = this.options.AmountCurrentlyInvested;

                if (this.InstallmentSaleSeedMoneyAmount > max)
                {
                    this.InstallmentSaleSeedMoneyAmount = max;
                }

                return max;
            }
        }

        [Category(Categories.InstallmentSale), PropertyOrder(2)]
        [DisplayName("Sale Value before Discount")]
        [MinimumValue(0), MaximumValue("MaxInstallmentSaleValueBeforeDiscount"), IncrementValue(250000), FormatString("C0")]
        [Editor(typeof(DecimalUpDownEditor), typeof(DecimalUpDownEditor))]
        public decimal InstallmentSaleValueBeforeDiscount
        {
            get
            {
                return this.options.InstallmentSaleValueBeforeDiscount;
            }
            set
            {
                if (this.options.InstallmentSaleValueBeforeDiscount == value) return;
                this.options.InstallmentSaleValueBeforeDiscount = value;

                if (!this.isInstallmentSaleValueChanging)
                {
                    this.isInstallmentSaleValueChanging = true;
                    this.InstallmentSaleValueAfterDiscount = value * (1 - this.InstallmentSaleNoteDiscountRate);
                    this.isInstallmentSaleValueChanging = false;
                }

                decimal max = this.options.AmountCurrentlyInvested - this.options.InstallmentSaleValueBeforeDiscount;

                if (this.InstallmentSaleSeedMoneyAmount > max)
                {
                    this.InstallmentSaleSeedMoneyAmount = max;
                }

                this.NotifyPropertyChanged(() => this.InstallmentSaleValueBeforeDiscount);
            }
        }

        [Browsable(false)]
        public decimal MaxInstallmentSaleValueBeforeDiscount
        {
            get
            {
                decimal max = this.options.AmountCurrentlyInvested;

                if (this.InstallmentSaleValueBeforeDiscount > max)
                {
                    this.InstallmentSaleValueBeforeDiscount = max;
                }

                return max;
            }
        }


        [Category(Categories.InstallmentSale), PropertyOrder(3)]
        [DisplayName("Discount Rate")]
        [MinimumValue(0), MaximumValue(0.75), IncrementValue(0.05), FormatString("P2")]
        [Editor(typeof(DecimalUpDownEditor), typeof(DecimalUpDownEditor))]
        public decimal InstallmentSaleNoteDiscountRate
        {
            get
            {
                return this.options.InstallmentSaleNoteDiscountRate;
            }
            set
            {
                if (this.options.InstallmentSaleNoteDiscountRate == value) return;
                this.options.InstallmentSaleNoteDiscountRate = value;
                this.InstallmentSaleValueAfterDiscount = this.InstallmentSaleValueAfterDiscount; 
                this.NotifyPropertyChanged(() => this.InstallmentSaleNoteDiscountRate);
                this.NotifyPropertyChanged(() => this.MaxInstallmentSaleValueAfterDiscount);
            }
        }

        [Category(Categories.InstallmentSale), PropertyOrder(4)]
        [DisplayName("Sale Value after Discount")]
        [MinimumValue(0), MaximumValue("MaxInstallmentSaleValueAfterDiscount"), IncrementValue(250000), FormatString("C0")]
        [Editor(typeof(DecimalUpDownEditor), typeof(DecimalUpDownEditor))]
        public decimal InstallmentSaleValueAfterDiscount
        {
            get
            {
                return this.options.InstallmentSaleValueAfterDiscount;
            }
            set
            {
                decimal previousValue = this.options.InstallmentSaleValueAfterDiscount;
                this.options.InstallmentSaleValueAfterDiscount = value;

                if (!this.isInstallmentSaleValueChanging)
                {
                    this.isInstallmentSaleValueChanging = true;
                    this.InstallmentSaleValueBeforeDiscount = value / (1 - this.InstallmentSaleNoteDiscountRate);
                    this.isInstallmentSaleValueChanging = false;
                }

                this.NotifyPropertyChanged(() => this.InstallmentSaleValueAfterDiscount);
                this.NotifyPropertyChanged(() => this.MaxInstallmentSaleValueBeforeDiscount);

                if (this.InstallmentSaleNoteAmount == previousValue || this.InstallmentSaleNoteAmount < value)
                {
                    this.InstallmentSaleNoteAmount = value;
                }
            }
        }

        [Browsable(false)]
        public decimal MaxInstallmentSaleValueAfterDiscount
        {
            get
            {
                decimal max = (this.options.AmountCurrentlyInvested) * (1 - this.InstallmentSaleNoteDiscountRate);

                if (this.InstallmentSaleValueAfterDiscount > max)
                {
                    this.InstallmentSaleValueAfterDiscount = max;
                }

                return max;
            }
        }

        [Category(Categories.InstallmentSale), PropertyOrder(5)]
        [DisplayName("Note Amount")]
        [MinimumValue(0), IncrementValue(250000), FormatString("C0")]
        [Editor(typeof(DecimalUpDownEditor), typeof(DecimalUpDownEditor))]
        [Description(
            "The Note Amount should equal the Sale Value after Discount unless it is a " +
            "self cancelling installment note, in which case the IRS tables or other " +
            "criteria should be used to set the interest rate.")]
        public decimal InstallmentSaleNoteAmount
        {
            get
            {
                return this.options.InstallmentSaleNoteAmount;
            }
            set
            {
                if (this.options.InstallmentSaleNoteAmount == value) return;
                this.options.InstallmentSaleNoteAmount = value;
                this.NotifyPropertyChanged(() => this.InstallmentSaleNoteAmount);
            }
        }

        [Category(Categories.InstallmentSale), PropertyOrder(6)]
        [DisplayName("Note Interest Rate")]
        [MinimumValue(0), MaximumValue(1), IncrementValue(0.0025), FormatString("P2")]
        [Editor(typeof(DecimalUpDownEditor), typeof(DecimalUpDownEditor))]
        [Description(
            "The Note Interest Rate should equal the applicable federal rate (AFR) " +
            "unless it is a grossed up SCIN.")]
        public decimal InstallmentSaleNoteInterestRate
        {
            get
            {
                return this.options.InstallmentSaleNoteInterestRate;
            }
            set
            {
                if (this.options.InstallmentSaleNoteInterestRate == value) return;
                this.options.InstallmentSaleNoteInterestRate = value;
                this.NotifyPropertyChanged(() => this.InstallmentSaleNoteInterestRate);
            }
        }

        [Category(Categories.InstallmentSale), PropertyOrder(7)]
        [DisplayName("Type of Note")]
        public InstallmentSaleNoteType InstallmentSaleNoteType
        {
            get
            {
                return this.options.InstallmentSaleNoteType;
            }
            set
            {
                if (this.options.InstallmentSaleNoteType == value) return;
                this.options.InstallmentSaleNoteType = value;
                this.NotifyPropertyChanged(() => this.InstallmentSaleNoteType);
            }
        }

        [Category(Categories.InstallmentSale), PropertyOrder(8)]
        [DisplayName("Note Term in Years")]
        [MinimumValue(0)]
        [Editor(typeof(IntegerUpDownEditor), typeof(IntegerUpDownEditor))]
        public int InstallmentSaleNoteTermInYears
        {
            get
            {
                return this.options.InstallmentSaleNoteTermInYears;
            }
            set
            {
                if (this.options.InstallmentSaleNoteTermInYears == value) return;
                this.options.InstallmentSaleNoteTermInYears = value;
                this.NotifyPropertyChanged(() => this.InstallmentSaleNoteTermInYears);
            }
        }

        [Category(Categories.InstallmentSale), PropertyOrder(9)]
        [DisplayName("Year to Toggle Off Grantor Status")]
        [Description("Assumes no income tax triggered when toggled off, but there may be a tax if a note to the grantor is in place that time.")]
        [MinimumValue(-1), FormatString("#0;NA")]
        [Editor(typeof(IntegerUpDownEditor), typeof(IntegerUpDownEditor))]
        public int InstallmentSaleYearToToggleOffGrantorTrustStatus
        {
            get
            {
                return this.options.InstallmentSaleYearToToggleOffGrantorTrustStatus;
            }
            set
            {
                if (this.options.InstallmentSaleYearToToggleOffGrantorTrustStatus == value) return;
                this.options.InstallmentSaleYearToToggleOffGrantorTrustStatus = value;
                this.NotifyPropertyChanged(() => this.InstallmentSaleYearToToggleOffGrantorTrustStatus);
            }
        }

        [Category(Categories.InstallmentSale)]
        [PropertyOrder(10)]
        [DisplayName("Additional Income for Installment Sale Trust")]
        [MinimumValue(0), MaximumValue(5e12), IncrementValue(50000), FormatString("C0")]
        [Editor(typeof(DecimalUpDownEditor), typeof(DecimalUpDownEditor))]
        public decimal AnnualAdditionalIncomeForInstallmentSaleTrust
        {
            get
            {
                return this.options.AnnualAdditionalIncomeForInstallmentSaleTrust;
            }
            set
            {
                if (this.options.AnnualAdditionalIncomeForInstallmentSaleTrust == value) return;
                this.options.AnnualAdditionalIncomeForInstallmentSaleTrust = value;
                this.NotifyPropertyChanged(() => AnnualAdditionalIncomeForInstallmentSaleTrust);
            }
        }

        [Category(Categories.InstallmentSale)]
        [PropertyOrder(11)]
        [DisplayName("Number of years of Additional Income for Installment Sale Trust")]
        [MinimumValue(0), MaximumValue(100)]
        [Editor(typeof(IntegerUpDownEditor), typeof(IntegerUpDownEditor))]
        public int NumberOfYearsAdditionalIncomeForInstallmentSaleTrust
        {
            get
            {
                return this.options.NumberOfYearsAdditionalIncomeForInstallmentSaleTrust;
            }
            set
            {
                if (this.options.NumberOfYearsAdditionalIncomeForInstallmentSaleTrust == value) return;
                this.options.NumberOfYearsAdditionalIncomeForInstallmentSaleTrust = value;
                this.NotifyPropertyChanged(() => NumberOfYearsAdditionalIncomeForInstallmentSaleTrust);
            }
        }

        [Browsable(false)]
        public PersonViewModel FirstDyingSpouse
        {
            get { return this.Spouse1.ProjectedYearOfDeath < this.Spouse2.ProjectedYearOfDeath ? this.Spouse1 : this.Spouse2; }
        }

        [Browsable(false)]
        public PersonViewModel SecondDyingSpouse
        {
            get { return this.Spouse1.ProjectedYearOfDeath < this.Spouse2.ProjectedYearOfDeath ? this.Spouse2 : this.Spouse1; }
        }

        [Browsable(false)]
        public IEnumerable<PersonViewModel> Spouses
        {
            get { return new[] { this.Spouse1, this.Spouse2 }; }
        }

        [Browsable(false)]
        public int NumberOfYears
        {
            get { return this.options.NumberOfYears; }
        }

        public void Bind(EstateProjectionOptions newOptions)
        {
            this.options = newOptions;

            this.Spouse1 = this.BindViewModel(this.Spouse1, newOptions.Spouse1);
            this.Spouse2 = this.BindViewModel(this.Spouse2, newOptions.Spouse2);

            this.Spouse1LifeInsurance = this.BindViewModel(this.Spouse1LifeInsurance, newOptions.Spouse1.LifeInsurance);
            this.Spouse2LifeInsurance = this.BindViewModel(this.Spouse2LifeInsurance, newOptions.Spouse2.LifeInsurance);
            this.SecondToDieLifeInsurance = this.BindViewModel(this.SecondToDieLifeInsurance, newOptions.SecondToDieLifeInsurance);

            this.ExistingSpouse1LifeInsurance = this.BindViewModel(this.ExistingSpouse1LifeInsurance, newOptions.Spouse1.ExistingLifeInsurance);
            this.ExistingSpouse2LifeInsurance = this.BindViewModel(this.ExistingSpouse2LifeInsurance, newOptions.Spouse2.ExistingLifeInsurance);
            this.ExistingSecondToDieLifeInsurance = this.BindViewModel(this.ExistingSecondToDieLifeInsurance, newOptions.ExistingSecondToDieLifeInsurance);

            this.RelayPropertyChanged(this.Spouse1);
            this.RelayPropertyChanged(this.Spouse2);
            this.RelayPropertyChanged(this.Spouse1LifeInsurance);
            this.RelayPropertyChanged(this.Spouse2LifeInsurance);
            this.RelayPropertyChanged(this.SecondToDieLifeInsurance);
            this.RelayPropertyChanged(this.ExistingSpouse1LifeInsurance);
            this.RelayPropertyChanged(this.ExistingSpouse2LifeInsurance);
            this.RelayPropertyChanged(this.ExistingSecondToDieLifeInsurance);

            this.NotifyPropertyChanged();
        }

        private LifeInsuranceOptionsViewModel BindViewModel(LifeInsuranceOptionsViewModel viewModel, LifeInsurancePolicy lifeInsurance)
        {
            if (viewModel == null)
            {
                viewModel = new LifeInsuranceOptionsViewModel(lifeInsurance);
            }
            else
            {
                viewModel.Bind(lifeInsurance);
            }

            return viewModel;
        }

        private PersonViewModel BindViewModel(PersonViewModel viewModel, Person person)
        {
            if (viewModel == null)
            {
                viewModel = new PersonViewModel(person);
            }
            else
            {
                viewModel.Bind(person);
            }

            return viewModel;
        }
    }
}