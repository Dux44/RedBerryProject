using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using RedBerryProject.Models;
using RedBerryProject.Services;

namespace RedBerryProject.Views.ReceiverPages
{
    public partial class ApplicationsHistory : Page
    {
        private readonly DataBaseService _dataBaseService = new DataBaseService("Data/database.db");
        private readonly UserData _userData;

        public ApplicationsHistory( UserData userData)
        {
            InitializeComponent();
            _userData = userData;
            LoadApplicationsHistory();
        }

        private void LoadApplicationsHistory()
        {
            try
            {
                // Перевірка, що дані користувача існують
                if (_userData == null)
                {
                    MessageBox.Show("Не вдалося знайти дані користувача.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Отримати список заяв користувача
                List<HelpApplicationData> applications = _dataBaseService.GetApplicationsByUserDataId(_userData.id);

                // Створити список об'єктів для відображення
                var applicationViewModels = new List<ApplicationHistoryViewModel>();

                foreach (var app in applications)
                {
                    // Отримати адресу пункту допомоги
                    string helpPointAddress = _dataBaseService.GetHelpPointAddressById(app.id_help_point);

                    // Створити об'єкт для відображення
                    var viewModel = new ApplicationHistoryViewModel
                    {
                        Id = app.id,
                        HelpPointAddress = helpPointAddress,
                        RequestedHelp = app.message_kind_of_help,
                        ReasonForHelp = app.message_why_ran_away,
                        Status = app.state,
                        SubmittedDate = app.created_at,
                        UpdatedDate = app.updated_at
                    };

                    applicationViewModels.Add(viewModel);
                }

                // Прив'язати дані до DataGrid
                listOfApplications.ItemsSource = applicationViewModels;

                // Показати повідомлення, якщо немає заяв
                noApplicationsMessage.Visibility = applicationViewModels.Count > 0
                    ? Visibility.Collapsed
                    : Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при завантаженні історії заяв: {ex.Message}",
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ViewDetails_Click(object sender, RoutedEventArgs e)
        {
            // Отримати вибрану заяву
            var selectedApplication = listOfApplications.SelectedItem as ApplicationHistoryViewModel;

            if (selectedApplication != null)
            {
                // Отримати повну інформацію про заяву з бази даних
                HelpApplicationData appData = _dataBaseService.GetApplicationById(selectedApplication.Id);

                if (appData != null)
                {
                    // Показати детальну інформацію у вікні
                    string details = $"Заява #{appData.id}\n\n" +
                                     $"Статус: {appData.state}\n" +
                                     $"Пункт допомоги: {selectedApplication.HelpPointAddress}\n\n" +
                                     $"Потрібна допомога: {appData.message_kind_of_help}\n\n" +
                                     $"Причина звернення: {appData.message_why_ran_away}\n\n" +
                                     $"Подано: {appData.created_at}\n" +
                                     $"Оновлено: {appData.updated_at}";

                    MessageBox.Show(details, "Деталі заяви", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Будь ласка, виберіть заяву для перегляду.",
                    "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // Клас для відображення даних у DataGrid
        public class ApplicationHistoryViewModel
        {
            public long Id { get; set; }
            public string HelpPointAddress { get; set; }
            public string RequestedHelp { get; set; }
            public string ReasonForHelp { get; set; }
            public string Status { get; set; }
            public DateTime SubmittedDate { get; set; }
            public DateTime UpdatedDate { get; set; }
        }
    }
}
