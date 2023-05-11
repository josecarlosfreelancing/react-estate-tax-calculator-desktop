using System;
using System.ComponentModel;
using EstateView.Attributes;
using EstateView.Core.Model;
using EstateView.View.PropertyEditors;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace EstateView.ViewModel
{
    public class PersonViewModel : ViewModel
    {
        private Person person;

        public PersonViewModel(Person person)
        {
            this.Bind(person);
        }

        [DisplayName("First Name")]
        [PropertyOrder(1)]
        [Category(null)]
        public string FirstName
        {
            get
            {
                return this.person.FirstName;
            }
            set
            {
                if (this.person.FirstName == value) return;
                this.person.FirstName = value;
                this.NotifyPropertyChanged();
            }
        }

        [DisplayName("Last Name")]
        [PropertyOrder(2)]
        [Category(null)]
        public string LastName
        {
            get
            {
                return this.person.LastName;
            }
            set
            {
                if (this.person.LastName == value) return;
                this.person.LastName = value;
                this.NotifyPropertyChanged();
            }
        }

        [PropertyOrder(3)]
        [MinimumValue(18), MaximumValue(120), IncrementValue(1)]
        [Editor(typeof(IntegerUpDownEditor), typeof(IntegerUpDownEditor))]
        [Category(null)]
        public int Age
        {
            get
            {
                return this.person.Age;
            }
            set
            {
                if (this.person.Age == value) return;
                this.person.Age = value;
                this.NotifyPropertyChanged();
            }
        }

        [PropertyOrder(4)]
        [Category(null)]
        public Sex Sex
        {
            get
            {
                return this.person.Sex;
            }
            set
            {
                this.person.Sex = value;
                this.NotifyPropertyChanged();
            }
        }

        [PropertyOrder(5)]
        [DisplayName("Tobacco User?")]
        [Category(null)]
        public bool IsSmoker
        {
            get
            {
                return this.person.IsSmoker;
            }
            set
            {
                this.person.IsSmoker = value;
                this.NotifyPropertyChanged();
            }
        }

        [PropertyOrder(6)]
        [DisplayName("Lifetime Gift Exclusion Used")]
        [MinimumValue(0), MaximumValue((double)ProjectionCalculator.Constants.FirstYearLifetimeGiftExclusionAmount), IncrementValue(250000), FormatString("C0")]
        [Editor(typeof(DecimalUpDownEditor), typeof(DecimalUpDownEditor))]
        [Category(null)]
        public decimal LifetimeGiftExclusionAmountUsed
        {
            get
            {
                return this.person.LifetimeGiftExclusionAmountUsed;
            }
            set
            {
                if (this.person.LifetimeGiftExclusionAmountUsed == value) return;
                this.person.LifetimeGiftExclusionAmountUsed = value;
                this.NotifyPropertyChanged(() => this.LifetimeGiftExclusionAmountUsed);
            }
        }

        [PropertyOrder(7)]
        [DisplayName("Actuarial Year of Death")]
        [Category(null)]
        public int ActuarialYearOfDeath
        {
            get
            {
                return (int)(DateTime.Now.Year + this.LifeExpectancyNumber);
            }
        }

        [PropertyOrder(8)]
        [DisplayName("Projected Year of Death")]
        [Category(null)]
        public int ProjectedYearOfDeath
        {
            get
            {
                return this.person.ProjectedYearOfDeath;
            }
            set
            {
                this.person.ProjectedYearOfDeath = value;
                this.NotifyPropertyChanged(() => this.ProjectedYearOfDeath);
            }
        }

        [Browsable(false)]
        public string LifeExpectancy
        {
            get
            {
                decimal lifeExpectancy = this.LifeExpectancyNumber;
                
                if (lifeExpectancy == 0)
                {
                    return string.Format("No life expectancy data available for age {0}", this.Age);
                }
                
                decimal totalAge = this.Age + lifeExpectancy;
                return string.Format("An additional {0} years (total age {1})", lifeExpectancy, totalAge);
            }
        }

        [Browsable(false)]
        public decimal LifeExpectancyNumber
        {
            get
            {
                return MortalityTable.GetLifeExpectancy(this.person.Age, this.person.Sex, this.person.IsSmoker);
            }
        }

        [Browsable(false)]
        public string DisplayValue
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.FirstName) || string.IsNullOrWhiteSpace(this.LastName))
                {
                    return "Input Below";
                }
                else
                {
                    return string.Format("{0} {1}", this.FirstName, this.LastName);
                }
            }
        }

        public void Bind(Person person)
        {
            this.person = person;
            this.NotifyPropertyChanged();
        }
   }
}