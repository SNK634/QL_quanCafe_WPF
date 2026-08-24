using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using QL_quanCafe.Data; // 🟢 Thêm namespace Data
using QL_quanCafe.Models;
using QL_quanCafe.Views.Core;

namespace QL_quanCafe
{
    // Class phụ phục vụ hiển thị Giỏ hàng
    public class CartItem
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double TotalPrice => Quantity * Price;
    }

    public partial class BanHangView : UserControl
    {
        public ObservableCollection<Product> AllProducts { get; set; }
        public ObservableCollection<CartItem> CartList { get; set; } = new ObservableCollection<CartItem>();

        public BanHangView()
        {
            InitializeComponent();
            LoadProducts();
            dgCart.ItemsSource = CartList;
        }

        private void LoadProducts()
        {
            using (var db = new CoffeeDbContext())
            {
                AllProducts = new ObservableCollection<Product>(
                    db.Products
                      .Where(p => p.IsAvailable)
                      .OrderBy(p => p.Name)
                      .ToList());
            }

            icProducts.ItemsSource = AllProducts;
        }

        // XỬ LÝ NÚT CHỌN MÓN
        private void BtnSelectProduct_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Product selectedProduct)
            {
                var existingItem = CartList.FirstOrDefault(item => item.Name == selectedProduct.Name);

                if (existingItem != null)
                {
                    existingItem.Quantity++;
                    dgCart.Items.Refresh();
                }
                else
                {
                    CartList.Add(new CartItem
                    {
                        Name = selectedProduct.Name,
                        Quantity = 1,
                        Price = selectedProduct.Price
                    });
                }

                UpdateGrandTotal();
            }
        }

        // CẬP NHẬT TỔNG TIỀN
        private void UpdateGrandTotal()
        {
            double total = CartList.Sum(item => item.TotalPrice);
            txtGrandTotal.Text = string.Format("{0:N0} đ", total);
        }

        // XỬ LÝ NÚT THANH TOÁN & IN HÓA ĐƠN
        private void BtnCheckout_Click(object sender, RoutedEventArgs e)
        {
            // 1. Kiểm tra giỏ hàng có món nào chưa
            if (CartList == null || CartList.Count == 0)
            {
                MessageBox.Show("Giỏ hàng đang trống! Vui lòng chọn món trước khi thanh toán.",
                                "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            double grandTotal = CartList.Sum(item => item.TotalPrice);

            // 🟢 2. LƯU HÓA ĐƠN VÀO SQL SERVER
            try
            {
                using (var db = new CoffeeDbContext())
                {
                    var hoaDonMoi = new HoaDon
                    {
                        NgayTao = DateTime.Now,
                        TongTien = (decimal)grandTotal,
                        TrangThai = "Đã thanh toán",
                        TenThuNgan = UserSession.CurrentUser?.FullName
                    };

                    db.HoaDons.Add(hoaDonMoi);
                    db.SaveChanges(); // 👈 LƯU THỰC SỰ VÀO SQL SERVER
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu hóa đơn vào CSDL: {ex.Message}", "Lỗi Database", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // 3. Tạo nội dung Hóa đơn (Bill) hiển thị thông báo
            string billDetails = "========================================\n";
            billDetails += "           HOÁ ĐƠN BÁN HÀNG           \n";
            billDetails += "              COFFEE SHOP              \n";
            billDetails += $"Ngày: {DateTime.Now:dd/MM/yyyy HH:mm:ss}\n";
            billDetails += "========================================\n\n";

            foreach (var item in CartList)
            {
                billDetails += $"{item.Name}\n";
                billDetails += $"   {item.Quantity} x {item.Price:N0}đ = {item.TotalPrice:N0}đ\n";
            }

            billDetails += "\n----------------------------------------\n";
            billDetails += $"TỔNG CỘNG: {grandTotal:N0} VNĐ\n";
            billDetails += "========================================\n";
            billDetails += "   Cảm ơn quý khách & Hẹn gặp lại!   ";

            // 4. Hiển thị Pop-up Bill
            MessageBox.Show(billDetails, "XÁC NHẬN THANH TOÁN", MessageBoxButton.OK, MessageBoxImage.Information);

            // 5. Thanh toán thành công -> Làm sạch giỏ hàng & reset tổng tiền
            CartList.Clear();
            UpdateGrandTotal();
        }

        // LỌC DANH MỤC
        private void LstCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AllProducts == null || icProducts == null) return;

            if (lstCategory.SelectedItem is ListBoxItem selectedItem)
            {
                string categoryTag = selectedItem.Tag?.ToString();

                if (string.IsNullOrEmpty(categoryTag) || categoryTag == "All")
                {
                    icProducts.ItemsSource = AllProducts;
                }
                else
                {
                    icProducts.ItemsSource = AllProducts.Where(p => p.CategoryCode == categoryTag).ToList();
                }
            }
        }
    }
}