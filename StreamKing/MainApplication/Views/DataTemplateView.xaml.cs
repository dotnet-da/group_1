using StreamKing.Data.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StreamKing.MainApplication.Views
{
    /// <summary>
    /// Interaktionslogik für Window1.xaml
    /// </summary>
    public partial class DataTemplateView : UserControl
    {
        public DataTemplateView()
        {
            InitializeComponent();
        }

        private void MediaTemplateButton_Clicked(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("MediaTemplateButton_Clicked");
            if(DataContext is not null)
            {
                Media currentMedia = (Media)DataContext;
                Console.WriteLine("(" + currentMedia.GetType() + ")" + currentMedia.TmdbId + ": " + currentMedia.Title);
            }
        }
    }
}
