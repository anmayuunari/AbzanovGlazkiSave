using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private AgentDB _currentAgent = new AgentDB();
        public AddEditPage(AgentDB SelectedAgent)
        {
            InitializeComponent();

            if (SelectedAgent != null) _currentAgent = SelectedAgent;

            DataContext = _currentAgent;


            if (_currentAgent.ID == 0)
            {
                TextBlockID.Visibility = Visibility.Hidden;
                LabelID.Visibility = Visibility.Hidden;
                DeleteEditButton.Visibility = Visibility.Hidden;
            }
            else ComboTypeAddEdit.SelectedIndex = _currentAgent.AgentTypeID - 1;

            
        }

        string validFIO = "абвгдеёжзийклмнопрстуфхцчшщъыьэюяabcdefghijklmnopqrstuvwxyz- ";
        string validEmail = "abcdefghijklmnopqrstuvwxyz-_.@1234567890";


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_currentAgent.Title)) errors.AppendLine("Укажите название");
            else
            {
                if (_currentAgent.Title.Length > 50) errors.AppendLine("Длина названия не должна превышать 50 символов");
                if (!_currentAgent.Title.ToLower().All(c => validFIO.Contains(c))) errors.AppendLine("Название должно содержать только буквы, дефис и пробел");
            }
            if (string.IsNullOrWhiteSpace(_currentAgent.Address)) errors.AppendLine("Укажите адрес");
            if (string.IsNullOrWhiteSpace(_currentAgent.INN)) errors.AppendLine("Укажите ИНН");
            if (string.IsNullOrWhiteSpace(_currentAgent.KPP)) errors.AppendLine("Укажите КПП");
            if (string.IsNullOrWhiteSpace(_currentAgent.DirectorName)) errors.AppendLine("Укажите директора");
            else
            {
                if (!_currentAgent.DirectorName.ToLower().All(c => validFIO.Contains(c))) errors.AppendLine("ФИО директора может содержать только буквы, дефис и пробел");
            }
            if (string.IsNullOrWhiteSpace(_currentAgent.Email)) errors.AppendLine("Укажите почту");
            else
            {
                if (!_currentAgent.Email.All(c => validEmail.Contains(c))) errors.AppendLine("Почта может содержать только английские буквы и символы @ . - и _");
                if (_currentAgent.Email.IndexOf('@') <= 0) errors.AppendLine("Нет знака @");
                if (_currentAgent.Email.LastIndexOf('.') == _currentAgent.Email.Length - 1) errors.AppendLine("Точка не может быть в конце почты");
                if (_currentAgent.Email.LastIndexOf('@') == _currentAgent.Email.Length - 1) errors.AppendLine("@ не может быть в конце почты");
                if (_currentAgent.Email.Count(c => c == '@') > 1) errors.AppendLine("@ не может быть написан два раза");
                int atIndex = _currentAgent.Email.IndexOf('@');
                if (!(_currentAgent.Email.Substring(atIndex + 1).Contains('.'))) errors.AppendLine("После точки в домене должны быть символы");
                if (_currentAgent.Email.Substring(atIndex + 1).StartsWith(".")) errors.AppendLine("Домен не может начинаться с .");
            }
            if (string.IsNullOrWhiteSpace(_currentAgent.Phone)) errors.AppendLine("Укажите телефон");
            else
            {
                if (!_currentAgent.Phone.All(c => char.IsDigit(c) || c == '+' || c == '-' || c == '(' || c == ')' || c == ' ')) errors.AppendLine("\"Телефон должен содержать только цифры и знаки +, -, (, ), пробел");
            }
            if (string.IsNullOrWhiteSpace(_currentAgent.Priority.ToString())) errors.AppendLine("Укажите приоритет");
            if (_currentAgent.Priority <= 0) errors.AppendLine("Приоритет не должен быть отрицательным или равняться нулю");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            if (_currentAgent.ID == 0) AbzanovGlazaEntities.GetContext().AgentDB.Add(_currentAgent);
            try
            {
                AbzanovGlazaEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void ChangeLogoButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myOpenFileDialog = new OpenFileDialog();
            if (myOpenFileDialog.ShowDialog() == true)
            {
                string fileName = System.IO.Path.GetFileName(myOpenFileDialog.FileName);

                _currentAgent.Logo = @"agents\" + fileName;

                AgentLogo.Source = new BitmapImage(new Uri(_currentAgent.Logo, UriKind.Relative));
            }
        }

        private void DeleteEditButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentAgent.CountProduct != 0)
                MessageBox.Show("Невозможно");
            else
            {
                if (MessageBox.Show("Точно?", "Точно преточно?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        AbzanovGlazaEntities.GetContext().AgentDB.Remove(_currentAgent);
                        AbzanovGlazaEntities.GetContext().SaveChanges();

                        MessageBox.Show("Ликвидирован");

                        Manager.MainFrame.GoBack();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
        }

        private void ComboTypeAddEdit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _currentAgent.AgentTypeID = ComboTypeAddEdit.SelectedIndex + 1;
        }

        private void ProductHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            ProductHistory historyWindow = new ProductHistory(_currentAgent);
            historyWindow.Show();
        }
    }
}
