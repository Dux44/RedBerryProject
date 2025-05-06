using RedBerryProject.Models;
using RedBerryProject.Services;
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

namespace RedBerryProject.Views.AdminPage
{
    /// <summary>
    /// Interaction logic for AdminWIndow.xaml
    /// </summary>
    public partial class AdminWIndow : Window
    {
        private readonly DataBaseService _dbService;
        private Admin _currentAdmin;
        //private HelpPoint _currentHelpPoint;
        private long _currentUserId;

        public AdminWIndow(long userId)
        {
            InitializeComponent();

            _dbService = new DataBaseService("Data/database.db");
            _currentUserId = userId;

            Loaded += AdminWindow_Loaded;

            // Налаштування подій для елементів керування
            ButtonExit.Click += ButtonExit_Click;
            ButtonEdit.Click += ButtonEdit_Click;
            ChoseWhatTypeOfApplication.SelectionChanged += ChoseWhatTypeOfApplication_SelectionChanged;
            SearchButton.Click += SearchButton_Click;
        }

        private void AdminWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAdminData();
            LoadApplicationsData();
            LoadStatistics();
        }

        private void LoadAdminData()
        {
            try
            {
                // Отримання даних адміністратора
                _currentAdmin = _dbService.GetAdminDataByUserId(_currentUserId);

                if (_currentAdmin == null)
                {
                    MessageBox.Show("Помилка завантаження даних адміністратора.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Перевірка чи потрібно заповнити дані адміністратора
                if (string.IsNullOrEmpty(_currentAdmin.firstname))
                {
                    ShowAdminEditDialog();
                }

                // Отримання даних про пункт допомоги
                //_currentHelpPoint = _dbService.GetHelpPointAddressById(_currentAdmin.id_help_point);

                //if (_currentHelpPoint != null)
                //{
                //    tbNameOfHelpPoint.Text = $"ПУНКТ ДОПОМОГИ №{_currentHelpPoint.Id}";
                //    tbAddresOfHelpPoint.Text = $"Адреса: {_currentHelpPoint.Address}";
                //}
                
                    tbNameOfHelpPoint.Text = $"ПУНКТ ДОПОМОГИ №{_currentAdmin.id_help_point}";
                    tbAddresOfHelpPoint.Text = $"Адреса: {_dbService.GetHelpPointAddressById(_currentAdmin.id_help_point)}";
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при завантаженні даних: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadApplicationsData(string status = null)
        {
            try
            {
                // Отримання заявок для DataGrid з детальною інформацією
                var applications = _dbService.GetExtendedApplicationsForHelpPoint(_currentAdmin.id_help_point, status);
                listOfApplications.ItemsSource = applications;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при завантаженні заявок: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadStatistics()
        {
            try
            {
                // Отримання статистики заявок
                var statistics = _dbService.GetApplicationStatisticsByHelpPoint(_currentAdmin.id_help_point);

                // Оновлення даних у блоках статистики
                totalApplicationsCount.Text = statistics["Total"].ToString();
                approvedApplicationsCount.Text = statistics["Одобрені"].ToString();
                pendingApplicationsCount.Text = statistics["У розгляді"].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при завантаженні статистики: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowAdminEditDialog()
        {
            // Створюємо просте діалогове вікно для заповнення даних адміністратора
            var dialog = new Window
            {
                Title = "Редагування даних адміністратора",
                Width = 400,
                Height = 300,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this,
                ResizeMode = ResizeMode.NoResize
            };

            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var label = new TextBlock
            {
                Text = "Будь ласка, заповніть ваші дані:",
                Margin = new Thickness(10),
                FontWeight = FontWeights.Bold,
                
            };
            Grid.SetColumnSpan(label, 2);
            Grid.SetRow(label, 0);
            grid.Children.Add(label);

            // Прізвище
            var lastnameLabel = new TextBlock { Text = "Прізвище:", Margin = new Thickness(10), VerticalAlignment = VerticalAlignment.Center };
            Grid.SetRow(lastnameLabel, 1);
            Grid.SetColumn(lastnameLabel, 0);
            grid.Children.Add(lastnameLabel);

            var lastnameTextBox = new TextBox { Margin = new Thickness(10), Padding = new Thickness(5), Text = _currentAdmin.secondname };
            Grid.SetRow(lastnameTextBox, 1);
            Grid.SetColumn(lastnameTextBox, 1);
            grid.Children.Add(lastnameTextBox);

            // Ім'я
            var firstnameLabel = new TextBlock { Text = "Ім'я:", Margin = new Thickness(10), VerticalAlignment = VerticalAlignment.Center };
            Grid.SetRow(firstnameLabel, 2);
            Grid.SetColumn(firstnameLabel, 0);
            grid.Children.Add(firstnameLabel);

            var firstnameTextBox = new TextBox { Margin = new Thickness(10), Padding = new Thickness(5), Text = _currentAdmin.firstname };
            Grid.SetRow(firstnameTextBox, 2);
            Grid.SetColumn(firstnameTextBox, 1);
            grid.Children.Add(firstnameTextBox);

            // По батькові
            var middlenameLabel = new TextBlock { Text = "По батькові:", Margin = new Thickness(10), VerticalAlignment = VerticalAlignment.Center };
            Grid.SetRow(middlenameLabel, 3);
            Grid.SetColumn(middlenameLabel, 0);
            grid.Children.Add(middlenameLabel);

            var middlenameTextBox = new TextBox { Margin = new Thickness(10), Padding = new Thickness(5), Text = _currentAdmin.middlename };
            Grid.SetRow(middlenameTextBox, 3);
            Grid.SetColumn(middlenameTextBox, 1);
            grid.Children.Add(middlenameTextBox);

            // Кнопки
            var buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(10),
                
            };
            Grid.SetColumnSpan(buttonPanel, 2);
            Grid.SetRow(buttonPanel, 4);

            var saveButton = new Button
            {
                Content = "Зберегти",
                Padding = new Thickness(20, 5, 20, 5),
                Margin = new Thickness(5),
                Background = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#3498db")),
                Foreground = System.Windows.Media.Brushes.White
            };

            var cancelButton = new Button
            {
                Content = "Скасувати",
                Padding = new Thickness(20, 5, 20, 5),
                Margin = new Thickness(5)
            };

            buttonPanel.Children.Add(saveButton);
            buttonPanel.Children.Add(cancelButton);
            grid.Children.Add(buttonPanel);

            dialog.Content = grid;

            // Обробники подій
            saveButton.Click += (s, args) =>
            {
                _currentAdmin.firstname = firstnameTextBox.Text.Trim();
                _currentAdmin.secondname = lastnameTextBox.Text.Trim();
                _currentAdmin.middlename = middlenameTextBox.Text.Trim();

                try
                {
                    _dbService.UpdateAdminData(_currentAdmin);
                    dialog.DialogResult = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка при збереженні даних: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            };

            cancelButton.Click += (s, args) => dialog.DialogResult = false;

            // Показуємо діалогове вікно
            bool? result = dialog.ShowDialog();

            // Якщо користувач скасував заповнення форми, але це перший вхід - показуємо попередження
            if (result != true && string.IsNullOrEmpty(_currentAdmin.firstname))
            {
                MessageBox.Show("Для продовження роботи необхідно заповнити персональні дані.",
                    "Попередження", MessageBoxButton.OK, MessageBoxImage.Warning);

                // Повторний виклик діалогу, якщо дані обов'язкові
                ShowAdminEditDialog();
            }
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            ShowAdminEditDialog();
        }

        private void ChoseWhatTypeOfApplication_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ChoseWhatTypeOfApplication.SelectedItem == null)
                return;

            string status = null;
            var selectedItem = ((ComboBoxItem)ChoseWhatTypeOfApplication.SelectedItem).Content.ToString();

            // Визначення фільтра за статусом
            switch (selectedItem)
            {
                case "У розгляді":
                    status = "У розгляді";
                    break;
                case "Одобрені":
                    status = "Одобрені";
                    break;
                case "Скасовані":
                    status = "Скасовані";
                    break;
                    // "Всі заявки" або інші варіанти - без фільтра
            }

            LoadApplicationsData(status);
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            // Реалізація пошуку по заявках
            //string searchQuery = SearchTextBox.Text.Trim().ToLower();
            //if (string.IsNullOrEmpty(searchQuery))
            //{
            //    // Якщо пошуковий запит порожній, відновлюємо повний список
            //    ChoseWhatTypeOfApplication_SelectionChanged(ChoseWhatTypeOfApplication, null);
            //    return;
            //}

            //try
            //{
            //    // Отримуємо поточні дані і фільтруємо їх
            //    if (listOfApplications.ItemsSource is List<ApplicationViewModel> applications)
            //    {
            //        var filteredApplications = applications.Where(a =>
            //            a.FullName.ToLower().Contains(searchQuery) ||
            //            a.Phone.ToLower().Contains(searchQuery) ||
            //            a.RequestedHelp.ToLower().Contains(searchQuery) ||
            //            a.Id.ToString().Contains(searchQuery)).ToList();

            //        listOfApplications.ItemsSource = filteredApplications;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Помилка при пошуку: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
        }

        private void ViewApplicationDetails_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is ApplicationViewModel application)
            {
                // Відкриття детального перегляду заявки
                var detailsWindow = new ApplicationDetailsWindow(application.UserDataId, _dbService);
                detailsWindow.Owner = this;
                detailsWindow.ShowDialog();

                // Оновлюємо дані після закриття вікна деталей
                LoadApplicationsData();
                LoadStatistics();
            }
        }

        private void ChangeApplicationStatus_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is ApplicationViewModel application)
            {
                // Створення меню для зміни статусу
                var statusMenu = new ContextMenu();

                var pendingMenuItem = new MenuItem { Header = "У розгляді" };
                pendingMenuItem.Click += (s, args) => UpdateApplicationStatus(application.Id, "У розгляді");
                statusMenu.Items.Add(pendingMenuItem);

                var approveMenuItem = new MenuItem { Header = "Одобрити" };
                approveMenuItem.Click += (s, args) => UpdateApplicationStatus(application.Id, "Одобрені");
                statusMenu.Items.Add(approveMenuItem);

                var cancelMenuItem = new MenuItem { Header = "Скасувати" };
                cancelMenuItem.Click += (s, args) => UpdateApplicationStatus(application.Id, "Скасовані");
                statusMenu.Items.Add(cancelMenuItem);

                statusMenu.IsOpen = true;
            }
        }

        private void UpdateApplicationStatus(long applicationId, string newStatus)
        {
            try
            {
                _dbService.UpdateApplicationState(applicationId, newStatus);
                MessageBox.Show($"Статус заявки змінено на '{newStatus}'", "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);

                // Оновлюємо дані після зміни статусу
                LoadApplicationsData();
                LoadStatistics();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при зміні статусу: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
