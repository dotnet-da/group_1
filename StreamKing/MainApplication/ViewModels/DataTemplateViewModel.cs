using StreamKing.Data.Media;
using StreamKing.MainApplication;

namespace StreamKing.MainApplication.ViewModels
{
    public class DataTemplateViewModel : BaseViewModel
    {
        public string? Title { get; set; }
        public string? Provider { get; set; }
        public string SeasonString { get; set; }
        public int? AmountSeasons { get; set; }
        public string? Tag { get; set; }
        public float? Rating { get; set; }
        public string? TagColor { get; set; }

        public DataTemplateViewModel()
        {
            SeasonString = "Seasons";
            Title="Test Title";
            Tag="Tag";
            TagColor = "Red";
            Rating = 5;
            Provider =  "Test Provider";
            AmountSeasons = 3;

        }
        public Media? Media { get; set; }

        public DataTemplateViewModel(Media media)
        {
            Media = media;
            Title = media.Title;
            Rating = media.TmdbId;
            TagColor = "Red";

            
        }

    }
}
