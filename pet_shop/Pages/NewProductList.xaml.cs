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
    /// Логика взаимодействия для NewProductList.xaml
    /// </summary>
    public partial class NewProductList : Page
    {
        public NewProductList()
        {
            InitializeComponent();
            try
            {
                //ProductListView.ItemsSource = Models.pets_shopEntities.GetContext().Product.ToList();
            }
            catch (Exception)
            {

            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void AscendingRadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void DescendingRadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
