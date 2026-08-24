using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using QL_quanCafe.Data;

namespace QL_quanCafe
{
    public partial class BaoCaoThongKeView : UserControl
    {
        public BaoCaoThongKeView()
        {
            InitializeComponent();
            // Mặc định chọn từ đầu tháng hiện tại đến hôm nay
            DateTime now = DateTime.Now;
            dpTuNgay.SelectedDate = new DateTime(now.Year, now.Month, 1);
            dpDenNgay.SelectedDate = now;

            ThongKe();
        }

        private void BtnThongKe_Click(object sender, RoutedEventArgs e)
        {
            ThongKe();
        }

        private void ThongKe()
        {
            if (!dpTuNgay.SelectedDate.HasValue || !dpDenNgay.SelectedDate.HasValue)
            {
                MessageBox.Show("Vui lòng chọn đầy đủ khoảng thời gian!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DateTime tuNgay = dpTuNgay.SelectedDate.Value.Date;
            // Tính đến hết ngày được chọn (23:59:59)
            DateTime denNgay = dpDenNgay.SelectedDate.Value.Date.AddDays(1).AddTicks(-1);

            using (var db = new CoffeeDbContext())
            {
                // Lấy danh sách hóa đơn nằm trong khoảng thời gian
                // (Giả định bạn có DbSet<HoaDon> trong CoffeeDbContext)
                var dsHoaDon = db.HoaDons
                                 .Where(h => h.NgayTao >= tuNgay && h.NgayTao <= denNgay)
                                 .ToList();

                // 1. Gán danh sách vào DataGrid
                dgHoaDon.ItemsSource = dsHoaDon;

                // 2. Tính toán các chỉ số
                decimal tongDoanhThu = dsHoaDon.Sum(h => h.TongTien);
                int tongSoDon = dsHoaDon.Count;
                decimal trungBinhDon = tongSoDon > 0 ? tongDoanhThu / tongSoDon : 0;

                // 3. Hiển thị lên giao diện
                txtTongDoanhThu.Text = string.Format("{0:N0} đ", tongDoanhThu);
                txtTongDonHang.Text = tongSoDon.ToString();
                txtTrungBinhDon.Text = string.Format("{0:N0} đ", trungBinhDon);
            }
        }
    }
}