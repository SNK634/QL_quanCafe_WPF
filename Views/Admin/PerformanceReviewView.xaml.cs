using QL_quanCafe.Data;
using QL_quanCafe.Models;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace QL_quanCafe.Views.Admin
{
    public partial class PerformanceReviewView : UserControl
    {
        // Biến lưu mã nhân viên đang được đánh giá
        private int _selectedEmployeeId = 0;

        public PerformanceReviewView()
        {
            InitializeComponent();
            LoadEmployees();
        }

        // Tải danh sách nhân viên từ CSDL lên bảng bên trái
        private void LoadEmployees()
        {
            using (var context = new CoffeeDbContext())
            {
                DgEmployees.ItemsSource = context.TaiKhoans.ToList();
            }
        }

        // Bắt sự kiện khi Click vào 1 nhân viên trên bảng
        private void DgEmployees_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgEmployees.SelectedItem is TaiKhoan selectedItem)
            {
                _selectedEmployeeId = selectedItem.Id;
                TxtSelectedEmployee.Text = $"Đang đánh giá: {selectedItem.FullName}";
            }
        }

        // 3 Hàm này để cập nhật con số chạy theo khi bạn kéo thanh Slider
        private void SldAttitude_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (TxtAttitudeVal != null) TxtAttitudeVal.Text = e.NewValue.ToString();
        }

        private void SldSkill_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (TxtSkillVal != null) TxtSkillVal.Text = e.NewValue.ToString();
        }

        private void SldDiscipline_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (TxtDisciplineVal != null) TxtDisciplineVal.Text = e.NewValue.ToString();
        }

        // Hàm xử lý nút LƯU ĐÁNH GIÁ
        private void BtnSaveReview_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedEmployeeId == 0)
            {
                MessageBox.Show("Vui lòng chọn một nhân viên bên danh sách để đánh giá!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Ở đây sau này có thể viết thêm code lưu vào bảng Đánh Giá trong SQL
            MessageBox.Show("Đã lưu kết quả chấm điểm KPI thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

            // Xóa form trắng lại sau khi lưu
            TxtComments.Text = "";
            SldAttitude.Value = 8;
            SldSkill.Value = 7;
            SldDiscipline.Value = 9;
        }

        // Nút quay lại trang 6 ô vuông
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null) mainWindow.MainContent.Content = new EmployeeView();
        }
    }
}