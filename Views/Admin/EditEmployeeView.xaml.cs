using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using QL_quanCafe.Data;
using QL_quanCafe.Models;

namespace QL_quanCafe.Views.Admin
{
    public partial class EditEmployeeView : UserControl
    {
        // Biến lưu trữ ID của nhân viên đang được sửa
        private int _employeeId;

        // Bổ sung tham số (TaiKhoan nv) vào constructor để nhận dữ liệu
        public EditEmployeeView(TaiKhoan nv)
        {
            InitializeComponent();

            // Đổ dữ liệu cũ của nhân viên lên các ô TextBox
            _employeeId = nv.Id;
            TxtFullName.Text = nv.FullName;
            TxtPhone.Text = nv.Phone;
            TxtEmail.Text = nv.Email;
            CmbRole.Text = nv.Role;
        }

        // HÀM LƯU THAY ĐỔI
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra rỗng
            if (string.IsNullOrWhiteSpace(TxtFullName.Text) || string.IsNullOrWhiteSpace(TxtPhone.Text))
            {
                MessageBox.Show("Vui lòng nhập đủ Họ tên và Số điện thoại!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var context = new CoffeeDbContext())
            {
                // Tìm nhân viên trong CSDL bằng ID
                var nvUpdate = context.TaiKhoans.Find(_employeeId);

                if (nvUpdate != null)
                {
                    // Cập nhật dữ liệu mới từ giao diện
                    nvUpdate.FullName = TxtFullName.Text;
                    nvUpdate.Phone = TxtPhone.Text;
                    nvUpdate.Email = TxtEmail.Text;
                    nvUpdate.Role = CmbRole.Text;

                    context.SaveChanges(); // Lưu vào Database

                    MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Quay lại trang danh sách sau khi lưu xong
                    BtnBack_Click(null, null);
                }
            }
        }

        // HÀM QUAY LẠI
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.MainContent.Content = new EmployeeListView();
            }
        }
        // HÀM XỬ LÝ KHI BẤM NÚT ĐỔI MẬT KHẨU
        private void BtnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            // Tạo một cửa sổ nhập liệu nhỏ (InputBox) nhanh gọn cho WPF
            Window passWindow = new Window
            {
                Title = "Đổi mật khẩu nhân viên",
                Width = 350,
                Height = 220,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize,
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FDFBF7"))
            };

            StackPanel stack = new StackPanel { Margin = new Thickness(20) };

            stack.Children.Add(new TextBlock { Text = "Nhập mật khẩu mới:", FontWeight = FontWeights.Bold, Margin = new Thickness(0, 0, 0, 5) });

            PasswordBox txtNewPass = new PasswordBox { Height = 35, FontSize = 14, Margin = new Thickness(0, 0, 0, 20) };
            stack.Children.Add(txtNewPass);

            Button btnConfirm = new Button
            {
                Content = "Xác nhận đổi",
                Height = 40,
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2E7D32")),
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold,
                Cursor = Cursors.Hand
            };

            btnConfirm.Click += (s, ev) =>
            {
                if (string.IsNullOrWhiteSpace(txtNewPass.Password))
                {
                    MessageBox.Show("Mật khẩu không được để trống!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                using (var context = new CoffeeDbContext())
                {
                    var nv = context.TaiKhoans.Find(_employeeId);
                    if (nv != null)
                    {
                        nv.Password = txtNewPass.Password; // Cập nhật mật khẩu mới
                        context.SaveChanges();
                        MessageBox.Show("Đổi mật khẩu thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        passWindow.Close();
                    }
                }
            };

            stack.Children.Add(btnConfirm);
            passWindow.Content = stack;
            passWindow.ShowDialog();
        }
    }
}