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
using System.Windows.Navigation;
using System.Windows.Shapes;

using RedBerryProject.Models;
using RedBerryProject.Services;

namespace RedBerryProject.Views.ReceiverPages
{
    /// <summary>
    /// Interaction logic for PersonalInfoEdit.xaml
    /// </summary>
    public partial class PersonalInfoEdit : Page
    {
        private UserData _receiver;
        private bool isPhoneCorrect = true;
        private bool isBankCardCorrect = true;
        private readonly Action _openPersonalInfoPage; 
        public PersonalInfoEdit(Action openPersonalInfoPage, ref UserData receiver)
        {
            InitializeComponent();
            _openPersonalInfoPage = openPersonalInfoPage;
            _receiver = receiver;
        }
        private void ButtonConfirmEditing_Click(object sender, RoutedEventArgs e)
        {
            if (!isPhoneCorrect)
            {
                MessageBox.Show("Не можу продовжити редагування бо номер телефону не коректний!", "Валідація редагування", MessageBoxButton.OK, MessageBoxImage.Warning);
                UserPhoneNumber.Focus();
                return;
            }
            else if (!isBankCardCorrect)
            {
                MessageBox.Show("Не можу продовжити редагування бо номер банківської карти введений непраильно!", "Валідація редагування", MessageBoxButton.OK,MessageBoxImage.Warning);
                UserBankCardNumber.Focus();
                return;
            }
            else if (UserChooseSex.SelectedIndex == -1)
            {
                MessageBox.Show("Не можу продовжити редагування бо стать не обрано!", "Валідація редагування",MessageBoxButton.OK, MessageBoxImage.Warning);
                UserChooseSex.Focus();
                return;
            }
            MessageBox.Show("Все наче коректно записую нові дані в БД");
            try
            {
                _receiver.firstname = UserName.Text;
                _receiver.secondname = UserSurname.Text;
                _receiver.middlename = UserMiddleName.Text;
                _receiver.nationality = UserNationality.Text;
                _receiver.date_of_birth = UserDateOfBirth.DisplayDate;

                var selectedItem = UserChooseSex.SelectedItem as ComboBoxItem;
                _receiver.gender = selectedItem?.Content?.ToString(); //тут отримати string не опис

                _receiver.address_of_birth = UserPlaceOfBirth.Text;
                _receiver.addres_offical = UserOfficialAddres.Text;
                _receiver.addres_current = UserCurrentAddres.Text;
                _receiver.phone_number = UserPhoneNumber.Text;
                _receiver.card_number = UserBankCardNumber.Text;

                string dbPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "database.db");
                var db = new DataBaseService(dbPath);
                db.UpdateUserData(_receiver);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Щось пішло не так -> {ex}","Помилка редагування",MessageBoxButton.OK,MessageBoxImage.Error);
            }
            finally
            {
                _openPersonalInfoPage.Invoke();
            }
        }
        private void ButtonCancelEdiging_Click(object sender, RoutedEventArgs e)
        {
            _openPersonalInfoPage.Invoke();
        }
        private void PersonalInfoEdit_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                UserName.Text = _receiver.firstname == null ? "" : _receiver.firstname;
                UserSurname.Text = _receiver.secondname == null ? "" : _receiver.secondname;
                UserMiddleName.Text = _receiver.middlename == null ? "" : _receiver.middlename;
                UserNationality.Text = _receiver.nationality == null ? "" : _receiver.nationality;
                UserDateOfBirth.Text = _receiver.date_of_birth.ToString(); //mb exeption
                UserPlaceOfBirth.Text = _receiver.address_of_birth == null ? "" : _receiver.address_of_birth;

