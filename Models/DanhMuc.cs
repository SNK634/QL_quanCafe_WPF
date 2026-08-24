using System.ComponentModel.DataAnnotations;

namespace QL_quanCafe.Models
{
    public class DanhMuc
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string MaDanhMuc { get; set; } = string.Empty;

        [Required]
        public string TenDanhMuc { get; set; } = string.Empty;
    }
}