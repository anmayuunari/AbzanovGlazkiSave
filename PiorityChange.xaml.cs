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
    /// Логика взаимодействия для PiorityChange.xaml
    /// </summary>
    public partial class PiorityChange : Window
    {
        private List<AgentDB> _selectedAgents;

        public event Action PriorityChangeEvent;

        public PiorityChange(List<AgentDB> selectedAgents)
        {
            InitializeComponent();

            _selectedAgents = selectedAgents;


            PriorityTextBox.Text = FindMaxPriority().ToString();

            //DataContext = selectedAgents;
        }

        private int FindMaxPriority()
        {
            return _selectedAgents.Max(p => p.Priority);
        }

        private void PriorityBackButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void PriorityChangeButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(PriorityTextBox.Text)) errors.AppendLine("Укажите приоритет");
            else
            {
                if (Convert.ToInt32(PriorityTextBox.Text) <= 0) errors.AppendLine("Приоритет не может быть нулевым или отрицальным");
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            foreach (AgentDB agent in _selectedAgents)
            {
                agent.Priority = Convert.ToInt32(PriorityTextBox.Text);
            }

            AbzanovGlazaEntities.GetContext().SaveChanges();
            MessageBox.Show("Информация сохранена");

            PriorityChangeEvent?.Invoke();

            this.DialogResult = true;
            this.Close();
        }
    }
}
