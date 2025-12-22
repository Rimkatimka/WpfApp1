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
    /// Логика взаимодействия для TovarAdd.xaml
    /// </summary>
    public partial class TovarAdd : Window
    {
        DemoMuhEntities db = new();
        public Tovar tovar1;
        private bool isNew;
        public TovarAdd(DemoMuhEntities context, Tovar tovar)
        {
            InitializeComponent();

            db = context;

            if (tovar == null)
            {
                tovar1 = new Tovar();
                isNew = true;
                ComboBoxCategory.ItemsSource = db.Categories.ToList();

            }
            else
            {
                tovar1 = tovar;
                isNew = false;
                TextBoxArticul.Text = tovar.Articul;
                TextBoxName.Text = tovar.Name;
                ComboBoxCategory.ItemsSource = db.Categories.ToList();
                ComboBoxCategory.Text = tovar.Category;
                TextBoxCost.Text = tovar.Cost.ToString();
                TextBoxCount.Text = tovar.Count.ToString();
                TextBoxSale.Text = tovar.Sale.ToString();
            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (isNew)
                {
                    tovar1.Articul = (TextBoxArticul.Text.Trim());
                }
                tovar1.Name = TextBoxName.Text;
                tovar1.Category = ComboBoxCategory.Text;
                tovar1.Cost = Convert.ToDouble(TextBoxCost.Text);
                tovar1.Count = Convert.ToDouble(TextBoxCount.Text);
                tovar1.Sale = Convert.ToDouble(TextBoxSale.Text);
                tovar1.Image = "/images/pictures.png";
                if (isNew)
                {
                    db.Tovar.Add(tovar1);
                }
                db.SaveChanges();
                Close();
            }
            catch { }
        }

        private void ButtonImageAdd_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "Picture Files (.png)|*.png";
            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                tovar1.Image = dialog.FileName;
            }

        }
    }
}
