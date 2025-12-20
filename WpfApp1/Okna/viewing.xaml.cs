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

        public viewing()
        {
            InitializeComponent();

            allTovars = db.Tovar.ToList();

            ComboBoxCategory.ItemsSource = db.Categories.ToList();
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

            //if (ComboBoxCategory.SelectedItem is Categories selectedCategory)
            //{
            //    filtered = filtered.Where(p => p.Categories != null &&
            //                                   p.Categories.Category == selectedCategory.Category);
            //}

            //if (ComboBoxCreator.SelectedItem is ComboBoxItem supItem &&
            //    supItem.Tag?.ToString() == "По умолчанию")
            //{

            //}
            //else if(ComboBoxCreator.SelectedItem is Suppliers selectedSupplier)
            //{
            //    filtered = filtered.Where(p => p.Suppliers != null &&
            //                                   p.Suppliers.Supplier == selectedSupplier.Supplier);
            //}
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

        //private void FilterCatChanged(object sender, EventArgs e) => ApplyFiltersAndSort();
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
        //DemoMuhEntities db = new();
        //private List<Tovar> list;

        //public viewing()
        //{
        //    InitializeComponent();

        //    list = db.Tovar.ToList();

        //    ComboBoxCreator.ItemsSource = new[] { "По умолчанию" }
        //        .Concat(db.Suppliers.Select(s => s.Supplier))
        //        .ToList();


        //    foreach (var item in list)
        //    {
        //        ListTovars.Items.Add(item);
        //    }
        //}

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    new auto().Show();
        //    Close();
        //}
        //void SearchTovar()
        //{
        //    if (list != null)
        //    {
        //        var result = list;

        //        if (!string.IsNullOrEmpty(TextBoxSearch.Text))
        //        {
        //            result = result.
        //                Where(w => w.Name.ToLower().
        //                StartsWith(TextBoxSearch.Text.ToLower())).
        //                ToList();
        //        }

        //        if (ComboBoxCreator.SelectedIndex != -1 && ComboBoxCreator.SelectedIndex != 0)
        //        {
        //            result = result.
        //                Where(w => w.Supplier.
        //                StartsWith(ComboBoxCreator.Text)).
        //                ToList();
        //        }

        //        if (ComboBoxSort.SelectedIndex == 0)
        //        {
        //            result = result.
        //                OrderBy(o => o.Count).
        //                ToList();
        //        }

        //        if (ComboBoxSort.SelectedIndex == 1)
        //        {
        //            result = result.
        //                OrderByDescending(o => o.Count).
        //                ToList();
        //        }

        //        ListTovars.Items.Clear();

        //        foreach (var item in result)
        //        {
        //            ListTovars.Items.Add(item);
        //        }
        //    }
        //}

        //private void TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    SearchTovar();
        //}

        //private void FilterChanged(object sender, EventArgs e)
        //{
        //    SearchTovar();
        //}

        //private void SortSelectionChanged(object sender, EventArgs e)
        //{
        //    SearchTovar();
        //}
        //private void ResetFilters_Click(object sender, RoutedEventArgs e)
        //{
        //    ComboBoxCategory.SelectedIndex = -1;
        //    ComboBoxCreator.SelectedIndex = -1;
        //    TextBoxSearch.Clear();
        //    ComboBoxSort.SelectedIndex = 0;

        //    SearchTovar();
        //}
    }
}