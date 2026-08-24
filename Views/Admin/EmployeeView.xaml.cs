using System.Windows;
using System.Windows.Controls;

namespace QL_quanCafe.Views.Admin
{
    public partial class EmployeeView : UserControl
    {
        public EmployeeView()
        {
            InitializeComponent();
        }

        // 1. Nút Danh sách nhân viên
        private void BtnEmployeeList_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null) mainWindow.MainContent.Content = new EmployeeListView();
        }

        // 2. Nút Thêm nhân viên mới
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null) mainWindow.MainContent.Content = new AddEmployeeView();
        }

        // 3. Nút Phân quyền tài khoản (Hàm này bạn bị thiếu)
        private void BtnRole_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null) mainWindow.MainContent.Content = new RoleManagementView();
        }

        // 4. Nút Lịch làm việc (Đã bỏ MessageBox, thay bằng lệnh mở trang)
        private void BtnWorkSchedule_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null) mainWindow.MainContent.Content = new WorkScheduleView();
        }

        // 5. Nút Bảng tính lương (Đã bỏ MessageBox, thay bằng lệnh mở trang)
        private void BtnPayroll_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null) mainWindow.MainContent.Content = new PayrollView();
        }

        // 6. Nút Đánh giá hiệu suất (Đã bỏ MessageBox, thay bằng lệnh mở trang)
        private void BtnPerformance_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null) mainWindow.MainContent.Content = new PerformanceReviewView();
        }
    }
}