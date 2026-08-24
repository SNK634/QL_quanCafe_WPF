using System.Windows;
using System.Windows.Controls;
using QL_quanCafe.Data;     // Khai báo để dùng được CoffeeDbContext
using QL_quanCafe.Models;   // Khai báo để dùng được bảng TaiKhoan

namespace QL_quanCafe.Views.Admin
{
    public partial class AddEmployeeView : UserControl
    {
        public AddEmployeeView()
        {
            InitializeComponent();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            // Tìm cửa sổ MainWindow chính và trả nó về trang Dashboard Nhân sự
            var mainWindow = (MainWindow)Window.GetWindow(this);
            if (mainWindow != null)
            {
                mainWindow.MainContent.Content = new EmployeeView();
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // 1. Lấy dữ liệu bắt buộc và kiểm tra
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string fullname = txtFullName.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(fullname))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ các ô có dấu (*) gồm: Tên đăng nhập, Mật khẩu và Họ tên!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 2. Tạo đối tượng Tài khoản mới chứa toàn bộ dữ liệu trên form
            TaiKhoan tkMoi = new TaiKhoan()
            {
                UserName = username,
                Password = password,
                FullName = fullname,
                Email = txtEmail.Text,
                Phone = txtPhone.Text,
                Address = txtAddress.Text,
                Role = cmbRole.Text,
                Gender = cmbGender.Text,
                Education = cmbEducation.Text,
                NgaySinh = dpNgaySinh.SelectedDate // Lấy dữ liệu ngày sinh từ DatePicker
            };

            // 3. Kết nối với Database và Lưu
            using (var context = new CoffeeDbContext())
            {
                context.TaiKhoans.Add(tkMoi);
                context.SaveChanges(); // Lệnh này chính thức ghi dữ liệu xuống file QuanCafe.db
            }

            // 4. Thông báo thành công
            MessageBox.Show("Đã lưu nhân viên thành công! Bây giờ bạn có thể dùng tài khoản này để đăng nhập.", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

            // Lưu xong thì tự động gọi hàm nút Back để quay về trang trước
            BtnBack_Click(sender, e);
        }
        private void BtnEmployeeList_Click(object sender, RoutedEventArgs e)
        {
            // Tìm cửa sổ chính MainWindow
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                // Chuyển nội dung khung hiển thị sang UserControl Danh sách nhân viên
                mainWindow.MainContent.Content = new EmployeeListView();
            }
        }
    }
}