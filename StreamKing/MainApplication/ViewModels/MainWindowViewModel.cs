namespace StreamKing.MainApplication.ViewModels
{
    public class MainWindowViewModel
    {
        public string? ActiveRegionImage { get; set; }

        public MainPage? MainPage { get; set; }

        public MainWindowViewModel()
        {
            MainPage = new LandingPageViewModel();
        }
    }

    public class MainPage
    {

    }
}
