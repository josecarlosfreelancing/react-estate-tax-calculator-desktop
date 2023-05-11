using System.ComponentModel;
using EstateView.Attributes;
using EstateView.Core.Model;
using EstateView.View.PropertyEditors;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EstateView.ViewModel
{
    public class LifeInsuranceOptionsViewModel : ViewModel
    {
        private LifeInsurancePolicy policy;

        public LifeInsuranceOptionsViewModel(LifeInsurancePolicy policy)
        {
            this.Bind(policy);
        }

        [DisplayName("Held In ILIT?")]
        [Category(null)]
        [PropertyOrder(1)]
        [Description(
            "For life insurance policies held in an irrevocable trust, the proceeds upon death are " +
            "not subject to federal estate taxes. However, the trust must be funded " +
            "using yearly or lifetime gifts. For life insurance policies not held in an irrevocable " +
            "trust, the premiums are not counted as gifts but the proceeds upon death " +
            "are included in the individual's estate for federal estate tax purposes.")]
        public bool IsInTrust
        {
            get
            {
                return this.policy.IsInTrust;
            }
            set
            {
                if (this.policy.IsInTrust == value) return;
                this.policy.IsInTrust = value;
                this.NotifyPropertyChanged(() => this.IsInTrust);
            }
        }

        [DisplayName("Policy Type")]
        [Category(null)]
        [PropertyOrder(2)]
        public LifeInsurancePolicyType PolicyType
        {
            get
            {
                return this.policy.PolicyType;
            }
            set
            {
                if (this.policy.PolicyType == value) return;
                this.policy.PolicyType = value;
                this.NotifyPropertyChanged(() => this.PolicyType);
            }
        }

        [DisplayName("Annual Premium")]
        [Category(null)]
        [PropertyOrder(3)]
        [MinimumValue(0), IncrementValue(1000), FormatString("C0")]
        [Editor(typeof(DecimalUpDownEditor), typeof(DecimalUpDownEditor))]
        public decimal AnnualPremium
        {
            get
            {
                return this.policy.AnnualPremium;
            }
            set
            {
                if (this.policy.AnnualPremium == value) return;
                this.policy.AnnualPremium = value;
                this.NotifyPropertyChanged(() => this.AnnualPremium);
            }
        }

        [DisplayName("Number of Years")]
        [Category(null)]
        [PropertyOrder(4)]
        [MinimumValue(0), IncrementValue(1)]
        [Editor(typeof(IntegerUpDownEditor), typeof(IntegerUpDownEditor))]
        public virtual int NumberOfYears
        {
            get
            {
                return this.policy.NumberOfYears;
            }
            set
            {
                if (this.policy.NumberOfYears == value) return;
                this.policy.NumberOfYears = value;
                this.NotifyPropertyChanged(() => this.NumberOfYears);
            }
        }

        [DisplayName("Additional Years Annual Premium")]
        [Category(null)]
        [PropertyOrder(5)]
        [MinimumValue(0), IncrementValue(1000), FormatString("C0")]
        [Editor(typeof(DecimalUpDownEditor), typeof(DecimalUpDownEditor))]
        public decimal AddtlYearsAnnualPremium
        {
            get
            {
                return this.policy.AddtlYearsAnnualPremium;
            }
            set
            {
                if (this.policy.AddtlYearsAnnualPremium == value) return;
                this.policy.AddtlYearsAnnualPremium = value;
                this.NotifyPropertyChanged(() => this.AddtlYearsAnnualPremium);
            }
        }

        [DisplayName("Number of Additional Years")]
        [Category(null)]
        [PropertyOrder(6)]
        [MinimumValue(0), IncrementValue(1)]
        [Editor(typeof(IntegerUpDownEditor), typeof(IntegerUpDownEditor))]
        public virtual int NumberOfAddtlYears
        {
            get
            {
                return this.policy.NumberOfAddtlYears;
            }
            set
            {
                if (this.policy.NumberOfAddtlYears == value) return;
                this.policy.NumberOfAddtlYears = value;
                this.NotifyPropertyChanged(() => this.NumberOfAddtlYears);
            }
        }

        [DisplayName("Death Benefit")]
        [Category(null)]
        [PropertyOrder(7)]
        [MinimumValue(0), IncrementValue(50000), FormatString("C0")]
        [Editor(typeof(DecimalUpDownEditor), typeof(DecimalUpDownEditor))]
        public decimal DeathBenefit
        {
            get
            {
                return this.policy.DeathBenefit;
            }
            set
            {
                if (this.policy.DeathBenefit == value) return;
                this.policy.DeathBenefit = value;
                this.NotifyPropertyChanged(() => this.DeathBenefit);
            }
        }

        public void Bind(LifeInsurancePolicy policy)
        {
            this.policy = policy;
            this.NotifyPropertyChanged();
        }
    }
}
