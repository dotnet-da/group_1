using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StreamKing.MainApplication.Views
{
    /// <summary>
    /// Interaction logic for DetailedMediaTemplateView.xaml
    /// </summary>
    public partial class DetailedMediaTemplateView : UserControl
    {
        public DetailedMediaTemplateView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            App._mainWindow.SetSelectedMedia(null);
        }
    }
}
