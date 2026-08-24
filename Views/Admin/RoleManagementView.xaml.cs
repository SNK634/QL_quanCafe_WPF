using QL_quanCafe.Data;
using QL_quanCafe.Models;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace QL_quanCafe.Views.Admin
{
    public partial class RoleManagementView : UserControl
    {
        // Biến lưu ID của nhân viên đang được click chọn
        private int _selectedEmployeeId = 0;

        public RoleManagementView()
        {
            InitializeComponent();
            LoadData(); // Gọi hàm tải dữ liệu khi vừa mở trang
        }

        // Tải danh sách nhân viên lên bảng
        private void LoadData()
        {
            using (var context = new CoffeeDbContext())
            {
                DgRoles.ItemsSource = context.TaiKhoans.ToList();
            }
        }

        // Bắt sự kiện khi bạn CLICK vào 1 dòng trên bảng
        private void DgRoles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgRoles.SelectedItem is TaiKhoan selectedItem)
            {
                _selectedEmployeeId = selectedItem.Id;
                TxtSelectedEmployee.Text = selectedItem.FullName + $" (Mã: {selectedItem.Id})";
                CmbNewRole.Text = selectedItem.Role; // Hiển thị quyền hiện tại lên ComboBox
            }
        }

        // Bắt sự kiện khi bấm nút CẬP NHẬT QUYỀN
        private void BtnSaveRole_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedEmployeeId == 0)
            {
                MessageBox.Show("Vui lòng click chọn một nhân viên bên bảng danh sách trước!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (CmbNewRole.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn chức vụ mới!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var context = new CoffeeDbContext())
            {
                // Tìm nhân viên trong CSDL
                var nvUpdate = context.TaiKhoans.Find(_selectedEmployeeId);
                if (nvUpdate != null)
                {
                    // Lấy chữ "Admin" hoặc "Nhân viên" từ ComboBox
                    string newRole = (CmbNewRole.SelectedItem as ComboBoxItem).Content.ToString();

                    nvUpdate.Role = newRole;
                    context.SaveChanges(); // Lưu vào SQL

                    MessageBox.Show($"Đã cấp quyền [{newRole}] cho nhân viên {nvUpdate.FullName} thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Tải lại bảng để thấy sự thay đổi ngay lập tức
                    LoadData();
                }
            }
        }

        // Nút quay lại
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.MainContent.Content = new EmployeeView();
            }
        }
    }
}