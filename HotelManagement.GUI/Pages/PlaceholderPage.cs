using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HotelManagement.GUI.Pages
{
    public class PlaceholderPage : Page
    {
        public PlaceholderPage(string title)
        {
            Background = Brushes.Transparent;

            Grid root = new Grid();
            TextBlock text = new TextBlock
            {
                Text = title,
                FontSize = 30,
                FontWeight = FontWeights.SemiBold,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0F172A")),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            root.Children.Add(text);
            Content = root;
        }
    }
}
