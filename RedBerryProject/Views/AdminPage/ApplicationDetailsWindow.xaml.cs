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
    /// Interaction logic for ApplicationDetailsWindow.xaml
    /// </summary>
    public partial class ApplicationDetailsWindow : Window
    {
        private readonly DataBaseService _dbService;
        private readonly long _applicationId;
        private HelpApplicationData _application;
        private UserData _userData;

        public ApplicationDetailsWindow(long applicationId, DataBaseService dbService)
        {
            InitializeComponent();

            _dbService = dbService;
            _applicationId = applicationId;

            Loaded += ApplicationDetailsWindow_Loaded;
            ButtonApprove.Click += ButtonApprove_Click;
            ButtonReject.Click += ButtonReject_Click;
            ButtonClose.Click += ButtonClose_Click;
        }

        private void ApplicationDetailsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadApplicationData();
        }

        private void LoadApplicationData()
        {
            try
            {
                // Завантаження даних заявки
                _application = _dbService.GetApplicationById(_applicationId);

                if (_application == null)
                {
                    MessageBox.Show("Не вдалося знайти заявку.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                    return;
                }

                // Завантаження даних користувача
                _userData = _dbService.GetUserDataById(_application.id_user_data);

                if (_userData == null)
                {
                    MessageBox.Show("Не вдалося знайти дані користувача.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                    return;
                }

                // Відображення даних заявки
                tbApplicationId.Text = $"Заявка #{_application.id}";
                tbStatus.Text = _application.state;
                tbCreatedDate.Text = _application.created_at.ToString();
                tbUpdatedDate.Text = _application.updated_at.ToString();

                // Відображення даних користувача
                tbFullName.Text = $"{_userData.secondname} {_userData.firstname} {_userData.middlename}";
                tbPhone.Text = _userData.phone_number;
                tbGender.Text = _userData.gender;
                tbBirthDate.Text = _userData.date_of_birth.ToString();
                tbNationality.Text = _userData.nationality;
                tbBirthAddress.Text = _userData.addres_of_birth;
                tbOfficialAddress.Text = _userData.addres_offical;
                tbCurrentAddress.Text = _userData.addres_current;
                tbCardNumber.Text = _userData.card_number;

                // Відображення повідомлень з заявки
                tbWhyRanAway.Text = _application.message_why_ran_away;
                tbKindOfHelp.Text = _application.message_kind_of_help;

                // Налаштування кнопок в залежності від статусу заявки
                UpdateButtonsVisibility();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при завантаженні даних: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        private void UpdateButtonsVisibility()
        {
            // Показуємо кнопки "Одобрити" і "Скасувати" тільки для заявок у стані "У розгляді"
            bool isPending = _application.state == "У розгляді";
            ButtonApprove.Visibility = isPending ? Visibility.Visible : Visibility.Collapsed;
            ButtonReject.Visibility = isPending ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ButtonApprove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _dbService.UpdateApplicationState(_applicationId, "Одобрені");
                MessageBox.Show("Заявку успішно одобрено.", "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
                _application.state = "Одобрені";
                _application.updated_at = DateTime.Now;
                tbStatus.Text = _application.state;
                tbUpdatedDate.Text = _application.updated_at.ToString();
                UpdateButtonsVisibility();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при оновленні статусу: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonReject_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _dbService.UpdateApplicationState(_applicationId, "Скасовані");
                MessageBox.Show("Заявку скасовано.", "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
                _application.state = "Скасовані";
                _application.updated_at = DateTime.Now;
                tbStatus.Text = _application.state;
                tbUpdatedDate.Text = _application.updated_at.ToString();
                UpdateButtonsVisibility();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при оновленні статусу: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
