using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using mdk02.models;

namespace mdk02
{
    public partial class MainWindow : Window
    {
        private AutoServiceEntities1 dbContext;

        public MainWindow()
        {
            InitializeComponent();
            dbContext = new AutoServiceEntities1();
            LoadServices();
        }

        private void LoadServices()
        {
            // Загрузка списка всех услуг
            ServicesDataGrid.ItemsSource = dbContext.Service.ToList();
        }

        private void AddServiceButton_Click(object sender, RoutedEventArgs e)
        {
            // Открытие окна для добавления новой услуги
            var addServiceWindow = new AddServiceWindow();
            addServiceWindow.ShowDialog();  // Показываем окно как модальное
            LoadServices();  // Перезагружаем список услуг
        }


        private void DeleteServiceButton_Click(object sender, RoutedEventArgs e)
        {
            // Удаление выбранной услуги
            if (ServicesDataGrid.SelectedItem != null)
            {
                var serviceToDelete = (Service)ServicesDataGrid.SelectedItem;
                dbContext.Service.Remove(serviceToDelete);
                dbContext.SaveChanges();
                LoadServices();
            }
        }

        private void SortAscButton_Click(object sender, RoutedEventArgs e)
        {
            // Сортировка услуг по названию в порядке возрастания
            var sortedServices = dbContext.Service.OrderBy(s => s.Title).ToList();
            ServicesDataGrid.ItemsSource = sortedServices;
        }


        private void SortDescButton_Click(object sender, RoutedEventArgs e)
        {
            // Сортировка услуг по названию в порядке убывания
            var sortedServices = dbContext.Service.OrderByDescending(s => s.Title).ToList();
            ServicesDataGrid.ItemsSource = sortedServices;
        }

        private void ViewClientsButton_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранную услугу
            var service = (Service)((Button)sender).DataContext;

            // Открываем окно для списка клиентов по выбранной услуге
            var clientsWindow = new ClientsWindow(service);
            clientsWindow.Show();
        }
    }
}
