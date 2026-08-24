using System.Linq;
using System.Windows;
using System.Windows.Controls;
using QL_quanCafe.Data;
using Microsoft.Win32;
using ClosedXML.Excel;

namespace QL_quanCafe.Views.Admin
{
    public partial class EmployeeListView : UserControl
    {
        public EmployeeListView()
        {
            InitializeComponent();
            LoadData();
        }

        // Tải dữ liệu lên bảng
        private void LoadData()
        {
            using (var context = new CoffeeDbContext())
            {
                DgEmployees.ItemsSource = context.TaiKhoans.ToList();
            }
        }

        // Quay lại 6 ô vuông
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null) mainWindow.MainContent.Content = new EmployeeView();
        }

        // In danh sách
        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(DgEmployees, "Danh Sách Nhân Viên");
            }
        }

        // Xuất file Excel
        private void BtnExportExcel_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel Files|*.xlsx";
            saveFileDialog.FileName = "DanhSachNhanVien.xlsx";

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    using (var context = new CoffeeDbContext())
                    {
                        var dsNhanVien = context.TaiKhoans.ToList();
                        using (var workbook = new XLWorkbook())
                        {
                            var worksheet = workbook.Worksheets.Add("DS Nhân Viên");

                            // Tạo Tiêu đề
                            worksheet.Cell(1, 1).Value = "Mã NV";
                            worksheet.Cell(1, 2).Value = "Tên Đăng Nhập";
                            worksheet.Cell(1, 3).Value = "Họ và Tên";
                            worksheet.Cell(1, 4).Value = "Số Điện Thoại";
                            worksheet.Cell(1, 5).Value = "Email";
                            worksheet.Cell(1, 6).Value = "Chức Vụ";

                            // Tạo Dữ liệu
                            int row = 2;
                            foreach (var nv in dsNhanVien)
                            {
                                worksheet.Cell(row, 1).Value = nv.Id;
                                worksheet.Cell(row, 2).Value = nv.UserName;
                                worksheet.Cell(row, 3).Value = nv.FullName;
                                worksheet.Cell(row, 4).Value = "'" + nv.Phone;
                                worksheet.Cell(row, 5).Value = nv.Email;
                                worksheet.Cell(row, 6).Value = nv.Role;
                                row++;
                            }

                            worksheet.Columns().AdjustToContents();
                            workbook.SaveAs(saveFileDialog.FileName);
                        }
                    }
                    MessageBox.Show("Xuất Excel thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        // Hàm xử lý khi bấm nút SỬA ở dưới dòng được chọn
        // Hàm xử lý khi bấm nút SỬA ở dưới dòng được chọn
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            // Lấy thông tin nhân viên của dòng được chọn
            var nhanVien = button?.Tag as QL_quanCafe.Models.TaiKhoan;

            if (nhanVien != null)
            {
                var mainWindow = Window.GetWindow(this) as MainWindow;
                if (mainWindow != null)
                {
                    // Mở Form Sửa và truyền đối tượng nhân viên qua
                    mainWindow.MainContent.Content = new EditEmployeeView(nhanVien);
                }
            }
        }

        // Hàm xử lý khi bấm nút XÓA ở dưới dòng được chọn
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var nhanVien = button?.Tag as QL_quanCafe.Models.TaiKhoan;

            if (nhanVien != null)
            {
                var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa nhân viên '{nhanVien.FullName}' không?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    using (var context = new CoffeeDbContext())
                    {
                        var item = context.TaiKhoans.Find(nhanVien.Id);
                        if (item != null)
                        {
                            context.TaiKhoans.Remove(item);
                            context.SaveChanges();

                            MessageBox.Show("Đã xóa nhân viên thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                            // Load lại bảng dữ liệu cho mới
                            LoadData();
                        }
                    }
                }
            }
        }

    }
}