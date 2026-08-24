using System;
using System.ComponentModel.DataAnnotations;

namespace QL_quanCafe.Models
{
    public class HoaDon
    {
        [Key]
        public int Id { get; set; }

        public DateTime NgayTao { get; set; } = DateTime.Now;

        public string? TenThuNgan { get; set; }

        public decimal TongTien { get; set; }
        public string TrangThai { get; set; }
    }
}