using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.DB;

namespace WpfApp1.Okna
{
    public partial class viewing : Window
    {
        DemoMuhEntities db = new();
        private List<Tovar> allTovars; // Кэшируем все товары для производительности

        public viewing(Users user)
        {
            InitializeComponent();
            AddButton.Visibility = Visibility.Hidden;
            ResetButton.Visibility = Visibility.Hidden;
            DeleteButton.Visibility = Visibility.Hidden;
            Filters.Visibility = Visibility.Hidden;
            ListTovars.MouseDoubleClick -= ListTovars_MouseDoubleClick;
            if (user != null)
            {
                TextBoxFIO.Text = user.FIO;

                if (user.Role == "Администратор")
                {
                    AddButton.Visibility = Visibility.Visible;
                    ResetButton.Visibility = Visibility.Visible;
                    DeleteButton.Visibility = Visibility.Visible;
                    Filters.Visibility = Visibility.Visible;
                    ListTovars.MouseDoubleClick += ListTovars_MouseDoubleClick;
                }
                if (user.Role == "Менеджер")
                {
                    Filters.Visibility = Visibility.Visible;
                }
            }
            
            Update();
        }
        public void Update()
        {
            ListTovars.ItemsSource = null;


            allTovars = db.Tovar.ToList();

            ComboBoxCreator.ItemsSource = db.Suppliers.ToList();

            ComboBoxSort.SelectedIndex = 0;

            ApplyFiltersAndSort();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new auto().Show();
            Close();
        }

        private void ApplyFiltersAndSort()
        {
            if (allTovars == null)
                return;

            var filtered = allTovars.AsQueryable();

            if (ComboBoxCreator.SelectedItem is Suppliers selectedSupplier)
            {
                if (selectedSupplier.Supplier != "По умолчанию")
                {
                    filtered = filtered.Where(p => p.Suppliers != null &&
                                                   p.Suppliers.Supplier == selectedSupplier.Supplier);
                }
            }

            if (!string.IsNullOrWhiteSpace(TextBoxSearch?.Text))
            {
                string searchText = TextBoxSearch.Text.Trim().ToLower();
                filtered = filtered.Where(p => p.Name != null &&
                                               p.Name.ToLower().Contains(searchText));
            }

            var listToSort = filtered.ToList();
            var sortedList = SortItems(listToSort);

            ListTovars.ItemsSource = sortedList;
        }

        private List<Tovar> SortItems(List<Tovar> items)
        {
            if (ComboBoxSort.SelectedItem is ComboBoxItem selectedItem)
            {
                switch (selectedItem.Content.ToString())
                {
                    case "По возрастанию цены":
                        return items.OrderBy(t => t.Cost).ToList();
                    case "По убыванию цены":
                        return items.OrderByDescending(t => t.Cost).ToList();
                    default:
                        return items;
                }
            }
            return items;
        }

        private void FilterCreatChanged(object sender, EventArgs e) => ApplyFiltersAndSort();
        private void SortSelectionChanged(object sender, EventArgs e) => ApplyFiltersAndSort();
        private void TextChanged(object sender, EventArgs e) => ApplyFiltersAndSort();

        private void ResetFilters_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxCreator.SelectedIndex = -1;
            TextBoxSearch.Clear();
            ComboBoxSort.SelectedIndex = 0;

            ApplyFiltersAndSort();
        }

        private void ApplyFilters_Click(object sender, RoutedEventArgs e)
        {
            ApplyFiltersAndSort();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new TovarAdd(db, null).ShowDialog();
            ListTovars.SelectedItem = null;
            Update();
        }

        private void ListTovars_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            new TovarAdd(db, ListTovars.SelectedItem as Tovar).ShowDialog();
            Update();
        }

        private void DeleteButtonc(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ListTovars.SelectedItem is Tovar selected)
                {
                    if (MessageBox.Show("Точно хотите удалитьб?", "Подтверждение", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                    {
                        try
                        {

                            db.Tovar.Remove(selected);
                            db.SaveChanges();
                            Update();
                        }
                        catch(Exception ex)
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