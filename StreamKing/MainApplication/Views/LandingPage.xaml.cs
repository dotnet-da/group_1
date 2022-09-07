using System.Windows.Controls;

namespace StreamKing.MainApplication.Views
{
    /// <summary>
    /// Interaction logic for LandingPage.xaml
    /// </summary>
    public partial class LandingPage : UserControl
    {
        public LandingPage()
        {
            InitializeComponent();
            SetMediaList();
        }

        public void SetMediaList()
        {
            DiscoverMediaListView.ItemsSource = App._mediaList;
        }
    }
}
