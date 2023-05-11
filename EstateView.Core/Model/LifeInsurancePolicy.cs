namespace EstateView.Core.Model
{
    public class LifeInsurancePolicy
    {
        public decimal GetPremiumForYear(int yearNumber)
        {
            if (yearNumber < this.NumberOfYears)
            {
                return this.AnnualPremium;
            } else if (yearNumber < this.NumberOfYears + this.NumberOfAddtlYears)
            {
                return this.AddtlYearsAnnualPremium;
            }

            return 0;
        }

        public decimal GetDeathBenefitForYear(int yearNumber)
        {
            if (this.PolicyType == LifeInsurancePolicyType.Permanent ||
                (this.PolicyType == LifeInsurancePolicyType.Term && yearNumber <= this.NumberOfYears + this.NumberOfAddtlYears))
            {
                return this.DeathBenefit;
            }

            return 0;
        }

        public LifeInsurancePolicyType PolicyType { get; set; }
        public bool IsInTrust { get; set; }
        public int NumberOfYears { get; set; }
        public decimal AnnualPremium { get; set; }
        public int NumberOfAddtlYears { get; set; }
        public decimal AddtlYearsAnnualPremium { get; set; }
        public decimal DeathBenefit { get; set; }

        public bool IsEmpty()
        {
            return 
                this.NumberOfYears + this.NumberOfAddtlYears == 0 ||
                this.AnnualPremium == 0 ||
                this.DeathBenefit == 0;
        }
    }

    public enum LifeInsurancePolicyType
    {
        Term,
        Permanent,
    }
}
