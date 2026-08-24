using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using QL_quanCafe.Views.Core;
using QL_quanCafe.Views.Admin;

namespace QL_quanCafe
{
    public partial class MainWindow : Window
    {
      
        public MainWindow(string role = "Admin")
        {
            InitializeComponent();

            // KIỂM TRA PHÂN QUYỀN
            if (role == "Nhân viên")
            {
                // Giấu đi 3 chức năng quản trị
                BtnMenu.Visibility = Visibility.Collapsed;
                BtnEmployee.Visibility = Visibility.Collapsed;
                BtnReport.Visibility = Visibility.Collapsed;
            }
        }

        private void btnQuanLyThucDon_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new QuanLyThucDonView();
        }

        private void QuanLyThucDon_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new QuanLyThucDonView();
        }

        // Sự kiện khi bấm nút ĐĂNG XUẤT
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất khỏi hệ thống?",
                                         "Xác nhận đăng xuất",
                                         MessageBoxButton.YesNo,
                                         MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // 1. Mở lại trang Đăng nhập (LoginWindow)
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();

                // 2. Đóng cửa sổ phần mềm chính hiện tại
                this.Close();
            }
        }

        private void btnBaoCaoThongKe_Click(object sender, RoutedEventArgs e)
        {
            // Nạp giao diện Báo cáo Thống kê vào khung MainContent bên phải
            MainContent.Content = new BaoCaoThongKeView();
        }
        private void QuanLyNhanVien_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new EmployeeView();
        }

        private void btnBanHang_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new BanHangView();
        }

        private void BtnNavEmployee_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new EmployeeView();
        }
    }

}