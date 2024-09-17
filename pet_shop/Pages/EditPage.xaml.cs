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
    /// Логика взаимодействия для EditPage.xaml
    /// </summary>
    public partial class EditPage : Page
    {
        private bool isAdding { get; set; } = false;
        private Models.Product _currentProduct = new Models.Product();
        public EditPage(Models.Product product)
        {
            InitializeComponent();
            if (product == null)
            {
                isAdding = true;
            }
            else
            {
                _currentProduct = product;
            }
            DataContext = _currentProduct;
            OnStart();
        }

        public void OnStart()
        {
            if (isAdding)
            {
                IdLabel.Visibility = Visibility.Hidden;
                IdTextBox.Visibility = Visibility.Hidden;
                CategoryComboBox.ItemsSource = Models.pets_shopEntities.GetContext().Categories.ToList();
                CounityTextBox.Text = string.Empty;
                UnitTextBox.Text = string.Empty;
                NameTextBox.Text = string.Empty;
                PriceTextBox.Text = string.Empty;
                SupplierTextBox.Text = string.Empty;
                DescriptionTextBox.Text = string.Empty;
            }
            else
            {
                IdLabel.Visibility = Visibility.Visible;
                IdTextBox.Visibility = Visibility.Visible;
                IdTextBox.Text = Models.pets_shopEntities.GetContext().Product.Max(d => d.ProductId+1).ToString();
                CategoryComboBox.SelectedItem = Models.pets_shopEntities.GetContext().Categories.Where(d => d.CategoryId == _currentProduct.ProductCategoryId).FirstOrDefault();
                CategoryComboBox.ItemsSource = Models.pets_shopEntities.GetContext().Categories.ToList();
                CounityTextBox.Text = _currentProduct.ProductCount.ToString();
                UnitTextBox.Text = _currentProduct.Units.UnitName.ToString();
                NameTextBox.Text = _currentProduct.Names.NamesProductNames.ToString();
                PriceTextBox.Text = _currentProduct.ProductCost.ToString();
                SupplierTextBox.Text = _currentProduct.Importers.ImportersName.ToString();
                DescriptionTextBox.Text = _currentProduct.ProductDescription.ToString();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Classes.Navigation.ActiveFrame.CanGoBack == true)
            {
                Classes.Navigation.ActiveFrame.GoBack();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
