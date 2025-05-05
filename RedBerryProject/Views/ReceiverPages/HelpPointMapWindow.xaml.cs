using System.Windows;
using System.Windows.Controls;
namespace RedBerryProject.Views.ReceiverPages
{
    public partial class HelpPointMapWindow : Window
    {
        public string SelectedHelpPoint { get; private set; }
        public int SelectedHelpPointIndex { get; private set; } // Додана властивість для індексу пункту

        public HelpPointMapWindow()
        {
            InitializeComponent();
        }

        private void HelpPoint_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            string pointInfo = (string)clickedButton.Tag;

            // Отримуємо індекс пункту з назви кнопки (HelpPoint1 -> 1, HelpPoint2 -> 2, ...)
            int pointIndex = int.Parse(clickedButton.Name.Substring(9));

            // Відображаємо інформацію про вибраний пункт
            SelectedPointName.Text = pointInfo;
            InfoPanel.Visibility = Visibility.Visible;

            // Активуємо кнопку "Обрати"
            SelectButton.IsEnabled = true;

            // Зберігаємо вибір та індекс
            SelectedHelpPoint = pointInfo;
            SelectedHelpPointIndex = pointIndex;

            // Анімація вибраного пункту (змінюємо колір на зелений)
            ResetButtonColors();
            clickedButton.Background = System.Windows.Media.Brushes.Green;
        }

        private void ResetButtonColors()
        {
            // Повертаємо усім кнопкам початковий колір
            foreach (UIElement element in ((Canvas)MapContainer.Children[1]).Children)
            {
                if (element is Button button)
                {
                    button.Background = System.Windows.Media.Brushes.Red;
                }
            }
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            // Закриваємо вікно з результатом "ОК"
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Закриваємо вікно з результатом "Cancel"
            DialogResult = false;
            Close();
        }
    }
}