using mdk02.models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace mdk02
{
    public partial class AddClientWindow : Window
    {
        private AutoServiceEntities1 dbContext;

        public AddClientWindow()
        {
            InitializeComponent();
            dbContext = new AutoServiceEntities1();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Получаем данные из полей
            string firstName = FirstNameTextBox.Text;
            string lastName = LastNameTextBox.Text;
            string patronymic = PatronymicTextBox.Text;
            DateTime? birthday = BirthdayDatePicker.SelectedDate;
            string gender = ((ComboBoxItem)GenderComboBox.SelectedItem)?.Content.ToString();
            string photoPath = PhotoPathTextBox.Text;

            if (birthday != null && !string.IsNullOrWhiteSpace(firstName) && !string.IsNullOrWhiteSpace(lastName) && !string.IsNullOrWhiteSpace(gender))
            {
                // Создание нового клиента
                var newClient = new Client
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Patronymic = patronymic,
                    Birthday = birthday,
                    RegistrationDate = DateTime.Now,
                    Email = "example@example.com",  // Замените на соответствующие данные
                    Phone = "1234567890",           // Замените на соответствующие данные
                    GenderCode = gender,
                    PhotoPath = photoPath
                };

                dbContext.Client.Add(newClient);
                dbContext.SaveChanges();  // Сначала сохраняем клиента в базе данных

                // Получаем текущую услугу (если она передана в окно)
                var serviceId = 1; // Убедитесь, что передаете правильный ID услуги
                var clientService = new ClientService
                {
                    ClientID= newClient.ID,
                    ServiceID= serviceId,
                    StartTime= DateTime.Now,
                      
                    
                };

                // Добавляем запись в таблицу ClientService
                dbContext.ClientService.Add(clientService);
                dbContext.SaveChanges();  // Сохраняем связь между клиентом и услугой

                MessageBox.Show("Клиент добавлен и связан с услугой!");
                Close();  // Закрываем окно
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля!");
            }
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();  // Закрываем окно без сохранения
        }
    }
}
