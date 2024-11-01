﻿using System;
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
    /// Логика взаимодействия для AdminProductList.xaml
    /// </summary>
    public partial class AdminProductList : Page
    {
        List<Models.Product> CurrentProductList { get; set; } = Models.pets_shopEntities.GetContext().Product.ToList();
        public AdminProductList()
        {
            InitializeComponent();
            OnStart();
        }
        public void OnStart()
        {
            ProductListView.ItemsSource = CurrentProductList;
            CountTextBlock.Text = $"{Models.pets_shopEntities.GetContext().Product.ToList().Count()} / {Models.pets_shopEntities.GetContext().Product.ToList().Count()}";
            if (Classes.Navigation.CurrentUser != null)
            {
                FioTextBlock.Visibility = Visibility.Visible;
                FioTextBlock.Text = $"{Classes.Navigation.CurrentUser.UserSurname}" +
                    $" {Classes.Navigation.CurrentUser.UserName} " +
                    $"{Classes.Navigation.CurrentUser.UserPatronymic}";
            }
            var FactoriesList = Models.pets_shopEntities.GetContext().Facroties.ToList();
            FactoriesList.Insert(0, new Models.Facroties() { FactoryName = "Все производители" });
            ComboBoxFactory.ItemsSource = FactoriesList.Select(d => d.FactoryName);
            ComboBoxFactory.SelectedIndex = 0;
            OnUpdate();
        }

        public void OnUpdate()
        {
            try
            {
                CurrentProductList = Models.pets_shopEntities.GetContext().Product.ToList().Where(d => d.Names.ToString().ToLower().Contains(SearchTextBox.Text.ToLower()) ||
                d.ProductDescription.ToString().ToLower().Contains(SearchTextBox.Text.ToLower()) ||
                d.Facroties.FactoryName.ToString().ToLower().Contains(SearchTextBox.Text.ToLower()) ||
                d.ProductCost.ToString().ToLower().Contains(SearchTextBox.Text.ToLower())).ToList();
                if (AscendingRadioButton.IsChecked == true)
                {
                    CurrentProductList = CurrentProductList.OrderBy(d => d.ProductCost).ToList();
                }
                if (DescendingRadioButton.IsChecked == true)
                {
                    CurrentProductList = CurrentProductList.OrderByDescending(d => d.ProductCost).ToList();
                }
                if (ComboBoxFactory.SelectedIndex != 0)
                {
                    CurrentProductList = CurrentProductList.Where(d => d.Facroties.FactoryName == ComboBoxFactory.SelectedItem.ToString()).ToList();
                }
                ProductListView.ItemsSource = CurrentProductList;
                CountTextBlock.Text = $"{CurrentProductList.Count} / {Models.pets_shopEntities.GetContext().Product.ToList().Count()}";
            }
            catch (Exception)
            {
            }
        }

        private void AscendingRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            OnUpdate();
        }

        private void DescendingRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            OnUpdate();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            OnUpdate();
        }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var _context = (sender as Button).DataContext as Models.Product;
            if (Models.pets_shopEntities.GetContext().OrderProduct.Where(d => d.OpProductId == _context.ProductId).Count() == 0)
            {
                Models.pets_shopEntities.GetContext().Product.Remove(_context);
                Models.pets_shopEntities.GetContext().SaveChanges();
                OnUpdate();
                System.Windows.MessageBox.Show("Товар удален!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                System.Windows.MessageBox.Show("Товар находится в корзине!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
        }

        private void ComboBoxFactory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OnUpdate();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Classes.Navigation.ActiveFrame.Navigate(new Pages.EditPage((sender as Button).DataContext as Models.Product));
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Classes.Navigation.ActiveFrame.CanGoBack == true)
            {
                Classes.Navigation.ActiveFrame.Navigate(new Pages.AuthorizationPage());
                Classes.Navigation.CurrentUser = null;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Classes.Navigation.ActiveFrame.Navigate(new Pages.EditPage(null));
        }
    }
}
