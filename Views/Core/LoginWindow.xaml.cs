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
using QL_quanCafe.ViewModels.Core;

namespace QL_quanCafe.Views.Core
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            this.DataContext = new LoginViewModel();
        }

        // === THÊM HÀM NÀY ĐỂ GỌI KHI BẤM NÚT ĐĂNG NHẬP (TRUYỀN QUYỀN SANG MAIN) ===
       
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string user = txtVisiblePassword.Text;      // ô nhập tên đăng nhập
            string pass = txtPassword.Password;  // ô nhập mật khẩu

            using (var db = new CoffeeDbContext())
            {
                var account = db.TaiKhoans
                                .FirstOrDefault(t => t.UserName == user
                                                  && t.Password == pass);

                if (account != null)
                {
                    MessageBox.Show("Role = " + account.Role); // kiểm tra thử

                    MainWindow mainWindow = new MainWindow(account.Role);
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!", "Lỗi");
                }
            }
        }
        // =========================================================================

        public void ResetPassword(string userName, string email, string newPassword)
        {
            using (var db = new CoffeeDbContext())
            {
                var account = db.TaiKhoans.FirstOrDefault(t => t.UserName == userName && t.Email == email);

                if (account != null)
                {
                    account.Password = newPassword;
                    db.SaveChanges();
                    MessageBox.Show("Đã đổi mật khẩu thành công!", "Thông báo");
                }
                else
                {
                    MessageBox.Show("Tên đăng nhập hoặc Email không đúng!", "Lỗi");
                }
            }
        }

        private void ForgotPassword_Click(object sender, MouseButtonEventArgs e)
        {
            var resetWindow = new ForgotPasswordWindow();
            resetWindow.ShowDialog();
        }

        private bool isSyncing = false;

        private void chkShowPassword_Checked(object sender, RoutedEventArgs e)
        {
            txtVisiblePassword.Text = txtPassword.Password;
            txtVisiblePassword.Visibility = Visibility.Visible;
            txtPassword.Visibility = Visibility.Collapsed;
        }

        private void chkShowPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            txtPassword.Password = txtVisiblePassword.Text;
            txtPassword.Visibility = Visibility.Visible;
            txtVisiblePassword.Visibility = Visibility.Collapsed;
        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!isSyncing)
            {
                isSyncing = true;
                txtVisiblePassword.Text = txtPassword.Password;
                isSyncing = false;
            }
        }

        private void txtVisiblePassword_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!isSyncing)
            {
                isSyncing = true;
                txtPassword.Password = txtVisiblePassword.Text;
                isSyncing = false;
            }
        }
    }
}