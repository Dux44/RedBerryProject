using RedBerryProject.Models;
using RedBerryProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RedBerryProject.Views.ReceiverPages
{
    /// <summary>
    /// Interaction logic for Application.xaml
    /// </summary>
    public partial class HelpApplication : Window
    {
        private UserData _user_data;
        private bool isFormValid = false;
        private string _selectedHelpTypes = ""; // Рядок для збереження вибраних типів допомоги
        private int _selectedPointOfHelp;

        public HelpApplication(UserData data)
        {
            InitializeComponent();
            _user_data = data;
        }

        private void MoneyHelpCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // Показати поле для введення номера банківської карти
            BankCardGrid.Visibility = Visibility.Visible;

            BankCardNumberTextBox.Text = _user_data.card_number == "" ? "" : _user_data.card_number;
            // Додаємо тип допомоги до рядка вибраних типів
            UpdateHelpTypesList("money", true);
        }

        private void MoneyHelpCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            // Приховати поле для введення номера банківської карти
            BankCardGrid.Visibility = Visibility.Collapsed;
            // Очистити поле, коли поле приховується
            BankCardNumberTextBox.Text = string.Empty;

            // Видаляємо тип допомоги з рядка вибраних типів
            UpdateHelpTypesList("money", false);
        }

        private void HelpApplication_Loaded(object sender, RoutedEventArgs e)
        {
            // Заповнюємо форму даними користувача
            LastNameTextBox.Text = _user_data.secondname == "" ? "" : _user_data.secondname;
            FirstNameTextBox.Text = _user_data.firstname == "" ? "" : _user_data.firstname;
            MiddleNameTextBox.Text = _user_data.middlename == "" ? "" : _user_data.middlename;
            CitizenshipTextBox.Text = _user_data.nationality == "" ? "" : _user_data.nationality;
            BirthDatePicker.SelectedDate = _user_data.date_of_birth;
            BirthPlaceTextBox.Text = _user_data.addres_of_birth == "" ? "" : _user_data.addres_of_birth;
            RegisteredAddressTextBox.Text = _user_data.addres_offical == "" ? "" : _user_data.addres_offical;
            ActualAddressTextBox.Text = _user_data.addres_current == "" ? "" : _user_data.addres_current;
            PhoneNumberTextBox.Text = _user_data.phone_number == "" ? "" : _user_data.phone_number;


            // Додаємо обробники подій для валідації текстових полів
            LastNameTextBox.LostFocus += ValidateTextBox;
            FirstNameTextBox.LostFocus += ValidateTextBox;
            MiddleNameTextBox.LostFocus += ValidateTextBox;
            CitizenshipTextBox.LostFocus += ValidateTextBox;
            BirthPlaceTextBox.LostFocus += ValidateTextBox;
            RegisteredAddressTextBox.LostFocus += ValidateTextBox;
            ActualAddressTextBox.LostFocus += ValidateTextBox;
            PhoneNumberTextBox.LostFocus += ValidatePhoneNumber;
            BankCardNumberTextBox.LostFocus += ValidateBankCardNumber;

            // Додаємо обробники подій для типів допомоги
            FoodHelpCheckBox.Checked += FoodHelpCheckBox_Checked;
            FoodHelpCheckBox.Unchecked += FoodHelpCheckBox_Unchecked;
            RelocationHelpCheckBox.Checked += RelocationHelpCheckBox_Checked;
            RelocationHelpCheckBox.Unchecked += RelocationHelpCheckBox_Unchecked;

            // Завантажуємо збережені типи допомоги, якщо вони є
            //if (_data.help_types != null && _data.help_types != "")
            //{
            //    _selectedHelpTypes = _data.help_types;
            //    LoadSelectedHelpTypes();
            //}
        }

        private void SelectHelpPointButton_Click(object sender, EventArgs e)
        {
            // Створюємо вікно з картою пунктів допомоги
            HelpPointMapWindow mapWindow = new HelpPointMapWindow();
            // Встановлюємо власника для модального вікна
            mapWindow.Owner = this;
            // Відкриваємо вікно як модальне
            bool? result = mapWindow.ShowDialog();
            // Якщо користувач вибрав пункт і натиснув "Обрати"
            if (result == true)
            {
                // Оновлюємо інформацію про вибраний пункт допомоги
                SelectedHelpPointTextBlock.Text = mapWindow.SelectedHelpPoint;
                SelectedHelpPointPanel.Visibility = Visibility.Visible;

                // Зберігаємо індекс пункту допомоги для подальшого запису в БД
                _selectedPointOfHelp = mapWindow.SelectedHelpPointIndex;

                // Тут можна використати індекс helpPointIndex для запису в БД або інших потреб
                // Наприклад:
                // SaveHelpPointIndexToDB(helpPointIndex);
            }
        }

        private void SubmitApplication_Click(object sender, RoutedEventArgs e)
        {
            // Перевіряємо валідність всіх полів перед відправкою
            if (ValidateAllFields())
            {
                // Зберігаємо дані користувача
                SaveUserData();

                InsertApplicationData();

                // Показуємо повідомлення про успішне відправлення
                MessageBox.Show("Заяву успішно подано!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);

                // Закриваємо вікно
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Будь ласка, виправте помилки у заяві.", "Помилка валідації", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void SaveUserData()
        {
            // Зберігаємо дані з форми в об'єкт UserData
            _user_data.secondname = LastNameTextBox.Text;
            _user_data.firstname = FirstNameTextBox.Text;
            _user_data.middlename = MiddleNameTextBox.Text;
            _user_data.nationality = CitizenshipTextBox.Text;
            _user_data.date_of_birth = BirthDatePicker.SelectedDate ?? DateTime.MinValue;
            _user_data.addres_of_birth = BirthPlaceTextBox.Text;
            _user_data.addres_offical = RegisteredAddressTextBox.Text;
            _user_data.addres_current = ActualAddressTextBox.Text;
            _user_data.phone_number = PhoneNumberTextBox.Text;

            // Якщо вибрана грошова допомога, зберігаємо номер картки
            if (MoneyHelpCheckBox.IsChecked == true)
            {
                _user_data.card_number = BankCardNumberTextBox.Text;
            }
            else
            {
                _user_data.card_number = "";
            }

            try
            {
                var db = new DataBaseService("Data/database.db");
                db.UpdateUserData(_user_data);
            }
            catch (Exception ex)
            {

            }
        }
        private void InsertApplicationData()
        {
            HelpApplicationData applicationData = new HelpApplicationData
            {
                id_user_data = _user_data.id,
                id_help_point = _selectedPointOfHelp,
                message_why_ran_away = RelocationReasonTextBox.Text,
                message_kind_of_help = _selectedHelpTypes,
                state = "у розгляді",
                created_at = DateTime.Now,
                updated_at = DateTime.MinValue,
            };
            try
            {
                var db = new DataBaseService("Data/database.db");
                db.InsertApplication(applicationData);
            }
            catch (Exception ex)
            {

            }
        }

        // Валідація всіх полів форми
        private bool ValidateAllFields()
        {
            bool isValid = true;

            // Перевіряємо обов'язкові текстові поля
            isValid &= ValidateField(LastNameTextBox, "Прізвище є обов'язковим");
            isValid &= ValidateField(FirstNameTextBox, "Ім'я є обов'язковим");
            isValid &= ValidateField(CitizenshipTextBox, "Громадянство є обов'язковим");
            isValid &= ValidateField(BirthPlaceTextBox, "Місце народження є обов'язковим");
            isValid &= ValidateField(RegisteredAddressTextBox, "Зареєстроване місце проживання є обов'язковим");
            isValid &= ValidateField(ActualAddressTextBox, "Фактичне місце проживання є обов'язковим");

            // Перевіряємо дату народження
            if (BirthDatePicker.SelectedDate == null)
            {
                SetError(BirthDatePicker, "Оберіть дату народження");
                isValid = false;
            }
            else if (BirthDatePicker.SelectedDate > DateTime.Now)
            {
                SetError(BirthDatePicker, "Дата народження не може бути в майбутньому");
                isValid = false;
            }
            else
            {
                ClearError(BirthDatePicker);
            }

            // Перевіряємо номер телефону
            if (string.IsNullOrWhiteSpace(PhoneNumberTextBox.Text))
            {
                SetError(PhoneNumberTextBox, "Номер телефону є обов'язковим");
                isValid = false;
            }
            else if (!IsValidPhoneNumber(PhoneNumberTextBox.Text))
            {
                SetError(PhoneNumberTextBox, "Неправильний формат номера телефону");
                isValid = false;
            }
            else
            {
                ClearError(PhoneNumberTextBox);
            }

            // Перевіряємо, чи обрано пункт допомоги
            if (SelectedHelpPointPanel.Visibility != Visibility.Visible)
            {
                MessageBox.Show("Будь ласка, оберіть пункт допомоги", "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
                isValid = false;
            }

            // Перевіряємо, чи обрано хоча б один тип допомоги
            if (!(MoneyHelpCheckBox.IsChecked == true || FoodHelpCheckBox.IsChecked == true || RelocationHelpCheckBox.IsChecked == true))
            {
                MessageBox.Show("Будь ласка, оберіть хоча б один тип допомоги", "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
                isValid = false;
            }

            // Якщо обрана грошова допомога, перевіряємо номер банківської карти
            if (MoneyHelpCheckBox.IsChecked == true)
            {
                if (string.IsNullOrWhiteSpace(BankCardNumberTextBox.Text))
                {
                    SetError(BankCardNumberTextBox, "Введіть номер банківської карти");
                    isValid = false;
                }
                else if (!IsValidCardNumber(BankCardNumberTextBox.Text))
                {
                    SetError(BankCardNumberTextBox, "Неправильний формат номера карти");
                    isValid = false;
                }
                else
                {
                    ClearError(BankCardNumberTextBox);
                }
            }

            return isValid;
        }

        private void ValidateTextBox(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                ValidateField(textBox, $"{GetFieldName(textBox)} є обов'язковим");
            }
        }

        private void ValidatePhoneNumber(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    SetError(textBox, "Номер телефону є обов'язковим");
                }
                else if (!IsValidPhoneNumber(textBox.Text))
                {
                    SetError(textBox, "Неправильний формат номера телефону");
                }
                else
                {
                    ClearError(textBox);
                }
            }
        }

        private void ValidateBankCardNumber(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && MoneyHelpCheckBox.IsChecked == true)
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    SetError(textBox, "Введіть номер банківської карти");
                }
                else if (!IsValidCardNumber(textBox.Text))
                {
                    SetError(textBox, "Неправильний формат номера карти");
                }
                else
                {
                    ClearError(textBox);
                }
            }
        }

        private bool ValidateField(TextBox textBox, string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                SetError(textBox, errorMessage);
                return false;
            }
            else
            {
                ClearError(textBox);
                return true;
            }
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            // Валідація номера телефону (приклад: +380123456789)
            string pattern = @"^\+?[\d]{10,15}$";
            return Regex.IsMatch(phoneNumber, pattern);
        }

        private bool IsValidCardNumber(string cardNumber)
        {
            // Валідація номера картки (16 цифр)
            string pattern = @"^[\d]{16}$";
            return Regex.IsMatch(cardNumber, pattern);
        }

        private void SetError(Control control, string errorMessage)
        {
            // Встановлюємо червону рамку для поля з помилкою
            control.BorderBrush = Brushes.Red;

            // Встановлюємо підказку з помилкою
            ToolTip tooltip = new ToolTip();
            tooltip.Content = errorMessage;
            control.ToolTip = tooltip;
        }

        private void ClearError(Control control)
        {
            // Повертаємо стандартну рамку
            control.ClearValue(Control.BorderBrushProperty);

            // Видаляємо підказку з помилкою
            control.ToolTip = null;
        }

        private string GetFieldName(TextBox textBox)
        {
            // Отримуємо назву поля на основі імені контролу
            if (textBox == LastNameTextBox) return "Прізвище";
            if (textBox == FirstNameTextBox) return "Ім'я";
            if (textBox == MiddleNameTextBox) return "По батькові";
            if (textBox == CitizenshipTextBox) return "Громадянство";
            if (textBox == BirthPlaceTextBox) return "Місце народження";
            if (textBox == RegisteredAddressTextBox) return "Зареєстроване місце проживання";
            if (textBox == ActualAddressTextBox) return "Фактичне місце проживання";
            if (textBox == PhoneNumberTextBox) return "Номер телефону";
            if (textBox == BankCardNumberTextBox) return "Номер банківської карти";

            return "Поле";
        }

        #region Управління типами допомоги

        // Обробники подій для типів допомоги
        private void FoodHelpCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            UpdateHelpTypesList("supply", true);
        }

        private void FoodHelpCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateHelpTypesList("supply", false);
        }

        private void RelocationHelpCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            UpdateHelpTypesList("house", true);
        }

        private void RelocationHelpCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateHelpTypesList("house", false);
        }

        // Метод для оновлення рядка з переліком типів допомоги
        private void UpdateHelpTypesList(string helpType, bool isAdding)
        {
            if (isAdding)
            {
                // Додаємо тип допомоги, якщо його ще немає в списку
                if (!_selectedHelpTypes.Contains(helpType))
                {
                    if (_selectedHelpTypes.Length > 0)
                    {
                        _selectedHelpTypes += "\\";
                    }
                    _selectedHelpTypes += helpType;
                }
            }
            else
            {
                // Видаляємо тип допомоги зі списку
                if (_selectedHelpTypes.Contains(helpType))
                {
                    // Розбиваємо рядок на окремі типи
                    string[] types = _selectedHelpTypes.Split('\\');
                    // Створюємо новий список без типу, який треба видалити
                    string newList = "";

                    foreach (string type in types)
                    {
                        if (type != helpType)
                        {
                            if (newList.Length > 0)
                            {
                                newList += "\\";
                            }
                            newList += type;
                        }
                    }

                    _selectedHelpTypes = newList;
                }
            }

            // Виводимо в консоль для відлагодження
            Console.WriteLine("Selected help types: " + _selectedHelpTypes);
        }
        #endregion
    }
}
