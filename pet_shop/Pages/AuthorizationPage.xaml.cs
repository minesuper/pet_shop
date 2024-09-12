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
        public AuthorizationPage()
        {
            InitializeComponent();
        }

        private void AuthButton_Click(object sender, RoutedEventArgs e)
        {
            try
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
                        var role = Models.pets_shopEntities.GetContext().User.Where(d => d.UserLogin == Login
                    && d.UserPassword == Password).FirstOrDefault().Role.RoleName;
                        switch (role.ToString())
                        {
                            case "Администратор":
                                Classes.Navigation.ActiveFrame.Navigate(new NewProductList());
                                MessageBox.Show("Успех!","Успех!",MessageBoxButton.OK, MessageBoxImage.Information);
                                break;
                            case "Клиент":
                                Classes.Navigation.ActiveFrame.Navigate(new NewProductList());
                                MessageBox.Show("Успех!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
                                break;
                            case "Менеджер":
                                Classes.Navigation.ActiveFrame.Navigate(new NewProductList());
                                MessageBox.Show("Успех!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
                                break;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Неправильный логин/пароль", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
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
    }
}