                switch (_receiver.gender)
                {
                    case "Чоловік":
                        UserChooseSex.SelectedIndex = 0;
                        break;
                    case "Жінка":
                        UserChooseSex.SelectedIndex = 1;
                        break;
                    default:
                        UserChooseSex.SelectedIndex = -1;
                        break;
                }
                UserOfficialAddres.Text = _receiver.addres_offical == null ? "" : _receiver.addres_offical;
                UserCurrentAddres.Text = _receiver.addres_current == null ? "" : _receiver.addres_current;
                UserPhoneNumber.Text = _receiver.phone_number == null ? "" : _receiver.phone_number;
                UserBankCardNumber.Text = _receiver.card_number == null ? "" : _receiver.card_number;
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Щось пішло не так! {ex}","Помилка завантаження", MessageBoxButton.OK,MessageBoxImage.Error);
            }
            

        }
        //Усе по валідації полів при записі інформації користувача
        private void FullName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^[a-zA-Zа-яА-ЯїЇіІєЄґҐ ]$");
        }
        private void FullName_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                var text = (string)e.DataObject.GetData(typeof(string));
                if (!Regex.IsMatch(text, @"^[a-zA-Zа-яА-ЯїЇіІєЄґҐ ]+$"))
                {
                    e.CancelCommand();
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        MessageBox.Show("Вставку було скасовано через наявність недопустимих символів", "Валідація вставки", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }));
                }
            }
            else
            {
                e.CancelCommand();
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    MessageBox.Show("Вставку було скасовано через наявність недопустимих символів", "Валідація вставки", MessageBoxButton.OK, MessageBoxImage.Warning);
                }));
            }
        }
        private void FullName_LostFocus(object sender, RoutedEventArgs e)
        {
            // 1) стирання усіх пустих рядків по боках та всередині
            // 2) перепитування користувача чи справді його ім'я складається з одної двох літер
            // 3) авотматична зміна типу регістра для усіх літер перша велика інші малі

            if (sender is TextBox textBox)
            {
                string textCleaned = textBox.Text.Trim(); //стирає лишні пробіли поза стрінгом
                if(textCleaned.Length == 0) { return; }
                textCleaned = CorrectionRegisters(textCleaned); 

                var tag = textBox.Tag?.ToString(); //ЗМІНИТИ TEXTBOX.TEXT на справді ту стрінгу яку валідуєм
                if(tag == "FirstName")
                {
                    if(!IsThatNameCorrect(textCleaned, tag))
                    {
                        textBox.Text = "";
                        return;
                    }
                }
                if(tag == "Surname")
                {
                    if (!IsThatNameCorrect(textCleaned, tag))
                    {
                        textBox.Text = "";
                        return;
                    }
                }
                if(tag == "MiddleName")
                {
                    if (!IsThatNameCorrect(textCleaned, tag))
                    {
                        textBox.Text = "";
                        return;
                    }
                }
                textBox.Text = textCleaned;
            }
        }
        private bool IsThatNameCorrect(string name, string typeofFullName)
        {
            if(name.Length <= 2)
            {
                MessageBoxResult result = MessageBox.Show($"Ви точно впевнені що ваше {typeofFullName} є {name}?", "Перевірка правильності імені", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if(result == MessageBoxResult.Yes)
                {
                    return true;
                }
                else if(result == MessageBoxResult.No)
                {
                    return false;
                }
            }
            return true;
        }
        private string CorrectionRegisters(string text)
        {
            string correctedText = string.Empty;
            char[] chars = text.ToCharArray();
            for(int i=0;i<chars.Length; i++) 
            {
                if (i == 0)
                {
                    correctedText += chars[i].ToString().ToUpper();
                }
                else
                {
                    if (chars[i] != ' ')
                    {
                        correctedText += chars[i].ToString().ToLower();
                    }
                }
            }
            return correctedText;
        }
        private void UserNationality_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^[a-zA-Zа-яА-ЯіІїЇєЄґҐ\- ]$");
        }
        private void UserNationality_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                var text = (string)e.DataObject.GetData(typeof(string));
                if (!Regex.IsMatch(text, @"^[a-zA-Zа-яА-ЯіІїЇєЄґҐ\- ]+$"))
                {
                    e.CancelCommand();
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        MessageBox.Show("Вставку було скасовано через наявність недопустимих символів", "Валідація вставки", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }));
                }
            }
            else
            {
                e.CancelCommand();
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    MessageBox.Show("Вставку було скасовано через наявність недопустимих символів", "Валідація вставки", MessageBoxButton.OK, MessageBoxImage.Warning);
                }));
            }
        }
        private void UserNationality_LostFocus(object sender, RoutedEventArgs e)
        {
            if(sender is TextBox textBox)
            {
                if (!string.IsNullOrWhiteSpace(textBox.Text))
                {
                    string cleanedInput = textBox.Text.Trim();

                    // Забираємо дефіси на початку, кінці і зайві дефіси
                    while (cleanedInput.StartsWith("-") || cleanedInput.EndsWith("-") || cleanedInput.Contains("--"))
                    {
                        cleanedInput = cleanedInput.Trim('-').Replace("--", "-");
                    }

                    string[] words = cleanedInput.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    string[] formattedWords = new string[words.Length];

                    for (int i = 0; i < words.Length; i++)
                    {
                        string[] parts = words[i].Split('-', StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < parts.Length; j++)
                        {
                            string part = parts[j];
                            if (part.Length > 1)
                                parts[j] = char.ToUpper(part[0]) + part.Substring(1).ToLower();
                            else
                                parts[j] = part.ToUpper();
                        }

                        formattedWords[i] = string.Join("-", parts);
                    }

                    textBox.Text = string.Join(" ", formattedWords).Trim();
                }
            }
        }
        private void UserPhoneNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // 1) заборона вставки більше ніж 9 включно із "+380 "
            // 2) заборона введення усього крім цифер
            if (sender is TextBox textBox)
            {
                // Дозволяємо тільки цифри
                if (!Regex.IsMatch(e.Text, @"^\d$"))
                {
                    e.Handled = true;
                    return;
                }

                // Поточний текст без пробілів
                string raw = new string(textBox.Text.Where(char.IsDigit).ToArray());

                // Якщо вже введено 12 цифр (3 від +380 + 9 користувача), забороняємо
                if (raw.Length >= 12)
                {
                    e.Handled = true;
                    return;
                }

                // Позиція введення — вставляємо нову цифру в правильне місце
                int caretIndex = textBox.CaretIndex;
                string digits = new string(textBox.Text.Where(char.IsDigit).ToArray()) + e.Text;

                // Форматуємо
                string formatted = FormatPhoneNumber(digits);
                textBox.Text = formatted;

                // Встановлюємо курсор у правильне місце (останній символ)
                textBox.CaretIndex = textBox.Text.Length;

                e.Handled = true;
            }
        }
        private string FormatPhoneNumber(string digits)
        {
            if (!digits.StartsWith("380"))
                digits = "380" + digits;

            var sb = new StringBuilder("+");
            for (int i = 0; i < digits.Length && i < 12; i++)
            {
                sb.Append(digits[i]);
                if (i == 2 || i == 4 || i == 7 || i == 9) sb.Append(' ');
            }

            return sb.ToString().TrimEnd();
        }

        private void UserPhoneNumber_PreviewKeyDown(object sender , KeyEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                // Заборонити видалення частини префіксу "+380"
                if (textBox.SelectionStart <= 5 &&
                    (e.Key == Key.Back || e.Key == Key.Delete || e.Key == Key.Left))
                {
                    e.Handled = true;
                    return;
                }

                // Видалення останньої цифри
                if (e.Key == Key.Back && textBox.SelectionLength == 0)
                {
                    int caretPos = textBox.CaretIndex;

                    // Видаляємо попередню цифру (і пробіл, якщо потрібно)
                    if (caretPos > 5)
                    {
                        // Видаляємо символ перед курсором
                        var digits = new string(textBox.Text.Where(char.IsDigit).ToArray());
                        digits = digits.Substring(0, Math.Max(3, digits.Length - 1)); // 4 = +380

                        textBox.Text = FormatPhoneNumber(digits);
                        textBox.CaretIndex = Math.Min(textBox.Text.Length, caretPos - 1);
                    }

                    e.Handled = true;
                }
            }
        }
        private void UserPhoneNumber_LostFocus(object sender, EventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (textBox.Text.Length > 5)
                {
                    string digits = new string(textBox.Text.Where(char.IsDigit).ToArray());

                    if (digits.Length != 12)
                    {
                        MessageBox.Show("Номер телефону повинен мати формат +380 XX XXX XX XX", "Валідація номеру телефона", MessageBoxButton.OK, MessageBoxImage.Warning);
                        isPhoneCorrect = false;
                        //textBox.Focus();
                    }
                    else isPhoneCorrect = true;
                }
            }
        }
        private void UserBankCardNumber_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if(!string.IsNullOrWhiteSpace(textBox.Text))
                {
                    string digitsOnly = new string(textBox.Text.Where(char.IsDigit).ToArray());

                    if (digitsOnly.Length != 16)
                    {
                        MessageBox.Show("Номер картки має містити рівно 16 цифр.", "Помилка валідації", MessageBoxButton.OK, MessageBoxImage.Warning);
                        isBankCardCorrect = false;
                    }
                    else isBankCardCorrect = true;
                }
            }
        }
        private void UserBankCardNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                // Заборонити введення, якщо це не цифра
                if (!char.IsDigit(e.Text, 0))
                {
                    e.Handled = true;
                    return;
                }

                // Видаляємо пробіли, додаємо нову цифру, якщо ще не досягнуто 16 цифр
                string digitsOnly = new string(textBox.Text.Where(char.IsDigit).ToArray());

                if (digitsOnly.Length >= 16)
                {
                    e.Handled = true;
                    return;
                }

                digitsOnly += e.Text;

                // Форматуємо по шаблону XXXX XXXX XXXX XXXX
                textBox.Text = FormatCardNumber(digitsOnly);
                textBox.CaretIndex = textBox.Text.Length;
                e.Handled = true;
            }
        }
        private string FormatCardNumber(string input)
        {
            var grouped = Enumerable.Range(0, input.Length).GroupBy(i => i / 4).Select(g => new string(g.Select(i => input[i]).ToArray()));
            return string.Join(" ", grouped);
        }
    }
}
