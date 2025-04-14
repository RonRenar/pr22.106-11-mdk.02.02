using System;
using System.Linq;
using System.Windows;
using mdk02.models;

namespace mdk02
{
    public partial class ClientsWindow : Window
    {
        private AutoServiceEntities1 dbContext;
        private Service currentService;

        // Добавим конструктор, принимающий объект Service
        public ClientsWindow(Service service)
        {
            InitializeComponent();
            dbContext = new AutoServiceEntities1();
            currentService = service;
            LoadClients();
        }

        // Метод для загрузки клиентов, связанных с выбранной услугой
        private void LoadClients()
        {
            // Загружаем из БД клиентов, связанных с услугой
            var rawClients = dbContext.Client
                .Where(c => c.ClientService.Any(cs => cs.ServiceID == currentService.ID))
                .ToList(); // <--- сначала грузим из БД

            // Потом обрабатываем в памяти
            var clients = rawClients.Select(c => new ClientViewModel
            {
                ID = c.ID,
                FullName = c.FullName,
                PhotoPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                            c.PhotoPath.Replace("Клиенты\\", "Clients\\"))
            }).ToList();

            ClientsDataGrid.ItemsSource = clients;
        }


        private void AddClientButton_Click(object sender, RoutedEventArgs e)
        {
            // Открытие окна для добавления нового клиента
            var addClientWindow = new AddClientWindow();
            addClientWindow.ShowDialog();  // Показываем окно как модальное
            LoadClients();  // Перезагружаем список клиентов
        }


        private void DeleteClientButton_Click(object sender, RoutedEventArgs e)
        {
            if (ClientsDataGrid.SelectedItem is ClientViewModel selectedClient)
            {
                // Получаем настоящего клиента из базы по ID
                var clientToDelete = dbContext.Client.Find(selectedClient.ID);

                if (clientToDelete != null)
                {
                    var clientServicesToDelete = dbContext.ClientService
                        .Where(cs => cs.ClientID == clientToDelete.ID && cs.ServiceID == currentService.ID)
                        .ToList();

                    dbContext.ClientService.RemoveRange(clientServicesToDelete);
                    dbContext.Client.Remove(clientToDelete);
                    dbContext.SaveChanges();
                    LoadClients();
                }
            }
        }

    }
}
