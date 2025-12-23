using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
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
        public OrderTovar ordertovar;
        private bool isNew;
        Users user;
        public ZakazAdd(DemoMuhEntities context, OrderTovar order1,Users user1)
        {
            InitializeComponent();
            user = user1;

            db = context;

            
            if (order1 == null)
            {
                ordertovar = new OrderTovar();
                isNew = true;
                ComboBoxAddress.ItemsSource = db.Addresses.ToList();
                ComboBoxStatus.ItemsSource = db.Statuses.ToList();
                ComboBoxArticul.ItemsSource = db.OrderTovar.ToList();
            }
            else
            {                
                ordertovar = order1;
                isNew = false;

                ComboBoxArticul.ItemsSource = db.Tovar.ToList();
                ComboBoxArticul.Text = order1.Articul;

                ComboBoxAddress.ItemsSource = db.Addresses.ToList();
                ComboBoxAddress.Text = order1.Orders.Addresses.Address;

                ComboBoxStatus.ItemsSource = db.Statuses.ToList();
                ComboBoxStatus.Text = order1.Orders.Status.ToString();

                DPDateDilivery.SelectedDate = order1.Orders.DateDilivery;
                DPDateOrder.SelectedDate = order1.Orders.DateOrder;

                ComboBoxArticul.Visibility = Visibility.Collapsed;

            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
                        try
            {
                if (isNew)
                {
                    Orders order = new Orders
                    {
                        IDOrder = db.Orders.Max(p => p.IDOrder) + 1,
                        DateOrder = DPDateOrder.SelectedDate,
                        DateDilivery = DPDateDilivery.SelectedDate,
                        AddressDilivery = ComboBoxAddress.SelectedIndex + 1,
                        Status = ComboBoxStatus.Text.ToString(),
                        IDUser = user.IDUser
                    };

                    OrderTovar newordertovar = new OrderTovar
                    {
                        IDOrder = order.IDOrder,
                        Count = 1,
                        Articul = (ComboBoxArticul.Text.Trim())
                    };
                    db.Orders.Add(order);

                    db.SaveChanges();
                    db.OrderTovar.Add(newordertovar);

                    db.SaveChanges();
                }
                else
                {
                    ordertovar.Articul = ComboBoxArticul.Text;
                    ordertovar.Orders.Status = ComboBoxStatus.Text;
                    ordertovar.Orders.AddressDilivery = ComboBoxAddress.SelectedIndex + 1;
                    ordertovar.Orders.DateOrder = DPDateOrder.SelectedDate;
                    ordertovar.Orders.DateDilivery = DPDateDilivery.SelectedDate;
                }
                db.SaveChanges();
                Close();
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
