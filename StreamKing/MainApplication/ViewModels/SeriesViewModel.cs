using StreamKing.Data.Media;
using StreamKing.MainApplication;

namespace StreamKing.MainApplication.ViewModels
{
    public class SeriesViewModel
    {
        public SeriesViewModel()
        { 
        }
        public Media Media { get; set; }

        public SeriesViewModel(Media media)
        {
            Media = media;
            
        }

    }
}
