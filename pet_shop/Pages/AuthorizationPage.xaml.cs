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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace pet_shop.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : Page
    {
        private int AuthButtonClickCount = 0;
        private string CaptchaText;
        private static Random Random = new Random();
        public AuthorizationPage()
        {
            InitializeComponent();
            LoginTextBox.Text = "nokupekidda2001@gmail.com";
            PasswordBox.Password = "JlFRCZ";
        }

        private void GenerateCaptcha()
        {
            GenerateText();
            CaptchaImage.Source = GenerateCaptchaImage();
        }
        private void GenerateText()
        {
            CaptchaText = "";
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            for (int i = 0; i < 4; i++)
            {
                CaptchaText += chars[Random.Next(chars.Length)];
            }
        }
        private BitmapSource GenerateCaptchaImage()
        {
            int width = 120;
            int height = 80;
            var visual = new DrawingVisual();
            using (var drawingContext = visual.RenderOpen())
            {
                var font = new Typeface(new FontFamily("Arial"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal);
                double x = 10;
                foreach (var ch in CaptchaText)
                {
                    drawingContext.PushTransform(new TranslateTransform(x, Random.Next(5, 15)));
                    var formattedText = new FormattedText(
                        ch.ToString(),
                        System.Globalization.CultureInfo.InvariantCulture,
                        FlowDirection.LeftToRight,
                        font,
                        36, new SolidColorBrush(Color.FromRgb(
                            (byte)Random.Next(256), (byte)Random.Next(256), (byte)Random.Next(256))), 1.25);
                    drawingContext.DrawText(formattedText, new Point(x, 10));
                    drawingContext.Pop();
                    x += 8;
                }
                for (int i = 0; i < 100; i++)
                {
                    double dotx = Random.Next(width);
                    double doty = Random.Next(height);
                    drawingContext.DrawEllipse(new SolidColorBrush(Color.FromRgb(128, 128, 128)), null, new Point(dotx, doty), 1.5, 1.5);
                }
            }
            var bitmap = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(visual);
            return bitmap;
        }
        private async void AuthButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AuthButtonClickCount == 0 || CaptchaTextBox.Text == CaptchaText)
                {
                    StringBuilder errors = new StringBuilder();
                    string Login = LoginTextBox.Text.ToString().Trim();
                    string Password = PasswordBox.Password.ToString().Trim();
                    if (string.IsNullOrEmpty(Login))
                    {
                        errors.AppendLine("Введите логин");
                    }
                    if (string.IsNullOrEmpty(Password))
                    {
                        errors.AppendLine("Введите пароль");
                    }
                    if (errors.Length > 0)
                    {
                        MessageBox.Show(errors.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else
                    {
                        if (Models.pets_shopEntities.GetContext().User.Any(d => d.UserLogin == Login
                        && d.UserPassword == Password))
                        {
                            var user = Models.pets_shopEntities.GetContext().User.Where(d => d.UserLogin == Login
                        && d.UserPassword == Password).FirstOrDefault();
                            Classes.Navigation.CurrentUser = user;
                            switch (user.Role.RoleName.ToString())
                            {
                                case "Администратор":
                                    Classes.Navigation.ActiveFrame.Navigate(new AdminProductList());
                                    MessageBox.Show("Успех!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
                                    AuthButtonClickCount = 0;
                                    if (CapthaGrid.Visibility != Visibility.Collapsed)
                                    {
                                        CapthaGrid.Visibility = Visibility.Collapsed;
                                    }
                                    break;
                                case "Клиент":
                                    Classes.Navigation.ActiveFrame.Navigate(new NewProductList());
                                    MessageBox.Show("Успех!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
                                    AuthButtonClickCount = 0;
                                    if (CapthaGrid.Visibility != Visibility.Collapsed)
                                    {
                                        CapthaGrid.Visibility = Visibility.Collapsed;
                                    }
                                    break;
                                case "Менеджер":
                                    Classes.Navigation.ActiveFrame.Navigate(new NewProductList());
                                    MessageBox.Show("Успех!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
                                    AuthButtonClickCount = 0;
                                    if (CapthaGrid.Visibility != Visibility.Collapsed)
                                    {
                                        CapthaGrid.Visibility = Visibility.Collapsed;
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            GenerateCaptcha();
                            CaptchaTextBox.Text = string.Empty;
                            if (AuthButtonClickCount > 0)
                            {
                                MessageBox.Show("Неправильный логин/пароль\nПодождите 10 секунд перед следующим вводом.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                                AuthButton.IsEnabled = false;
                                await Task.Delay(10000);
                                AuthButton.IsEnabled = true;
                            }
                            else
                            {
                                MessageBox.Show("Неправильный логин/пароль", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            AuthButtonClickCount++;
                            if (CapthaGrid.Visibility != Visibility.Visible)
                            {
                                CapthaGrid.Visibility = Visibility.Visible;
                            }
                        }
                    }
                }
                else if (string.IsNullOrEmpty(CaptchaTextBox.Text))
                {
                    MessageBox.Show("Каптча не решена!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Каптча решена неправильно!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    CaptchaTextBox.Text = string.Empty;
                    GenerateCaptcha();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GuestButton_Click(object sender, RoutedEventArgs e)
        {
            Classes.Navigation.ActiveFrame.Navigate(new NewProductList());
        }

        private void RegenerateCaptchaButton_Click(object sender, RoutedEventArgs e)
        {
            GenerateCaptcha();
        }
    }
}
