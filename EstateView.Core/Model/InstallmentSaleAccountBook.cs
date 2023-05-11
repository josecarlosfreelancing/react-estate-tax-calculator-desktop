namespace EstateView.Core.Model
{
    public class InstallmentSaleAccountBook : AccountBook
    {
        public Account Estate { get; private set; }
        public Account Trust { get; private set; }

        public InstallmentSaleAccountBook()
        {
            this.Estate = this.RegisterAccount("Estate");
            this.Trust = this.RegisterAccount("Trust");
        }
    }
}
