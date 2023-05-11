using EstateView.Core.Model.Scenarios;
using System;
using System.Linq;
using EstateView.Core.Model;

namespace EstateView.ViewModel.ClientLetter
{
    public class InstallmentSalePageViewModel : ViewModel
    {
        public InstallmentSalePageViewModel(EstatePlanningScenario previousScenario, InstallmentSaleScenario installmentSaleScenario)
        {
            if (installmentSaleScenario.Options.InstallmentSaleYearToToggleOffGrantorTrustStatus == -1)
            {
                this.TrustIncomeTaxPaymentText = "We are showing that you will pay the income taxes for the trust for your entire lifetimes.";
            }
            else if (installmentSaleScenario.Options.InstallmentSaleYearToToggleOffGrantorTrustStatus == 0)
            {
                this.TrustIncomeTaxPaymentText = "We are nevertheless assuming that you will want the trust to pay its own income taxes in this illustration.";
            }
            else
            {
                this.TrustIncomeTaxPaymentText =
                    "We are showing that you will pay the income taxes for the trust for " +
                    installmentSaleScenario.Options.InstallmentSaleYearToToggleOffGrantorTrustStatus +
                    " year(s) after the trust is established.";
            }

            if (installmentSaleScenario.Options.InstallmentSaleNoteType == InstallmentSaleNoteType.SelfCancelling)
            {
                this.NoteDetailsText =
                    "We are showing use of a \"Self-Cancelling Installment Note\", which is also known as a \"SCIN\", " +
                    "which can be cancelled on death, and not be considered an asset of your estates for estate tax purposes. " +
                    "To comply with the IRS actuarial tables we have used a " + 
                    installmentSaleScenario.Options.InstallmentSaleNoteInterestRate.ToString("P2") + 
                    " interest rate, and the note must balloon if you are still alive after " +
                    installmentSaleScenario.Options.InstallmentSaleNoteTermInYears +
                    " year(s), which can cause it to backfire. ";

                decimal grossedUpAmount = installmentSaleScenario.Options.InstallmentSaleNoteAmount - installmentSaleScenario.Options.InstallmentSaleValueAfterDiscount;

                if (grossedUpAmount > 0)
                {
                    this.NoteDetailsText +=
                        "We also \"grossed up\" (increased) the principal by " +
                        grossedUpAmount.ToString("C0") +
                        " to adhere to the actuarial requirements to reflect that a borrower would agree to have " +
                        "a higher payment amount in exchange for the self canceling feature.";
                }
            }
            else
            {
                this.NoteDetailsText =
                        "In exchange for the transfer we have you receiving a " +
                        installmentSaleScenario.Options.InstallmentSaleNoteInterestRate.ToString("P2") +
                        " note for " +
                        installmentSaleScenario.Options.InstallmentSaleNoteAmount.ToString("C0") +
                        ", which represents the sales price of " +
                        installmentSaleScenario.Options.InstallmentSaleValueAfterDiscount.ToString("C0") +
                        ". The note is payable interest only and will balloon (be payable in full) after" +
                        installmentSaleScenario.Options.InstallmentSaleNoteTermInYears +
                        " year(s). Before or at such time it may be refinanced, or possibly converted to what " +
                        "is called a Self Canceling Installment Note (\"SCIN\").";
            }

            this.SeedCapitalAmount = installmentSaleScenario.Options.InstallmentSaleSeedMoneyAmount;
            this.SaleValueBeforeDiscount = installmentSaleScenario.Options.InstallmentSaleValueBeforeDiscount;
            this.SaleValueAfterDiscount = installmentSaleScenario.Options.InstallmentSaleValueAfterDiscount;
            this.NoteDiscountRate = installmentSaleScenario.Options.InstallmentSaleNoteDiscountRate;
            this.CurrentYear = DateTime.Now.Year;
            this.YearOfSecondDeath = installmentSaleScenario.Options.SecondDyingSpouse.ProjectedYearOfDeath;
            this.TrustValueOnSecondDeath = installmentSaleScenario.Projections.Last().InstallmentSaleTrustValue;
            this.EstateTaxSavings =
                previousScenario.Projections.Last().EstateTaxDue -
                installmentSaleScenario.Projections.Last().EstateTaxDue;
        }

        public string TrustIncomeTaxPaymentText { get; set; }

        public decimal SeedCapitalAmount { get; set; }

        public decimal SaleValueBeforeDiscount { get; set; }

        public decimal SaleValueAfterDiscount { get; set; }

        public decimal NoteDiscountRate { get; set; }

        public string NoteDetailsText { get; set; }

        public int CurrentYear { get; set; }

        public int YearOfSecondDeath { get; set; }

        public decimal TrustValueOnSecondDeath { get; set; }

        public decimal EstateTaxSavings { get; set; }
    }
}
