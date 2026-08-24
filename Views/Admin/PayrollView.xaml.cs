using QL_quanCafe.Data;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using ClosedXML.Excel;
using System.Collections.Generic;

namespace QL_quanCafe.Views.Admin
{
    public partial class PayrollView : UserControl
    {
        // 1. CẤU TRÚC LẠI BẢNG LƯƠNG ĐỂ CHỨA CÁC CHỈ SỐ MỚI
        public class PayrollRecord
        {
            public int Id { get; set; }
            public string FullName { get; set; }
            public string Role { get; set; }

            public int SoCaThuong { get; set; }  // 1 ca = 4 tiếng
            public int SoCaLe { get; set; }      // Ca làm ngày Lễ/Tết
            public int SaoDanhGia { get; set; }  // Đánh giá từ 1 đến 5 sao

            public decimal LuongCoBan { get; set; } // Tổng lương ca thường (hoặc lương tháng cứng)
            public decimal LuongLe { get; set; }    // Tiền nhân hệ số Lễ/Tết
            public decimal ThuongPhat { get; set; } // Tiền cộng/trừ từ Đánh giá sao (KPI)

            // Tự động tính: Tổng thực lãnh
            public decimal TotalSalary => LuongCoBan + LuongLe + ThuongPhat;
        }

        public PayrollView()
        {
            InitializeComponent();
            CmbMonth.SelectedIndex = DateTime.Now.Month - 1;
            TxtYear.Text = DateTime.Now.Year.ToString();
            LoadPayrollData();
        }

        // 2. LOGIC TÍNH LƯƠNG CHÍNH XÁC THEO CÔNG THỨC BẠN YÊU CẦU
        private void LoadPayrollData()
        {
            var dsBangLuong = new ObservableCollection<PayrollRecord>();

            using (var context = new CoffeeDbContext())
            {
                var dsNhanVien = context.TaiKhoans.ToList();
                Random rnd = new Random();

                foreach (var nv in dsNhanVien)
                {
                    // --- DỮ LIỆU ĐẦU VÀO GIẢ LẬP ---
                    // (Tương lai bạn thay các biến này bằng dữ liệu đọc từ DB Chấm công)
                    int caThuong = rnd.Next(15, 30);
                    int caLe = rnd.Next(0, 4);
                    int saoDanhGia = rnd.Next(2, 6); // Random đánh giá từ 2 đến 5 sao

                    decimal luongCB = 0;
                    decimal luongL = 0;
                    decimal kpi = 0;

                    // --- BẮT ĐẦU TÍNH TOÁN ---
                    if (nv.Role == "Admin" || nv.Role == "Quản lý")
                    {
                        // QUẢN LÝ: Tính theo tháng
                        luongCB = 7000000; // Cứng 7 triệu/tháng

                        // Tính lương 1 ca của quản lý = 7tr / 30 ngày / 3 ca
                        decimal luong1CaQL = 7000000m / 30m / 3m;

                        // Lễ tết: Quản lý 300%
                        luongL = caLe * (luong1CaQL * 3m);
                        // Đánh giá sao: 10% tổng lương cơ bản
                        if (saoDanhGia >= 4) kpi = luongCB * 0.1m;
                        else if (saoDanhGia < 3) kpi = -(luongCB * 0.1m);
                    }
                    else
                    {
                        // NHÂN VIÊN: Tính theo giờ (25k/h) - 1 ca = 4h -> 1 ca = 100k
                        luongCB = caThuong * 4 * 25000;

                        // Lễ tết: Nhân viên 200% -> 50k/h -> 1 ca = 200k
                        luongL = caLe * 4 * 50000;

                        // Đánh giá sao: 10% lương mỗi giờ (tính trên tổng số ca thường)
                        if (saoDanhGia >= 4) kpi = luongCB * 0.1m;
                        else if (saoDanhGia < 3) kpi = -(luongCB * 0.1m);
                    }

                    // Gắn vào danh sách hiển thị
                    dsBangLuong.Add(new PayrollRecord
                    {
                        Id = nv.Id,
                        FullName = nv.FullName,
                        Role = nv.Role,
                        SoCaThuong = caThuong,
                        SoCaLe = caLe,
                        SaoDanhGia = saoDanhGia,
                        LuongCoBan = luongCB,
                        LuongLe = luongL,
                        ThuongPhat = kpi
                    });
                }
            }

            DgPayroll.ItemsSource = dsBangLuong;
        }

        // ================= CÁC NÚT XỬ LÝ GIỮ NGUYÊN =================
        private void BtnCalculate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int thangChon = Convert.ToInt32(CmbMonth.Text.Replace("Tháng", "").Trim());
                int namChon = Convert.ToInt32(TxtYear.Text.Trim());
                DateTime thoiGianThucTe = DateTime.Now;

