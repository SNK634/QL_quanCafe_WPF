using QL_quanCafe.Data;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace QL_quanCafe.Views.Admin
{
    public partial class WorkScheduleView : UserControl
    {
        // Khai báo một lớp (Class) tạm để chứa dữ liệu 1 dòng trên bảng Lịch làm việc
        public class ScheduleRow
        {
            public int EmpId { get; set; }
            public string EmpName { get; set; }
            public string Mon { get; set; }
            public string Tue { get; set; }
            public string Wed { get; set; }
            public string Thu { get; set; }
            public string Fri { get; set; }
            public string Sat { get; set; }
            public string Sun { get; set; }
        }

        // Danh sách chứa lịch làm việc
        private ObservableCollection<ScheduleRow> _scheduleList;

        public WorkScheduleView()
        {
            InitializeComponent();
            LoadEmployeeToSchedule();

            // Đặt ngày mặc định là hôm nay
            DpWeekStart.SelectedDate = DateTime.Today;
        }

        // Tự động nhảy ngày kết thúc (cộng thêm 6 ngày) khi chọn ngày bắt đầu
        private void DpWeekStart_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DpWeekStart.SelectedDate.HasValue)
            {
                DpWeekEnd.SelectedDate = DpWeekStart.SelectedDate.Value.AddDays(6);
            }
        }

        // Tải danh sách nhân viên từ CSDL lên bảng chia ca
        private void LoadEmployeeToSchedule()
        {
            _scheduleList = new ObservableCollection<ScheduleRow>();

            using (var context = new CoffeeDbContext())
            {
                var danhSachNhanVien = context.TaiKhoans.ToList();

                foreach (var nv in danhSachNhanVien)
                {
                    _scheduleList.Add(new ScheduleRow
                    {
                        EmpId = nv.Id,
                        EmpName = nv.FullName,
                        Mon = "Nghỉ",
                        Tue = "Nghỉ",
                        Wed = "Nghỉ",
                        Thu = "Nghỉ",
                        Fri = "Nghỉ",
                        Sat = "Nghỉ",
                        Sun = "Nghỉ"
                    });
                }
            }

            // Gắn dữ liệu vào bảng
            DgSchedule.ItemsSource = _scheduleList;
        }

        // Nút Lưu Lịch (Tạm thời hiện thông báo, sau này nếu có bảng Lịch trong DB thì viết lệnh Insert vào đây)
        private void BtnSaveSchedule_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Đã lưu lịch làm việc thành công cho tuần này!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Nút Quay lại
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