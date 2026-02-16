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

            AgentListView.ItemsSource = currentAgents;
            TableList = currentAgents;

            ChangePage(0, 0);
            

            //AgentListView.ItemsSource = currentAgents.ToList();
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
    }
}
