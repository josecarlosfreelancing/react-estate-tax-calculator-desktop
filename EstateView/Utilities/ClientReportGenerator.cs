using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using EstateView.Core.Model;
using EstateView.Core.Model.Scenarios;
using EstateView.Core.Utilities;
using EstateView.ViewModel.ClientLetter;

namespace EstateView.Utilities
{
    public class ClientReportGenerator
    {
        private static class Constants
        {
            public const int ScreenDotsPerInch = 96;
            public const int PrintDotsPerInch = 384;
            public static readonly Size ScreenPageSize = new Size(8.5 * ScreenDotsPerInch, 11 * ScreenDotsPerInch);
            public static readonly Thickness ScreenPageMargin = new Thickness(1 * ScreenDotsPerInch);
            public const string PageBreakMarker = "##PAGE_BREAK##";
            public const string ClientReportFileExtension = ".dot";
        }

        public void GenerateClientLetter(EstatePlanningScenario scenario)
        {
            string filename =
                Path.Combine(Path.GetTempPath(), Path.GetFileName(Path.GetRandomFileName()) +
                Constants.ClientReportFileExtension);

            try
            {
                using (Stream output = File.OpenWrite(filename))
                {
                    this.GenerateWordDocument(scenario.Options, output);
                }

                var startInfo = new ProcessStartInfo(filename);
                startInfo.UseShellExecute = true;
                Process.Start(startInfo);
            }
            catch (IOException e)
            {
                MessageBox.Show("Failed to generate client letter.\nError: " + e.Message);
            }
        }
        
        private void GenerateWordDocument(EstateProjectionOptions options, Stream output)
        {
            FlowDocument clientReport = this.GenerateFlowDocument(options);
            TextRange textRange = new TextRange(clientReport.ContentStart, clientReport.ContentEnd);
            
            using (MemoryStream ms = new MemoryStream())
            {
                textRange.Save(ms, DataFormats.Rtf);
                ms.Position = 0;

                using (StreamReader reader = new StreamReader(ms))
                {
                    StreamWriter writer = new StreamWriter(output);

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        line = line.Replace(Constants.PageBreakMarker, "\\page {\\footer\\pard\\qr Page \\chpgn  of {\\field{\\*\\fldinst  NUMPAGES }}\\par}");
                        writer.WriteLine(line);
                    }

                    writer.Flush();
                }
            }
        }

        private FlowDocument GenerateFlowDocument(EstateProjectionOptions options)
        {
            FlowDocument clientReport = new FlowDocument();

            this.AddPage(this.GenerateCoverPage(options), clientReport);
            this.AddPageBreak(clientReport);
            this.AddPage(this.GenerateIntroductionPage(options), clientReport);
            this.AddPageBreak(clientReport);
            this.AddPage(this.GenerateAssumptionsPage(options), clientReport);
            this.AddPageBreak(clientReport);

            NoPlanningScenario noPlanningScenario = new NoPlanningScenario(options, "ILLUSTRATION 1 - NO PLANNING");
            this.AddPage(this.GenerateNoPlanningExplanationPage(noPlanningScenario), clientReport);
            this.AddPage(ScenarioHelper.GenerateTrustLogisticsPage(noPlanningScenario), clientReport);

            BypassTrustScenario bypassTrustScenario = new BypassTrustScenario(options, "ILLUSTRATION 2 - BYPASS TRUST");
            this.AddPage(this.GenerateBypassTrustExplanationPage(bypassTrustScenario), clientReport);
            this.AddPage(ScenarioHelper.GenerateTrustLogisticsPage(bypassTrustScenario), clientReport);

            AnnualGiftingScenario annualGiftingScenario = new AnnualGiftingScenario(options, "ILLUSTRATION 3 - ANNUAL GIFTING");
            this.AddPage(this.GenerateAnnualGiftingExplanationPage(annualGiftingScenario), clientReport);
            this.AddPage(ScenarioHelper.GenerateTrustLogisticsPage(annualGiftingScenario), clientReport);

            DiscountedGiftingScenario discountedGiftingScenario = new DiscountedGiftingScenario(options, "ILLUSTRATION 4 - DISCOUNTED GIFTING");
            this.AddPage(this.GenerateDiscountedGiftingExplanationPage(annualGiftingScenario, discountedGiftingScenario), clientReport);
            this.AddPage(ScenarioHelper.GenerateTrustLogisticsPage(discountedGiftingScenario), clientReport);

            LifeInsuranceScenario lifeInsuranceScenario = new LifeInsuranceScenario(options, "ILLUSTRATION 5 - LIFE INSURANCE TRUST");
            this.AddPage(this.GenerateLifeInsuranceExplanationPage(discountedGiftingScenario, lifeInsuranceScenario), clientReport);
            this.AddPage(ScenarioHelper.GenerateTrustLogisticsPage(lifeInsuranceScenario), clientReport);

            InstallmentSaleScenario installmentSaleScenario = new InstallmentSaleScenario(options, "ILLUSTRATION 6 - INSTALLMENT SALE");
            this.AddPage(this.GenerateInstallmentSaleLetterPage(discountedGiftingScenario, installmentSaleScenario), clientReport);
            this.AddPage(ScenarioHelper.GenerateTrustLogisticsPage(installmentSaleScenario), clientReport);

            return clientReport;
        }

