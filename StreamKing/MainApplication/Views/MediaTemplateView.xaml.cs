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
                Media currentMedia = (Media)DataContext;
                Console.WriteLine("(" + currentMedia.GetType() + ")" + currentMedia.TmdbId + ": " + currentMedia.Title);

                ((MainWindow)Window.GetWindow(this)).SetSelectedMedia(currentMedia);
            }
        }
    }
}
