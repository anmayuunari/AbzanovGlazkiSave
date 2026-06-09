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

namespace AbzanovGlazki
{
    /// <summary>
    /// Логика взаимодействия для ProductHistory.xaml
    /// </summary>
    public partial class ProductHistory : Window
    {
        private AgentDB _currentAgent = new AgentDB();
        private ProductSale _currentProductSale = new ProductSale();
        public ProductHistory(AgentDB SelectedAgent)
        {
            InitializeComponent();

            var currentProductHistory = AbzanovGlazaEntities.GetContext().ProductSale.ToList();

            currentProductHistory = currentProductHistory.Where(p => p.AgentID == SelectedAgent.ID).ToList();

            ProductHistoryListView.ItemsSource = currentProductHistory;

            _currentAgent = SelectedAgent;
            //_currentProductSale = SelectedProductSale;
        }

        private void AddProductHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            AddProductHistory addhistoryWindow = new AddProductHistory(_currentAgent);

            addhistoryWindow.SomeEvent += ProductDataUpdate;

            addhistoryWindow.Show();
        }

        private void ProductDataUpdate()
        {
            ProductHistoryListView.ItemsSource = AbzanovGlazaEntities.GetContext().ProductSale.ToList().Where(p => p.AgentID == _currentAgent.ID).ToList();
        }

        private void DeleteProductHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            var currentProductSale = (sender as Button).DataContext as ProductSale;

            if (MessageBox.Show("Точно?", "Точно преточно?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    AbzanovGlazaEntities.GetContext().ProductSale.Remove(currentProductSale);
                    AbzanovGlazaEntities.GetContext().SaveChanges();

                    ProductHistoryListView.ItemsSource = AbzanovGlazaEntities.GetContext().ProductSale.ToList().Where(p => p.AgentID == _currentAgent.ID).ToList();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
    }
}
