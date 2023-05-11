using System.Collections.Generic;

namespace EstateView.Core.Model
{
    public static class MortalityTable
    {
        private static readonly Dictionary<int, LifeExpectancy> lifeExpectancies = new Dictionary<int, LifeExpectancy>(); 

        static MortalityTable()
        {
            MortalityTable.AddLifeExpectancy(35, 41.8M, 37.2M, 45.4M, 39.9M);
            MortalityTable.AddLifeExpectancy(36, 40.9M, 36.3M, 44.5M, 39M);
            MortalityTable.AddLifeExpectancy(37, 39.9M, 35.4M, 43.5M, 38.1M);
            MortalityTable.AddLifeExpectancy(38, 39M, 34.4M, 42.6M, 37.1M);
            MortalityTable.AddLifeExpectancy(39, 38.1M, 33.5M, 41.6M, 36.2M);
            MortalityTable.AddLifeExpectancy(40, 37.1M, 32.6M, 40.7M, 35.3M);
            MortalityTable.AddLifeExpectancy(41, 36.2M, 31.8M, 39.7M, 34.4M);
            MortalityTable.AddLifeExpectancy(42, 35.3M, 30.9M, 38.8M, 33.5M);
            MortalityTable.AddLifeExpectancy(43, 34.3M, 30M, 37.9M, 32.6M);
            MortalityTable.AddLifeExpectancy(44, 33.4M, 29.1M, 36.9M, 31.7M);
            MortalityTable.AddLifeExpectancy(45, 32.5M, 28.2M, 36M, 30.8M);
            MortalityTable.AddLifeExpectancy(46, 31.6M, 27.4M, 35.1M, 29.9M);
            MortalityTable.AddLifeExpectancy(47, 30.7M, 26.6M, 34.2M, 29M);
            MortalityTable.AddLifeExpectancy(48, 29.8M, 25.7M, 33.3M, 28.2M);
            MortalityTable.AddLifeExpectancy(49, 28.9M, 24.9M, 32.3M, 27.3M);
            MortalityTable.AddLifeExpectancy(50, 28M, 24.1M, 31.4M, 26.4M);
            MortalityTable.AddLifeExpectancy(51, 27.1M, 23.3M, 30.5M, 25.6M);
            MortalityTable.AddLifeExpectancy(52, 26.2M, 22.5M, 29.7M, 24.8M);
            MortalityTable.AddLifeExpectancy(53, 25.4M, 21.7M, 28.8M, 24M);
            MortalityTable.AddLifeExpectancy(54, 24.5M, 20.9M, 27.9M, 23.2M);
            MortalityTable.AddLifeExpectancy(55, 23.6M, 20.1M, 27M, 22.4M);
            MortalityTable.AddLifeExpectancy(56, 22.8M, 19.4M, 26.2M, 21.7M);
            MortalityTable.AddLifeExpectancy(57, 22M, 18.6M, 25.4M, 20.9M);
            MortalityTable.AddLifeExpectancy(58, 21.1M, 17.9M, 24.5M, 20.2M);
            MortalityTable.AddLifeExpectancy(59, 20.3M, 17.2M, 23.7M, 19.5M);
            MortalityTable.AddLifeExpectancy(60, 19.5M, 16.5M, 22.9M, 18.7M);
            MortalityTable.AddLifeExpectancy(61, 18.7M, 15.8M, 22.1M, 18M);
            MortalityTable.AddLifeExpectancy(62, 18M, 15.2M, 21.3M, 17.4M);
            MortalityTable.AddLifeExpectancy(63, 17.2M, 14.5M, 20.5M, 16.7M);
            MortalityTable.AddLifeExpectancy(64, 16.4M, 13.9M, 19.7M, 16M);
            MortalityTable.AddLifeExpectancy(65, 15.7M, 13.2M, 19M, 15.3M);
            MortalityTable.AddLifeExpectancy(66, 15M, 12.6M, 18.2M, 14.7M);
            MortalityTable.AddLifeExpectancy(67, 14.3M, 12.1M, 17.5M, 14.1M);
            MortalityTable.AddLifeExpectancy(68, 13.6M, 11.5M, 16.8M, 13.5M);
            MortalityTable.AddLifeExpectancy(69, 12.9M, 10.9M, 16M, 12.9M);
            MortalityTable.AddLifeExpectancy(70, 12.2M, 10.4M, 15.3M, 12.3M);
            MortalityTable.AddLifeExpectancy(71, 11.6M, 9.9M, 14.6M, 11.7M);
            MortalityTable.AddLifeExpectancy(72, 11M, 9.3M, 14M, 11.2M);
            MortalityTable.AddLifeExpectancy(73, 10.4M, 8.8M, 13.3M, 10.6M);
            MortalityTable.AddLifeExpectancy(74, 9.8M, 8.3M, 12.6M, 10.1M);
            MortalityTable.AddLifeExpectancy(75, 9.2M, 7.8M, 12M, 9.6M);
            MortalityTable.AddLifeExpectancy(76, 8.7M, 7.4M, 11.4M, 9.1M);
            MortalityTable.AddLifeExpectancy(77, 8.2M, 7M, 10.8M, 8.6M);
            MortalityTable.AddLifeExpectancy(78, 7.6M, 6.5M, 10.2M, 8.2M);
            MortalityTable.AddLifeExpectancy(79, 7.1M, 6.1M, 9.6M, 7.7M);
            MortalityTable.AddLifeExpectancy(80, 6.6M, 5.7M, 9M, 7.3M);
            MortalityTable.AddLifeExpectancy(81, 6.2M, 5.4M, 8.5M, 6.9M);
            MortalityTable.AddLifeExpectancy(82, 5.8M, 5M, 8M, 6.5M);
            MortalityTable.AddLifeExpectancy(83, 5.4M, 4.7M, 7.5M, 6.2M);
            MortalityTable.AddLifeExpectancy(84, 5M, 4.4M, 7M, 5.8M);
            MortalityTable.AddLifeExpectancy(85, 4.6M, 4M, 6.5M, 5.4M);
            MortalityTable.AddLifeExpectancy(86, 4.3M, 3.8M, 6.2M, 5.2M);
            MortalityTable.AddLifeExpectancy(87, 4M, 3.6M, 5.8M, 4.9M);
            MortalityTable.AddLifeExpectancy(88, 3.8M, 3.3M, 5.4M, 4.6M);
            MortalityTable.AddLifeExpectancy(89, 3.5M, 3.1M, 5M, 4.3M);
            MortalityTable.AddLifeExpectancy(90, 3.2M, 2.8M, 4.6M, 4.1M);
            MortalityTable.AddLifeExpectancy(91, 3M, 2.7M, 4.3M, 3.8M);
            MortalityTable.AddLifeExpectancy(92, 2.8M, 2.5M, 4M, 3.6M);
            MortalityTable.AddLifeExpectancy(93, 2.6M, 2.4M, 3.7M, 3.3M);
            MortalityTable.AddLifeExpectancy(94, 2.4M, 2.2M, 3.3M, 3M);
            MortalityTable.AddLifeExpectancy(95, 2.2M, 2.1M, 3M, 2.8M);
        }

