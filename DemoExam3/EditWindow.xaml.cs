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


using System;
using System.Linq;
using System.Windows;

namespace DemoExam3
{
    public partial class EditWindow : Window
    {
        private Partners _partner;
        private bool _isEditMode;
        private Entities _context;

        public EditWindow(object partnerData = null)
        {
            InitializeComponent();
            _context = new Entities();
            cmbPartnerType.ItemsSource = _context.PartnerTypes.ToList();

            if (cmbPartnerType.Items.Count > 0)
                cmbPartnerType.SelectedIndex = 0;

            if (partnerData != null)
            {
                _isEditMode = true;
                Title = "Редактирование партнера";
                LoadData(partnerData);
            }
            else
            {
                _isEditMode = false;
                Title = "Добавление партнера";
                _partner = new Partners();
            }
        }

        private void LoadData(object data)
        {
            var props = data.GetType().GetProperties();
            int id = 0;
            foreach (var p in props)
                if (p.Name == "PartnerID") id = (int)p.GetValue(data);

            _partner = _context.Partners.First(x => x.PartnerID == id);
            txtPartnerName.Text = _partner.PartnerName;
            txtDirector.Text = _partner.Director;
            txtPhone.Text = _partner.Phone;
            txtAddress.Text = _partner.Fddres;
            txtEmail.Text = _partner.Email;
            txtRating.Text = _partner.Reiting?.ToString() ?? "0";

            if (_partner.PartnerTypeID.HasValue)
                cmbPartnerType.SelectedValue = _partner.PartnerTypeID;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверка что выбран тип
                if (cmbPartnerType.SelectedItem == null)
                {
                    MessageBox.Show("Выберите тип партнера", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _partner.PartnerName = txtPartnerName.Text;
                _partner.Director = txtDirector.Text;
                _partner.Phone = txtPhone.Text;
                _partner.Fddres = txtAddress.Text;
                _partner.Email = txtEmail.Text;
                _partner.Reiting = string.IsNullOrEmpty(txtRating.Text) ? 0 : int.Parse(txtRating.Text);
                _partner.PartnerTypeID = ((PartnerTypes)cmbPartnerType.SelectedItem).PartnerTypeID;

                if (_isEditMode)
                    _context.Entry(_partner).State = System.Data.Entity.EntityState.Modified;
                else
                {
                    _partner.PartnerID = _context.Partners.Any() ? _context.Partners.Max(x => x.PartnerID) + 1 : 1;
                    _context.Partners.Add(_partner);
                }

                _context.SaveChanges();
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}