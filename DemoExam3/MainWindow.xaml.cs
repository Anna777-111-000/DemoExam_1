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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DemoExam3
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadPartners();
        }

        private void LoadPartners()
        {
            using (var ctx = new Entities())
            {
                var list = ctx.Partners
                    .Select(p => new
                    {
                        p.PartnerID,
                        p.PartnerName,
                        p.Director,
                        p.Phone,
                        p.Reiting,
                        // Берем тип напрямую через Join
                        PartnerType = ctx.PartnerTypes
                            .Where(t => t.PartnerTypeID == p.PartnerTypeID)
                            .Select(t => t.PartnerType)
                            .FirstOrDefault() ?? "Не указан",
                        Discount = "0%" // Просто заглушка, раз нет продуктов
                    })
                    .ToList();

                cardsControl.ItemsSource = list;
            }
        }

        private void Card_Click(object sender, RoutedEventArgs e)
        {
            var win = new EditWindow(((Border)sender).DataContext);
            win.Owner = this;
            if (win.ShowDialog() == true)
                LoadPartners();
        }

        private void AddPartner_Click(object sender, RoutedEventArgs e)
        {
            var win = new EditWindow();
            win.Owner = this;
            if (win.ShowDialog() == true)
                LoadPartners();
        }
    }
}