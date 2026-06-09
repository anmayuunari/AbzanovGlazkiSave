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
    /// 
    public partial class Agent : Page
    {

        int CountRecords;
        int CountPage;
        int CurrentPage = 0;

        //bool PriorityFlag = true;

        List<AgentDB> CurrentPageList = new List<AgentDB>();
        List<AgentDB> TableList;
        public Agent()
        {
            InitializeComponent();

            var currentAgents = AbzanovGlazaEntities.GetContext().AgentDB.ToList();

            AgentListView.ItemsSource = currentAgents;

            ComboTypeSort.SelectedIndex = 0;
            ComboTypeFilt.SelectedIndex = 0;

            UpdateAgents();
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

            if (ComboTypeFilt.SelectedIndex >= 1 && ComboTypeFilt.SelectedIndex <= 6)
            {
                currentAgents = currentAgents.Where(p => p.AgentTypeID == ComboTypeFilt.SelectedIndex).ToList();
            }

            
           

            currentAgents = currentAgents.Where(p => p.Title.ToLower().Contains(TBoxSearch.Text.ToLower()) || p.Email.ToLower().Contains(TBoxSearch.Text.ToLower()) || p.PhoneSearch.Contains(TBoxSearch.Text)).ToList();

            AgentListView.ItemsSource = currentAgents;
            TableList = currentAgents;

            ChangePage(0, 0);
            
        }

        private void ChangePage(int direction, int? selectedPage)
        {
            CurrentPageList.Clear();
            CountRecords = TableList.Count;

            if (CountRecords % 10 > 0)
            {
                CountPage = CountRecords / 10 + 1;
            }
            else
            {
                CountPage = CountRecords / 10;
            }

            Boolean Ifupdate = true;

            int min;

            if (selectedPage.HasValue)
            {
                if (selectedPage >= 0 && selectedPage <= CountPage)
                {
                    CurrentPage = (int)selectedPage;
                    min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                    for (int i = CurrentPage * 10; i < min; i++)
                    {
                        CurrentPageList.Add(TableList[i]);
                    }
                }
            }
            else
            {
                switch (direction)
                {
                    case 1:
                        if (CurrentPage > 0)
                        {
                            CurrentPage--;
                            min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                            for (int i = CurrentPage * 10; i < min; i++)
                            {
                                CurrentPageList.Add(TableList[i]);
                            }
                        }
                        else
                        {
                            Ifupdate = false;
                        }
                        break;
                    case 2:
                        if (CurrentPage < CountPage - 1)
                        {
                            CurrentPage++;
                            min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                            for (int i = CurrentPage * 10; i < min; i++)
                            {
                                CurrentPageList.Add(TableList[i]);
                            }
                        }
                        else
                            Ifupdate = false;
                        break;
                }
            }
            if (Ifupdate)
            {
                PageListBox.Items.Clear();

                for(int i = 1; i <= CountPage; i++)
                {
                    PageListBox.Items.Add(i);
                }
                PageListBox.SelectedIndex = CurrentPage;

                AgentListView.ItemsSource = CurrentPageList;

                AgentListView.Items.Refresh();
            }
        }

        private void RButtonUp_Checked(object sender, RoutedEventArgs e)
        {
            UpdateAgents();
        }

        private void RButtonDown_Checked(object sender, RoutedEventArgs e)
        {
            UpdateAgents();
        }

        private void LeftDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(1, null);
        }

        private void RightDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(2, null);
        }

        private void PageListBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ChangePage(0, Convert.ToInt32(PageListBox.SelectedItem.ToString()) - 1);
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (AgentListView.SelectedItem is AgentDB selectedAgent)
            {
                Manager.MainFrame.Navigate(new AddEditPage(selectedAgent));
            }
            else
            {
                MessageBox.Show("Выберите агента для редактирования", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var currentAgent = (sender as Button).DataContext as AgentDB;

            var currentProductSale = AbzanovGlazaEntities.GetContext().AgentDB.ToList();

            currentProductSale = currentProductSale.Where(p => p.ID == currentAgent.ID).ToList();

            if (currentProductSale.Count != 0)
                MessageBox.Show("Невозможно");
            else
            {
                if (MessageBox.Show("Точно?", "Точно преточно?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        AbzanovGlazaEntities.GetContext().AgentDB.Remove(currentAgent);
                        AbzanovGlazaEntities.GetContext().SaveChanges();

                        AgentListView.ItemsSource = AbzanovGlazaEntities.GetContext().AgentDB.ToList();

                        UpdateAgents();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(null));
        }

        private void AgentListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AgentListView.SelectedItems.Count >= 2)
            {
                PriorityAgentChangeButton.Visibility = Visibility.Visible;
            }
            else PriorityAgentChangeButton.Visibility = Visibility.Hidden;
        }

        private void PriorityAgentChangeButton_Click(object sender, RoutedEventArgs e)
        {
            //if (AgentListView.SelectedItems is AgentDB selectedAgents

            var selectedAgents = AgentListView.SelectedItems.Cast<AgentDB>().ToList();
            PiorityChange priorityWindow = new PiorityChange(selectedAgents);

            priorityWindow.PriorityChangeEvent += PriorityChangeEventFunc;

            bool? result = priorityWindow.ShowDialog();

        }

        private void PriorityChangeEventFunc()
        {
            //UpdateAgents();
            ChangePage(0, CurrentPage);
        }
    }
}
