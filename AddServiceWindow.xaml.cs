using mdk02.models;
using System;
using System.Windows;

namespace mdk02
{
    public partial class AddServiceWindow : Window
    {
        private AutoServiceEntities1 dbContext;

        public AddServiceWindow()
        {
            InitializeComponent();
            dbContext = new AutoServiceEntities1();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Получаем данные из полей
            string serviceName = ServiceNameTextBox.Text;
            string serviceDescription = ServiceDescriptionTextBox.Text;
            decimal serviceCost;

            if (decimal.TryParse(ServiceCostTextBox.Text, out serviceCost))
            {
                var newService = new Service
                {
                    Title = serviceName,
                    Description = serviceDescription,
                    Cost = serviceCost,
                    DurationInSeconds = 3600,  // по умолчанию 1 час
                    Discount = 0
                };

                dbContext.Service.Add(newService);
                dbContext.SaveChanges();
                MessageBox.Show("Услуга добавлена!");
                Close();  // Закрываем окно
            }
            else
            {
                MessageBox.Show("Некорректная цена!");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();  // Закрываем окно без сохранения
        }
    }
}