                if (namChon > thoiGianThucTe.Year || (namChon == thoiGianThucTe.Year && thangChon > thoiGianThucTe.Month))
                {
                    MessageBox.Show("Không thể tính lương cho các tháng trong tương lai!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (namChon < 2025)
                {
                    MessageBox.Show("Kỳ tính lương không hợp lệ!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Vui lòng nhập Tháng và Năm hợp lệ!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            LoadPayrollData(); // Click TÍNH LƯƠNG sẽ chạy lại hàm random và tính toán mới ở trên
            MessageBox.Show($"Đã áp dụng công thức Lễ Tết & Đánh giá sao cho Tháng {CmbMonth.Text.Replace("Tháng", "").Trim()}/{TxtYear.Text}!", "Hoàn tất", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(DgPayroll, $"Bảng Lương {CmbMonth.Text} - Năm {TxtYear.Text}");
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.MainContent.Content = new EmployeeView();
            }
        }

        // ================= CẬP NHẬT XUẤT EXCEL THÊM CỘT =================
        private void BtnExportExcel_Click(object sender, RoutedEventArgs e)
        {
            var dataList = DgPayroll.ItemsSource as IEnumerable<PayrollRecord>;

            if (dataList == null || !dataList.Any())
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel Files|*.xlsx";
            saveFileDialog.FileName = $"BangLuong_{CmbMonth.Text.Replace(" ", "")}_{TxtYear.Text}.xlsx";

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Bảng Lương");

                        // Tạo Tiêu đề cột mới
                        worksheet.Cell(1, 1).Value = "Mã NV";
                        worksheet.Cell(1, 2).Value = "Họ và Tên";
                        worksheet.Cell(1, 3).Value = "Chức Vụ";
                        worksheet.Cell(1, 4).Value = "Số Ca Thường";
                        worksheet.Cell(1, 5).Value = "Số Ca Lễ";
                        worksheet.Cell(1, 6).Value = "Sao Đánh Giá";
                        worksheet.Cell(1, 7).Value = "Lương Cơ Bản";
                        worksheet.Cell(1, 8).Value = "Thưởng Lễ";
                        worksheet.Cell(1, 9).Value = "Thưởng/Phạt (KPI)";
                        worksheet.Cell(1, 10).Value = "TỔNG THỰC LÃNH";

                        int row = 2;
                        foreach (var item in dataList)
                        {
                            worksheet.Cell(row, 1).Value = item.Id;
                            worksheet.Cell(row, 2).Value = item.FullName;
                            worksheet.Cell(row, 3).Value = item.Role;
                            worksheet.Cell(row, 4).Value = item.SoCaThuong;
                            worksheet.Cell(row, 5).Value = item.SoCaLe;
                            worksheet.Cell(row, 6).Value = item.SaoDanhGia;
                            worksheet.Cell(row, 7).Value = item.LuongCoBan;
                            worksheet.Cell(row, 8).Value = item.LuongLe;
                            worksheet.Cell(row, 9).Value = item.ThuongPhat;
                            worksheet.Cell(row, 10).Value = item.TotalSalary;
                            row++;
                        }

                        worksheet.Columns().AdjustToContents();
                        workbook.SaveAs(saveFileDialog.FileName);
                    }
                    MessageBox.Show("Xuất file Excel Bảng lương thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Có lỗi xảy ra khi xuất Excel: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        // ================= XỬ LÝ NÚT XEM CÔNG THỨC =================
        private void BtnViewFormula_Click(object sender, RoutedEventArgs e)
        {
            string congThuc = "📌 QUY ĐỊNH TÍNH LƯƠNG NHÂN SỰ:\n\n" +
                              "1. ĐỐI VỚI QUẢN LÝ / ADMIN:\n" +
                              "   - Lương cứng: 7.000.000 VNĐ/tháng.\n" +
                              "   - Ngày Lễ/Tết: Lương ca lễ x 300%.\n\n" +
                              "2. ĐỐI VỚI NHÂN VIÊN PHỤC VỤ:\n" +
                              "   - Lương cơ bản: 25.000 VNĐ/giờ.\n" +
                              "   - Mỗi ca làm việc = 4 tiếng (100.000 VNĐ/ca).\n" +
                              "   - Ngày Lễ/Tết: Nhân 200% lương (50.000 VNĐ/giờ).\n\n" +
                              "3. ĐÁNH GIÁ THƯỞNG/PHẠT (KPI):\n" +
                              "   - Đạt từ 4 đến 5 sao: Thưởng thêm 10% tổng lương cơ bản.\n" +
                              "   - Bị đánh giá 1 đến 2 sao: Phạt trừ 10% tổng lương cơ bản.\n" +
                              "   - Đạt 3 sao: Không thưởng không phạt.";

            MessageBox.Show(congThuc, "Công thức tính lương", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}