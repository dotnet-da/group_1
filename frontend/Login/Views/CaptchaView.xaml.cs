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
using System.Drawing;

namespace frontend.Login.Views
{
    /// <summary>
    /// Interaktionslogik für CaptchaView.xaml
    /// </summary>
    public partial class CaptchaView : UserControl
    {
        int num = 0;
        public CaptchaView()
        {
            InitializeComponent();
            LoadCaptcha();
        }

        private void LoadCaptcha()
        {
            Random random = new Random();
            num = random.Next(500, 1000);
            //var img = new Bitma
            
        }
    }
}
