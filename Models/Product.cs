using System.ComponentModel.DataAnnotations;

namespace QL_quanCafe.Models
{
    public class Product
    {
        // 🟢 BỔ SUNG KHÓA CHÍNH (PRIMARY KEY) ĐỂ HẾT LỖI
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? SubName { get; set; }

        public int Price { get; set; }

        public string? CategoryCode { get; set; }

        public bool IsAvailable { get; set; } = true;
    }
}