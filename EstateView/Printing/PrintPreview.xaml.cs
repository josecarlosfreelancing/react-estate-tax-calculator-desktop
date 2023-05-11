using System.Windows;
using System.Windows.Documents;
using EstateView.Core;

namespace EstateView.Printing
{
    /// <summary>
    /// Interaction logic for PrintPreview.xaml
    /// </summary>
    public partial class PrintPreview : Window
    {
        private PrintPreview(FlowDocument flowDocument)
        {
            this.InitializeComponent();
            this.DocumentViewer.Document = flowDocument;
        }

        public static void Show(FlowDocument document)
        {
            Throw.IfNull(() => document);

            PrintPreview printPreview = new PrintPreview(document);
            printPreview.Owner = Application.Current.MainWindow;
            printPreview.ShowDialog();
        }
    }
}
