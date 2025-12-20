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
using System.Windows.Shapes;
using WpfApp1.DB;

namespace WpfApp1.Okna
{
    /// <summary>
    /// Логика взаимодействия для viewing.xaml
    /// </summary>
    /// 
    public partial class viewing : Window
    {

        DemoMuhEntities db = new();
        public viewing()
        {
            InitializeComponent();

            ListTovars.ItemsSource = db.Tovar.ToList();

            ComboBoxCategory.ItemsSource = db.Categories.ToList();

            ComboBoxCreator.ItemsSource = db.Suppliers.ToList();

            ComboBoxSort.SelectedIndex = 0;
            ComboBoxCategory.SelectedIndex = 2;
            ComboBoxCreator.SelectedIndex = 2;
            //result = result.Where(w => w.Nazvanie.IndexOf(tbSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }
        public void SearchTextChanged(object sender, TextChangedEventArgs e)
        {
            //Update();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Window();
            window = new auto();
            window.Show();
            Close();
        }

        private void ResetFilters_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ApplyFilters_Click(object sender, RoutedEventArgs e)
        {

        }
        private void FilterChanged(object sender, RoutedEventArgs e)
        {
            //Update();
        }
        private void FilterCreatChanged(object sender, RoutedEventArgs e)
        {
            var import = db.Tovar.ToList();
            if (ComboBoxCreator.SelectedItem != null && ComboBoxCreator.Text != "По умолчанию")
            {
                import = import.Where(p => p.Suppliers.Supplier.Contains((ComboBoxCreator.SelectedItem as Suppliers).Supplier)).ToList();
            }
        }
        private void FilterCatChanged(object sender, RoutedEventArgs e)
        {
            var import = db.Tovar.ToList();
            if (ComboBoxCategory.SelectedItem != null && ComboBoxCategory.Text != "По умолчанию")
            {
                import = import.Where(p => p.Categories.Category.Contains((ComboBoxCategory.SelectedItem as Categories).Category)).ToList();
            }
            SortChanged(import);
        }
        private void SortChanged(IEnumerable<Tovar> import)
        {
            switch (ComboBoxSort.Text)
            {
                case "По возрастанию цены":
                    ListTovars.ItemsSource = import.OrderByDescending(t => t.Cost);
                    break;
                case "По убыванию цены":
                    ListTovars.ItemsSource = import.OrderBy(t => t.Cost);
                    break;
                case "По умолчанию":
                    ListTovars.ItemsSource = import;
                    break;
            }
        }

        private void TextChanged(object sender, RoutedEventArgs e)
        {
            var import = db.Tovar.ToList();
            if (!string.IsNullOrEmpty(TextBoxSearch.Text))
            {
                import = import.Where(p => p.Name.ToLower().Contains(TextBoxSearch.Text.ToLower())).ToList();
            }
            SortChanged(import);
        }
    } 
}
