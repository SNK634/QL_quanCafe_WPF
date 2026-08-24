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
using System.Windows.Shapes;
using QL_quanCafe.Data;
using System.Text.RegularExpressions; // Thêm thư viện này để kiểm tra mật khẩu mạnh

namespace QL_quanCafe.Views.Core
{
    /// <summary>
    /// Interaction logic for ForgotPasswordWindow.xaml
    /// </summary>
    public partial class ForgotPasswordWindow : Window
    {
        public ForgotPasswordWindow()
        {
            InitializeComponent();
        }

        // Hàm kiểm tra mật khẩu mạnh
        private bool IsStrongPassword(string password)
        {
            var regex = new Regex(@"^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{6,}$");
            return regex.IsMatch(password);
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            string newPassword = txtNewPass.Password;

            // Kiểm tra mật khẩu mạnh trước khi tiến hành tìm và lưu
            if (!IsStrongPassword(newPassword))
            {
                MessageBox.Show("Mật khẩu quá yếu!\nVui lòng nhập ít nhất 6 ký tự, bao gồm chữ HOA, số và ký tự đặc biệt (VD: @, #, !).",
                                "Lỗi bảo mật", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var db = new CoffeeDbContext())
            {
                // Tìm tài khoản bằng Email (phải nhập đúng admin@cafe.com)
                var account = db.TaiKhoans.FirstOrDefault(t => t.Email == txtEmail.Text);

                if (account != null)
                {
                    account.Password = newPassword; // Cập nhật mật khẩu mới đã đạt chuẩn
                    db.SaveChanges();

                    MessageBox.Show("Đổi mật khẩu thành công! Hãy đăng nhập lại.");
                    this.Close(); // Đóng cửa sổ quên mật khẩu, quay về LoginWindow
                }
                else
                {
                    MessageBox.Show("Email không tồn tại trong hệ thống!");
                }
            }
        }
    }
}