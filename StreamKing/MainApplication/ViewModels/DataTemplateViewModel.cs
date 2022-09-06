using StreamKing.Data.Media;
using StreamKing.MainApplication;
using System;

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
        public string? SeasonOrRuntimeInformation { get; set; }
        public Media? Media { get; set; }

        public DataTemplateViewModel()
        {
            // Media Debbing Object
            Media = new Media();
            Media.Title="Ich weiß noch immer was du letzten Sommer getan hast";
            Media.Tagline="UNWATCHED";
            Media.StreamingInfos =  new System.Collections.Generic.List<StreamingInfo>();
            StreamingInfo streamingInfo = new StreamingInfo();
            streamingInfo.Name = "Test Provider";
            Media.StreamingInfos.Add(streamingInfo);


            Title = trimTitle(Media.Title);
            Tag = Media.Tagline;
            Provider = Media.StreamingInfos[0].Name;
            SeasonOrRuntimeInformation = calculateHours(135);
            SeasonString = "Seasons";
            AmountSeasons = 3;
            TagColor = "Red";
            Rating = 0.5f;


        }

        private string? trimTitle(string title)
        {
            int titleLength = title.Length;
            string returnString = title;
            if (titleLength == 0)
            {
                returnString = "No Title available";
                return returnString;
            }
            else if (titleLength > 38)
            {
                returnString = string.Concat(title.AsSpan(0, 36), "...");
                return returnString;
            }
            else
            {
                return returnString;
            }
        }

        private string? calculateHours(int minutesInput)
        {
            int hours;
            int minutes;

            if (minutesInput == 0)
            {
                return "No time available";
            }
            hours = minutesInput / 60;
            minutes = minutesInput % 60;

            string returnTime = hours + "h " + minutes + "min";
            return returnTime;

        }

        public DataTemplateViewModel(Media media)
        {
            Media = media;
            Rating = 5;
            //Title = media.Title;

            SeasonOrRuntimeInformation = "1h 30min";
            TagColor = "Red";

            
        }

    }
}
