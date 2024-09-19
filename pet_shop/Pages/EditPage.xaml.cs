using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
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
                IdTextBox.Text = Models.pets_shopEntities.GetContext().Product.Max(d => d.ProductId + 1).ToString();
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
            try
            {
                StringBuilder errors = new StringBuilder();
                if (string.IsNullOrEmpty(CategoryComboBox.Text))
                {
                    errors.AppendLine("Выберите категорию!");
                }
                if (string.IsNullOrEmpty(CounityTextBox.Text))
                {
                    errors.AppendLine("Заполните количество!");
                    var countity = Int32.TryParse(CounityTextBox.Text, out var res);
                    if (!countity)
                    {
                        errors.AppendLine("Неправильно заполнено поле с количеством!");
                    }
                    else
                    {
                        if (res < 0)
                        {
                            errors.AppendLine("Измените количество на другое число!");
                        }
                    }
                }
                if (string.IsNullOrEmpty(UnitTextBox.Text))
                {
                    errors.AppendLine("Заполните единицу измерения!");
                }
                if (string.IsNullOrEmpty(NameTextBox.Text))
                {
                    errors.AppendLine("Заполните название!");
                }
                if (string.IsNullOrEmpty(PriceTextBox.Text))
                {
                    errors.AppendLine("Заполните стоимость!");
                    var price = Decimal.TryParse(PriceTextBox.Text, out var res);
                    if (price)
                    {
                        if (res < 0)
                        {
                            errors.AppendLine("Измените стоимость на другое число!");
                        }
                        else
                        {
                            if (PriceTextBox.Text.Split(',')[1].Length > 2)
                            {
                                errors.AppendLine("Измените стоимость на другое число!");
                            }
                        }
                    }
                }
                if (string.IsNullOrEmpty(SupplierTextBox.Text))
                {
                    errors.AppendLine("Заполните поставщика!");
                }
                if (string.IsNullOrEmpty(DescriptionTextBox.Text))
                {
                    errors.AppendLine("Заполните описание!");
                }

                if (errors.Length > 0)
                {
                    System.Windows.MessageBox.Show(errors.ToString(), "Ошибка!",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    //SAVE
                }

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ImageImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Выберите изображение";
            dialog.Filter = "Изображения (*.jpeg;*.jpg;*.png) | *.jpeg;*.jpg;*.png";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                _currentProduct.NameOfImage = dialog.FileName.Split('\\').Last();
                _currentProduct.ProductImage = File.ReadAllBytes(dialog.FileName);
                ImageImage.Source = new BitmapImage(new Uri(dialog.FileName));
            }
        }
    }
}
