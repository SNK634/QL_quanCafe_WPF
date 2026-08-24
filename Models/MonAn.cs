using System.ComponentModel.DataAnnotations;

namespace QL_quanCafe.Models
{
    public class MonAn
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string TenMon { get; set; } = string.Empty;

        public string? TenTiengAnh { get; set; }

        public decimal GiaBan { get; set; }

        public string? CategoryCode { get; set; }

        public bool IsAvailable { get; set; } = true;
    }
}