using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EstateView.ViewModel.Logistics
{
    public class TotalPassedToBeneficiariesViewModel
    {
        public TotalPassedToBeneficiariesViewModel()
        {
            this.Items = new ObservableCollection<TotalPassedToBeneficiariesItemViewModel>();
        }

        public IList<TotalPassedToBeneficiariesItemViewModel> Items { get; private set; }

        public decimal Total
        {
            get
            {
                return this.Items.Sum(item => item.Amount);
            }
        }
    }
}