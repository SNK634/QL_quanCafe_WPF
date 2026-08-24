using QL_quanCafe.Data;
using QL_quanCafe.Models;
using System.Linq;
using System.Text.RegularExpressions; // Thư viện để dùng Regex
using System.Windows;

namespace QL_quanCafe.Views.Admin
{
    public partial class AddEmployeeWindow : Window
    {
        public AddEmployeeWindow()
        {
            InitializeComponent();
        }

        // Hàm kiểm tra mật khẩu mạnh
        private bool IsStrongPassword(string password)
        {
            // Regex: Ít nhất 6 ký tự, 1 chữ HOA, 1 chữ số, 1 ký tự đặc biệt
            var regex = new Regex(@"^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{6,}$");
            return regex.IsMatch(password);
        }

        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            string userName = txtUserName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Password;
            string role = cmbRole.Text;

            // 1. Kiểm tra không được để trống
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 2. Kiểm tra mật khẩu mạnh
            if (!IsStrongPassword(password))
            {
                MessageBox.Show("Mật khẩu quá yếu!\nVui lòng nhập ít nhất 6 ký tự, bao gồm chữ HOA, số và ký tự đặc biệt (VD: @, #, !).",
                                "Lỗi bảo mật", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // 3. Lưu vào Database
            using (var db = new CoffeeDbContext())
            {
                // Kiểm tra xem tên đăng nhập đã tồn tại chưa
                if (db.TaiKhoans.Any(t => t.UserName == userName))
                {
                    MessageBox.Show("Tên đăng nhập này đã tồn tại! Vui lòng chọn tên khác.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Tạo đối tượng tài khoản mới
                var newAccount = new TaiKhoan
                {
                    UserName = userName,
                    Email = email,
                    Password = password,
                    Role = role
                };

                db.TaiKhoans.Add(newAccount);
                db.SaveChanges(); // Lưu vào SQLite

                MessageBox.Show($"Đã tạo tài khoản {userName} thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close(); // Đóng form thêm mới
            }
        }
    }
}