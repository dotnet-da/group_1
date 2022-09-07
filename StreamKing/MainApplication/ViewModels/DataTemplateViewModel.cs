using StreamKing.Data.Media;
using StreamKing.MainApplication;
using System;
using System.Windows;
using System.Windows.Controls;


namespace StreamKing.MainApplication.ViewModels
{
    public class DataTemplateViewModel : BaseViewModel
    {
        public string? Title { get; set; }
        public string? Provider { get; set; }
        public string SeasonString { get; set; }
        public int? AmountSeasons { get; set; }
        public string? Tag { get; set; }
        public float Rating { get; set; }
        public string? TagColor { get; set; } 
        public string? SeasonOrRuntimeInformation { get; set; }
        public Media? Media { get; set; }
        public string? ImageURL;
        public Uri ImageUri { get; set; }

        /*
         * ProprtyChanged for visibility of StarRating
         */
        private Visibility visibilityOne = Visibility.Visible;
        public Visibility VisibilityOne
        {
            get
            {
                return visibilityOne;
            }
            set
            {
                visibilityOne = value;

                NotifyPropertyChanged("VisibilityOne");
            }
        }

        // Visibility star 2
        private Visibility visibilityTwo;
        public Visibility VisibilityTwo
        {
            get
            {
                return visibilityTwo;
            }
            set
            {
                visibilityTwo = value;

                NotifyPropertyChanged("VisibilityTwo");
            }
        }

        // Visibility Star 3
        private Visibility visibilityThree;
        public Visibility VisibilityThree
        {
            get
            {
                return visibilityThree;
            }
            set
            {
                visibilityThree = value;

                NotifyPropertyChanged("VisibilityThree");
            }
        }

        // Visibility Star 4
        private Visibility visibilityFour;
        public Visibility VisibilityFour
        {
            get
            {
                return visibilityFour;
            }
            set
            {
                visibilityFour = value;

                NotifyPropertyChanged("VisibilityFour");
            }
        }

        // Visibility Star 5
        private Visibility visibilityFive;
        public Visibility VisibilityFive
        {
            get
            {
                return visibilityFive;
            }
            set
            {
                visibilityFive = value;

                NotifyPropertyChanged("VisibilityFive");
            }
        }

        public DataTemplateViewModel()
        {
            // Media Debbing Object
            Media = new Media();
            Media.BackdropURL = "https://image.tmdb.org/t/p/w500/eSVvx8xys2NuFhl8fevXt41wX7v.jpg";
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
            showRatingStars(Rating);
            ImageURL = Media.BackdropURL;
            ImageUri = new Uri(Media.BackdropURL);
        }

        // trims the title if it es too long too fit in the titlebox
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

        // Calculates the movie runtime from minutes in a hour and Minutes Format
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

        public void showRatingStars(float rating)
        {
            if (rating < 1.5f)
            {
                showOneStar();
            }
            else if (rating >= 1.5f && rating < 2.5f)
            {
                showTwoStar();
            }
            else if (rating >= 2.5f && rating < 3.5f)
            {
                showThreeStar();
            }
            else if (rating >= 3.5f && rating < 4.5f)
            {
                showFourStar();
            }
            else 
            {
                showFiveStars();
            }
        }

        private void showFiveStars()
        {
            VisibilityOne = Visibility.Visible;
            VisibilityTwo = Visibility.Visible;
            VisibilityThree = Visibility.Visible;
            VisibilityFour = Visibility.Visible;
            VisibilityFive = Visibility.Visible;
        }

        private void showFourStar()
        {
            VisibilityOne = Visibility.Visible;
            VisibilityTwo = Visibility.Visible;
            VisibilityThree = Visibility.Visible;
            VisibilityFour = Visibility.Visible;
            VisibilityFive = Visibility.Hidden;
        }

        private void showThreeStar()
        {
            VisibilityOne = Visibility.Visible;
            VisibilityTwo = Visibility.Visible;
            VisibilityThree = Visibility.Visible;
            VisibilityFour = Visibility.Hidden;
            VisibilityFive = Visibility.Hidden;
        }

        private void showTwoStar()
        {
            VisibilityOne = Visibility.Visible;
            VisibilityTwo = Visibility.Visible;
            VisibilityThree = Visibility.Hidden;
            VisibilityFour = Visibility.Hidden;
            VisibilityFive = Visibility.Hidden;
        }

        private void showOneStar()
        {
            VisibilityOne = Visibility.Visible;
            VisibilityTwo = Visibility.Hidden;
            VisibilityThree = Visibility.Hidden;
            VisibilityFour = Visibility.Hidden;
            VisibilityFive = Visibility.Hidden;
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
