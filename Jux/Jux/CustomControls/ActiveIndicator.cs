using System.Threading.Tasks;
using Xamarin.Forms;

namespace Jux.CustomControls
{
    public class ActiveIndicator
    {
        public static async Task<StackLayout> DisplayBusy()
        {
            ActivityIndicator busyState = new ActivityIndicator()
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                WidthRequest = 100,
                HeightRequest = 100,
                Color = Color.OrangeRed
            };
            busyState.IsRunning = true;

            StackLayout StackResults = new StackLayout()
            {
                VerticalOptions = LayoutOptions.End,
                WidthRequest = 100,
                HeightRequest = 100
            };

            await Task.Run(() =>
            {
                StackResults.Children.Add(busyState);
            });

            return StackResults;
        }
    }
}
