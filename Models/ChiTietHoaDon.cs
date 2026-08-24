using System.ComponentModel.DataAnnotations;

namespace QL_quanCafe.Models
{
    public class ChiTietHoaDon
    {
        [Key]
        public int Id { get; set; }

        public int HoaDonId { get; set; }

        public int ProductId { get; set; }

        public string TenMon { get; set; } = string.Empty;

        public int SoLuong { get; set; }

        public decimal DonGia { get; set; }

        public decimal ThanhTien => SoLuong * DonGia;
    }
}