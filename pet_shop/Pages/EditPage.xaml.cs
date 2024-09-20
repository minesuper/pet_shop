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
                }
                else
                {
                    var countity = Int32.TryParse(CounityTextBox.Text, out var res);
                    if (!countity)
                    {
                        errors.AppendLine("Неправильно заполнено поле с количеством!");
                    }
                    else
                    {
                        if (res < 0)
                        {
                            errors.AppendLine("Количество - отрицательное число!");
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
                }
                else
                {
                    var price = Decimal.TryParse(PriceTextBox.Text, out var res);
                    if (price)
                    {
                        if (res < 0)
                        {
                            errors.AppendLine("Стоимость отрицательная!");
                        }
                        else
                        {
                            if (PriceTextBox.Text.Split(',').Last().Length > 2)
                            {
                                errors.AppendLine("У стоимости число знаков после запятой больше двух!");
                            }
                        }
                    }
                    else
                    {
                        errors.AppendLine("Стоимость - не число!");
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
                    return;
                }
                var selectedCategory = CategoryComboBox.SelectedItem as Models.Categories;
                _currentProduct.ProductCategoryId = Models.pets_shopEntities.GetContext().Categories.
                    Where(d => d.CategoryName == selectedCategory.CategoryName).FirstOrDefault().CategoryId;
                _currentProduct.ProductCost = Convert.ToDecimal(PriceTextBox.Text);
                _currentProduct.ProductCount = Convert.ToInt32(CounityTextBox.Text);
                _currentProduct.ProductDescription = DescriptionTextBox.Text;
                var name = Models.pets_shopEntities.GetContext().Names.
                    Where(d => d.NamesProductNames == NameTextBox.Text).
                    FirstOrDefault();
                if (name != null)
                {
                    _currentProduct.ProductNameId = name.NamesId;
                }
                else
                {
                    Models.Names new_name = new Models.Names() { NamesProductNames = NameTextBox.Text };
                    Models.pets_shopEntities.GetContext().Names.Add(new_name);
                    Models.pets_shopEntities.GetContext().SaveChanges();
                    _currentProduct.ProductNameId = new_name.NamesId;
                }
                var units = Models.pets_shopEntities.GetContext().Units.
                Where(d => d.UnitName == UnitTextBox.Text).
                FirstOrDefault();
                if (units != null)
                {
                    _currentProduct.ProductUnitId = units.id;
                }
                else
                {
                    Models.Units new_unit = new Models.Units() { UnitName = UnitTextBox.Text };
                    Models.pets_shopEntities.GetContext().Units.Add(new_unit);
                    Models.pets_shopEntities.GetContext().SaveChanges();
                    _currentProduct.ProductUnitId = new_unit.id;
                }

                var suplliers = Models.pets_shopEntities.GetContext().Importers.
                Where(d => d.ImportersName == SupplierTextBox.Text).
                FirstOrDefault();
                if (suplliers != null)
                {
                    _currentProduct.ProductImporterId = suplliers.ImportersId;
                }
                else
                {
                    Models.Importers new_supplier = new Models.Importers() { ImportersName = SupplierTextBox.Text };
                    Models.pets_shopEntities.GetContext().Importers.Add(new_supplier);
                    Models.pets_shopEntities.GetContext().SaveChanges();
                    _currentProduct.ProductImporterId = new_supplier.ImportersId;
                }

                if (isAdding)
                {
                    _currentProduct.ProductSale = 0;
                    _currentProduct.ProductSaleMax = 0;
                    _currentProduct.ProductFactoryId = 1;
                    _currentProduct.ProductArticleNumber = string.Empty;
                    Models.pets_shopEntities.GetContext().Product.Add(_currentProduct);
                }
                Models.pets_shopEntities.GetContext().SaveChanges();
                System.Windows.MessageBox.Show("Данные добавлены!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
                
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private string GetImageFromFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Выберите изображение";
            dialog.Filter = "Изображения (*.jpeg;*.jpg;*.png) | *.jpeg;*.jpg;*.png";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                return dialog.FileName;
            }
            return string.Empty;
        }
        private void ImageImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string path = GetImageFromFile();
            BitmapImage img = new BitmapImage(new Uri(path));
            while (img.Width > 300 && img.Height > 200)
            {
                System.Windows.MessageBox.Show("Размер должен быть 300x200", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                path = GetImageFromFile();
                img = new BitmapImage(new Uri(path));
            }
            _currentProduct.NameOfImage = path.Split('\\').Last();
            _currentProduct.ProductImage = File.ReadAllBytes(path);
            ImageImage.Source = img;
        }
    }
}
