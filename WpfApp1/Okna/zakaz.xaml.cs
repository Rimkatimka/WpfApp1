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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfAnimatedGif;
using WpfApp1.DB;

namespace WpfApp1.Okna
{
    /// <summary>
    /// Логика взаимодействия для zakaz.xaml
    /// </summary>
    public partial class zakaz : Window
    {
        DemoMuhEntities db = new();
        public zakaz()
        {
            InitializeComponent();
            //DataContext = db.Users;
            ZakazList.ItemsSource = db.View_1.AsNoTracking().ToList();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Window();
            window = new auto();
            window.Show();
            Close();
        }
    }
}
