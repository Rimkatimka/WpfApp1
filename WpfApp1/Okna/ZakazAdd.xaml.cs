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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp1.DB;

namespace WpfApp1.Okna
{
    /// <summary>
    /// Логика взаимодействия для ZakazAdd.xaml
    /// </summary>
    public partial class ZakazAdd : Window
    {        
        DemoMuhEntities db = new();
        public OrderTovar order;
        private bool isNew;
        public ZakazAdd(DemoMuhEntities context, OrderTovar order1)
        {
            InitializeComponent();

            db = context;

            if (order1 == null)
            {
                order = new OrderTovar();
                isNew = true;
                ComboBoxAddress.ItemsSource = db.Addresses.ToList();
                ComboBoxStatus.ItemsSource = db.Statuses.ToList();
            }
            else
            {                
                order = order1;
                isNew = false;
                TextBoxArticul.Text = order1.Articul;
                ComboBoxAddress.ItemsSource = db.Addresses.ToList();
                ComboBoxAddress.ItemsSource = db.Addresses.Where(p => p.IDAddress == order1.Orders.AddressDilivery).ToList();
                ComboBoxStatus.ItemsSource = order1.Orders.Status;
                DPDateDilivery.SelectedDate = order1.Orders.DateDilivery;
                DPDateOrder.SelectedDate = order1.Orders.DateOrder;                    
            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (isNew)
                {
                    order.Articul = (TextBoxArticul.Text.Trim());
                }
                order.Articul = TextBoxArticul.Text;
                //order.Orders.Status = db.Statuses.Where(p=> p.Status == ComboBoxStatus.Text.ToString());
                order.Orders.AddressDilivery = ComboBoxAddress.SelectedIndex;
                order.Orders.DateOrder = DPDateOrder.SelectedDate;
                order.Orders.DateDilivery = DPDateDilivery.SelectedDate;

                if (isNew)
                {
                    db.OrderTovar.Add(order);
                }
                db.SaveChanges();
                Close();
            }
            catch { }
        }

    }
}
