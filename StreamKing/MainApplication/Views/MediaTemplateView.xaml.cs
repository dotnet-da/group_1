using StreamKing.Data.Media;
using System;
using System.Windows;
using System.Windows.Controls;

namespace StreamKing.MainApplication.Views
{
    /// <summary>
    /// Interaktionslogik für Window1.xaml
    /// </summary>
    public partial class MediaTemplateView : UserControl
    {
        public MediaTemplateView()
        {
            InitializeComponent();
        }

        private void MediaTemplateButton_Clicked(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("MediaTemplateButton_Clicked");
            if (DataContext is not null)
            {
                try
                {
                    Media currentMedia = (Media)DataContext;
                    Console.WriteLine("(" + currentMedia.GetType() + ")" + currentMedia.TmdbId + ": " + currentMedia.Title);

                    ((MainWindow)Window.GetWindow(this)).SetSelectedMedia(currentMedia);
                }
                catch (Exception)
                {
                    Console.WriteLine("Current DataContext is not of Type Media, now trying MovieEntry");
                    try
                    {
                        MovieEntry currentMedia = (MovieEntry)DataContext;
                        Console.WriteLine("(" + currentMedia.GetType() + ")" + currentMedia.Movie.TmdbId + ": " + currentMedia.Movie.Title);

                        ((MainWindow)Window.GetWindow(this)).SetSelectedMovieEntry(currentMedia);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Current DataContext is not of Type Media, now trying SeriesEntry");
                        try
                        {
                            SeriesEntry currentMedia = (SeriesEntry)DataContext;
                            Console.WriteLine("(" + currentMedia.GetType() + ")" + currentMedia.Series.TmdbId + ": " + currentMedia.Series.Title);

                            ((MainWindow)Window.GetWindow(this)).SetSelectedSeriesEntry(currentMedia);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("No types suited, doing nothing.");
                        }
                    }

                }

            }
        }
    }
}
