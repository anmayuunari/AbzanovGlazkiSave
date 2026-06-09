using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace AbzanovGlazki
{
    /// <summary>
    /// Логика взаимодействия для AddProductHistory.xaml
    /// </summary>
    public partial class AddProductHistory : Window
    {
        public event Action SomeEvent;

        private AgentDB _currentAgent = new AgentDB();
        private ProductSale _currentProductSale = new ProductSale();
        public AddProductHistory(AgentDB SelectedAgent)
        {
            InitializeComponent();

            //var currentProductSale = AbzanovGlazaEntities.GetContext().ProductSale.ToList();
            //var currentProductSale = AbzanovGlazaEntities.GetContext().ProductSale;
            //DataContext = SelectedProductSale;

            DataContext = _currentProductSale;

            ProductFullNameComboBox.ItemsSource = AbzanovGlazaEntities.GetContext().Product.ToList();
            //ProductFullNameComboBox.ItemsSource = AbzanovGlazaEntities.GetContext().ProductSale.ToList();

            _currentAgent = SelectedAgent;

        }

        private void AddProductHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            _currentProductSale.AgentID = _currentAgent.ID;
            _currentProductSale.ProductID = ProductFullNameComboBox.SelectedIndex + 1;

            StringBuilder errors = new StringBuilder();
            if (_currentProductSale.AgentID == 0) errors.AppendLine("Укажите товар");
            if (_currentProductSale.ProductCount <= 0) errors.AppendLine("Укажите количество товара");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            AbzanovGlazaEntities.GetContext().ProductSale.Add(_currentProductSale);
            AbzanovGlazaEntities.GetContext().SaveChanges();

            SomeEvent?.Invoke();

            this.Close();
        }
    }
}
