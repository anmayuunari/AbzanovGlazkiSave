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

            if (ComboTypeSort.SelectedIndex == 0 && RButtonUp.IsChecked.Value)
            {
                AgentListView.ItemsSource = currentAgents.OrderBy(p => p.Title).ToList();
                currentAgents = currentAgents.OrderBy(p => p.Title).ToList();
            }
            if (ComboTypeSort.SelectedIndex == 0 && RButtonDown.IsChecked.Value)
            {
                AgentListView.ItemsSource = currentAgents.OrderByDescending(p => p.Title).ToList();
                currentAgents = currentAgents.OrderByDescending(p => p.Title).ToList();
            }

            if (ComboTypeSort.SelectedIndex == 1 && RButtonUp.IsChecked.Value)
            {
                AgentListView.ItemsSource = currentAgents.OrderBy(p => p.Discount).ToList();
                currentAgents = currentAgents.OrderBy(p => p.Discount).ToList();
            }
            if (ComboTypeSort.SelectedIndex == 1 && RButtonDown.IsChecked.Value)
            {
                AgentListView.ItemsSource = currentAgents.OrderByDescending(p => p.Discount).ToList();
                currentAgents = currentAgents.OrderByDescending(p => p.Discount).ToList();
            }

            if (ComboTypeSort.SelectedIndex == 2 && RButtonUp.IsChecked.Value)
            {
                AgentListView.ItemsSource = currentAgents.OrderBy(p => p.Priority).ToList();
                currentAgents = currentAgents.OrderBy(p => p.Priority).ToList();
            }
            if (ComboTypeSort.SelectedIndex == 2 && RButtonDown.IsChecked.Value)
            {
                AgentListView.ItemsSource = currentAgents.OrderByDescending(p => p.Priority).ToList();
                currentAgents = currentAgents.OrderByDescending(p => p.Priority).ToList();
            }

            if(ComboTypeFilt.SelectedIndex == 1)
            {
                currentAgents = currentAgents.Where(p => p.AgentTypeFull.Contains("МФО")).ToList();
            }
            if (ComboTypeFilt.SelectedIndex == 2)
            {
                currentAgents = currentAgents.Where(p => p.AgentTypeFull.Contains("ООО")).ToList();
            }
            if (ComboTypeFilt.SelectedIndex == 3)
            {
                currentAgents = currentAgents.Where(p => p.AgentTypeFull.Contains("ЗАО")).ToList();
            }
            if (ComboTypeFilt.SelectedIndex == 4)
            {
                currentAgents = currentAgents.Where(p => p.AgentTypeFull.Contains("МКК")).ToList();
            }
            if (ComboTypeFilt.SelectedIndex == 5)
            {
                currentAgents = currentAgents.Where(p => p.AgentTypeFull.Contains("ОАО")).ToList();
            }
            if (ComboTypeFilt.SelectedIndex == 6)
            {
                currentAgents = currentAgents.Where(p => p.AgentTypeFull.Contains("ПАО")).ToList();
            }

            currentAgents = currentAgents.Where(p => p.Title.ToLower().Contains(TBoxSearch.Text.ToLower()) || p.Email.ToLower().Contains(TBoxSearch.Text.ToLower()) || p.PhoneSearch.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();

            

            AgentListView.ItemsSource = currentAgents.ToList();
        }   

        private void RButtonUp_Checked(object sender, RoutedEventArgs e)
        {
            UpdateAgents();
        }

        private void RButtonDown_Checked(object sender, RoutedEventArgs e)
        {
            UpdateAgents();
        }


    }
}
