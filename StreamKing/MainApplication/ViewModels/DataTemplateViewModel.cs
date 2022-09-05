using StreamKing.Data.Media;
using StreamKing.MainApplication;

namespace StreamKing.MainApplication.ViewModels
{
    public class DataTemplateViewModel
    {
        public DataTemplateViewModel()
        { 
        }
        public Media Media { get; set; }

        public DataTemplateViewModel(Media media)
        {
            Media = media;
            
        }

    }
}
