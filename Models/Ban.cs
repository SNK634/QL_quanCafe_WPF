using System.ComponentModel.DataAnnotations;

namespace QL_quanCafe.Models
{
    public class Ban
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string TenBan { get; set; } = string.Empty;

        // Trạng thái: "Trống", "Có khách", "Đã đặt"
        public string TrangThai { get; set; } = "Trống";
    }
}