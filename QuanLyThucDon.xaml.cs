using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using QL_quanCafe.Data;
using QL_quanCafe.Models;

namespace QL_quanCafe
{
    public partial class QuanLyThucDonView : UserControl
    {
        public ObservableCollection<Product> Products { get; set; } = new();

        public QuanLyThucDonView()
        {
            InitializeComponent();
            LoadData();
        }

        /// <summary>
        /// Đọc dữ liệu từ SQL Server
        /// </summary>
        private void LoadData()
        {
            using (var db = new CoffeeDbContext())
            {
                Products = new ObservableCollection<Product>(
                    db.Products
                      .AsNoTracking()
                      .OrderBy(x => x.Name)
                      .ToList());
            }

            dgProducts.ItemsSource = Products;
        }

        /// <summary>
        /// Chọn dòng trên DataGrid
        /// </summary>
        private void DgProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgProducts.SelectedItem is not Product selected)
                return;

            txtProductName.Text = selected.Name;
            txtSubName.Text = selected.SubName;
            txtPrice.Text = selected.Price.ToString();

            chkIsAvailable.IsChecked = selected.IsAvailable;

            foreach (ComboBoxItem item in cboCategory.Items)
            {
                if (item.Tag?.ToString() == selected.CategoryCode)
                {
                    cboCategory.SelectedItem = item;
                    break;
                }
            }
        }

        /// <summary>
        /// Thêm món mới
        /// </summary>
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
                return;

            string category = ((ComboBoxItem)cboCategory.SelectedItem)
                                .Tag
                                .ToString();

            using (var db = new CoffeeDbContext())
            {
                bool existed = db.Products.Any(x =>
                    x.Name.ToLower() == txtProductName.Text.Trim().ToLower());

                if (existed)
                {
                    MessageBox.Show(
                        "Tên món đã tồn tại!",
                        "Thông báo",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    return;
                }

                Product product = new Product
                {
                    Name = txtProductName.Text.Trim(),
                    SubName = txtSubName.Text.Trim(),
                    Price = int.Parse(txtPrice.Text.Trim()),
                    CategoryCode = category,
                    IsAvailable = chkIsAvailable.IsChecked == true
                };

                db.Products.Add(product);
                db.SaveChanges();
            }

            LoadData();

            MessageBox.Show(
                "Thêm món thành công!",
                "Thông báo",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            ClearForm();
        }        /// <summary>
                 /// Cập nhật món
                 /// </summary>
        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (dgProducts.SelectedItem is not Product selectedProduct)
            {
                MessageBox.Show(
                    "Vui lòng chọn món cần sửa!",
                    "Thông báo",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            if (!ValidateInput())
                return;

            string category = ((ComboBoxItem)cboCategory.SelectedItem)
                                .Tag
                                .ToString();

            using (var db = new CoffeeDbContext())
            {
                var product = db.Products.FirstOrDefault(x => x.Id == selectedProduct.Id);

                if (product == null)
                {
                    MessageBox.Show(
                        "Không tìm thấy món!",
                        "Lỗi",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                bool existed = db.Products.Any(x =>
                    x.Id != product.Id &&
                    x.Name.ToLower() == txtProductName.Text.Trim().ToLower());

                if (existed)
                {
                    MessageBox.Show(
                        "Tên món đã tồn tại!",
                        "Thông báo",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                product.Name = txtProductName.Text.Trim();
                product.SubName = txtSubName.Text.Trim();
                product.Price = int.Parse(txtPrice.Text.Trim());
                product.CategoryCode = category;
                product.IsAvailable = chkIsAvailable.IsChecked == true;

                db.SaveChanges();
            }

            LoadData();

            MessageBox.Show(
                "Cập nhật thành công!",
                "Thông báo",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            ClearForm();
        }

        /// <summary>
        /// Xóa món
        /// </summary>
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgProducts.SelectedItem is not Product selectedProduct)
            {
                MessageBox.Show(
                    "Vui lòng chọn món cần xóa!",
                    "Thông báo",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"Bạn có chắc muốn xóa món '{selectedProduct.Name}' ?",
                "Xác nhận",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            using (var db = new CoffeeDbContext())
            {
                var product = db.Products.FirstOrDefault(x => x.Id == selectedProduct.Id);

                if (product != null)
                {
                    db.Products.Remove(product);
                    db.SaveChanges();
                }
            }

            LoadData();

            MessageBox.Show(
                "Đã xóa món thành công!",
                "Thông báo",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            ClearForm();
        }        /// <summary>
                 /// Tìm kiếm món
                 /// </summary>
        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();

            using (var db = new CoffeeDbContext())
            {
                var list = db.Products
                    .AsNoTracking()
                    .Where(p =>
                        p.Name.ToLower().Contains(keyword) ||
                        (p.SubName != null && p.SubName.ToLower().Contains(keyword)))
                    .OrderBy(p => p.Name)
                    .ToList();

                Products = new ObservableCollection<Product>(list);
                dgProducts.ItemsSource = Products;
            }
        }

        /// <summary>
        /// Kiểm tra dữ liệu nhập
        /// </summary>
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                MessageBox.Show(
                    "Vui lòng nhập tên món!",
                    "Thông báo",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                txtProductName.Focus();
                return false;
            }

            if (cboCategory.SelectedItem == null)
            {
                MessageBox.Show(
                    "Vui lòng chọn danh mục!",
                    "Thông báo",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                cboCategory.Focus();
                return false;
            }

            if (!int.TryParse(txtPrice.Text.Trim(), out int price) || price <= 0)
            {
                MessageBox.Show(
                    "Giá bán phải là số nguyên lớn hơn 0!",
                    "Thông báo",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                txtPrice.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Xóa dữ liệu trên Form
        /// </summary>
        private void ClearForm()
        {
            txtProductName.Clear();
            txtSubName.Clear();
            txtPrice.Clear();

            cboCategory.SelectedIndex = -1;
            chkIsAvailable.IsChecked = true;

            dgProducts.SelectedItem = null;
        }
    }
}