        private void AddPage(UIElement from, FlowDocument to)
        {
            Image image = this.RenderToImage(from);
            to.Blocks.Add(new BlockUIContainer(image));
        }

        private void AddPage(FlowDocument from, FlowDocument to)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                TextRange fromRange = new TextRange(from.ContentStart, from.ContentEnd);
                fromRange.Save(ms, DataFormats.Rtf);

                TextRange toRange = new TextRange(to.ContentEnd, to.ContentEnd);
                toRange.Load(ms, DataFormats.Rtf);
            }
        }

        private void AddPageBreak(FlowDocument flowDocument)
        {
            flowDocument.Blocks.Add(new Paragraph(new Run(Constants.PageBreakMarker)));
        }

        private FlowDocument GeneratePage(string pageResourceName, ViewModel.ViewModel pageViewModel)
        {
            string pageXaml = AssemblyResourceHelper.ReadResource(Assembly.GetAssembly(typeof(ClientReportGenerator)), pageResourceName);
            FlowDocument page = (FlowDocument)XamlReader.Parse(pageXaml);
            page.DataContext = pageViewModel;
            this.EnsureDatabindingCompleted();
            return page;
        }

        private FlowDocument GenerateCoverPage(EstateProjectionOptions options)
        {
            return this.GeneratePage(
                "EstateView.View.ClientLetter.CoverPage.xaml",
                new CoverPageViewModel(options));
        }

        private FlowDocument GenerateIntroductionPage(EstateProjectionOptions options)
        {
            return this.GeneratePage(
                "EstateView.View.ClientLetter.IntroductionPage.xaml",
                new IntroductionPageViewModel(options));
        }

        private FlowDocument GenerateAssumptionsPage(EstateProjectionOptions options)
        {
            return this.GeneratePage(
                "EstateView.View.ClientLetter.AssumptionsPage.xaml",
                new AssumptionsPageViewModel(options));
        }

        private FlowDocument GenerateNoPlanningExplanationPage(NoPlanningScenario scenario)
        {
            return this.GeneratePage(
                "EstateView.View.ClientLetter.NoPlanningPage.xaml",
                new NoPlanningPageViewModel(scenario));
        }

        private FlowDocument GenerateBypassTrustExplanationPage(BypassTrustScenario scenario)
        {
            return this.GeneratePage(
                "EstateView.View.ClientLetter.BypassTrustPage.xaml",
                new BypassTrustPageViewModel(scenario));
        }

        private FlowDocument GenerateAnnualGiftingExplanationPage(AnnualGiftingScenario scenario)
        {
            return this.GeneratePage(
                "EstateView.View.ClientLetter.AnnualGiftingPage.xaml",
                new AnnualGiftingPageViewModel(scenario));
        }

        private FlowDocument GenerateDiscountedGiftingExplanationPage(AnnualGiftingScenario annualGiftingScenario, DiscountedGiftingScenario discountedGiftingScenario)
        {
            return this.GeneratePage(
            "EstateView.View.ClientLetter.DiscountedGiftingPage.xaml",
            new DiscountedGiftingPageViewModel(annualGiftingScenario, discountedGiftingScenario));
        }

        private FlowDocument GenerateLifeInsuranceExplanationPage(DiscountedGiftingScenario discountedGiftingScenario, LifeInsuranceScenario lifeInsuranceScenario)
        {
            return this.GeneratePage(
            "EstateView.View.ClientLetter.LifeInsurancePage.xaml",
            new LifeInsurancePageViewModel(discountedGiftingScenario, lifeInsuranceScenario));
        }

        private FlowDocument GenerateInstallmentSaleLetterPage(DiscountedGiftingScenario discountedGiftingScenario, InstallmentSaleScenario installmentSaleScenario)
        {
            return this.GeneratePage(
                "EstateView.View.ClientLetter.InstallmentSalePage.xaml",
                new InstallmentSalePageViewModel(discountedGiftingScenario, installmentSaleScenario));
        }

        private void EnsureDatabindingCompleted()
        {
            Dispatcher.CurrentDispatcher.Invoke(
                DispatcherPriority.Background,
                new Action(delegate { }));
        }

        private Image RenderToImage(UIElement pageElement)
        {
            Viewbox viewbox = new Viewbox();
            viewbox.Stretch = Stretch.Uniform;
            viewbox.Width = Constants.ScreenPageSize.Width - (Constants.ScreenPageMargin.Left + Constants.ScreenPageMargin.Right);
            viewbox.Height = Constants.ScreenPageSize.Height - (Constants.ScreenPageMargin.Top + Constants.ScreenPageMargin.Bottom);
            viewbox.Child = pageElement;

            viewbox.Measure(new Size(viewbox.Width, viewbox.Height));
            viewbox.Arrange(new Rect(new Size(viewbox.Width, viewbox.Height)));
            viewbox.UpdateLayout();

            RenderTargetBitmap target = new RenderTargetBitmap(
                (int)(viewbox.Width * (Constants.PrintDotsPerInch / Constants.ScreenDotsPerInch)),
                (int)(viewbox.Height * (Constants.PrintDotsPerInch / Constants.ScreenDotsPerInch)),
                Constants.PrintDotsPerInch,
                Constants.PrintDotsPerInch,
                PixelFormats.Pbgra32);

            target.Render(viewbox);

            return new Image { Source = target };
        }
    }
}
