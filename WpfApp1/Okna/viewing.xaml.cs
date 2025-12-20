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
        private List<Tovar> allTovars;

        public viewing()
        {
            InitializeComponent();

            allTovars = db.Tovar.ToList();

            ComboBoxCategory.ItemsSource = db.Categories.ToList();
            ComboBoxCreator.ItemsSource = db.Suppliers.ToList();

            ComboBoxCategory.SelectedIndex = 2;
            ComboBoxCreator.SelectedIndex = 2;
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

            if (ComboBoxCategory.SelectedItem is Categories selectedCategory)
            {
                filtered = filtered.Where(p => p.Categories != null &&
                                               p.Categories.Category == selectedCategory.Category);
            }

            if (ComboBoxCreator.SelectedItem is Suppliers selectedSupplier)
            {
                filtered = filtered.Where(p => p.Suppliers != null &&
                                               p.Suppliers.Supplier == selectedSupplier.Supplier);
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

        private void FilterCatChanged(object sender, EventArgs e) => ApplyFiltersAndSort();
        private void FilterCreatChanged(object sender, EventArgs e) => ApplyFiltersAndSort();
        private void SortSelectionChanged(object sender, EventArgs e) => ApplyFiltersAndSort();
        private void TextChanged(object sender, EventArgs e) => ApplyFiltersAndSort();

        private void ResetFilters_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxCategory.SelectedIndex = -1;
            ComboBoxCreator.SelectedIndex = -1;
            TextBoxSearch.Clear();
            ComboBoxSort.SelectedIndex = 0;

            ApplyFiltersAndSort();
        }

        private void ApplyFilters_Click(object sender, RoutedEventArgs e)
        {
            ApplyFiltersAndSort();
        }
    }
}