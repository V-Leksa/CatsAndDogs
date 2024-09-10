using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CatsAndDogs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenImageButton_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Выбор картинки",
                Filter = "Картинки|*.png; *.png; *.bmp; *.jpg|Все файлы (*.*)|*.*",
                CheckFileExists = true,
            };

            if (dialog.ShowDialog(this) != true) return;

            var file = dialog.FileName;
                        
            // Проверка существования файла
            if (!File.Exists(file))
            {
                MessageBox.Show("Файл не существует: " + file);
                return;
            }

            // Установка изображения в ImageView
            ImageView.Source = new BitmapImage(new Uri(file));

            // Чтение файла в массив байтов
            byte[] imageBytes = File.ReadAllBytes(file);

            // Предсказание с использованием массива байтов
            var result = CatDogClassifer.PredictAllLabels(new CatDogClassifer.ModelInput
            {
                ImageSource = imageBytes 
            });

            // Извлечение класса с максимальной вероятностью
            var bestPrediction = result.FirstOrDefault(); // Получение первой пары (с максимальной вероятностью)

            if (!bestPrediction.Equals(default(KeyValuePair<string, float>)))
            {
                ResultText.Text = $"{bestPrediction.Key} - {bestPrediction.Value:p0}";
            }
            else
            {
                ResultText.Text = "Не удалось получить результат предсказания.";
            }
        }
    }
}