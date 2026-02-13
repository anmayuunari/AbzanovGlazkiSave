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

namespace AbzanovGlazki
{
    /// <summary>
    /// Логика взаимодействия для Agent.xaml
    /// </summary>
    public partial class Agent : Page
    {
        public Agent()
        {
            InitializeComponent();

            var currentAgents = AbzanovGlazaEntities.GetContext().AgentDB.ToList();

            AgentListView.ItemsSource = currentAgents;

            ComboTypeSort.SelectedIndex = 0;
            ComboTypeFilt.SelectedIndex = 0;

            UpdateAgents();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage());
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateAgents();
        }
        private void ComboTypeSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAgents();
        }
        private void ComboTypeFilt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAgents();
        }

        private void UpdateAgents()
        {
            var currentAgents = AbzanovGlazaEntities.GetContext().AgentDB.ToList();
        }
    }
}
