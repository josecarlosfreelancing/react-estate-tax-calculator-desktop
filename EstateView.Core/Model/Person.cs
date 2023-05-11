namespace EstateView.Core.Model
{
    public class Person
    {
        public Person()
        {
            this.LifeInsurance = new LifeInsurancePolicy();
            this.ExistingLifeInsurance = new LifeInsurancePolicy();
        }
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Sex Sex { get; set; }
        public int Age { get; set; }
        public bool IsSmoker { get; set; }
        public decimal LifetimeGiftExclusionAmountUsed { get; set; }
        public int ProjectedYearOfDeath { get; set; }

        public LifeInsurancePolicy LifeInsurance { get; set; }
        public LifeInsurancePolicy ExistingLifeInsurance { get; set; }

        public string Name
        {
            get { return string.Format("{0} {1}", this.FirstName, this.LastName); }
        }
    }
}