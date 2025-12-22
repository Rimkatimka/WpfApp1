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
using WpfApp1.DB;
using WpfAnimatedGif;

namespace WpfApp1.Okna
{
    /// <summary>
    /// Логика взаимодействия для auto.xaml
    /// </summary>
    public partial class auto : Window
    {
        DemoMuhEntities db = new();
        public auto()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var user = db.Users.FirstOrDefault(u => u.Login == Login.Text && u.Password == Password.Text);
            if (user != null)
            {
                Window window = new Window();
                window = new viewing(db.Users.FirstOrDefault(u => u.Login == Login.Text && u.Password == Password.Text));
                window.Show();
                Close();
            }
            else
            {
                this.WindowStyle = WindowStyle.None;
                this.WindowState = WindowState.Normal;
                this.ResizeMode = ResizeMode.NoResize;
                this.Topmost = true;
                this.Left = 0;
                this.Top = 0;
                this.Width = SystemParameters.PrimaryScreenWidth;
                this.Height = SystemParameters.PrimaryScreenHeight;
                this.Background = new SolidColorBrush(Colors.Black);

                Giff.Visibility = Visibility.Visible;
                var controller = ImageBehavior.GetAnimationController(Giff);
                //ImageBehavior.SetRepeatBehavior(Giff, new RepeatBehavior(1)); // Установите повтор анимации на 1 раз
                controller.Play(); //Запуск анимации
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Window window = new Window();

            window = new viewing(null);
            window.Show();
            Close();
        }
        private void Giff_AnimationCompleted(object sender, RoutedEventArgs e)
        {
            Giff.Visibility = Visibility.Collapsed; // Скрыть GIF после завершения
            ImageBehavior.SetRepeatBehavior(Giff, new RepeatBehavior(0)); // Возвращаем повтор анимации в 0, чтобы больше не воспроизводилась
        }
    }
}
