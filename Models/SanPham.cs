using System.ComponentModel.DataAnnotations;

namespace QL_quanCafe.Models
{
    public class SanPham
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string MaSanPham { get; set; } = string.Empty;

        [Required]
        public string TenSanPham { get; set; } = string.Empty;

        public decimal GiaBan { get; set; }

        public string? DonViTinh { get; set; }

        public int SoLuongTon { get; set; } = 0;
    }
}