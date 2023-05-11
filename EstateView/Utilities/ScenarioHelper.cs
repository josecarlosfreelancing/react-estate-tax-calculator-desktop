using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using EstateView.Core.Model.Scenarios;
using EstateView.View;
using EstateView.ViewModel.Logistics;

namespace EstateView.Utilities
{
    public static class ScenarioHelper
    {
        public static ImageSource GetThumbnailForScenario(EstatePlanningScenario scenario, double width, double height)
        {
            UIElement trustLogisticsPage = ScenarioHelper.GenerateTrustLogisticsPage(scenario);
            return ScenarioHelper.RenderToImage(trustLogisticsPage, width, height);
        }

        public static UIElement GenerateTrustLogisticsPage(EstatePlanningScenario scenario)
        {
            BoxesViewModel boxesViewModel = new BoxesViewModel(scenario);
            BoxesView boxesView = new BoxesView();
            boxesView.DataContext = boxesViewModel;

            return boxesView;
        }

        private static ImageSource RenderToImage(UIElement pageElement, double width, double height)
        {
            Viewbox viewbox = new Viewbox();
            viewbox.Stretch = Stretch.Uniform;
            viewbox.Width = width;
            viewbox.Height = height;
            viewbox.Child = pageElement;

            viewbox.Measure(new Size(viewbox.Width, viewbox.Height));
            viewbox.Arrange(new Rect(new Size(viewbox.Width, viewbox.Height)));
            viewbox.UpdateLayout();

            RenderTargetBitmap target = new RenderTargetBitmap(
                (int)width,
                (int)height,
                96,
                96,
                PixelFormats.Pbgra32);

            target.Render(viewbox);

            return target;
        }
    }
}