        public static decimal GetLifeExpectancy(int age, Sex sex, bool isSmoker)
        {
            LifeExpectancy lifeExpectancy;

            if (MortalityTable.lifeExpectancies.TryGetValue(age, out lifeExpectancy))
            {
                return 
                    sex == Sex.Male && isSmoker ? lifeExpectancy.MaleSmoker :
                    sex == Sex.Male && !isSmoker ? lifeExpectancy.MaleNonSmoker :
                    sex == Sex.Female && isSmoker ? lifeExpectancy.FemaleSmoker :
                    sex == Sex.Female && !isSmoker ? lifeExpectancy.FemaleNonSmoker :
                    0;
            }

            return 0;
        }

        private static void AddLifeExpectancy(int attainedAge, decimal maleNonSmoker, decimal maleSmoker, decimal femaleNonSmoker, decimal femaleSmoker)
        {
            MortalityTable.lifeExpectancies.Add(attainedAge, new LifeExpectancy(attainedAge, maleNonSmoker, maleSmoker, femaleNonSmoker, femaleSmoker));
        }
    }

    public class LifeExpectancy
    {
        public LifeExpectancy(int attainedAge, decimal maleNonSmoker, decimal maleSmoker, decimal femaleNonSmoker, decimal femaleSmoker)
        {
            this.AttainedAge = attainedAge;
            this.MaleNonSmoker = maleNonSmoker;
            this.MaleSmoker = maleSmoker;
            this.FemaleNonSmoker = femaleNonSmoker;
            this.FemaleSmoker = femaleSmoker;
        }

        public int AttainedAge { get; private set; }
        public decimal MaleNonSmoker { get; private set; }
        public decimal MaleSmoker { get; private set; }
        public decimal FemaleNonSmoker { get; private set; }
        public decimal FemaleSmoker { get; private set; }
    }
}
