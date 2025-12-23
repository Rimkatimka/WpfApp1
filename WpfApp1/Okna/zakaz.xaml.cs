using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        Users user;
        public zakaz(Users user1)
        {
            user = user1;
            InitializeComponent();
            TextBoxFIO.Text = user.FIO;

            AddButton.Visibility = Visibility.Collapsed;
            DeleteButton.Visibility = Visibility.Collapsed;
            ZakazList.MouseDoubleClick -= ZakazList_MouseDoubleClick;

            if (user.Role == "Администратор")
            {
                AddButton.Visibility = Visibility.Visible;
                DeleteButton.Visibility = Visibility.Visible;
                ZakazList.MouseDoubleClick += ZakazList_MouseDoubleClick;
            }
            Update();
        }
        public void Update()
        {
            ZakazList.ItemsSource = null;
            ZakazList.ItemsSource = db.OrderTovar.ToList();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            viewing window = new viewing(user);
            window.Show();
            Close();
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            new ZakazAdd(db, null, user).ShowDialog();
        }

        private void ZakazList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new ZakazAdd(db, ZakazList.SelectedItem as OrderTovar, user).ShowDialog();
            Update();
        }
        private void DeleteButtonc(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ZakazList.SelectedItem is OrderTovar selected)
                {
                    if (MessageBox.Show("Точно хотите удалить?", "Подтверждение", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                    {
                        try
                        {

                            db.OrderTovar.Remove(selected);
                            db.SaveChanges();
                            Update();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch { }
        }
    }
}
