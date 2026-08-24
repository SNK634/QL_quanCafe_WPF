using System; // Nhớ có dòng này ở trên cùng

namespace QL_quanCafe.Models
{
    public class TaiKhoan
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public string Education { get; set; }
        public string Address { get; set; }

        // CỘT MỚI THÊM: Ngày sinh (Dùng DateTime? để cho phép rỗng nếu chưa nhập)
        public DateTime? NgaySinh { get; set; }
    }
}